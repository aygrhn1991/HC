using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace CWGK.Models
{
    public static class ModelConvert<T> where T : new()
    {
        public static IList<T> ConvertToModel(DataTable dt)
        {
            // 定义集合    
            IList<T> ts = new List<T>();
            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    // 检查DataTable是否包含此列
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite)
                            continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            if (pi.GetMethod.ReturnParameter.ParameterType.Name == "Int32")
                            {
                                value = Convert.ToInt32(value);
                            }
                            if (pi.GetMethod.ReturnParameter.ParameterType.Name == "DateTime")
                            {
                                value = Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (pi.GetMethod.ReturnParameter.ParameterType.Name == "String")
                            {
                                value = value.ToString();
                            }
                            if (pi.GetMethod.ReturnParameter.ParameterType.Name == "Boolean")
                            {
                                value = value.ToString() == "0" ? false : true;
                            }
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }


        public static string GetJson<T>(T obj)
        {
            //记住,添加引用 System.ServiceModel.Web 
            //如果不添加上面的引用,System.Runtime.Serialization.Json; Json是出不来的哦
            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                json.WriteObject(ms, obj);
                string szJson = Encoding.UTF8.GetString(ms.ToArray());
                return szJson;
            }
        }
    }

    public static class ModelConvertHelper
    {
        public static IList<T> ConvertToModel<T>(this DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new List<T>();
            }
            return ModelConvert<T>.ConvertToModel(dt);
        }

    }
}