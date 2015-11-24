using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace DashBoard.Common.Data
{
    /// <summary>
    /// Database factory
    /// </summary>
    public class DbFactory
    {
        private static Dictionary<string, DbHelper> _instanceDict;

        /// <summary>
        /// Get the instance
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static DbHelper GetInstance(ConnectionStringSettings settings)
        {
            if (_instanceDict == null)
            {
                _instanceDict = new Dictionary<string, DbHelper>();
            }
            if (_instanceDict.ContainsKey(settings.ConnectionString))
            {
                return _instanceDict[settings.ConnectionString];
            }
            DbHelper dbHelper = new DbHelper(settings);
            _instanceDict.Add(settings.ConnectionString, dbHelper);

            return dbHelper;
        }

        /// <summary>
        /// Create a database instance
        /// </summary>
        /// <param name="key">Connection string name</param>
        /// <returns></returns>
        public static DbHelper Create(string key)
        {
            DbHelper instance = GetInstance(ConfigurationManager.ConnectionStrings[key]);
            instance.SetCommandType(CommandType.StoredProcedure);

            return instance;
        }

        /// <summary>
        /// Create a database instance
        /// </summary>
        /// <returns></returns>
        public static DbHelper Create()
        {
            DbHelper instance = GetInstance(ConfigurationManager.ConnectionStrings["dashboard"]);
            instance.SetCommandType(CommandType.StoredProcedure);

            return instance;
        }
    }
}
