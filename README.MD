# Pose and Movement Database

- Net10 web api
- PostgreSQL database
- Npgsql for application data access
- Entity Framework Core for identity

Migrations (for Identity Tables):
- dotnet ef migrations add <Migration Name>
- dotnet ef database update

MVP:
- user can reorder poses in a sequence!
- user can reorder sequences in a session!

implemented, but not tested:
- unauthroized user can view all sessions and sequences
- user can add a sequence to a session
- user can add pose to a sequence
- user can remove pose from a sequence
- user can update the name of a session
- user can update the name of a sequence
- user can get all poses, in correct order, for sequence ID
- User can perform All CRUDS operations a pose without a sequence or session (pose database)

Database:
- change pose_variant to store things like 'hands up', 'hands down', 'one leg up', 'twisting', etc
- create pose_difficulty table (stores easy, medium, hard, expert)
- should you just add a difficulty column to pose table?

https://www.daniwinksflexibility.com/flexopedia/hip-flexor-stretches

