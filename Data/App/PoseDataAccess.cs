using Npgsql;
using PoseDatabaseWebApi.Data.Dto.Identity;
using PoseDatabaseWebApi.Data.Dto.Pose;

namespace PoseDatabaseWebApi.Data.App
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
                    pose_id, pose_name, photo_url
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
                    PhotoUrl = reader.GetString(2)
                });
            }

            return results;
        }

        public async Task<int> InsertPoseAsync(PoseDto poseCreateObj)
        {
            await using var cmd = dataSource.CreateCommand("INSERT INTO app.pose (pose_name, photo_url) VALUES (@pn, @url) RETURNING pose_id;");
            cmd.Parameters.AddWithValue("pn", poseCreateObj.PoseName);
            cmd.Parameters.AddWithValue("url", poseCreateObj.PhotoUrl);
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
