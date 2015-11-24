using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common.Data
{
    /// <summary>
    /// Safe data type convertion
    /// </summary>
    public class DataConvert
    {
        /// <summary>
        /// Convert object to boolean
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns></returns>
        public static bool ToBoolean(object value)
        {
            bool result = false;
            if (value != null)
            {
                string str = value.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    bool.TryParse(str, out result);
                }
            }
            return result;
        }

        /// <summary>
        /// Convert object to datetime
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value)
        {
            DateTime result = DateTime.MinValue;
            if (value != null)
            {
                string str = value.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    DateTime.TryParse(str, out result);
                }
            }
            return result;
        }

        /// <summary>
        /// Convert object to UTC time
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns></returns>
        public static DateTime ToUTCTime(object value)
        {
            return ToDateTime(value).ToUniversalTime();
        }

        /// <summary>
        /// Convert object to local time
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns></returns>
        public static DateTime ToLocalTime(object value)
        {
            return ToDateTime(value).ToLocalTime();
        }

        /// <summary>
        /// Convert object to short integer
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns></returns>
        public static short ToInt16(object value)
        {
            short result = 0;
            if (value != null)
            {
                string str = value.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    short.TryParse(str, out result);
                }
            }
            return result;
        }

        /// <summary>
        /// Convert object to integer
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns></returns>
        public static int ToInt32(object value)
        {
            int result = 0;
            if (value != null)
            {
                string str = value.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    int.TryParse(str, out result);
                }
            }
            return result;
        }

        /// <summary>
        /// Convert object to long integer
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns></returns>
        public static long ToInt64(object value)
        {
            long result = 0;
            if (value != null)
            {
                string str = value.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    long.TryParse(str, out result);
                }
            }
            return result;
        }

        /// <summary>
        /// Convert object to decimal
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns></returns>
        public static decimal ToDecimal(object value)
        {
            decimal result = 0;
            if (value != null)
            {
                string str = value.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    decimal.TryParse(str, out result);
                }
            }
            return result;
        }

        /// <summary>
        /// Convert object to double
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns></returns>
        public static double ToDouble(object value)
        {
            double result = 0;
            if (value != null)
            {
                string str = value.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    double.TryParse(str, out result);
                }
            }
            return result;
        }

        /// <summary>
        /// Convert object to string or empty
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <returns></returns>
        public static string ToString(object value)
        {
            return (value != null) ? value.ToString() : string.Empty;
        }
    }
}
