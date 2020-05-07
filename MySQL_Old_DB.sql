create database TaskMGR;
use TaskMGR;

create table Project(
	idProjet int(11) primary key AUTO_INCREMENT not null,
	name varchar(40) not null,
	description text not null,
	budget double not null,
	priority int not null,
	duration time not null,
	startingDate datetime not null,
	endingDate datetime not null
) ENGINE = InnoDB;

create table Worker(
	idWorker int(11) primary key AUTO_INCREMENT not null,
	lastName varchar(30) not null,
	firstName varchar(30) not null,
	dateOfBirht date not null,
	adress tinytext not null,
	zipCode int(8),
	city tinytext,
	country tinytext
) ENGINE = InnoDB;

create table Team (
	idTeam int(11) primary key AUTO_INCREMENT not null,
	Leader int(11) ,
	isBusy bit,

	foreign key (Leader) references Worker(idWorker)
) ENGINE = InnoDB;

create table Assignment (
	idAssignment int(11) AUTO_INCREMENT not null,
	Project int(11) ,
	Team int(11),
	Manager int(11),
	deadLine datetime,

	primary key (idAssignment, Project, Team),
	foreign key (Project) references Project(idProjet),
	foreign key	(Team) references team(idTeam),
	foreign key	(Manager) references Worker(idWorker)
) ENGINE = InnoDB;

create table Role (
	idRole int(11) primary key AUTO_INCREMENT not null,
	name varchar(20),
	Techs text
) ENGINE = InnoDB;

create table Membership(
	idMembership int(11) AUTO_INCREMENT not null,
	Worker int(11) not null,
	Team int(11) not null,
	Role int(11) not null,

	primary key (idMembership, Worker, Team),
	foreign key (Worker) references Worker(idWorker),
	foreign key (team) references Team(idTeam),
	foreign key (Role) references Role(idRole)
) ENGINE = InnoDB;

create table Task(
	idTask int(11) primary key AUTO_INCREMENT not null,
	project int(11),
	team int(11),
	title varchar(50) not null,
	description text not null,
	priority int not null,
	difficulty int not null,
	deadLine datetime not null,
	progress float not null,

	foreign key (project) references Project(idProjet),
	foreign key (team) references Team(idTeam)
)ENGINE = InnoDB;

create table Booking(
	idBooking int(11) AUTO_INCREMENT,
	member int(11),
	Task int(11),

	primary key(idBooking, member, Task),
	foreign key (member) references Membership(Worker),
	foreign key (Task) references Task(idTask)
) ENGINE = InnoDB;

create table Issue(
	idIssue int(11) primary key AUTO_INCREMENT not null,
	Name varchar(30) not null
) ENGINE = InnoDB;

create table Flag(
	idFlag int(11) primary key AUTO_INCREMENT not null,
	task int(11),
	member int(11),
	issue int(11),

	foreign key (task) references Task(idTask),
	foreign key (member) references Membership(Worker),
	foreign key (issue) references Issue(idIssue)
) ENGINE = InnoDB;

create table Credentials(
	userID varchar(50) not null,
	password varchar(50) not null,
	credsFor int not null,
	PermissionLevel int(2) not null

	primary key (credsFor),
	foreign key (credsFor) references Worker(idWorker)
) ENGINE = InnoDB;
