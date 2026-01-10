using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using PoseDatabaseWebApi.Data.Dto.Pose;
using PoseDatabaseWebApi.Data.Dto.Sequence;
using PoseDatabaseWebApi.Data.Dto.Session;
using System.Collections.Generic;

namespace PoseDatabaseWebApi.Data.App
{
    public class SequenceDataAccess : ISequenceDataAccess
    {
        private readonly NpgsqlDataSource _dataSource;
        private readonly string dummySeshName = "My Session #";

        public SequenceDataAccess(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        public async Task<List<SequenceDto>> SelectSequenceListAsync()
        {
            var results = new List<SequenceDto>();
            var sql = @"SELECT * FROM app.get_all_sequence_data()";

            await using var cmd = _dataSource.CreateCommand(sql);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new SequenceDto
                {
                    SequenceId = reader.GetInt32(0),
                    SequenceName = reader.GetString(1),
                    SequenceAlternateName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                });
            }

            return results;
        }

        public async Task<SequenceDto> GetSequenceByIdAsync(int seqId)
        {
            var sql = @"SELECT 
	                      seq.sequence_id
	                    , sequence_name
	                    , sequence_alternate_name
	                    , seqp.sequence_pose_id
	                    , p.pose_id
	                    , p.pose_name
	                    , pv.variant_name
                    FROM app.sequence_data seq
                    JOIN app.sequence_pose seqp ON seq.sequence_id = seqp.sequence_id
                    JOIN app.pose p ON seqp.pose_id = p.pose_id
                    LEFT JOIN app.pose_variant pv ON pv.pose_variant_id = seqp.pose_variant_id
                    WHERE seq.sequence_id = @sequenceId
                    ORDER BY seqp.sequence_pose_order;".Trim();
           
            await using var cmd = _dataSource.CreateCommand(sql);
            cmd.Parameters.AddWithValue("sequenceId", seqId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null; // or throw new KeyNotFoundException($"Sequence {seqId} not found");

            SequenceDto sequence = new();
            sequence.Poses = new List<PoseDto>();
            sequence.SequenceId = reader.GetInt32(0);
            sequence.SequenceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            sequence.SequenceAlternateName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);

            var firstPose = new PoseDto
            {
                PoseId = reader.GetInt32(4),
                PoseName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
            };
            sequence.Poses.Add(firstPose);

            while (await reader.ReadAsync()) {
                var pose = new PoseDto
                {
                    PoseId = reader.GetInt32(4),
                    PoseName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                };
                sequence.Poses.Add(pose);
            }

            return sequence;
        }

        public async Task<int> InsertSequenceAsync(SequenceDto sequenceCreateObj, string loggedInUserId)
        {
            await using var conn = await _dataSource.OpenConnectionAsync();
            var tx = await conn.BeginTransactionAsync();
            try
            {
                await using (var cmd = conn.CreateCommand())
                {
                    // create session ...
                    cmd.Transaction = tx;
                    cmd.CommandText = @"
                        INSERT INTO app.session_data
                        DEFAULT VALUES
                        RETURNING session_id;
                    ";
                    //cmd.Parameters.AddWithValue("sn", $"My Session #1" ?? string.Empty);

                    var sessionIdObj = await cmd.ExecuteScalarAsync();
                    var sessionId = Convert.ToInt32(sessionIdObj);
                    cmd.Parameters.Clear();

                    // insert user_session link
                    cmd.CommandText = @"
                        INSERT INTO app.user_session(user_id, session_id)
                        VALUES (@uId, @seshId);
                    ";
                    cmd.Parameters.AddWithValue("uId", loggedInUserId);
                    cmd.Parameters.AddWithValue("seshId", sessionId);
                    await cmd.ExecuteScalarAsync();
                    cmd.Parameters.Clear();

                    // create sequence ...
                    cmd.CommandText = @"
                        INSERT INTO app.sequence_data (sequence_name)
                        VALUES (@seqname)
                        RETURNING sequence_id;
                    ";
                    cmd.Parameters.AddWithValue("seqname", sequenceCreateObj.SequenceName);
                    var sequenceIdObj = await cmd.ExecuteScalarAsync();
                    var sequenceId = Convert.ToInt32(sequenceIdObj);
                    cmd.Parameters.Clear();

                    // link sequence to session
                    cmd.CommandText = @"
                        INSERT INTO app.session_sequence (session_id, sequence_id)
                        VALUES (@seshId, @seqId)
                    ;";

                    cmd.Parameters.AddWithValue("seshId", sessionId);
                    cmd.Parameters.AddWithValue("seqId", sequenceId);
                    await cmd.ExecuteScalarAsync();

                    await tx.CommitAsync();
                    return sequenceId;
                }
            }
            catch
            {
                // What is going on here?
                try { await tx.RollbackAsync(); } catch { /* swallow rollback exceptions but log them */ }
                throw;
            }
        }

        public async Task<int> UpdateSequenceAsync(SequenceDto seqDto)
        {
            if (seqDto.SequenceId == null)
            {
                return -1;
            }
            var sequenceUpdate = @"UPDATE app.sequence_data SET sequence_name = @seqName WHERE sequence_id = @seqId";
            await using var cmd = _dataSource.CreateCommand(sequenceUpdate);

            cmd.Parameters.AddWithValue("seqId", seqDto.SequenceId);
            cmd.Parameters.AddWithValue("seqName", seqDto.SequenceName);
            await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            return Convert.ToInt32(seqDto.SequenceId);
        }

        public async Task<bool> AddPoseToSequenceAsync(SequencePoseDto seqPoseDto)
        {
            var sql = @"INSERT INTO app.sequence_pose (sequence_id, pose_id) values (@seqId, @poseId, @order)";
            try 
            {
                await using var cmd = _dataSource.CreateCommand(sql);
                cmd.Parameters.AddWithValue("seqId", seqPoseDto.SequenceId);
                cmd.Parameters.AddWithValue("poseId", seqPoseDto.PoseId);
                cmd.Parameters.AddWithValue("order", seqPoseDto.SequencePoseOrder);
                int rowsEffected = await cmd.ExecuteNonQueryAsync();

                return rowsEffected > 0;
            }
            catch(NpgsqlException ex)
            {   
                // log the expection
                return false;
            }
        }

        public async Task<bool> RemovePoseFromSequenceAsync(int seqPoseId)
        {
            var sql = @"DELETE FROM app.sequence_pose WHERE sequence_pose_id = @seqPoseId";
            try
            {
                await using var cmd = _dataSource.CreateCommand(sql);
                cmd.Parameters.AddWithValue("seqPoseId", seqPoseId);
                int rowsEffected = await cmd.ExecuteNonQueryAsync();

                return rowsEffected > 0;
            }
            catch (NpgsqlException ex)
            {
                // log the expection
                return false;
            }
        }
    }
}
