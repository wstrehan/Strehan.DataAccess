using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionalTestDatabaseIntegration
{
    /// <summary>
    /// This is a test model object that will match with both input parameters in stored procedures as well as result columns
    /// </summary>
    public class Test
    {
        public DateTime TestDate { get; set; }
        public string TestString { get; set; }
        public int TestInt { get; set; }
    }
}
