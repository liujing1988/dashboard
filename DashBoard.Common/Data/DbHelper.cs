using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DashBoard.Common.Data
{
    /// <summary>
    /// SQL Server helper
    /// </summary>
    public class DbHelper : IDisposable
    {
        #region Properties

        private DbConnection _dbConnection;
        private DbTransaction _dbTransaction;
        private DbCommand _dbCommand;
        private CommandType _commandType = CommandType.Text;

        private string _connectionString;
        private string _providerName;

        /// <summary>
        /// Database connection
        /// </summary>
        internal DbConnection DbConnection
        {
            get
            {
                return _dbConnection;
            }
            set
            {
                _dbConnection = value;
            }
        }

        /// <summary>
        /// Database transaction
        /// </summary>
        internal DbTransaction DbTransaction
        {
            get
            {
                return _dbTransaction;
            }
            set
            {
                _dbTransaction = value;
            }
        }

        /// <summary>
        /// Database command
        /// </summary>
        internal DbCommand DbCommand
        {
            get
            {
                if (_dbConnection == null)
                {
                    Open(_connectionString, _providerName);
                }
                if (_dbCommand == null)
                {
                    _dbCommand = DbConnection.CreateCommand();
                    _dbCommand.CommandType = _commandType;
                }
                return _dbCommand;
            }
            set
            {
                _dbCommand = value;
            }
        }

        #endregion

        /// <summary>
        /// Create an instance
        /// </summary>
        public DbHelper()
        {

        }

        /// <summary>
        /// Create an instance and open a database connection
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="providerName">Provider name</param>
        public DbHelper(string connectionString, string providerName)
        {
            Open(connectionString, providerName);
        }

        /// <summary>
        /// Create an instance and open a database connection
        /// </summary>
        /// <param name="settings">Connection string settings</param>
        public DbHelper(ConnectionStringSettings settings)
        {
            Open(settings.ConnectionString, settings.ProviderName);
        }

        /// <summary>
        /// Open a database connection
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="providerName">Provider name</param>
        public void Open(string connectionString, string providerName)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = connectionString;
                _providerName = providerName;
            }
            try
            {
                if (DbConnection == null)
                {
                    DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory(providerName);
                    DbConnection = dbProviderFactory.CreateConnection();
                    DbConnection.ConnectionString = connectionString;
                }
                if (DbConnection.State == ConnectionState.Closed)
                {
                    DbConnection.Open();
                }
            }
            catch (DbException ex)
            {
                throw new Exception("Failed to open the database connection: " + ex.Message);
            }
        }

        /// <summary>
        /// Open a database connection
        /// </summary>
        /// <param name="settings">Connection string settings</param>
        public void Open(ConnectionStringSettings settings)
        {
            Open(settings.ConnectionString, settings.ProviderName);
        }

        /// <summary>
        /// Close database connection
        /// </summary>
        public void Close()
        {
            if (DbConnection != null)
            {
                DbConnection.Close();
                DbConnection.Dispose();
                DbConnection = null;
            }
        }

        /// <summary>
        /// Set command type
        /// </summary>
        public void SetCommandType(CommandType commandType)
        {
            _commandType = commandType;
        }

        /// <summary>
        /// Execute query and return rows affected
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="commandType">Command type</param>
        /// <returns>Rows affected</returns>
        public int ExecuteNonQuery(string commandText, List<DbParameter> parameters, CommandType commandType)
        {
            int result = 0;
            try
            {
                using (var dbCommand = FillCommand(commandText, parameters, commandType))
                {
                    result = dbCommand.ExecuteNonQuery();
                }
            }
            catch (DbException ex)
            {
                throw new Exception("Failed at ExecuteNonQuery: " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Execute query and return rows affected
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>Rows affected</returns>
        public int ExecuteNonQuery(string commandText, List<DbParameter> parameters = null)
        {
            return ExecuteNonQuery(commandText, parameters, _commandType);
        }

        /// <summary>
        /// Execute query and return the first value of the first row
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="commandType">Command type</param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, List<DbParameter> parameters, CommandType commandType)
        {
            object result = null;
            try
            {
                using (var dbCommand = FillCommand(commandText, parameters, commandType))
                {
                    result = dbCommand.ExecuteScalar();
                }
            }
            catch (DbException ex)
            {
                throw new Exception("Failed at ExecuteScalar: " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Execute query and return the first value of the first row
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, List<DbParameter> parameters = null)
        {
            return ExecuteScalar(commandText, parameters, _commandType);
        }

        /// <summary>
        /// Execute query and return data reader
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="commandType">Command type</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText, List<DbParameter> parameters, CommandType commandType)
        {
            try
            {
                using (var dbCommand = FillCommand(commandText, parameters, commandType))
                {
                    return dbCommand.ExecuteReader();
                }
            }
            catch (DbException ex)
            {
                throw new Exception("Failed at ExecuteReader: " + ex.Message);
            }
        }

        /// <summary>
        /// Execute query and return data reader
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string commandText, List<DbParameter> parameters = null)
        {
            return ExecuteReader(commandText, parameters, _commandType);
        }

        /// <summary>
        /// Execute query and return bool
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="commandType">Command type</param>
        /// <returns>true or false</returns>
        public bool Execute(string commandText, List<DbParameter> parameters, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, parameters, commandType) > 0 ? true : false;
        }

        /// <summary>
        /// Execute query and return bool
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>true or false</returns>
        public bool Execute(string commandText, List<DbParameter> parameters = null)
        {
            return Execute(commandText, parameters, _commandType);
        }

        /// <summary>
        /// Get the data table
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="commandType">Command type</param>
        /// <returns></returns>
        public DataTable GetDataTable(string commandText, List<DbParameter> parameters, CommandType commandType)
        {
            DataTable dt = new DataTable();
            using (DbDataReader dataReader = ExecuteReader(commandText, parameters, commandType))
            {
                dt.Load(dataReader);
            }

            return dt;
        }

        /// <summary>
        /// Get the data table
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns></returns>
        public DataTable GetDataTable(string commandText, List<DbParameter> parameters = null)
        {
            return GetDataTable(commandText, parameters, _commandType);
        }

        /// <summary>
        /// Get the first row
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="commandType">Command type</param>
        /// <returns></returns>
        public DataRow GetDataRow(string commandText, List<DbParameter> parameters, CommandType commandType)
        {
            DataRow dr = null;
            try
            {
                using (DbDataReader dataReader = ExecuteReader(commandText, parameters, commandType))
                {
                    if (dataReader.Read())
                    {
                        DataTable dt = new DataTable();
                        dr = dt.NewRow();
                        for (int i = 0; i < dataReader.FieldCount; i++)
                        {
                            dt.Columns.Add(dataReader.GetName(i));
                            dr[dataReader.GetName(i)] = dataReader.GetValue(i);
                        }
                    }
                }
            }
            catch (DbException ex)
            {
                throw new Exception("Failed at GetRow: " + ex.Message);
            }

            return dr;
        }

        /// <summary>
        /// Get the first row
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns></returns>
        public DataRow GetDataRow(string commandText, List<DbParameter> parameters = null)
        {
            return GetDataRow(commandText, parameters, _commandType);
        }

        /// <summary>
        /// Begin transaction
        /// </summary>
        /// <returns></returns>
        public void BeginTransaction()
        {
            DbConnection.BeginTransaction();
        }

        /// <summary>
        /// Commit transaction
        /// </summary>
        public void Commit()
        {
            if (DbTransaction != null)
            {
                DbTransaction.Commit();
                DbTransaction.Dispose();
                DbTransaction = null;
            }
        }

        /// <summary>
        /// Rollback transaction
        /// </summary>
        public void Rollback()
        {
            if (DbTransaction != null)
            {
                DbTransaction.Rollback();
                DbTransaction.Dispose();
                DbTransaction = null;
            }
        }

        /// <summary>
        /// Create a command parameter
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="value">Command value</param>
        /// <returns></returns>
        public DbParameter NewParameter(string name, object value)
        {
            DbParameter param = DbCommand.CreateParameter();
            param.ParameterName = "@" + name;
            param.Value = value;
            return param;
        }

        /// <summary>
        /// Create a command parameter
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="value">Command value</param>
        /// <param name="dbType">Data type</param>
        /// <returns></returns>
        public DbParameter NewParameter(string name, object value, DbType dbType)
        {
            DbParameter param = DbCommand.CreateParameter();
            param.DbType = dbType;
            param.ParameterName = "@" + name;
            param.Value = value;
            if (dbType == DbType.DateTime)
            {
                DateTime minDate = new DateTime(1753, 1, 1);
                if (DataConvert.ToDateTime(value) < minDate)
                {
                    value = minDate;
                }
                param.Value = value;
            }
            return param;
        }

        /// <summary>
        /// Dispose command
        /// </summary>
        private void DisposeCommand()
        {
            if (DbCommand != null)
            {
                DbCommand.Dispose();
                DbCommand = null;
            }
        }

        /// <summary>
        /// Set command parameters
        /// </summary>
        /// <param name="commandText">Query text or stored procedure name</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="commandType">Command type</param>
        private DbCommand FillCommand(string commandText, List<DbParameter> parameters, CommandType commandType)
        {
            DbCommand dbCommand = DbConnection.CreateCommand();
            dbCommand.CommandText = commandText;
            dbCommand.CommandType = commandType;
            dbCommand.Parameters.Clear();
            if (parameters != null)
            {
                dbCommand.Parameters.AddRange(parameters.ToArray());
            }
            return dbCommand;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            DisposeCommand();
            this.Close();
        }
    }
}
