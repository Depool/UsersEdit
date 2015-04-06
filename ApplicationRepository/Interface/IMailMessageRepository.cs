using ApplicationRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repository.Generic.Interface;

namespace ApplicationRepository.Interface
{
    public interface IMailMessageRepository : IGenericRepository<MailMessage>
    {

    }
}
