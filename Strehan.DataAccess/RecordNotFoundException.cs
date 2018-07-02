/*
 Copyright (C) 2018 William Strehan
*/

using System;

namespace Strehan.DataAccess
{
    /// <summary>
    /// Thrown when a record is not found by Id in a stored procedure call
    /// </summary>
    [Serializable]
    public class RecordNotFoundException : Exception
    {
        /// <summary>
        /// Id of the record not found
        /// </summary>
        public string Input { get; private set; }

        /// <summary>
        /// Name of stored procedure that failed to find record
        /// </summary>
        public string StoredProcedureName { get; private set; }

        /// <summary>
        /// Creates a new RecordNotFoundException with the Id, Stored Procedure Name, and a message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="message"></param>
        public RecordNotFoundException(string input, string storedProcedureName, string message) : base(message)
        {
            Input = input;
            StoredProcedureName = storedProcedureName;
        }

        /// <summary>
        /// Added the Id and stored procedure name in ToString()
        /// </summary>
        /// <returns>Verbose data for Logging</returns>
        public override string ToString()
        {
            return string.Format(base.ToString() + ", Input: {0}, SPROC: {1}, ", Input, StoredProcedureName);
        }
    }
}
