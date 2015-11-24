using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DashBoard.Common.Data
{
    /// <summary>
    /// Data row reader class
    /// </summary>
    public class DataRowReader
    {
        private DataRow _dataRow;

        /// <summary>
        /// Log error if column not exists
        /// </summary>
        public bool ThrowException { get; set; }

        /// <summary>
        /// Create instance
        /// </summary>
        /// <param name="dataRow">Data row to read</param>
        /// <param name="throwException">Throw exception if column not exists</param>
        public DataRowReader(DataRow dataRow, bool throwException = false)
        {
            _dataRow = dataRow;
            ThrowException = throwException;
        }

        /// <summary>
        /// Get object from data row
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        public object GetObject(string columnName)
        {
            object result = null;
            if (_dataRow != null)
            {
                if (_dataRow.Table.Columns.Contains(columnName))
                {
                    result = _dataRow[columnName];
                }
                else if (ThrowException)
                {
                    throw new DataException(string.Format("Column {0} not exists", columnName));
                }
            }
            return result;
        }

        /// <summary>
        /// Get object from data row
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns></returns>
        public object GetObject(int columnIndex)
        {
            object result = null;
            if (_dataRow != null)
            {
                if (_dataRow.Table.Columns.Count > columnIndex && columnIndex >= 0)
                {
                    result = _dataRow[columnIndex];
                }
                else if (ThrowException)
                {
                    throw new DataException(string.Format("Column index {0} out of range", columnIndex));
                }
            }
            return result;
        }

        /// <summary>
        /// Get string from data row
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        public string GetString(string columnName)
        {
            string result = string.Empty;
            object value = GetObject(columnName);
            if (value != null)
            {
                result = value.ToString();
            }
            return result;
        }

        /// <summary>
        /// Get string from data row
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns></returns>
        public string GetString(int columnIndex)
        {
            string result = string.Empty;
            object value = GetObject(columnIndex);
            if (value != null)
            {
                result = value.ToString();
            }
            return result;
        }

        /// <summary>
        /// Get short integer from data row
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        public short GetInt16(string columnName)
        {
            short result = 0;
            object value = GetObject(columnName);
            if (value != null)
            {
                result = DataConvert.ToInt16(value);
            }
            return result;
        }

        /// <summary>
        /// Get short integer from data row
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns></returns>
        public short GetInt16(int columnIndex)
        {
            short result = 0;
            object value = GetObject(columnIndex);
            if (value != null)
            {
                result = DataConvert.ToInt16(value);
            }
            return result;
        }

        /// <summary>
        /// Get integer from data row
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        public int GetInt32(string columnName)
        {
            int result = 0;
            object value = GetObject(columnName);
            if (value != null)
            {
                result = DataConvert.ToInt32(value);
            }
            return result;
        }

        /// <summary>
        /// Get integer from data row
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns></returns>
        public int GetInt32(int columnIndex)
        {
            int result = 0;
            object value = GetObject(columnIndex);
            if (value != null)
            {
                result = DataConvert.ToInt32(value);
            }
            return result;
        }

        /// <summary>
        /// Get long integer from data row
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        public long GetInt64(DataRow dataRow, string columnName)
        {
            long result = 0;
            object value = GetObject(columnName);
            if (value != null)
            {
                result = DataConvert.ToInt64(value);
            }
            return result;
        }

        /// <summary>
        /// Get long integer from data row
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns></returns>
        public long GetInt64(int columnIndex)
        {
            long result = 0;
            object value = GetObject(columnIndex);
            if (value != null)
            {
                result = DataConvert.ToInt64(value);
            }
            return result;
        }

        /// <summary>
        /// Get decimal from data row
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        public decimal GetDecimal(string columnName)
        {
            decimal result = 0;
            object value = GetObject(columnName);
            if (value != null)
            {
                result = DataConvert.ToDecimal(value);
            }
            return result;
        }

        /// <summary>
        /// Get decimal from data row
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns></returns>
        public decimal GetDecimal(int columnIndex)
        {
            decimal result = 0;
            object value = GetObject(columnIndex);
            if (value != null)
            {
                result = DataConvert.ToDecimal(value);
            }
            return result;
        }

        /// <summary>
        /// Get double from data row
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        public double GetDouble(string columnName)
        {
            double result = 0;
            object value = GetObject(columnName);
            if (value != null)
            {
                result = DataConvert.ToDouble(value);
            }
            return result;
        }

        /// <summary>
        /// Get double from data row
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns></returns>
        public double GetDouble(int columnIndex)
        {
            double result = 0;
            object value = GetObject(columnIndex);
            if (value != null)
            {
                result = DataConvert.ToDouble(value);
            }
            return result;
        }

        /// <summary>
        /// Get bool from data row
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        public bool GetBoolean(string columnName)
        {
            bool result = false;
            object value = GetObject(columnName);
            if (value != null)
            {
                result = DataConvert.ToBoolean(value);
            }
            return result;
        }

        /// <summary>
        /// Get bool from data row
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns></returns>
        public bool GetBoolean(int columnIndex)
        {
            bool result = false;
            object value = GetObject(columnIndex);
            if (value != null)
            {
                result = DataConvert.ToBoolean(value);
            }
            return result;
        }

        /// <summary>
        /// Get datetime from data row
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        public DateTime GetDateTime(string columnName)
        {
            DateTime result = DateTime.MinValue;
            object value = GetObject(columnName);
            if (value != null)
            {
                result = DataConvert.ToDateTime(value);
            }
            return result;
        }

        /// <summary>
        /// Get datetime from data row
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns></returns>
        public DateTime GetDateTime(DataRow dataRow, int columnIndex)
        {
            DateTime result = DateTime.MinValue;
            object value = GetObject(columnIndex);
            if (value != null)
            {
                result = DataConvert.ToDateTime(value);
            }
            return result;
        }
    }
}
