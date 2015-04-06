using ApplicationRepository.Interface;
using ApplicationRepository.Models;
using Infrastructure.Repository.Generic.Concrete.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRepository.Concrete.Entity
{
    public class MailMessageRepository : GenericRepository<UsersEntities, MailMessage>, IMailMessageRepository
    {


    }
}
