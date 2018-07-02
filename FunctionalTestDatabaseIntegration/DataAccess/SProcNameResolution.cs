using System;
using System.Collections.Generic;
using System.Text;

namespace Strehan.DataAccess
{
    /// <summary>
    /// StoreageMappingSQL is injected into the DataAccess constructor and provides the stored procedure names that that DataAccess object needs
    /// </summary>
    public class SProcNameResolution : IContext
    {

        /// <summary>
        /// Obtains a stored procedure name used to call a stored procedure in SQL Server
        /// </summary>
        /// <typeparam name="T">The type of the model used for CRUD operations</typeparam>
        /// <param name="context">List, Insert, Update, Delete, or other operation</param>
        /// <returns></returns>
        public object GetMappingItem<T>(object context)
        {
            return GetMappingItem(context);
        }

        public object GetMappingItem(object context)
        {
            StoredProcedureTypes storageFunction = (StoredProcedureTypes)context;
            return "test." + storageFunction.ToString() + "Test";
        }

    }
}
