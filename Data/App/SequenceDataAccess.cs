//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using PoseDatabaseWebApi.Data.Dto.Pose;
using PoseDatabaseWebApi.Data.Dto.Sequence;
//using PoseDatabaseWebApi.Data.Dto.Session;
//using System.Collections.Generic;

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
                    SequenceName = reader.GetString(1)
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

            //if (!await reader.ReadAsync())
            //    return null; // or throw new KeyNotFoundException($"Sequence {seqId} not found");

            SequenceDto sequence = new();
            sequence.Poses = new List<PoseDto>();
            sequence.SequenceId = reader.GetInt32(0);
            sequence.SequenceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);

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

        public async Task<List<SequenceDto>> GetSequencesAndPosesBySessionIdAsync(int seshId)
        {
            // currently not getting variant aka "side from this query
            var sql = @"SELECT
                          seq.sequence_id
                        , sequence_name
	                    , p.pose_id
	                    , p.pose_name
	                    , p.photo_url
                    FROM app.session_data sesh
                    JOIN app.session_sequence sesh_seq ON sesh.session_id = sesh_seq.session_id
                    JOIN app.sequence_data seq ON sesh_seq.sequence_id = seq.sequence_id
                    LEFT JOIN app.sequence_pose seqp ON seq.sequence_id = seqp.sequence_id
                    LEFT JOIN app.pose p ON seqp.pose_id = p.pose_id
                    WHERE sesh.session_id = @seshId
                    ORDER BY seqp.sequence_pose_order;".Trim();

            await using var cmd = _dataSource.CreateCommand(sql);
            cmd.Parameters.AddWithValue("seshId", seshId);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null; // or throw new KeyNotFoundException($"Sequence {seqId} not found");

            //List<SequenceDto> sequences = new();
            var sequences = new Dictionary<int, SequenceDto>();

            var firstSequenceId = reader.GetInt32(0);

            var firstSequence = new SequenceDto
            {
                SequenceId = reader.GetInt32(0),
                SequenceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                Poses = new List<PoseDto>
                {
                    new PoseDto
                    {
                        PoseId = reader.GetInt32(2),
                        PoseName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        PhotoUrl = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                    }
                },
            };

            sequences.Add(firstSequenceId, firstSequence);

            while (await reader.ReadAsync())
            {
                var sequenceId = reader.GetInt32(0);

                if (!sequences.TryGetValue(sequenceId, out var sequence))
                {
                    // create empty pose list here and add the current row's pose once below
                    sequence = new SequenceDto
                    {
                        SequenceId = sequenceId,
                        SequenceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        Poses = new List<PoseDto>()
                    };
                    sequences.Add(sequenceId, sequence);
                }

                if (!reader.IsDBNull(2)) 
                {
                    var pose = new PoseDto
                    {
                        PoseId = reader.GetInt32(2),
                        PoseName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        PhotoUrl = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                    };
                    sequence.Poses.Add(pose);
                }
            }

            return sequences.Values.ToList();
        }

        public async Task<int> InsertSequenceAsync(SequenceCreateDto sequenceCreateObj)
        {
            await using var conn = await _dataSource.OpenConnectionAsync();
            var tx = await conn.BeginTransactionAsync();
            
            await using (var cmd = conn.CreateCommand())
            {
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

                cmd.Parameters.AddWithValue("seshId", sequenceCreateObj.SessionId);
                cmd.Parameters.AddWithValue("seqId", sequenceId);
                await cmd.ExecuteScalarAsync();

                await tx.CommitAsync();
                return sequenceId;
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
