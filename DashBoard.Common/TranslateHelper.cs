using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Common
{
    public class TranslateHelper
    {
        /// <summary>
        /// 交易类型转换
        /// </summary>
        /// <param name="strategytype">交易类型代码</param>
        /// <returns>交易类型名称</returns>
        public static string ConvertStrategyType(string strategytype)
        {
            string result = null;

            switch (strategytype)
            {
                case "0":
                    result = "普通";
                    break;
                case "1":
                    result = "TWAP";
                    break;
                case "2":
                    result = "时序";
                    break;
                case "3":
                    result = "盘口";
                    break;
                case "4":
                    result = "ETF";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 实时数据json时间格式转换
        /// </summary>
        /// <param name="Minute">传入时间，格式为yyyyMMdd.HHmiss或者yyyyMMdd</param>
        /// <returns>传出时间，格式为yyyy-MM-dd</returns>
        public static string ConvertMinute(double Minute)
        {
            string result;
            string[] date = null;
            if (Minute.ToString().Contains('.'))
            {
                date = Minute.ToString().Split('.');

                while (date[1].Length < 6)
                {
                    date[1] = date[1] + "0";
                }
                string datetime = date[0].Substring(0, 4) + "-" + date[0].Substring(4, 2) +
                    "-" + date[0].Substring(6, 2) + " " + date[1].Substring(0, 2) + ":" +
                    date[1].Substring(2, 2) + ":" + date[1].Substring(4, 2);
                result = datetime;
            }
            else
            {
                string sdate = Minute.ToString().Substring(0, 4) + "-" + Minute.ToString().Substring(4, 2) +
                         "-" + Minute.ToString().Substring(6, 2);
                result = sdate;
            }

            return result;
        }

        /// <summary>
        /// string类型的时间转为int类型
        /// </summary>
        /// <param name="datetime">时间格式yyyy-MM-dd</param>
        /// <returns>整型，格式yyyyMMdd</returns>
        public static int ConvertDate(string datetime)
        {
            int date = -1;
            if (datetime != null)
            {
                string[] DT = datetime.Split('-');
                string dt = null;
                for (int i = 0; i < DT.Length; i++)
                {
                    if (DT[i].Length < 2)
                    {
                        DT[i] = "0" + DT[i];
                    }
                    dt += DT[i];
                }

                date = Int32.Parse(dt);
            }
            return date;
        }

        /// <summary>
        /// 分钟数据格式转换
        /// </summary>
        /// <param name="datetime">时间格式Mss</param>
        /// <returns>字符串，格式MMdd</returns>
        public static string ConvertTime(int datetime)
        {
            string time = null;
            if (datetime > 0 && datetime / 1000 <= 0)
            {
                time = "0" + datetime.ToString().Substring(0, 1) + ":" + datetime.ToString().Substring(1, 2);
            }
            else
            {
                time = datetime.ToString().Substring(0, 2) + ":" + datetime.ToString().Substring(2, 2);
            }
            return time;
        }

        /// <summary>
        /// 交易方向转换
        /// </summary>
        /// <param name="BSFlag">交易方向标识</param>
        /// <returns></returns>
        public static string ConvertBSFlag(string BSFlag)
        {
            string result = null;
            if (BSFlag == "S")
            {
                result = "卖出";
            }
            if (BSFlag == "B")
            {
                result = "买入";
            }
            if (BSFlag == "[")
            {
                result = "逆回购";
            }
            if (BSFlag == "卖出")
            {
                result = "S";
            }
            if (BSFlag == "买入")
            {
                result = "B";
            }
            if (BSFlag == "逆回购")
            {
                result = "[";
            }
            return result;
        }

        /// <summary>
        /// 撤单标识转换
        /// </summary>
        /// <param name="CancelFlag">是否撤单</param>
        /// <returns></returns>
        public static string ConvertCancelFlag(string CancelFlag)
        {
            string result = null;
            if (CancelFlag == "T")
            {
                result = "已撤单";
            }
            if (CancelFlag == "F")
            {
                result = "未撤单";
            }
            if (CancelFlag == "已撤单")
            {
                result = "T";
            }
            if (CancelFlag == "未撤单")
            {
                result = "F";
            }
            return result;
        }

        /// <summary>
        /// 市场名称转换
        /// </summary>
        /// <param name="Market">市场</param>
        /// <returns></returns>
        public static string ConvertMarket(string Market)
        {
            string result = null;
            if (Market == "1")
            {
                result = "上海";
            }
            if (Market == "2")
            {
                result = "深圳";
            }
            if (Market == "3")
            {
                result = "中金";
            }
            if (Market == "4")
            {
                result = "郑商";
            }
            if (Market == "5")
            {
                result = "大商";
            }
            if (Market == "6")
            {
                result = "上期";
            }
            if (Market == "上海")
            {
                result = "1";
            }
            if (Market == "深圳")
            {
                result = "2";
            }
            if (Market == "中金")
            {
                result = "3";
            }
            if (Market == "郑商")
            {
                result = "4";
            }
            if (Market == "大商")
            {
                result = "5";
            }
            if (Market == "上期")
            {
                result = "6";
            }
            return result;
        }
    }
}
