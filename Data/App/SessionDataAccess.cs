using Npgsql;
using PoseDatabaseWebApi.Data.Dto.Pose;
using PoseDatabaseWebApi.Data.Dto.Session;
using System.Collections.Generic;

namespace PoseDatabaseWebApi.Data.App
{
    public class SessionDataAccess
    {
        private readonly NpgsqlDataSource dataSource;

        public SessionDataAccess(NpgsqlDataSource dataSource)
        {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        public async Task<int> InsertSessionAsync(SessionDto sessionCreateObj)
        {
            var sql = @"INSERT INTO app.session_data(session_name, session_alternate_name) VALUES (@sn, @sal) RETURNING session_id;";
            await using var cmd = dataSource.CreateCommand(sql);
            
            cmd.Parameters.AddWithValue("sn", sessionCreateObj.SessionName);
            cmd.Parameters.AddWithValue("sal", sessionCreateObj.SessionAlternateName);
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
