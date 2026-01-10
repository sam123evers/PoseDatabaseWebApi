using Npgsql;
using PoseDatabaseWebApi.Data.Dto.Sequence;
using PoseDatabaseWebApi.Data.Dto.Session;

namespace PoseDatabaseWebApi.Data.App
{
    public class SessionDataAccess : ISessionDataAccess
    {
        private readonly NpgsqlDataSource _dataSource;

        public SessionDataAccess(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }


        public async Task<List<SessionDto>> SelectAllSessionsAndSequencesAsync()
        {
            var sessionList = new List<SessionDto>();
            var selectUnauthSeshAndSeq = @"
                SELECT
                      sesh.session_id
	                , session_name
                    , seq.sequence_id
	                , sequence_name
                FROM app.session_data sesh
                JOIN app.session_sequence sesh_seq ON sesh.session_id = sesh_seq.session_id
                JOIN app.sequence_data seq ON sesh_seq.sequence_id = seq.sequence_id;
            ".Trim();
            await using var cmd = _dataSource.CreateCommand(selectUnauthSeshAndSeq);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) { return null; }

            var sessions = new Dictionary<int, SessionDto>();

            var firstSessionId = reader.GetInt32(0);

            var firstSession = new SessionDto
            {
                SessionId = firstSessionId,
                SessionName = reader.GetString(1),
                Sequences = new List<SequenceDto>
                {
                    new SequenceDto
                    {
                        SequenceId = reader.GetInt32(2),
                        SequenceName = reader.GetString(3)
                    }
                }
            };

            sessions.Add(firstSessionId, firstSession);

            while (await reader.ReadAsync())
            {
                var seshId = reader.GetInt32(0);

                if (!sessions.TryGetValue(seshId, out var session))
                {
                    session = new SessionDto
                    {
                        SessionId = seshId,
                        SessionName = reader.GetString(1),
                        Sequences = new List<SequenceDto>()
                    };
                    sessions.Add(seshId, session);
                }

                var seq = new SequenceDto
                {
                    SequenceId = reader.GetInt32(2),
                    SequenceName = reader.GetString(3)
                };

                session.Sequences.Add(seq);

            }

            return ([.. sessions.Values]);
        }

        public async Task<List<SessionDto>> SelectMySessionsAndSequencesAsync(string userId)
        {
            var sessionList = new List<SessionDto>();
            var selectSeshAndSeqByUserId = @"
                SELECT
                      sesh.session_id
	                , session_name
                    , seq.sequence_id
	                , sequence_name
                FROM app.session_data sesh
                JOIN app.session_sequence sesh_seq ON sesh.session_id = sesh_seq.session_id
                JOIN app.sequence_data seq ON sesh_seq.sequence_id = seq.sequence_id
                JOIN app.user_session user_sesh ON user_sesh.session_id = sesh.session_id
                JOIN identity.""AspNetUsers"" users ON user_sesh.user_id = users.""Id""
                WHERE user_sesh.user_id = @userId;
            ".Trim();
            await using var cmd = _dataSource.CreateCommand(selectSeshAndSeqByUserId);
            cmd.Parameters.AddWithValue("userId", userId);
            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) { return null; }

            var sessions = new Dictionary<int, SessionDto>();

            var firstSessionId = reader.GetInt32(0);

            var firstSession = new SessionDto
            {
                SessionId = firstSessionId,
                SessionName = reader.GetString(1),
                Sequences = new List<SequenceDto>
                {
                    new SequenceDto
                    {
                        SequenceId = reader.GetInt32(2),
                        SequenceName = reader.GetString(3)
                    }
                }
            };

            sessions.Add(firstSessionId, firstSession);

            while (await reader.ReadAsync())
            {
                var seshId = reader.GetInt32(0);

                if (!sessions.TryGetValue(seshId, out var session))
                {
                    session = new SessionDto
                    {
                        SessionId = seshId,
                        SessionName = reader.GetString(1),
                        Sequences = new List<SequenceDto>()
                    };
                    sessions.Add(seshId, session);
                }

                var seq = new SequenceDto
                {
                    SequenceId = reader.GetInt32(2),
                    SequenceName = reader.GetString(3)
                };

                session.Sequences.Add(seq);

            }

            return ([.. sessions.Values]);
        }

        public async Task<int> InsertSessionAsync(SessionDto sessionCreateObj, string userId)
        {
            var sessionInsert = @"INSERT INTO app.session_data(session_name, session_alternate_name) VALUES (@sn, @sal) RETURNING session_id;";
            await using var cmd = _dataSource.CreateCommand(sessionInsert);
            
            cmd.Parameters.AddWithValue("sn", sessionCreateObj.SessionName);
            cmd.Parameters.AddWithValue("sal", sessionCreateObj.SessionAlternateName);
            var seshId = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            var userSessionInsert = @"
                INSERT INTO app.user_session(user_id, session_id) VALUES (@uId, @seshId);
            ";

            await using var cmd2 = _dataSource.CreateCommand(userSessionInsert);
            cmd.Parameters.AddWithValue("uId", userId);
            cmd.Parameters.AddWithValue("seshId", Convert.ToInt32(seshId));
            await cmd.ExecuteNonQueryAsync();
            cmd.Parameters.Clear();

            if (seshId == null || seshId == DBNull.Value)
            {
                return -1;
            }

            return Convert.ToInt32(seshId);
        }

        public async Task<int> UpdateSessionAsync(SessionDto seshDto)
        {
            if (seshDto.SessionId == null)
            {
                return -1;
            }

            var sessionUpdate = @"UPDATE app.session_data SET session_name = @seshName WHERE session_id = @seshId";
            await using var cmd = _dataSource.CreateCommand(sessionUpdate);

            cmd.Parameters.AddWithValue("seshId", seshDto.SessionId);
            cmd.Parameters.AddWithValue("seshName", seshDto.SessionName);
            await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();

            

            return (int)seshDto.SessionId;
        }
    }
}
