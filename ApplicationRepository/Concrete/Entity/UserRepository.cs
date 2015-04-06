using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationRepository.Models;
using ApplicationRepository.Interface;
using System.IO;
using Infrastructure.Repository.Generic.Concrete.Entity;

namespace ApplicationRepository.Concrete.Entity
{
    public sealed class UserRepository : GenericRepository<UsersEntities, User>, IUserRepository
    {
        public UserRepository() : base() {}

        public User GetById(int id)
        {
            return context.Set<User>().Find(id);
        }
        public MemoryStream GetPhoto(int id)
        {
            int? imid = GetById(id).ImageId;

            var image = (imid == null) ? null : context.Image.Find(imid).ImageContent;
            var stream = (image != null) ? new MemoryStream(image.ToArray()) :
                                           new MemoryStream();

            return stream;
        }
    }
}
