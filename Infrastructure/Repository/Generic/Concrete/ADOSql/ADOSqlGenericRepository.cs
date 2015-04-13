using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Infrastructure.Repository.EntitiesConverter;
using Infrastructure.Repository.QueryHelper;
using Infrastructure.Repository.Generic.Interface;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace Infrastructure.Repository.Generic.Concrete.ADOSql
{
    public abstract class ADOSqlGenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        protected Database dbEnterpriseInstance;

        private T lastInsertedEntity = null;

        public ADOSqlGenericRepository()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            dbEnterpriseInstance = factory.CreateDefault();
        }

        public ADOSqlGenericRepository(string connStringName)
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            dbEnterpriseInstance = factory.Create(connStringName);
        }

        private List<String> getTableColumns()
        {
            List<string> colNames = new List<string>();
            using (IDataReader columns = dbEnterpriseInstance.ExecuteReader(CommandType.Text, 
                                                                            String.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}' AND TABLE_SCHEMA='dbo'", typeof(T).Name)))
            {
                while (columns.Read())
                    colNames.Add(columns[0] as String);
            }
            return colNames;
        }

        private List<String> getPrimaryKey()
        {
            List<string> pk = new List<string>();
            using (IDataReader primaryKeys = dbEnterpriseInstance.ExecuteReader(CommandType.Text,
                                                                            String.Format("SELECT Col.Column_Name from\n" +
                                                                            "INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab,\n" +
                                                                            "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col\n" +
                                                                            "WHERE\n" +
                                                                            "Col.Constraint_Name = Tab.Constraint_Name\n" +
                                                                            "AND Col.Table_Name = Tab.Table_Name\n" +
                                                                            "AND Constraint_Type = 'PRIMARY KEY'\n" +
                                                                            "AND Col.Table_Name = '{0}'", typeof(T).Name)))
            {
                while (primaryKeys.Read())
                    pk.Add(primaryKeys[0] as String);
            }
            return pk;
        }

        public virtual IEnumerable<T> GetAll()
        {
            List<T> allEntities = new List<T>();

            using (IDataReader reader = dbEnterpriseInstance.ExecuteReader(CommandType.Text,
                                                                           String.Format("Select * from [{0}]", typeof(T).Name)))
            {
                while (reader.Read())
                        allEntities.Add(StaticEntitiesConverter.ReaderRowToEntity<T>(reader));
            }

            return allEntities;
        }

        public virtual void Add(T instance)
        {
            List<string> colNames = getTableColumns();
            if (colNames.Contains("Id"))
                colNames.Remove("Id");

            Dictionary<string, object> record = StaticEntitiesConverter.GetProperiesValues<T>(colNames, instance);
            KeyValuePair<string, string> data = StaticQueryHelper.InsertDictionaryToStrings(record, typeof(T).Name);

            using (DbCommand insert = dbEnterpriseInstance.GetSqlStringCommand(String.Format("INSERT INTO [{0}] {1} VALUES {2}", typeof(T).Name, data.Key, data.Value))) 
            {
                foreach (string key in record.Keys)
                    if ((record[key] as byte[]) != null && insert is SqlCommand)
                        (insert as SqlCommand).Parameters.Add(String.Format("@{0}", key), SqlDbType.VarBinary, -1).Value = record[key] as byte[];
                dbEnterpriseInstance.ExecuteNonQuery(insert);
                lastInsertedEntity = instance;
            }

        }

        public virtual void Modify(T instance)
        {
            string pkName = getPrimaryKey()[0];

            List<string> colNames = getTableColumns();
            if (colNames.Contains("Id"))
                colNames.Remove("Id");

            Dictionary<string, object> record = StaticEntitiesConverter.GetProperiesValues<T>(colNames, instance);


            String set = StaticQueryHelper.GetModifySetStatementArg(record);
            string pkValS = null;
            Object pkVal = instance.GetType().GetProperty(pkName).GetValue(instance);
            if (pkVal.GetType() == typeof(string))
                pkValS = String.Format("'{0}'", pkVal);
            else
                pkValS = pkVal.ToString();


            using (DbCommand modify = dbEnterpriseInstance.GetSqlStringCommand(String.Format("UPDATE [{0}] SET {1} WHERE {2} = {3}", typeof(T).Name, set, pkName, pkValS)))
            {
                foreach (string key in record.Keys)
                    if ((record[key] as byte[]) != null && modify is SqlCommand)
                        (modify as SqlCommand).Parameters.Add(String.Format("@{0}", key), SqlDbType.VarBinary).Value = record[key] as byte[];
                try
                {
                    dbEnterpriseInstance.ExecuteNonQuery(modify);

                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
        }

        public virtual void Delete(T instance)
        {
            string pkName = getPrimaryKey()[0];
            string pkValS = null;
            Object pkVal = instance.GetType().GetProperty(pkName).GetValue(instance);
            if (pkVal.GetType() == typeof(string))
                pkValS = String.Format("'{0}'", pkVal);
            else
                pkValS = pkVal.ToString();
            dbEnterpriseInstance.ExecuteNonQuery(CommandType.Text, String.Format("DELETE FROM [{0}] WHERE {1} = {2}",typeof(T).Name, pkName, pkValS));
        }

        public virtual T FindFirst(Func<T, bool> filter)
        {
            IEnumerable<T> result = GetAll();
            return result.Where(filter).FirstOrDefault();

        }

        public virtual IEnumerable<T> FindAll(Func<T, bool> filter)
        {
            IEnumerable<T> result = GetAll();
            return result.Where(filter);
        }

        public void SaveChanges()
        {
            if (lastInsertedEntity != null && lastInsertedEntity.GetType().GetProperty("Id") != null)
            {
                int id = 0;
                string query = String.Format("Select IDENT_CURRENT('{0}')", typeof(T).Name);
                object res = dbEnterpriseInstance.ExecuteScalar(CommandType.Text, query);
                id = System.Convert.ToInt32(res);
                lastInsertedEntity.
                    GetType().GetProperty("Id").SetValue(lastInsertedEntity,
                                                                        id);
                lastInsertedEntity = null;
            }
            //????? solid break
        }
    }
}
