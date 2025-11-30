Kinetics, Postures, Bearings, Movements, Poses, Gait, Carriage
since there does not seem to be a word that captures both movement and postures, perhaps it would be best to create a new database called movement DB

The Movement Gait

The Carriage Gait

Net10 web api for the ~~Pose Database~~

possibly useful link: https://eliot-jones.com/2017/6/asp-net-core-identity-post-1

video breakdown of roles, claims etc:
https://learn.microsoft.com/en-us/shows/seth-juarez/aspnet-core-authorization-barry-dorrans

search for "Using ASP.NET Core Identity without Entity Framework Core"

call builder.Services.AddIdentity<TUser, TRole>() in Program.cs

Tables:
- AspNetRoles
- AspNetUsers
- AspNetUserLogins
- AspNetUserRoles
- AspNetUserClaims
- AspNetRoleClaims
- AspNetUserTokens

you have all of these tables besides UserTokens

- never return PasswordHash, SecurityStamp, etc. Do not map sensitive fields
- •	Always use Identity APIs (UserManager/SignInManager) and server-side normalization/validation; never accept or write PasswordHash, SecurityStamp, or ConcurrencyStamp from client DTOs.

•	Pick clear schema names: e.g., identity (Identity tables), app (application data), audit, reporting.
•	Create schemas and grant least-privilege roles (separate DB user for app).
•	Prefer schema-qualified table names in SQL (avoid relying on search_path).

•	include Search Path in the connection string, or
•	create an NpgsqlDataSource per schema, or

ToDo:
- create user (yourself) with Admin Role, who has access to everything
- create a 'new user' who is a user role, who has less access