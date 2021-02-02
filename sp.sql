--1
CREATE PROC sp_GetCompany
    @CompanyId int
AS 
BEGIN 
    SELECT *
    FROM tbl_Companies
    WHERE CompanyId = @CompanyId
END
GO

--2
CREATE PROC sp_GetALLCompany
AS 
BEGIN 
    SELECT *
    FROM tbl_Companies
END
GO

--3
CREATE PROC sp_AddCompany
    @CompanyId int OUTPUT,
    @Name varchar(MAX),
    @Address  varchar(MAX),
    @City varchar(MAX),
    @State varchar(MAX),
    @PostalCode varchar(MAX)
AS
BEGIN 
    INSERT INTO tbl_Companies ([Name], [Address], City, [State], PostalCode) 
    VALUES(@Name, @Address, @City, @State, @PostalCode);
    SELECT @CompanyId = SCOPE_IDENTITY();
END
GO

--4
CREATE PROC sp_UpdateCompany
    @CompanyId int,
    @Name varchar(MAX),
    @Address  varchar(MAX),
    @City varchar(MAX),
    @State varchar(MAX),
    @PostalCode varchar(MAX)
AS
BEGIN 
    UPDATE tbl_Companies  
    SET 
        [Name] = @Name, 
        [Address] = @Address,
        City=@City, 
        [State]=@State, 
        PostalCode=@PostalCode
    WHERE CompanyId=@CompanyId;
    SELECT @CompanyId = SCOPE_IDENTITY();
END
GO

--5
CREATE PROC sp_RemoveCompany
    @CompanyId int
AS 
BEGIN 
    DELETE
    FROM tbl_Companies
    WHERE CompanyId  = @CompanyId
END
GO