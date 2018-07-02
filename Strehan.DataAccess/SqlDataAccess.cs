/*
 Copyright (C) 2018 William Strehan
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Strehan.DataAccess
{
    /// <summary>
    /// Generic Data Access Class for CRUD operations in a database.
    /// </summary>
    public class SqlDataAccess : IDataAccess
    {
        readonly IContext _contextMapping;
        readonly string _connectionString;

        private const string cConnectionStringKey = "ConnectionString";

        /// <summary>
        /// Creates a data access object
        /// </summary>
        /// <param name="contextMapping">Object used to obtain stored procedure names</param>
        /// <param name="connectionString"></param>
        public SqlDataAccess(IContext contextMapping, string connectionString)
        {
            _contextMapping = contextMapping;
            _connectionString = connectionString;
        }

        /// <summary>
        /// Uses a SqlDataReader to read in all the fields returned in one row.  
        /// Fields names of object of type T must match up with fiels returned by the stored procedure.
        /// </summary>
        /// <remarks>
        /// Each property in the Type T must correspond with a field retuned in the stored procedure passed into the method.
        /// </remarks>
        /// <typeparam name="T">Type of object being read back from the database</typeparam>
        /// <param name="rdr"></param>
        /// <returns></returns>
        protected T ReadInObject<T>(SqlDataReader rdr)
        {
            //Create instane of object that will be populated
            T obj = (T)Activator.CreateInstance(typeof(T), new object[] { });

            //Use reflection to get all properties in object and read in each corresponding field from the stored procedure call
            foreach (var prop in typeof(T).GetProperties())
            {
                //Read field from SQLDataReader
                obj.GetType().GetProperty(prop.Name).SetValue(obj, rdr[prop.Name]);
            }

            return obj;
        }


        /// <summary>
        /// Reads in all the rows availabe in the SqlDataReader
        /// </summary>
        /// <remarks>
        /// Each property in the Type T must correspond with a field retuned in the stored procedure passed into the method.
        /// </remarks>
        /// <typeparam name="T">Type of object in list returned</typeparam>
        /// <param name="rdr"></param>
        /// <returns></returns>
        protected List<T> ReadInList<T>(SqlDataReader rdr)
        {
            List<T> list = new List<T>();

            while (rdr.Read())
            {
                //Read in entire row from SQLDataReader
                Object obj = ReadInObject<T>(rdr);

                //Add row to list that will be returned
                list.Add((T)obj);
            }
            return list;
        }




        /// <summary>
        /// Reads in all the rows availabe in the SqlDataReader
        /// </summary>
        /// <remarks>
        /// Each property in the Type T must correspond with a field retuned in the stored procedure passed into the method.
        /// </remarks>
        /// <typeparam name="TID"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public T GetObjectById<TID, T>(TID id, object context = null)
        {
            string sProcName = _contextMapping.GetMappingItem<T>(context) as string;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                //Setup execution of stored procedure
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.Add(new SqlParameter("@Id", id));

                //Call Stored procedure and read byack row
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.Read())
                    {
                        return ReadInObject<T>(sqlDataReader);
                    }
                    else
                    {
                        //This really should never happened if application is designed properly
                        throw new RecordNotFoundException(id.ToString(), sProcName, "Record not found");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputParameterObj"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public T GetObjectWithParameters<T>(object inputParameterObj, object context = null)
        {
            string sProcName = _contextMapping.GetMappingItem<T>(context) as string;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                //Setup execution of stored procedure
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Create an input parameter object for each property in inputParameterObj object
                foreach (var prop in inputParameterObj.GetType().GetProperties())
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@" + prop.Name, inputParameterObj.GetType().GetProperty(prop.Name).GetValue(inputParameterObj)));
                }

               
                //Call Stored procedure and read byack row
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.Read())
                    {
                        return ReadInObject<T>(sqlDataReader);
                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (var prop in inputParameterObj.GetType().GetProperties())
                        {
                            sb.Append(prop.Name + ":" + inputParameterObj.GetType().GetProperty(prop.Name).GetValue(inputParameterObj) + ",");
                        }
                        sb.Length -= 1; //Get rid of last comma

                        //This really should never happened if application is designed properly
                        throw new RecordNotFoundException(sb.ToString(), sProcName, "Record not found");
                    }
                }
            }
        }


        /// <summary>
        /// Get all rows returned by the stored procedure.
        /// </summary>
        /// <remarks>
        /// Each property in the Type T must correspond with a field retuned in the stored procedure passed into the method.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<T> GetList<T>(object context = null)
        {
            string sProcName = _contextMapping.GetMappingItem<T>(context) as string;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                //Setup Stored Procedure and Read Back Row
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    return ReadInList<T>(sqlDataReader);
                }
            }
        }

        /// <summary>
        /// Get all rows returned by the stored procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputParameterObj"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<T> GetListWithParameters<T>(object inputParameterObj, object context = null)
        {
            string sProcName = _contextMapping.GetMappingItem<T>(context) as string;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                //Setup Stored Procedure and Read Back Row
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Create an input parameter object for each property in parameter object
                foreach (var prop in inputParameterObj.GetType().GetProperties())
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@" + prop.Name, inputParameterObj.GetType().GetProperty(prop.Name).GetValue(inputParameterObj)));
                }

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    return ReadInList<T>(sqlDataReader);
                }
            }
        }


        /// <summary>
        /// Inserts a row into the database
        /// </summary>
        /// <remarks>
        /// Each property in Type T must have a corresponding input paramater in the stored procedure.
        /// Stored Procedure must have the Id as an output parameter
        /// </remarks>
        /// <typeparam name="TID"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="insertObj"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public TID Insert<TID, T>(T insertObj, object context = null)
        {
            string sProcName = _contextMapping.GetMappingItem<T>(context) as string;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Create an input parameter object for each property in type T
                foreach (var prop in typeof(T).GetProperties())
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@" + prop.Name, typeof(T).GetProperty(prop.Name).GetValue(insertObj)));
                }

                //Setup output parameter
                SqlDbType dbType = SqlDbType.Int;
                if (typeof(TID) == typeof(short)) dbType = SqlDbType.SmallInt;
                else if (typeof(TID) == typeof(long)) dbType = SqlDbType.BigInt;
                else if (typeof(TID) == typeof(byte)) dbType = SqlDbType.TinyInt;

                SqlParameter outputParameter = new SqlParameter("@Id", dbType)
                {
                    Direction = ParameterDirection.Output
                };
                sqlCommand.Parameters.Add(outputParameter);

                sqlCommand.ExecuteNonQuery();

                TID id = (TID)sqlCommand.Parameters["@ID"].Value;
                return id;
            }
        }

        /// <summary>
        /// Updates a row into the database
        /// </summary>
        /// <remarks>
        /// Each property in Type T must have a corresponding input paramater in the stored procedure
        /// </remarks>
        /// <typeparam name="TID"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="updateObj"></param>
        /// <param name="context"></param>
        public void UpdateById<TID, T>(TID id, T updateObj, object context = null)
        {
            string sProcName = _contextMapping.GetMappingItem<T>(context) as string;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var prop in typeof(T).GetProperties())
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@" + prop.Name, typeof(T).GetProperty(prop.Name).GetValue(updateObj)));
                }

                sqlCommand.Parameters.Add(new SqlParameter("@Id", id));
                sqlCommand.ExecuteNonQuery();
            }
        }



        /// <summary>
        /// Calls a stored procedure that only takes an Id as the parameter
        /// </summary>
        /// <typeparam name="TID"></typeparam>
        /// <param name="id"></param>
        /// <param name="context"></param>
        public void IdCall<TID>(TID id, object context = null)
        {
            string sProcName = _contextMapping.GetMappingItem(context) as string;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.Add(new SqlParameter("@Id", id));
                sqlCommand.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Calls a stored procedure that only takes an Id as the parameter
        /// </summary>
        /// <typeparam name="TID"></typeparam>
        /// <typeparam name="T">Not used in the stored procedure call but only used to figure out what stored procedure name to use</typeparam>
        /// <param name="id"></param>
        /// <param name="context"></param>
        public void IdCall<TID, T>(TID id, object context = null)
        {
            string sProcName = _contextMapping.GetMappingItem<T>(context) as string;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.Add(new SqlParameter("@Id", id));
                sqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a stored procedure that does not return any data but has parameters
        /// </summary>
        /// <param name="inputParameterObj"></param>
        /// <param name="context"></param>
        public void ParameterCall(object inputParameterObj, object context)
        {
            string sProcName = _contextMapping.GetMappingItem(context) as string;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Create an input parameter object for each property in parameter object
                foreach (var prop in inputParameterObj.GetType().GetProperties())
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@" + prop.Name, inputParameterObj.GetType().GetProperty(prop.Name).GetValue(inputParameterObj)));
                }

                sqlCommand.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Executes a stored procedure that does not return any data and takes no parmeters
        /// </summary>
        /// <param name="context"></param>
        public void NoParameterCall(object context)
        {
            string sProcName = _contextMapping.GetMappingItem(context) as string;

            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}