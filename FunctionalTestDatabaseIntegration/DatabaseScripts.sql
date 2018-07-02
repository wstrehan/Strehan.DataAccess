CREATE DATABASE [DataAccessTest]

Go

use [DataAccessTest]

GO

CREATE LOGIN DataAccessTest_user
    WITH PASSWORD = 'abcdeft';

GO


CREATE USER DataAccessTest_user FOR login DataAccessTest_user;
GO 

EXEC sp_executesql N'CREATE SCHEMA Test;';

Go

Grant Execute on Schema::Test to DataAccessTest_user;

Go


/****** Object:  StoredProcedure [Test].[GetListTest]    Script Date: 6/27/2018 10:49:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Returns two rows consisting of a Date, a string, and an integer
-- =============================================
CREATE PROCEDURE [Test].[GetListTest]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	CREATE TABLE dbo.#TestTable
	   (   
	   TestDate DateTime NOT NULL, 
	   TestString NVARCHAR(20) NOT NULL, 
	   TestInt int NOT NULL
	   )

	Insert Into #TestTable values (GETDATE(), 'String', 0);
	Insert Into #TestTable values (GETDATE(), 'String', 0);

	SELECT * from #TestTable
END
GO
/****** Object:  StoredProcedure [Test].[GetListWithParametersTest]    Script Date: 6/27/2018 10:49:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Returns two rows consisting of a Date, a string, and an integer
-- =============================================
CREATE PROCEDURE [Test].[GetListWithParametersTest]
	-- Add the parameters for the stored procedure here
	@TestDate as date,
	@TestString as NVARCHAR(50),
	@TestInt as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	CREATE TABLE dbo.#TestTable
	   (   
	   TestDate DateTime NOT NULL, 
	   TestString NVARCHAR(20) NOT NULL, 
	   TestInt int NOT NULL
	   )

	Insert Into #TestTable values (GETDATE(), 'String', 0);
	Insert Into #TestTable values (GETDATE(), 'String', 0);

	SELECT * from #TestTable
END
GO
/****** Object:  StoredProcedure [Test].[GetObjectByIdTest]    Script Date: 6/27/2018 10:49:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Returns single row consisting of a Date, a string, and an integer
-- =============================================
CREATE PROCEDURE [Test].[GetObjectByIdTest]
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT GETDATE() as TestDate, CONVERT(NVARCHAR(20),@Id) as TestString, @Id as TestInt 
END
GO
/****** Object:  StoredProcedure [Test].[GetObjectWithParametersTest]    Script Date: 6/27/2018 10:49:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Returns single row consisting of a Date, a string, and an integer
-- =============================================
CREATE PROCEDURE [Test].[GetObjectWithParametersTest]
	-- Add the parameters for the stored procedure here
	@TestDate as date,
	@TestString as NVARCHAR(50),
	@TestInt as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @TestDate as TestDate, @TestString as TestString, @TestInt as TestInt 
END
GO
/****** Object:  StoredProcedure [Test].[IdCallTest]    Script Date: 6/27/2018 10:49:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Returns two rows consisting of a Date, a string, and an integer
-- =============================================
CREATE PROCEDURE [Test].[IdCallTest]
	-- Add the parameters for the stored procedure here
    @Id as int

AS
BEGIN
	
	Declare @Temp int
END
GO
/****** Object:  StoredProcedure [Test].[InsertTest]    Script Date: 6/27/2018 10:49:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Returns two rows consisting of a Date, a string, and an integer
-- =============================================
CREATE PROCEDURE [Test].[InsertTest]
	-- Add the parameters for the stored procedure here
	@TestDate as date,
	@TestString as NVARCHAR(50),
	@TestInt as int,
	@Id as int out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	set @Id = '1'
END
GO
/****** Object:  StoredProcedure [Test].[NoParameterCallTest]    Script Date: 6/27/2018 10:49:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Returns two rows consisting of a Date, a string, and an integer
-- =============================================
CREATE PROCEDURE [Test].[NoParameterCallTest]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	
	Declare @Temp int
END
GO
/****** Object:  StoredProcedure [Test].[ParameterCallTest]    Script Date: 6/27/2018 10:49:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Returns two rows consisting of a Date, a string, and an integer
-- =============================================
CREATE PROCEDURE [Test].[ParameterCallTest]
	-- Add the parameters for the stored procedure here
	@TestDate as date,
	@TestString as NVARCHAR(50),
	@TestInt as int
AS
BEGIN
	
	Declare @Temp int
END
GO
/****** Object:  StoredProcedure [Test].[UpdateByIdTest]    Script Date: 6/27/2018 10:49:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Returns two rows consisting of a Date, a string, and an integer
-- =============================================
CREATE PROCEDURE [Test].[UpdateByIdTest]
	-- Add the parameters for the stored procedure here
    @Id as int,
	@TestDate as date,
	@TestString as NVARCHAR(50),
	@TestInt as int

AS
BEGIN
	
	Declare @Temp int
END
GO
