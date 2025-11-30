using Npgsql;
using PoseDatabaseWebApi.Data.Dto.Identity;
using PoseDatabaseWebApi.Data.Dto.Pose;

namespace PoseDatabaseWebApi.Data
{
    public class PoseDataAccess : IPoseDataAccess
    {
        private readonly NpgsqlDataSource dataSource;

        public PoseDataAccess(NpgsqlDataSource dataSource)
        {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        #region Poses

        public async Task<List<PoseDto>> SelectPoseListAsync()
        {
            var results = new List<PoseDto>();
            var sql = @"
                SELECT 
                    pose_id, pose_name, photo_url, pose_variations
                FROM
                    app.pose
                WHERE is_deleted = FALSE;
                ".Trim();

            await using var cmd = dataSource.CreateCommand(sql);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new PoseDto
                {
                    PoseId = reader.GetInt32(0),
                    PoseName = reader.GetString(1),
                    PhotoUrl = reader.GetString(2),
                    PoseVariations = reader.IsDBNull(3) ? null : reader.GetFieldValue<int[]>(3)
                });
            }

            return results;
        }

        public async Task<int> InsertPoseAsync(PoseDto poseCreateObj)
        {
            await using var cmd = dataSource.CreateCommand("INSERT INTO pose (pose_name, photo_url, pose_variations) VALUES (@pn, @url, @vars) RETURNING pose_id;");
            cmd.Parameters.AddWithValue("pn", poseCreateObj.PoseName);
            cmd.Parameters.AddWithValue("url", poseCreateObj.PhotoUrl);
            cmd.Parameters.AddWithValue("vars", (object?)poseCreateObj.PoseVariations ?? DBNull.Value);
            var result = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            if (result == null || result == DBNull.Value)
            {
                return -1;
            }

            return Convert.ToInt32(result);
        }

        public async Task<int> UpdatePoseAsync(UpdatePoseDto poseUpdateObj)
        {
            var sql = @"
                UPDATE app.pose
                SET
                    pose_name       = COALESCE(@pn, pose.pose_name),
                    photo_url       = COALESCE(@url, pose.photo_url),
                    pose_variations = COALESCE(@vars, pose.pose_variations)
                WHERE pose_id       = @pid
                RETURNING pose_id;
                ".Trim();
            await using var cmd = dataSource.CreateCommand(sql);
            cmd.Parameters.AddWithValue("pn", (object?)poseUpdateObj.PoseName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("url", (object?)poseUpdateObj.PhotoUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("vars", (object?)poseUpdateObj.PoseVariations ?? DBNull.Value);
            cmd.Parameters.AddWithValue("pid", poseUpdateObj.PoseId);
            var result = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            if (result == null || result == DBNull.Value)
            {
                return -1;
            }

            return Convert.ToInt32(result);
        }

        public async Task<int> SetDeletePoseAsync(int poseId)
            // return boolean?
        {
            var sql = @"
                UPDATE app.pose
                SET is_deleted = TRUE
                WHERE pose_id = @pid
                RETURNING pose_id;
                ".Trim();
            await using var cmd = dataSource.CreateCommand(sql);
            cmd.Parameters.AddWithValue("pid", poseId);
            var result = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            if (result == null || result == DBNull.Value)
            {
                return -1;
            }

            return Convert.ToInt32(result);
        }

        #endregion

    }
}
