
create table Project(
	idProjet int primary key identity not null,
	name varchar(40) not null,
	description text not null,
	budget float not null,
	priority int not null,
	duration time not null,
	startingDate datetime not null,
	endingDate datetime not null
) 

create table Worker(
	idWorker int primary key identity not null,
	lastName varchar(30) not null,
	firstName varchar(30) not null,
	dateOfBirht date not null,
	adress varchar(100) not null,
	zipCode int,
	city varchar(30),
	country varchar(30)
) 

create table Team (
	idTeam int primary key identity not null,
	Leader int references Worker,
	isBusy bit
) 

create table Assignment (
	idAssignment int identity not null,
	Project int ,
	Team int,
	Manager int,
	deadLine datetime,

	primary key (idAssignment, Project, Team),
	foreign key (Project) references Project(idProjet),
	foreign key	(Team) references team(idTeam),
	foreign key	(Manager) references Worker(idWorker)
) 

create table Role (
	idRole int primary key identity not null,
	name varchar(20),
	Techs text
) 

create table Membership(
	idMembership int identity not null,
	Worker int not null,
	Team int not null,
	Role int not null,

	primary key (idMembership, Worker, Team),
	foreign key (Worker) references Worker(idWorker),
	foreign key (team) references Team(idTeam),
	foreign key (Role) references Role(idRole)
) 

create table Task(
	idTask int primary key identity not null,
	project int,
	team int,
	title varchar(50) not null,
	description text not null,
	priority int not null,
	difficulty int not null,
	deadLine datetime not null,
	progress float not null,

	foreign key (project) references Project(idProjet),
	foreign key (team) references Team(idTeam)
)

create table Booking(
	idBooking int identity,
	member int,
	Task int,

	primary key(idBooking, member, Task),
	foreign key (member) references Worker(idWorker),
	foreign key (Task) references Task(idTask)
) 

create table Issue(
	idIssue int primary key identity not null,
	Name varchar(30) not null
) 

create table Flag(
	idFlag int primary key identity not null,
	task int,
	member int,
	issue int,

	foreign key (task) references Task(idTask),
	foreign key (member) references Worker(idWorker),
	foreign key (issue) references Issue(idIssue)
) 

create table Credentials(
	userID varchar(50) not null,
	[password] varchar(50) not null,
	credsFor int not null,
	PermissionLevel int not null

	primary key (credsFor),
	foreign key (credsFor) references Worker(idWorker)
) 
