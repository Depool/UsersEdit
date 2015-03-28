using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using ApplicationRepository.Concrete.Entity;
using ApplicationRepository.Interface;
using ApplicationRepository.Concrete.ADOSql;
using UsersEdit.Models.ViewModels.Profile;
using UsersEdit.CustomMappers;

namespace UsersEdit.Controllers
{
    public class ProfileController : Controller
    {
        private IUserRepository dalUser;
        private IImageRepository dalImage;

        public ProfileController(IUserRepository userRepository, IImageRepository imageRepository)
        {
            dalUser = userRepository;
            dalImage = imageRepository;
            ProfileMappers.UserRepository = userRepository;
            ProfileMappers.ImageRepository = imageRepository;
        }

        public ActionResult Index()
        {
            return View(dalUser.GetAll());
        }

        public ActionResult GetPhoto(int id)
        {
            MemoryStream photo = dalUser.GetPhoto(id);
            return new FileStreamResult(photo, "image/jpeg");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Add(AddUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                UserInfo newUserInfo = ProfileMappers.AddViewModelToUser(user);
                if (newUserInfo.UserPhoto != null)
                {
                    dalImage.Add(newUserInfo.UserPhoto);
                    dalImage.SaveChanges();
                }

                if (newUserInfo.User != null)
                {
                    dalUser.Add(newUserInfo.User);
                    dalUser.SaveChanges();
                }

                return RedirectToAction("Index", "Profile");
            }

            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedUser = dalUser.GetById((int)id);

            if (selectedUser == null)
                return HttpNotFound();

            return View(ProfileMappers.UserToEditViewModel(selectedUser));
        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                var user = dalUser.GetById(editVM.Id);
                if (user == null)
                    return HttpNotFound("User not found");

                UserInfo editedUserInfo = ProfileMappers.EditViewModelToUser(editVM, user);

                if (editedUserInfo.UserPhoto != null)
                {
                    dalImage.Add(editedUserInfo.UserPhoto);
                    dalImage.SaveChanges();
                }

                if (editedUserInfo.User != null)
                {
                    dalUser.Modify(editedUserInfo.User);
                    dalUser.SaveChanges();
                }

                return RedirectToAction("Index", "Profile");
            }

            return View(editVM);
            
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedUser = dalUser.GetById((int)id);

            if (selectedUser == null)
                return HttpNotFound();

            return View(selectedUser);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                var user = dalUser.GetById(id);
                if (user == null)
                    return HttpNotFound("User not found");

                if (user.Image != null)
                    dalImage.Delete(user.Image);
                //dalImage.SaveChanges();
                dalUser.Delete(user);
                dalUser.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Delete", id);
            }
        }

        public ActionResult ValidateLogin(string Login)
        {
            return Json(dalUser.FindFirst(user => user.Login == Login) == null,
                                      JsonRequestBehavior.AllowGet);
        }

        public ActionResult GoingToEdit()
        {
            if (AjaxRequestExtensions.IsAjaxRequest(Request))
                return Json("You are going to change user info. Are you sure?", JsonRequestBehavior.AllowGet);
            return HttpNotFound();
        }
	}
}