using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDBDog
{
    public static class Extension
    {

        #region 序列化
        public static T De<T>(this string data, T type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(data, type);
        }
        public static T De<T>(this string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
        }
        public static string Serialize(this object data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
        }

        #endregion

        #region 字典与参数处理

        /// <summary>
        /// 创建实例，从参数字典中复制属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="para"></param>
        /// <returns></returns>
        public static T FromNameValuePairs<T>(this KeyValuePair<string, string>[] para) where T : class, new()
        {
            var t = new T();
            Type type = typeof(T);//t.GetType();
            type.GetProperties().Where(r => r.CanWrite).ToList().ForEach(r =>
            {
                var p = para.FirstOrDefault(f => f.Key == r.Name);
                if (!string.IsNullOrEmpty(p.Value))
                     r.SetValue(t, Convert.ChangeType(p.Value, r.PropertyType));

            });
            return t;
        }
        public static T CopyNameValuePairs<T>(this KeyValuePair<string, string>[] para,T oldData) where T : class, new()
        {
            Type type = typeof(T);//t.GetType();
            type.GetProperties().Where(r => r.CanWrite).ToList().ForEach(r =>
            {
                var p = para.FirstOrDefault(f => f.Key == r.Name);
                if (!string.IsNullOrEmpty(p.Value))
                    r.SetValue(oldData, Convert.ChangeType(p.Value, r.PropertyType));

            });
            return oldData;
        }

        /// <summary>
        /// 从对象中导出属性字典
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string>[] ToDict(this object o)
        {
            //反射所有的属性及值
            Type type = o.GetType();
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            type.GetProperties().ToList().ForEach(r =>
            {
                list.Add(new KeyValuePair<string, string>(r.Name, r.GetValue(o)?.ToString()));
            });
            return list.ToArray();
        }

        #endregion


    }

}
