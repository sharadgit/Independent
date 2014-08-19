/*
select * from AspNetUsers
select * from AspNetRoles 
select * from AspNetUserRoles
*/

declare @UserName nvarchar(50) = 'myusername',
		@RoleName nvarchar(50) = 'Users',
		@UserId   nvarchar(256),
		@RoleId   nvarchar(256)

select @UserId = Id 
  from AspNetUsers 
 where UserName = @UserName

select @RoleId = Id 
  from AspNetRoles 
 where Name = @RoleName

insert into AspNetUserRoles(
		UserId,		RoleId
	)
select	@UserId,	@RoleId
select @@ROWCOUNT