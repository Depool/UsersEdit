using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlTypes;

namespace ApplicationRepository.QueryHelper
{
    internal static class StaticQueryHelper
    {
        public static KeyValuePair<string, string> InsertDictionaryToStrings(Dictionary<string, object> dict)
        {
            StringBuilder key = new StringBuilder("(");
            StringBuilder value = new StringBuilder("(");

            foreach (KeyValuePair<string, object> col in dict)
            {
                key.Append(col.Key + ",");
                if (col.Value == null)
                {
                   value.Append("NULL,");
                }
                else
                if (col.Value.GetType() == typeof(string))
                {
                    value.Append(String.Format("'{0}',", col.Value));
                }
                else
                if (col.Value.GetType() == typeof(DateTime))
                {
                    value.Append(String.Format("'{0}',", ((DateTime)col.Value).ToString("yyyy-MM-dd HH:mm:ss")));
                }
                else
                if (col.Value.GetType() == typeof(bool))
                {
                    value.Append(String.Format("'{0}',",(bool)col.Value == true ? 1 : 0));
                }
                else
                if (col.Value.GetType() == typeof(byte[]))
                {
                    value.Append(String.Format("@{0},", col.Key));
                }
                else
                {
                    value.Append(col.Value.ToString() + ",");
                }
            }

            key.Remove(key.Length - 1, 1); value.Remove(value.Length - 1, 1);
            key.Append(")"); value.Append(")");
            return new KeyValuePair<string, string>(key.ToString(), value.ToString());
        }


        public static String GetModifySetStatementArg(Dictionary<string, object> dict)
        {
            StringBuilder res = new StringBuilder(String.Empty);
            foreach (KeyValuePair<string, object> col in dict)
            {
                res.Append(String.Format("{0} = ", col.Key));
                if (col.Value == null)
                {
                    res.Append("NULL,");
                }
                else
                if (col.Value.GetType() == typeof(string))
                {
                    res.Append(String.Format("'{0}',", col.Value));
                }
                else if (col.Value.GetType() == typeof(DateTime))
                {
                    res.Append(String.Format("'{0}',", ((DateTime)col.Value).ToString("yyyy-MM-dd HH:mm:ss")));
                }
                else
                if (col.Value.GetType() == typeof(bool))
                {
                    res.Append(String.Format("'{0}',",(bool)col.Value == true ? 1 : 0));
                }
                else
                if (col.Value.GetType() == typeof(byte[]))
                {
                    res.Append(String.Format("@{0},", col.Key));
                }
                else
                {
                    res.Append(col.Value.ToString() + ",");
                }
            }
            res.Remove(res.Length - 1, 1);
            return res.ToString();
        }
    }
}
