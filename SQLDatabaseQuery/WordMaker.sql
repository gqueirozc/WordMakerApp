CREATE DATABASE WordMaker

CREATE TABLE tbWords (
    WordID INT PRIMARY KEY IDENTITY(1,1),
    Word NVARCHAR(255) NOT NULL
);

CREATE TABLE tbDefinitions (
    DefinitionID INT PRIMARY KEY IDENTITY(1,1),
    WordID INT,
    Language NVARCHAR(100) NOT NULL,
    Definition NVARCHAR(MAX),
    Example NVARCHAR(MAX),
    FOREIGN KEY (WordID) REFERENCES tbWords(WordID)
);

CREATE TABLE adminUsers (
    AdminID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255) NOT NULL,
	CorporateEmail NVARCHAR(255) NOT NULL,
	PhoneNumber NVARCHAR(255) NOT NULL
);

CREATE TABLE clientUsers (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(255) NOT NULL,
	Email NVARCHAR(255) NOT NULL,
	PhoneNumber NVARCHAR(255) NOT NULL,
	MainLanguage NVARCHAR(255) NOT NULL,
	UserLevel NVARCHAR(255) NOT NULL,
	UserPoints NVARCHAR(255) NOT NULL
);