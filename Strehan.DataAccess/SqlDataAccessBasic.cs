/*
 Copyright (C) 2018 William Strehan
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Strehan.DataAccess
{

    /// <summary>
    /// Generic Data Access Class for CRUD operations in a database.
    /// </summary>
    /// <typeparam name="TID">Type used by the row Id</typeparam>
    /// <typeparam name="T">Datatype that contains fields used to line up database calls with objects passed in</typeparam>
    public class SqlDataAccessBasic<TID, T>
    {
        string _connectionString;

        public SqlDataAccessBasic(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Uses a SqlDataReader to read in all the fields returned in one row.  
        /// Fields names of object of type T must match up with fiels returned by the stored procedure.
        /// </summary>
        /// <remarks>
        /// Each property in the Type T must correspond with a field retuned in the stored procedure passed into the method.
        /// </remarks>
        protected T ReadInObject(SqlDataReader rdr)
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
        protected List<T> ReadInList(SqlDataReader rdr)
        {
            List<T> list = new List<T>();

            while (rdr.Read())
            {
                //Read in entire row from SQLDataReader
                Object obj = ReadInObject(rdr);

                //Add row to list that will be returned
                list.Add((T)obj);
            }
            return list;
        }


        /// <summary>
        /// Read in a single row from the database that matches up with the Primary Key field passed in
        /// </summary>
        /// <remarks>
        /// Each property in the Type T must correspond with a field retuned in the stored procedure passed into the method.
        /// Throws an exception if the row doesn't exist in the database.
        /// </remarks>
        public T GetById(TID id, string sProcName)
        {
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
                        return ReadInObject(sqlDataReader);
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
        /// Get all rows returned by the stored procedure.
        /// </summary>
        /// <remarks>
        /// Each property in the Type T must correspond with a field retuned in the stored procedure passed into the method.
        /// </remarks>
        public List<T> GetList(string sProcName)
        {
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
                    return ReadInList(sqlDataReader);
                }
            }
        }


        /// <summary>
        /// Inserts a row into the database
        /// </summary>
        /// <remarks>
        /// Each property in Type T must have a corresponding input paramater in the stored procedure
        /// </remarks>
        public TID Insert(Object obj, string sProcName)
        {
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
                    sqlCommand.Parameters.Add(new SqlParameter("@" + prop.Name, typeof(T).GetProperty(prop.Name).GetValue(obj)));
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
        public void Update(TID id, Object obj, string sProcName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                foreach (var prop in typeof(T).GetProperties())
                {
                    sqlCommand.Parameters.Add(new SqlParameter("@" + prop.Name, typeof(T).GetProperty(prop.Name).GetValue(obj)));
                }

                sqlCommand.Parameters.Add(new SqlParameter("@Id", id));
                sqlCommand.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Executes a stored procedure that does not return any data.
        /// </summary>
        /// <remarks>
        /// The stored procedure must have an input Parameter named @Id 
        /// </remarks>
        public void IdCall(TID id, string sProcName)
        {
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
        /// Executes a stored procedure that does not return any data and takes no parmeters
        /// </summary>
        public void NoParameterCall(string sProcName)
        {
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