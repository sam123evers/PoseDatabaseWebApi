using Npgsql;
using PoseDatabaseWebApi.Data.Dto;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace PoseDatabaseWebApi.Data
{
    public class PoseWebData : IPoseWebData
    {
        private readonly NpgsqlDataSource dataSource;

        public PoseWebData(NpgsqlDataSource dataSource)
        {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var results = new List<UserDto>();

            await using var cmd = dataSource.CreateCommand("SELECT user_id, first_name, last_name, user_name, email FROM public.user_data WHERE is_deleted = FALSE");
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(new UserDto
                {
                    UserDataId = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3)
                });
            }

            return results;
        }

        public async Task<int> CreateUserAsync(UserDto userCreateObj)
        {
            await using var cmd = dataSource.CreateCommand("INSERT INTO user_data (first_name, last_name, email, user_name, is_deleted) VALUES (@first_name, @last_name, @email, @user_name, @is_deleted) RETURNING user_id;");
            cmd.Parameters.AddWithValue("first_name", userCreateObj.FirstName);
            cmd.Parameters.AddWithValue("last_name", userCreateObj.LastName);
            cmd.Parameters.AddWithValue("email", userCreateObj.Email);
            cmd.Parameters.AddWithValue("user_name", userCreateObj.UserName);
            cmd.Parameters.AddWithValue("is_deleted", false);
            var result = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            if (result == null || result == DBNull.Value)
            {
                return -1;
            }

            return Convert.ToInt32(result);
        }
    }
}
