create database test1
use test1
go

-------------------------------------------------- SKILL ----------------------------------------------------------------------

CREATE TABLE skill (
  [idskill] int NOT NULL IDENTITY,
  [skillname] nvarchar(50) NOT NULL,
  [categoryid] int DEFAULT NULL,
  PRIMARY KEY ([idskill])
 ,
  CONSTRAINT [categoryid_skill] FOREIGN KEY ([categoryid]) REFERENCES category ([categoryid])
)   ;

CREATE INDEX [categoryid_idx] ON skill ([categoryid]);


-------------------------------------------------- CATEGORY ----------------------------------------------------------------------

CREATE TABLE category (
  [categoryid] int NOT NULL IDENTITY,
  [categoryname] nvarchar(50) NOT NULL,
  PRIMARY KEY ([categoryid])
)   ;


-------------------------------------------------- TRAINEE ----------------------------------------------------------------------

CREATE TABLE trainee (
  [traineeid] int NOT NULL IDENTITY,
  [name] nvarchar(100) DEFAULT NULL,
  [email] varchar(50) NOT NULL,
  [phone] varchar(50) DEFAULT NULL,
  [address] nvarchar(max),
  [isActive] smallint NOT NULL,
  [password] varchar(45) NOT NULL,
  [activationcode] varchar(45) NOT NULL,
  [resetpasswordcode] varchar(45) DEFAULT NULL,
  PRIMARY KEY ([traineeid])
)   ;


-------------------------------------------------- COURSE ----------------------------------------------------------------------

CREATE TABLE course (
  [courseid] varchar(50) NOT NULL,
  [title] nvarchar(255) NOT NULL,
  [description] nvarchar(max),
  [city] nvarchar(50) NOT NULL,
  [address] nvarchar(255) NOT NULL,
  [time] nvarchar(50) NOT NULL,
  [createddate] datetime2(0) DEFAULT NULL,
  [trainee_course] int NOT NULL,
  [idskill] int DEFAULT NULL,
  [isnotification] int NOT NULL,
  [image_course] varchar(500) DEFAULT NULL,
  PRIMARY KEY ([courseid])
 ,
  CONSTRAINT [idskill_course] FOREIGN KEY ([idskill]) REFERENCES skill ([idskill]),
  CONSTRAINT [trainee_course] FOREIGN KEY ([trainee_course]) REFERENCES trainee ([traineeid])
)  ;

CREATE INDEX [traineeid_idx] ON course ([trainee_course]);
CREATE INDEX [idskill_course_idx] ON course ([idskill]);


-------------------------------------------------- MENTOR ----------------------------------------------------------------------

CREATE TABLE mentor (
  [mentorid] int NOT NULL IDENTITY,
  [name] varchar(100) DEFAULT NULL,
  [email] varchar(50) NOT NULL,
  [phone] varchar(50) DEFAULT NULL,
  [address] varchar(max),
  [isActive] smallint NOT NULL,
  [password] varchar(45) NOT NULL,
  [idskill] int NOT NULL,
  [activationcode] varchar(45) NOT NULL,
  [resetpasswordcode] varchar(45) DEFAULT NULL,
  [isreceivenotification] int NOT NULL,
  PRIMARY KEY ([mentorid])
 ,
  CONSTRAINT [idskill] FOREIGN KEY ([idskill]) REFERENCES skill ([idskill])
)   ;

CREATE INDEX [idskill_idx] ON mentor ([idskill]);

-------------------------------------------------- ASSIGNED ----------------------------------------------------------------------

CREATE TABLE assigned (
  [mentorid] int NOT NULL,
  [notes] nvarchar(max),
  [startday] date DEFAULT NULL,
  [endday] date DEFAULT NULL,
  [address] nvarchar(max),
  [courseid] varchar(50) NOT NULL,
  PRIMARY KEY ([mentorid],[courseid])
 ,
  CONSTRAINT [courseid] FOREIGN KEY ([courseid]) REFERENCES course ([courseid]),
  CONSTRAINT [mentorid] FOREIGN KEY ([mentorid]) REFERENCES mentor ([mentorid])
)  ;

CREATE INDEX [courseid_idx] ON assigned ([courseid]);



