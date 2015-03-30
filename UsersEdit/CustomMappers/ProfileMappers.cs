using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationRepository.Models;
using ApplicationRepository.Concrete.Entity;
using ApplicationRepository.Concrete.ADOSql;
using ApplicationRepository.Interface;
using UsersEdit.Models.ViewModels.Profile;
using System.IO;


namespace UsersEdit.CustomMappers
{
    struct UserInfo
    {
        public User User { get; set; }
        public Image UserPhoto { get; set; }
    };

    static class ProfileMappers
    {
        private static IUserRepository uDal = null;
        private static IImageRepository imDal = null;

        public static IUserRepository UserRepository
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("IUserRepository must be non-null");

                if (uDal == null)
                    uDal = value;
            }
        }

        public static IImageRepository ImageRepository
        {
            set
            {
                if (value == null)
                    throw new ArgumentNullException("IImageRepository must be non-null");

                if (imDal == null)
                    imDal = value;
            }
        }

        public static void ConvertBasePropertiesFromViewToUser(AddEditViewModelBase vmUser, User user)
        {
            user.Id = vmUser.Id;
            user.BirthDay = vmUser.BirthDay;
            user.FirstName = vmUser.FirstName;
            user.LastName = vmUser.LastName;
            user.Phone = vmUser.Phone;
            user.IsActive = vmUser.IsActive;
            user.BlockDescription = vmUser.BlockDescription;
            user.Email = vmUser.Email;
            user.DateUpdated = DateTime.Now;
            user.RoleId = vmUser.RoleId;

            user.Age = (byte)(((new DateTime(1, 1, 1) + (DateTime.Now - vmUser.BirthDay)).Year - 1));
        }

        private static Image PostFileToImage(HttpPostedFileBase file)
        {
            if (file == null)
                return null;

            Image res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                file.InputStream.CopyTo(ms);
                res = imDal.ImageContentToImage(ms.GetBuffer(), file.FileName);
            }
            return res;
        }

        public static UserInfo AddViewModelToUser(AddUserViewModel vmUser)
        {
            User user = new User();
            ConvertBasePropertiesFromViewToUser(vmUser, user);

            user.Login = vmUser.Login;
            user.Password = vmUser.Password;
            user.DateCreated = DateTime.Now;

            Image userPhoto = PostFileToImage(vmUser.Photo);

            if (userPhoto == null)
                user.ImageId = null;
            else
                user.ImageId = userPhoto.Id;

            return new UserInfo() { User = user, UserPhoto = userPhoto };
        }

        public static EditUserViewModel UserToEditViewModel(User user)
        {
            if (user == null)
                throw new NullReferenceException("User must be non-null");

            EditUserViewModel editUser = new EditUserViewModel();

            editUser.Email = user.Email;
            editUser.Phone = user.Phone;
            editUser.FirstName = user.FirstName;
            editUser.LastName = user.LastName;
            editUser.BirthDay = user.BirthDay;
            editUser.IsActive = user.IsActive;
            editUser.BlockDescription = user.BlockDescription;
            editUser.BirthDay = user.BirthDay;
            editUser.RoleId = user.RoleId;

            return editUser;
        }

        public static UserInfo EditViewModelToUser(EditUserViewModel editVM, User user)
        {
            ConvertBasePropertiesFromViewToUser(editVM, user);

            if (!editVM.RemovePhoto)
            {
                Image userPhoto = PostFileToImage(editVM.Photo);
                if (userPhoto != null)
                {
                    if (user.Image != null)
                    {
                        imDal.Delete(user.Image);
                        imDal.SaveChanges();
                    }
                    user.ImageId = userPhoto.Id;
                    return new UserInfo { User = user, UserPhoto = userPhoto };
                }
            }
            else
            {
                if (user.Image != null)
                {
                    imDal.Delete(user.Image);
                    user.ImageId = null;
                    imDal.SaveChanges();
                }
            }
            return new UserInfo { User = user, UserPhoto = null };
        }
    }
}