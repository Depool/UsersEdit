using ApplicationRepository.Interface;
using ApplicationRepository.Models;
using Infrastructure.Repository.Generic.Concrete.ADOSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRepository.Concrete.ADOSql
{
    public class ADOSqlMailMessageRepository : ADOSqlGenericRepository<MailMessage>, IMailMessageRepository
    {
        public ADOSqlMailMessageRepository() : base() { }
        public ADOSqlMailMessageRepository(string connString) : base(connString) { }

    }
}
