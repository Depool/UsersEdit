using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationRepository.Models;
using ApplicationRepository.Interface;
using System.IO;

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

        public override void Add(User instance)
        {
            if (context.User.Count() == 0)
                instance.Id = 1;
            else
                instance.Id = 1 + context.User.Max(usr => usr.Id);
            base.Add(instance);
        }
    }
}
