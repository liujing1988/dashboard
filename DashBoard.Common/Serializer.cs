using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace DashBoard.Common
{
    /// <summary>
    /// XML/JSON serialize and deserialize
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Deep copy
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object Clone(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// Serialize an object into xml string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Serialize(object value, Encoding encoding = null)
        {
            string result = string.Empty;
            if (value != null)
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    new XmlSerializer(value.GetType()).Serialize(ms, value);
                    result = encoding.GetString(ms.ToArray());
                }
            }
            return result;
        }

        /// <summary>
        /// Serialize an object to string and write into the file
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        public static void Serialize(object value, string path, Encoding encoding = null)
        {
            if (value != null)
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                using (StreamWriter sw = new StreamWriter(Path.GetFullPath(path), false, encoding))
                {
                    new XmlSerializer(value.GetType()).Serialize(sw, value);
                }
            }
        }

        /// <summary>
        /// Deserialize a string to an object
        /// </summary>
        /// <param name="xmlString"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xmlString, Encoding encoding = null)
        {
            T result = default(T);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                using (MemoryStream ms = new MemoryStream(encoding.GetBytes(xmlString)))
                {
                    object value = serializer.Deserialize(ms);
                    if (value != null && value.GetType() == typeof(T))
                    {
                        result = (T)value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to deserialize the string into the object of " + typeof(T).Name + ": " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Read xml string from text reader and deserialize to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T Deserialize<T>(TextReader reader)
        {
            T result = default(T);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                object value = serializer.Deserialize(reader);
                if (value != null && value.GetType() == typeof(T))
                {
                    result = (T)value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to deserialize the string into the object of " + typeof(T).Name + ": " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Read xml string from the file and deserialize to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T DeserializeFile<T>(string path, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            using (StreamReader reader = new StreamReader(Path.GetFullPath(path), encoding))
            {
                return Deserialize<T>(reader);
            }
        }

        /// <summary>
        /// Convert an object to json string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Convert an object to json string, and write into the file
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        public static void ToJson(object value, string path)
        {
            using (StreamWriter sw = new StreamWriter(Path.GetFullPath(path)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(sw, value);
            }
        }

        /// <summary>
        /// Convert json string to an object
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T ParseJson<T>(string jsonString)
        {
            T result = default(T);
            try
            {
                result = JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to deserialize the string into the object of " + typeof(T).Name + ": " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Read json string from text reader and convert to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T ParseJson<T>(Stream stream)
        {
            T result = default(T);
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = (T)serializer.Deserialize(reader, typeof(T));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to deserialize the string into the object of " + typeof(T).Name + ": " + ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Read json string from the file and convert to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T ParseJsonFile<T>(string path)
        {
            using (FileStream stream = new FileStream(Path.GetFullPath(path), FileMode.Open, FileAccess.Read))
            {
                return ParseJson<T>(stream);
            }
        }
    }
}
