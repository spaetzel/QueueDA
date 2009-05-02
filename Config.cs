using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaetzel.QueueDA
{
    public static class Config
    {
        private static string _connectionString;
        private static string _tableName;
        private static TimeSpan _lockTime;

        internal static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        internal static string TableName
        {
            get
            {
                return _tableName;
            }
        }

        internal static TimeSpan LockTime
        {
            get
            {
                return _lockTime;
            }
        }

        public static void SetConfigurations(string connectionString, string tableName, TimeSpan lockTime)
        {
            _connectionString = connectionString;
            _tableName = tableName;
            _lockTime = lockTime;

        }

    }
}
