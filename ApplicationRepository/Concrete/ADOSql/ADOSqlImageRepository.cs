using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationRepository.Models;
using ApplicationRepository.Interface;
using ApplicationRepository.EntitiesConverter;
using System.Data.SqlClient;
using System.IO;

namespace ApplicationRepository.Concrete.ADOSql
{
    public sealed class ADOSqlImageRepository : ADOSqlGenericRepository<Image>, IImageRepository
    {
        private int? maxId = null;

        public ADOSqlImageRepository() : base() { }
        public ADOSqlImageRepository(string connString) : base(connString) {}

        public override IEnumerable<Image> GetAll()
        {
           IEnumerable<Image> res = base.GetAll();

           if (maxId == null)
               maxId = res.Max(img => img.Id);

           return res;
        }

        public override void Add(Image instance)
        {
            if (maxId == null)
                GetAll();
            instance.Id = (int)maxId + 1;
            maxId++;

            base.Add(instance);
        }

        public Image GetById(int id)
        {
            Image res = null;
            try
            {
                conn.Open();
                SqlCommand query = new SqlCommand(String.Format("SELECT * FROM [Image] WHERE Id = {0}", id), conn);

                using (SqlDataReader reader = query.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        res = StaticEntitiesConverter.ReaderRowToEntity<Image>(reader);
                    }
                }
            }
            finally
            {
                conn.Close();
            }

            //get image

            return res;
        }

        public Image ImageContentToImage(byte[] content, string name)
        {
            if (content.Length > 0)
            {
                Image newImage = new Image();

                if (maxId == null)
                    GetAll();
                newImage.Id = (int)maxId + 1;
                maxId++;

                newImage.ImageName = name;
                newImage.ImageContent = content;
                return newImage;
            }
            else
                return null;
        }
    }
}
