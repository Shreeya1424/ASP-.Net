-- create table User
create table Users
(
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    MobileNo NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME NOT NULL
);

drop table Department
-- create table DEPARTMENT
create table Department
(
	DepartmentID INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(250),
    IsActive BIT NOT NULL DEFAULT 1,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

--create table DOCTORE

create table Doctor
(
	DoctorID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Qualification NVARCHAR(100) NOT NULL,
    Specialization NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- create table DoctorDepartment

create table DoctorDepartment
(
	DoctorDepartmentID INT PRIMARY KEY IDENTITY(1,1),
    DoctorID INT NOT NULL,
    DepartmentID INT NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (DoctorID) REFERENCES Doctor(DoctorID),
    FOREIGN KEY (DepartmentID) REFERENCES Department(DepartmentID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

--create table PATIENT
CREATE TABLE Patient (
    PatientID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    DateOfBirth DATETIME NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(100) NOT NULL,
    Address NVARCHAR(250) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- create table Appointment
create table Appointment
(
	 AppointmentID INT PRIMARY KEY IDENTITY(1,1),
    DoctorID INT NOT NULL,
    PatientID INT NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    AppointmentStatus NVARCHAR(20) NOT NULL,
    Description NVARCHAR(250) NOT NULL,
    SpecialRemarks NVARCHAR(100) NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    UserID INT NOT NULL,
    TotalConsultedAmount DECIMAL(5,2),
    FOREIGN KEY (DoctorID) REFERENCES Doctor(DoctorID),
    FOREIGN KEY (PatientID) REFERENCES Patient(PatientID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

insert into Users values ('shreeya','1245','asd@gmail.com','1234567890',0,GETDATE(),GETDATE())
select * from Users

insert into Department values ('cse','1245',0,GETDATE(),GETDATE(),1)
select * from Department

insert into Doctor values ('shreeya','1245','asd@gmail.com','degree','md',0,GETDATE(),GETDATE(),1)
select * from Doctor

insert into DoctorDepartment values (1,1,GETDATE(),GETDATE(),1)
select * from DoctorDepartment

insert into Patient values ('shreeya','2025-02-02','f','asd@gmail.com','1234567890','patidad','rajkot','gujarat',0,GETDATE(),GETDATE(),1)
select * from Patient

INSERT INTO Appointment (
    DoctorID,
    PatientID,
    AppointmentDate,
    AppointmentStatus,
    Description,
    SpecialRemarks,
    Created,
    Modified,
    UserID,
    TotalConsultedAmount
)
VALUES (1,2,'2025-06-25 10:30:00','General Checkup','Pending','None',GETDATE(),GETDATE(),1,300.00);
select * from Appointment


----------------------------------------PROC FOR USERS TABLE----------------------------------------------
-------INSERT----------
CREATE OR ALTER PROC PR_Users_insert
	@UserName NVARCHAR(100),
    @Password NVARCHAR(100),
    @Email NVARCHAR(100),
    @MobileNo NVARCHAR(100)
AS
BEGIN
	INSERT INTO UserS(UserName, Password, Email, MobileNo, IsActive, Created, Modified)
    VALUES (@UserName, @Password, @Email, @MobileNo, 1, GETDATE(), GETDATE())
END

EXECUTE PR_Users_insert 'YASH','2168','FBEYGF@GMAIL.COM','1265478930'

------UPDATE---------
CREATE OR ALTER PROC PR_Users_update	
	@UserID INT,
    @UserName NVARCHAR(100),
    @Password NVARCHAR(100),
    @Email NVARCHAR(100),
    @MobileNo NVARCHAR(100)
AS
BEGIN
    UPDATE Users
    SET UserName = @UserName, Password = @Password, Email = @Email, MobileNo = @MobileNo, Modified = GETDATE()
    WHERE UserID = @UserID
END
EXECUTE PR_Users_update 1,'YASHHH','2168','FBEYGF@GMAIL.COM','1265478930'

---------DELETE--------

CREATE OR ALTER PROCEDURE PR_Users_Delete
    @UserID INT
AS
BEGIN
    DELETE FROM Users WHERE UserID = @UserID
END

--------SELECT ALL--------

CREATE OR ALTER PROCEDURE PR_Users_GetAll
AS
BEGIN
    SELECT 
        UserID,
        UserName,
        Password,
        Email,
        MobileNo,
        IsActive,
        Created,
        Modified
    FROM UserS
END

------SELECT BY PRIMARY KEY

CREATE OR ALTER PROCEDURE PR_UserS_ByID
    @UserID INT
AS
BEGIN
    SELECT 
        UserID,
        UserName,
        Password,
        Email,
        MobileNo,
        IsActive,
        Created,
        Modified
    FROM UserS
    WHERE UserID = @UserID
END

---------------------------------------------PROC FOR DEPARTMENT TABLE--------------------------------------------

----INSERT----
CREATE OR ALTER PROCEDURE PR_Department_insert
    @DepartmentName NVARCHAR(100),
    @Description NVARCHAR(250),
    @UserID INT
AS
BEGIN
    INSERT INTO Department(DepartmentName, Description, IsActive, Created, Modified, UserID)
    VALUES (@DepartmentName, @Description, 1, GETDATE(), GETDATE(), @UserID)
END

EXECUTE PR_Department_insert 'cse','1245',1

-----UPDATE-----
CREATE OR ALTER PROCEDURE PR_Department_Update
    @DepartmentID INT,
    @DepartmentName NVARCHAR(100),
    @Description NVARCHAR(250)
AS
BEGIN
    UPDATE Department
    SET DepartmentName = @DepartmentName, Description = @Description, Modified = GETDATE()
    WHERE DepartmentID = @DepartmentID
END

EXECUTE PR_Department_Update 1,'cse','1245'

-----DELETE------
CREATE OR ALTER PROCEDURE PR_Department_Delete
    @DepartmentID INT
AS
BEGIN
    DELETE FROM Department WHERE DepartmentID = @DepartmentID
END

----SELECT ALL-------

CREATE OR ALTER PROCEDURE PR_Departments_GetAll
AS
BEGIN
    SELECT 
        DepartmentID,
        DepartmentName,
        Description,
        IsActive,
        Created,
        Modified,
        UserID
    FROM Department
END

-----SELECT BY PRIMARY KEY

CREATE OR ALTER PROCEDURE PR_DepartmentByID
    @DepartmentID INT
AS
BEGIN
    SELECT 
        DepartmentID,
        DepartmentName,
        Description,
        IsActive,
        Created,
        Modified,
        UserID
    FROM Department
    WHERE DepartmentID = @DepartmentID
END

--------------------------------------PROC FOR DOCTOR TABLE--------------------------------------------------

-----INSERT-----
CREATE OR ALTER PROCEDURE PR_Doctor_Insert
    @Name NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @Qualification NVARCHAR(100),
    @Specialization NVARCHAR(100),
    @UserID INT
AS
BEGIN
    INSERT INTO Doctor(Name, Phone, Email, Qualification, Specialization, IsActive, Created, Modified, UserID)
    VALUES (@Name, @Phone, @Email, @Qualification, @Specialization, 1, GETDATE(), GETDATE(), @UserID)
END

EXECUTE PR_Doctor_Insert 'shreeya','1245','asd@gmail.com','degree','md',1

-----UPDATE-----
CREATE OR ALTER PROCEDURE PR_Doctor_Update
    @DoctorID INT,
    @Name NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Email NVARCHAR(100),
    @Qualification NVARCHAR(100),
    @Specialization NVARCHAR(100)
AS
BEGIN
    UPDATE Doctor
    SET Name = @Name, Phone = @Phone, Email = @Email, Qualification = @Qualification, 
        Specialization = @Specialization, Modified = GETDATE()
    WHERE DoctorID = @DoctorID
END

EXECUTE PR_Doctor_Update 1,'shreeya','1245','asd@gmail.com','degree','md'

-----DELETE-----
CREATE OR ALTER PROCEDURE PR_Doctor_Delete
    @DoctorID INT
AS
BEGIN
    DELETE FROM Doctor WHERE DoctorID = @DoctorID
END

----SELECT ALL-------

CREATE OR ALTER PROCEDURE PR_Doctors_GetAll
AS
BEGIN
    SELECT 
        DoctorID,
        Name,
        Phone,
        Email,
        Qualification,
        Specialization,
        IsActive,
        Created,
        Modified,
        UserID
    FROM Doctor
END

-----SELECT BY PRIMARY KEY

CREATE OR ALTER PROCEDURE PR_Doctor_ByID
    @DoctorID INT
AS
BEGIN
    SELECT 
        DoctorID,
        Name,
        Phone,
        Email,
        Qualification,
        Specialization,
        IsActive,
        Created,
        Modified,
        UserID
    FROM Doctor
    WHERE DoctorID = @DoctorID
END

--------------------------------------PROC FOR DOCTORdEPARTMENT TABLE--------------------------------------------------

-----INSERT-----
CREATE OR ALTER PROCEDURE PR_DoctorDepartment_Insert
    @DoctorID INT,
    @DepartmentID INT,
    @UserID INT
AS
BEGIN
    INSERT INTO DoctorDepartment(DoctorID, DepartmentID, Created, Modified, UserID)
    VALUES (@DoctorID, @DepartmentID, GETDATE(), GETDATE(), @UserID)
END

EXECUTE PR_DoctorDepartment_Insert 1,1,1

-----UPDATE------
CREATE OR ALTER PROCEDURE PR_DoctorDepartment_Update
    @DoctorDepartmentID INT,
    @DoctorID INT,
    @DepartmentID INT
AS
BEGIN
    UPDATE DoctorDepartment
    SET DoctorID = @DoctorID, DepartmentID = @DepartmentID, Modified = GETDATE()
    WHERE DoctorDepartmentID = @DoctorDepartmentID
END

EXECUTE PR_DoctorDepartment_Update 1,1,1

----DELETE------
CREATE OR ALTER PROCEDURE PR_DoctorDepartment_Delete
    @DoctorDepartmentID INT
AS
BEGIN
    DELETE FROM DoctorDepartment WHERE DoctorDepartmentID = @DoctorDepartmentID
END

----SELECT ALL-------

CREATE OR ALTER PROCEDURE PR_DoctorDepartments_GetAll
AS
BEGIN
    SELECT 
        DoctorDepartmentID,
        DoctorID,
        DepartmentID,
        Created,
        Modified,
        UserID
    FROM DoctorDepartment
END

-----SELECT BY PRIMARY KEY

CREATE OR ALTER PROCEDURE PR_DoctorDepartment_ByID
    @DoctorDepartmentID INT
AS
BEGIN
    SELECT 
        DoctorDepartmentID,
        DoctorID,
        DepartmentID,
        Created,
        Modified,
        UserID
    FROM DoctorDepartment
    WHERE DoctorDepartmentID = @DoctorDepartmentID
END

--------------------------------------PROC FOR PATIENT TABLE--------------------------------------------------

------INSERT----
CREATE OR ALTER PROCEDURE PR_Patient_Insert
    @Name NVARCHAR(100),
    @DateOfBirth DATETIME,
    @Gender NVARCHAR(10),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(100),
    @Address NVARCHAR(250),
    @City NVARCHAR(100),
    @State NVARCHAR(100),
    @UserID INT
AS
BEGIN
    INSERT INTO Patient(Name, DateOfBirth, Gender, Email, Phone, Address, City, State, IsActive, Created, Modified, UserID)
    VALUES (@Name, @DateOfBirth, @Gender, @Email, @Phone, @Address, @City, @State, 1, GETDATE(), GETDATE(), @UserID)
END

EXECUTE PR_Patient_Insert'shreeya','2025-02-02','f','asd@gmail.com','1234567890','patidad','rajkot','gujarat',2

-----UPDATE-----
CREATE OR ALTER PROCEDURE PR_Patient_Update
    @PatientID INT,
    @Name NVARCHAR(100),
    @DateOfBirth DATETIME,
    @Gender NVARCHAR(10),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(100),
    @Address NVARCHAR(250),
    @City NVARCHAR(100),
    @State NVARCHAR(100)
AS
BEGIN
    UPDATE Patient
    SET Name = @Name, DateOfBirth = @DateOfBirth, Gender = @Gender, Email = @Email,
        Phone = @Phone, Address = @Address, City = @City, State = @State, Modified = GETDATE()
    WHERE PatientID = @PatientID
END

EXECUTE PR_Patient_Update 2,'shreeya','2025-02-02','f','asd@gmail.com','1234567890','patidad','rajkot','gujarat'

----DELETE------
CREATE OR ALTER PROCEDURE PR_Patient_Delete
    @PatientID INT
AS
BEGIN
    DELETE FROM Patient WHERE PatientID = @PatientID
END

----SELECT ALL-------

CREATE OR ALTER PROCEDURE PR_Patients_GetAll
AS
BEGIN
    SELECT 
        PatientID,
        Name,
        DateOfBirth,
        Gender,
        Email,
        Phone,
        Address,
        City,
        State,
        IsActive,
        Created,
        Modified,
        UserID
    FROM Patient
END

-----SELECT BY PRIMARY KEY

CREATE OR ALTER PROCEDURE PR_Patient_ByID
    @PatientID INT
AS
BEGIN
    SELECT 
        PatientID,
        Name,
        DateOfBirth,
        Gender,
        Email,
        Phone,
        Address,
        City,
        State,
        IsActive,
        Created,
        Modified,
        UserID
    FROM Patient
    WHERE PatientID = @PatientID
END

--------------------------------------PROC FOR APPOINTMENT TABLE--------------------------------------------------

------INSERT----
CREATE OR ALTER PROCEDURE PR_Appointment_Insert
    @DoctorID INT,
    @PatientID INT,
    @AppointmentDate DATETIME,
    @AppointmentStatus NVARCHAR(20),
    @Description NVARCHAR(250),
    @SpecialRemarks NVARCHAR(100),
    @UserID INT,
    @TotalConsultedAmount DECIMAL(5,2)
AS
BEGIN
    INSERT INTO Appointment(DoctorID, PatientID, AppointmentDate, AppointmentStatus,
                            Description, SpecialRemarks, Created, Modified, UserID, TotalConsultedAmount)
    VALUES (@DoctorID, @PatientID, @AppointmentDate, @AppointmentStatus, @Description,
            @SpecialRemarks, GETDATE(), GETDATE(), @UserID, @TotalConsultedAmount)
END

EXECUTE PR_Appointment_Insert 1,2,'2025-06-25 10:30:00','General Checkup','Pending','None',1,300.0

-----UPDATE-----
CREATE OR ALTER PROCEDURE PR_Appointment_Update
    @AppointmentID INT,
    @DoctorID INT,
    @PatientID INT,
    @AppointmentDate DATETIME,
    @AppointmentStatus NVARCHAR(20),
    @Description NVARCHAR(250),
    @SpecialRemarks NVARCHAR(100),
    @TotalConsultedAmount DECIMAL(5,2)
AS
BEGIN
    UPDATE Appointment
    SET DoctorID = @DoctorID, PatientID = @PatientID, AppointmentDate = @AppointmentDate,
        AppointmentStatus = @AppointmentStatus, Description = @Description,
        SpecialRemarks = @SpecialRemarks, Modified = GETDATE(), TotalConsultedAmount = @TotalConsultedAmount
    WHERE AppointmentID = @AppointmentID
END

EXECUTE PR_Appointment_Update 1,1,2,'2025-06-25 10:30:00','General Checkup','Pending','None',300.0
------DELETE------

CREATE OR ALTER PROCEDURE PR_Appointment_Delete
    @AppointmentID INT
AS
BEGIN
    DELETE FROM Appointment WHERE AppointmentID = @AppointmentID
END

----SELECT ALL-------

CREATE OR ALTER PROCEDURE PR_Appointments_GetAll
AS
BEGIN
    SELECT 
        AppointmentID,
        DoctorID,
        PatientID,
        AppointmentDate,
        AppointmentStatus,
        Description,
        SpecialRemarks,
        Created,
        Modified,
        UserID,
        TotalConsultedAmount
    FROM Appointment
END

-----SELECT BY PRIMARY KEY

CREATE OR ALTER PROCEDURE PR_Appointment_ByID
    @AppointmentID INT
AS
BEGIN
    SELECT 
        AppointmentID,
        DoctorID,
        PatientID,
        AppointmentDate,
        AppointmentStatus,
        Description,
        SpecialRemarks,
        Created,
        Modified,
        UserID,
        TotalConsultedAmount
    FROM Appointment
    WHERE AppointmentID = @AppointmentID
END
