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
using UsersEdit.Infrastructure.Authentication;
using UsersEdit.CustomAttributes.Authorization;
using Infrastructure.Algo;
using ApplicationBusinessLayer.Mail;
using UsersEdit.App_Start;
using ApplicationRepository.Models;

namespace UsersEdit.Controllers
{
    [CustomAuthorized]
    public class ProfileController : Controller
    {
        private IUserRepository dalUser;
        private IImageRepository dalImage;
        private IRoleRepository dalRole;
        private IMailMessageRepository dalMail;

        public ProfileController(IRepositoryFactory factory)
        {
            dalRole = factory.CreateRoleRepository();
            dalUser = factory.CreateUserRepository();
            dalImage = factory.CreateImageRepository();
            dalMail = factory.CreateMailMessageRepository();

            ProfileMappers.UserRepository = dalUser;
            ProfileMappers.ImageRepository = dalImage;
        }

        [HttpGet]

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Profile");
            }
            ViewBag.InvalidCredentials = false;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signer = new FormsAuthenticationService(this.HttpContext);
                string passWord = PasswordHasher.Hash(user.Password).Hash;

                var realUser = dalUser.FindFirst(usr => usr.Login == user.UserName && 
                                                 usr.Password == passWord);
                if (realUser != null)
                {
                    signer.SignIn(realUser, user.RememberMe);
                    if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Profile");
                }
            }
            ViewBag.InvalidCredentials = true;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var signer = new FormsAuthenticationService(this.HttpContext);
            signer.SignOut();
            return RedirectToAction("Index", "Home");
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

        [CustomAuthorized(Roles="admin")]
        public ActionResult Add()
        {
            ViewBag.RoleId = new SelectList(dalRole.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]

        [CustomAuthorized(Roles = "admin")]
        public ActionResult Add(AddUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                UserInfo newUserInfo = ProfileMappers.AddViewModelToUser(user);
                newUserInfo.User.Password = PasswordHasher.Hash(user.Password).Hash;
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

                string subject = String.Format("Регистрация пользователя {0}", user.Login);
                EmailController mailGetter = new EmailController();

                MailMessage mailToSend = new MailMessage
                {
                    From = String.Empty,
                    To = ((UserPrincipal)User).Information.Email,
                    Subject = subject,
                    Body = mailGetter.EmailToText(mailGetter.Registration(String.Format("Регистрация пользователя {0}", user.Login),
                                                                         ((UserPrincipal)User).Information.Email)).Content
                };
                dalMail.Add(mailToSend);
                dalMail.SaveChanges();

                return RedirectToAction("Index", "Profile");
            }

            ViewBag.RoleId = new SelectList(dalRole.GetAll(), "Id", "Name", user.RoleId);
            return View(user);
        }

        [CustomAuthorized(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var selectedUser = dalUser.GetById((int)id);

            if (selectedUser == null)
                return HttpNotFound();

            ViewBag.RoleId = new SelectList(dalRole.GetAll(), "Id", "Name", selectedUser.RoleId);

            return View(ProfileMappers.UserToEditViewModel(selectedUser));
        }

        [HttpPost]

        [CustomAuthorized(Roles = "admin")]
        public ActionResult Edit(EditUserViewModel editVM)
        {
            var user = dalUser.GetById(editVM.Id);
            if (ModelState.IsValid)
            {
                if (user == null)
                    return HttpNotFound("User not found");

                if (user.IsActive && !editVM.IsActive)
                {
                    string subject = String.Format("Пользователь {0} забанен", user.Login);
                    EmailController mailGetter = new EmailController();

                    MailMessage mailToSend = new MailMessage
                    {
                        From = String.Empty,
                        To = ((UserPrincipal)User).Information.Email,
                        Subject = subject,
                        Body = mailGetter.EmailToText(mailGetter.Block(String.Format("Пользователь {0} добавлен в бан список", user.Login),
                                                                       editVM.BlockDescription,
                                                                       ((UserPrincipal)User).Information.Email)).Content
                    };
                    dalMail.Add(mailToSend);
                    dalMail.SaveChanges();
                }

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

            ViewBag.RoleId = new SelectList(dalRole.GetAll(), "Id", "Name", user.RoleId);
            return View(editVM);
        }

        [CustomAuthorized(Roles = "admin")]
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

        [CustomAuthorized(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                var user = dalUser.GetById(id);
                if (user == null)
                    return HttpNotFound("User not found");

                if (user.Image != null)
                    dalImage.Delete(user.Image);
                dalUser.Delete(user);
                dalUser.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Delete", id);
            }
        }
        public ActionResult Details(int? id)
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