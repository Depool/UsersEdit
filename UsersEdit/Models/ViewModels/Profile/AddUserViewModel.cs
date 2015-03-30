﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsersEdit.ModelBinders;
using System.ComponentModel.DataAnnotations;
using System.Collections;


namespace UsersEdit.Models.ViewModels.Profile
{
    [ModelBinder(typeof(DateTimeCustomBinder))]
    public class AddUserViewModel : AddEditViewModelBase
    {
        [Required(ErrorMessage = "Login is required")]
        [StringLength(25, ErrorMessage = "Login length must not exceed 25")]
        [Remote("ValidateLogin", "Profile", ErrorMessage = "Such login already exists")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, ErrorMessage = "Password length must not exceed 50")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Repeat the password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords must be equal")]
        public string RepeatPassword { get; set; }
    }
}