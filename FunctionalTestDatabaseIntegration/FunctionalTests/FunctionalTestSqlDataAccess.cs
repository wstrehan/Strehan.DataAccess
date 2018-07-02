using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strehan.DataAccess;
using System.Collections.Generic;

namespace FunctionalTestDatabaseIntegration
{
    /// <summary>
    /// Functional tests to make sure that the SqlDataAccess class can execute stored procedures on SQL Server
    /// and read back the results correctly.
    /// </summary>
    /// <remarks>
    /// These tests are not real unit tests because the tests are connecting to an outside resource (SQL Server).   
    /// These tests are functional tests because the whole point of this DataAccess library is to call stored
    /// procedures in a SQL Server Datbase.  The UnitTesting framework built in to Visual Studio is used
    /// to execute these functional tests for convenience.
    /// </remarks>
    [TestClass]
    public class FunctionalTestSqlDataAccess
    {
        const string cConnectionString = "Data Source=(local);Initial Catalog=DataAccessTest;User Id=DataAccessTest_user;Password='abcdeft'";

        [TestMethod]
        public void TestGetObjectById()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            Test test = dataAccess.GetObjectById<int, Test>(1, StoredProcedureTypes.GetObjectById);
            Assert.IsNotNull(test);

        }

        [TestMethod]
        [ExpectedException(typeof(System.IndexOutOfRangeException))]
        public void TestGetObjectByIdBadGenericType()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            //Purposely send in an ArrayList object which of course won't work since the properties of the object need to match up with the results
            System.Collections.ArrayList test = dataAccess.GetObjectById<int, System.Collections.ArrayList>(1, StoredProcedureTypes.GetObjectById);
        }

        [TestMethod]
        public void TestGetObjectWithParameters()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            Test inputParameters = new Test
            {
                TestDate = System.DateTime.Now,
                TestInt = 1,
                TestString = "String"
            };

            Test test = dataAccess.GetObjectWithParameters<Test>(inputParameters, StoredProcedureTypes.GetObjectWithParameters);
            Assert.IsNotNull(test);

        }

        [TestMethod]
        public void TestGetList()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            List<Test> test = dataAccess.GetList<Test>(StoredProcedureTypes.GetList);
            Assert.IsNotNull(test);
            Assert.AreNotEqual(0, test.Count); //Fails if no records returned
            Assert.AreNotEqual(1, test.Count); //Fails if only 1 record returned

        }


        [TestMethod]
        public void TestGetListWithParameters()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            Test inputParameters = new Test
            {
                TestDate = System.DateTime.Now,
                TestInt = 1,
                TestString = "String"
            };

            List<Test> test = dataAccess.GetListWithParameters<Test>(inputParameters, StoredProcedureTypes.GetListWithParameters);
            Assert.IsNotNull(test);
            Assert.AreNotEqual(0, test.Count); //Fails if no records returned
            Assert.AreNotEqual(1, test.Count); //Fails if only 1 record returned

        }


        [TestMethod]
        public void TestInsert()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            Test insertData = new Test
            {
                TestDate = System.DateTime.Now,
                TestInt = 1,
                TestString = "String"
            };

            int id = dataAccess.Insert<int,Test>(insertData, StoredProcedureTypes.Insert);

        }

        [TestMethod]
        public void TestUpdateById()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            Test updatetData = new Test
            {
                TestDate = System.DateTime.Now,
                TestInt = 1,
                TestString = "String"
            };

            dataAccess.UpdateById<int, Test>(1, updatetData, StoredProcedureTypes.Insert);

        }

        [TestMethod]
        public void TestIdCall()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            dataAccess.IdCall<int>(1, StoredProcedureTypes.IdCall);

        }


        [TestMethod]
        public void TestIdCall2()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            dataAccess.IdCall<int,Test>(1, StoredProcedureTypes.IdCall);

        }


        [TestMethod]
        public void TestParameterCall()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            Test inputParameters = new Test
            {
                TestDate = System.DateTime.Now,
                TestInt = 1,
                TestString = "String"
            };

            dataAccess.ParameterCall(inputParameters, StoredProcedureTypes.ParameterCall);
        }



        [TestMethod]
        public void TestNoParameterCall()
        {
            IDataAccess dataAccess = new SqlDataAccess(new SProcNameResolution(), cConnectionString);

            dataAccess.NoParameterCall(StoredProcedureTypes.NoParameterCall);
        }




    }
}
