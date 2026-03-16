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
- user can remove a sequence from a session.. which will delete the sequence entirely

Issues:
- creating a session adds a session to the db and creates a link between the session and the user, but does not create any sequences in the session
- creating a sequence also creates a session with a default name


Like spotify, we want to be able to easily create a session, and then add sequences to it, and then add poses to those sequences.
We should also be able to create multiple sequences and then add them to a session
So we should be able to create a sequence without a session, and then later add it to a session
The issue is in creating multiple sequences, on one hand, we want all sequences to be tied to a session, aka no floating sequences, 
but we also don't want a new session for every sequence that is created


Flow:  The user starts by creating a session.
it's implied that all of the sequences are going to be part of that session.
If there are 2 sessions, the user should be "in" one when they create a sequence.
unless the user says they want to move it to a different session,
which can have a default name

So there is no such thing a creating a sequence "by itself" as far as the user is concerned
If the user creates a sequence, we add it to the session they are currently working on

implemented, but not tested:
- unauthroized user can view all sessions and sequences
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