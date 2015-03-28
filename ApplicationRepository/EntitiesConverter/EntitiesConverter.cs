using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace ApplicationRepository.EntitiesConverter
{
    internal static class StaticEntitiesConverter
    {
        public static T ReaderRowToEntity<T>(IDataReader reader) where T : new()
        {
            T res = new T();

            PropertyInfo[] properties = res.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            for (int i = 0; i < reader.FieldCount; ++i)
            {
                string colName = reader.GetName(i);
                PropertyInfo neededProp = properties.Where(p => p.Name == colName).FirstOrDefault();
                if (neededProp != null)
                {
                    if (reader[i] == System.DBNull.Value)
                        neededProp.SetValue(res, null);
                    else
                        neededProp.SetValue(res, reader[i]);
                }
            }
            return res;
        }

        public static Dictionary<string, object> GetProperiesValues<T>(List<string> columns, T instance) where T : new()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach(string col in columns)
                result.Add(col,instance.GetType().GetProperty(col).GetValue(instance));

            return result;
        }
    }
}
