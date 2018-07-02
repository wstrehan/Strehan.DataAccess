using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strehan.DataAccess;
using System.Collections.Generic;

namespace FunctionalTestDatabaseIntegration
{



    /// <summary>
    /// Functional tests to make sure that the SqlDataAccessBasic class can execute stored procedures on SQL Server
    /// and read back the results correctly.
    /// </summary>
    /// <remarks>
    /// These tests are not real unit tests because the tests are connecting to an outside resource (SQL Server).   
    /// These tests are functional tests because the whole point of this DataAccess library is to call stored
    /// procedures in a SQL Server Datbase.  The UnitTesting framework built in to Visual Studio is used
    /// to execute these functional tests for convenience.
    /// </remarks>
    [TestClass]
    public class FunctionalTestSqlDataAccessBasic
    {
        const string cConnectionString = "Data Source=(local);Initial Catalog=DataAccessTest;User Id=DataAccessTest_user;Password='abcdeft'";

        [TestMethod]
        public void TestGetObjectById()
        {
            SqlDataAccessBasic<int, Test> dataAccess = new SqlDataAccessBasic<int, Test>(cConnectionString);

            Test test = dataAccess.GetById(1, "Test.GetObjectByIdTest");

            Assert.IsNotNull(test);

        }

        [TestMethod]
        public void TestGetList()
        {
            SqlDataAccessBasic<int, Test> dataAccess = new SqlDataAccessBasic<int, Test>(cConnectionString);

            List<Test> test = dataAccess.GetList("Test.GetListTest");
            Assert.IsNotNull(test);
            Assert.AreNotEqual(0, test.Count); //Fails if no records returned
            Assert.AreNotEqual(1, test.Count); //Fails if only 1 record returned

        }


        [TestMethod]
        public void TestInsert()
        {
            SqlDataAccessBasic<int, Test> dataAccess = new SqlDataAccessBasic<int, Test>(cConnectionString);

            Test insertData = new Test
            {
                TestDate = System.DateTime.Now,
                TestInt = 1,
                TestString = "String"
            };

            int id = dataAccess.Insert(insertData, "Test.InsertTest");

        }

        [TestMethod]
        public void TestUpdateById()
        {
            SqlDataAccessBasic<int, Test> dataAccess = new SqlDataAccessBasic<int, Test>(cConnectionString);

            Test updatetData = new Test
            {
                TestDate = System.DateTime.Now,
                TestInt = 1,
                TestString = "String"
            };

            dataAccess.Update(1, updatetData, "Test.UpdateByIdTest");

        }

        [TestMethod]
        public void TestIdCall()
        {
            SqlDataAccessBasic<int, Test> dataAccess = new SqlDataAccessBasic<int, Test>(cConnectionString);

            dataAccess.IdCall(1, "Test.IdCallTest");
        }


        [TestMethod]
        public void TestNoParameterCall()
        {
            SqlDataAccessBasic<int, Test> dataAccess = new SqlDataAccessBasic<int, Test>(cConnectionString);

            dataAccess.NoParameterCall("Test.NoParameterCallTest");
        }


    }
}
