using Dashboard.Common;
using DashBoard.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace DashBoard.Logic
{
    public class StockDicManager
    {
        private static string _dictPath = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\stock.dat";//获取物理路径.
        //private static string _dictPath = "D:\\DashBoard\\DashBoard.Web\\App_Data\\stock.dat";
        /// <summary>
        /// 字典文件路径
        /// </summary>
        public static string DictPath
        {
            get { return _dictPath; }
        }

        /// <summary>
        /// 股票信息字典
        /// </summary>
        private static List<StockItem> _stockList = new List<StockItem>();

        /// <summary>
        /// 初始化字典
        /// </summary>
        public static bool LoadDict()
        {
            if (File.Exists(_dictPath))
            {
                return LoadFromFile(_dictPath);
            }
            return false;
        }

        /// <summary>
        /// 初始化字典
        /// </summary>
        /// <param name="path">字典路径</param>
        public static bool LoadFromFile(string path)
        {
            bool result = false;
            try
            {
                string fullpath = Path.GetFullPath(path);
                if (File.Exists(fullpath))
                {
                    _stockList = Serializer.ParseJsonFile<List<StockItem>>(fullpath);
                    result = true;
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// 根据股票代码获取股票名字
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        public static string GetStockName(string stockCode)
        {
            string result = string.Empty;
            foreach (var item in _stockList)
            {
                if (item.StockCode == stockCode)
                {
                    result = item.StockName;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据股票代码获取股票信息
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        public static StockItem GetStockItem(string stockCode)
        {
            StockItem stock = null;
            foreach (var item in _stockList)
            {
                if (item.StockCode == stockCode)
                {
                    stock = item;
                    break;
                }
            }
            return stock;
        }

        /// <summary>
        /// 搜索股票信息
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static List<StockItem> FindStockItems(string searchText)
        {
            List<StockItem> result = new List<StockItem>();
            foreach (var item in _stockList)
            {
                if (item.SearchText.Contains(searchText))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// 清空股票字典
        /// </summary>
        public static void Clear()
        {
            _stockList.Clear();
        }
    }
}
