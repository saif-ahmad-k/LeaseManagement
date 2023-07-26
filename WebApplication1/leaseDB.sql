CREATE TABLE tblCustomer (
    ID int identity(1,1) NOT NULL,
    LastName varchar(255) NOT NULL
    
    PRIMARY KEY (ID)
);
CREATE TABLE tblLender (
    ID int identity(1,1) NOT NULL,
    Name varchar(255) NOT NULL
    
    PRIMARY KEY (ID)
);
CREATE TABLE tblLesseDetail (
    ID int identity(1,1) NOT NULL,
    Lesse varchar(255),
    LesseDate DateTime,
    FundDate DateTime,
	FirstPaymentDate DateTime,
	FirstPaymentToBankDate DateTime,
	LesseMaturityDate DateTime,
	Description nvarchar(max),
	LeaseID varchar(150),
	DealOrigin varchar(50),
	LeaseType varchar(50),
	TotalLeaase  varchar(150),
	MonthlyPayment  varchar(150),
	Term  varchar(150),
	Lender varchar(50),
	LeaseFee  varchar(150),
	VIN nvarchar(max),
	Residual  varchar(150),
	EstimatedPropertyTax  varchar(150),
	MMA  varchar(150),
	AccountRep1 nvarchar(140),
	AccountRep2 nvarchar(140),
	AccountRep3 nvarchar(140),
	InsuranceProvider nvarchar(300),
	InsuranceExpiry DateTime,
	VendorCost  varchar(150),
	LeaseDocumentForm nvarchar(100),
	TitleHolder varchar(50),
	IsSaleTaxExept bit,
	LeaseTerminationDate datetime,
	TitleStatus nvarchar(40),
	UCCStatus nvarchar(40),
	UCCDate datetime,
	PropertyTaxStatus nvarchar(50),
	CollateralStreet nvarchar(200),
	CollateralCity varchar(40),
	CollateralState varchar(10),
	CollateralZipCode varchar(50),
	CollateralCountry varchar(50),
	MailingStreet nvarchar(200),
	MailingCity varchar(40),
	MailingState varchar(10),
	MailingZipCode  varchar(150),
	Comments1 nvarchar(350),
	Comments2 nvarchar(350),
	LeaseYear  varchar(150),
	LeaseMonthAndYear nvarchar(30),
	FundedMonthAndYear nvarchar(30),
	LeaseCount  varchar(150),
	BANKCHK_LESSEEBANK_TAB nvarchar(330),
	BANKCHK_BANKYEAR_TAB nvarchar(330),
	BANKCHK_BANKMONTH_TAB nvarchar(330),
	LESSEECHK_LESSEEBANK_TAB nvarchar(330),
	LESSEECHK_LESSEEYEAR_TAB nvarchar(330),
	LESSEECHK_LESSEEMONTH_TAB nvarchar(330),
	PRIMARY KEY (ID),
	CustomerID int FOREIGN KEY REFERENCES tblCustomer(ID),
	LenderID int FOREIGN KEY REFERENCES tblLender(ID)
);
CREATE TABLE tblUser (
    Id uniqueidentifier NOT NULL,
    FullName varchar(255) NOT NULL,
    Password nvarchar(max),
	Email nvarchar(200),
	Phone nvarchar(50),
	Role varchar(40),
    PRIMARY KEY (ID)
);
--INSERT INTO tblUser  VALUES (NEWID(),'Administrator','Y1ZqR2VwWkhHTTMyb1pKMjRPOUhVVlZPN05QdFZEczV6Tlg1OGt4bVVHWT0','admin@yopmail.com','56896652','Admin')

--alter table tblLesseDetail add LeaseRate decimal(18,4)
--alter table tblLesseDetail add LicensePlate nvarchar(150)

