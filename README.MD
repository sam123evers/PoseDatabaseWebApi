Kinetics, Postures, Bearings, Movements, Poses, Gait, Carriage
since there does not seem to be a word that captures both movement and postures, perhaps it would be best to create a new database called movement DB

The Movement Gait

The Carriage Gait

The Carriage Database

Net10 web api for the ~~Pose Database~~

TO-DO:
- change UserController to AccountController
- - Login and Logout Endpoints
https://dev.to/cristiansifuentes/aspnet-core-identity-in-net-10-from-login-page-to-production-grade-security-4n6o

Database Considerations:
- use large gaps in pose_order values (1000, 2000, 3000) to allow for easy reordering without needing to update many rows

Migrations:
- dotnet ef migrations add <Migration Name>
- dotnet ef database update

MVP:
- user can create a sequence (which creates a session)
- user can create a session session 
- when a sesssion is created it's tied to the user that created it
- user must be logged in to create a session or sequence
- user can view all their sessions
- user can add poses to a sequence
- user can reorder poses in a sequence

- never return PasswordHash, SecurityStamp, etc. Do not map sensitive fields
- Always use Identity APIs (UserManager/SignInManager) and server-side normalization/validation; never accept or write PasswordHash, SecurityStamp, or ConcurrencyStamp from client DTOs.

•	Create schemas and grant least-privilege roles (separate DB user for app).
•	include Search Path in the connection string, or
•	create an NpgsqlDataSource per schema, or

User Stories:

User Can perform All CRUDS operations a pose without a sequence or session (pose databse)
If a user creates a session:
- the session is tied to the user in a relational table
- a sequence is created automatically
- the sequence is tied to the user through the session

A user can update the order of the poses in a sequence
A user can update the order of the sequences in a session

1) User visits website
- Home page with Login and Sign Up options
- User can see list of poses without logging in

Sequence:
- ~~dropdown list of all sequences GetAllSequences~~
- user selects sequence by name
- ~~get sequence by ID GetSequenceById~~
- get all poses, in correct order, for sequence ID

Sequence:
UpdateSequence - Name - Order of Poses
Delete Move from Sequence
Move pose within sequence
Delete Sequence
Move Sequence to new session

ToDo:
- create user (yourself) with Admin Role, who has access to everything
- create a 'new user' who is a user role, who has less access

Function info:
ExecuteReaderAsync(): For functions returning a result set (e.g., a table, a record, or multiple rows/columns). This returns an NpgsqlDataReader to iterate through the results.
ExecuteScalarAsync(): For functions returning a single scalar value (e.g., an integer, a string, a count).
ExecuteNonQueryAsync(): For functions or procedures that do not return any data (e.g., functions performing data modification only).