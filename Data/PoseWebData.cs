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

            await using var cmd = dataSource.CreateCommand("SELECT user_id, first_name, last_name, email FROM public.user_data");
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
    }
}
