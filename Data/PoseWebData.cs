using Npgsql;
using PoseDatabaseWebApi.Data.Dto;
using System.Data.Common;
using System.Runtime.CompilerServices;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace PoseDatabaseWebApi.Data
{
    public class PoseWebData : IPoseWebData
    {
        private readonly NpgsqlDataSource dataSource;

        public PoseWebData(NpgsqlDataSource dataSource)
        {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        #region Users

        public async Task<List<UserDto>> GetUsersAsync()
            // change to select users
        {
            var results = new List<UserDto>();
            var sql = @"
                SELECT 
                    user_id, first_name, last_name, user_name, email
                FROM
                    public.user_data
                WHERE is_deleted = FALSE;
                ".Trim();

            await using var cmd = dataSource.CreateCommand(sql);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new UserDto
                {
                    UserDataId = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    UserName = reader.GetString(3),
                    Email = reader.GetString(4)
                });
            }

            return results;
        }

        public async Task<int> CreateUserAsync(UserDto userCreateObj)
            // change to insert user
        {
            await using var cmd = dataSource.CreateCommand("INSERT INTO user_data (first_name, last_name, email, user_name, is_deleted) VALUES (@fn, @ln, @e, @un, @del) RETURNING user_id;");
            cmd.Parameters.AddWithValue("fn", userCreateObj.FirstName);
            cmd.Parameters.AddWithValue("ln", userCreateObj.LastName);
            cmd.Parameters.AddWithValue("e", userCreateObj.Email);
            cmd.Parameters.AddWithValue("un", userCreateObj.UserName);
            // Set is_deleted default value to false
            cmd.Parameters.AddWithValue("del", false);
            var result = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            if (result == null || result == DBNull.Value)
            {
                return -1;
            }

            return Convert.ToInt32(result);
        }

        public async Task<int> UpdateUserAsync(UpdateUserDto userUpdateObj)
        {
            var sql = @"
                UPDATE public.user_data
                SET
                    first_name = COALESCE(@fn, user_data.first_name),
                    last_name  = COALESCE(@ln, user_data.last_name),
                    email      = COALESCE(@e, user_data.email),
                    user_name  = COALESCE(@un, user_data.user_name)
                WHERE user_id = @uid
                RETURNING user_id;
                ".Trim();
            await using var cmd = dataSource.CreateCommand(sql);
            cmd.Parameters.AddWithValue("fn", (object?)userUpdateObj.FirstName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("ln", (object?)userUpdateObj.LastName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("e", (object?)userUpdateObj.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("un", (object?)userUpdateObj.UserName ?? DBNull.Value);
            // Set is_deleted default value to false
            // user cannot deleted pose on update
            cmd.Parameters.AddWithValue("del", false);
            cmd.Parameters.AddWithValue("uid", userUpdateObj.UserDataId);
            var result = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            if (result == null || result == DBNull.Value)
            {
                return -1;
            }

            return Convert.ToInt32(result);
        }

        public async Task<int> SetDeleteUserAsync(int userDataId)
        {
            var sql = @"
                UPDATE public.user_data
                SET is_deleted = TRUE
                WHERE user_id = @uid
                RETURNING user_id;
                ".Trim();
            await using var cmd = dataSource.CreateCommand(sql);
            cmd.Parameters.AddWithValue("uid", userDataId);
            var result = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            if (result == null || result == DBNull.Value)
            {
                return -1;
            }

            return Convert.ToInt32(result);
        }

        #endregion Users

        #region Poses

        public async Task<List<PoseDto>> SelectPoseListAsync()
        {
            var results = new List<PoseDto>();
            var sql = @"
                SELECT 
                    pose_id, pose_name, photo_url, pose_variations
                FROM
                    public.pose
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
                UPDATE public.pose
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
                UPDATE public.pose
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
