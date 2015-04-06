using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using UsersEdit.CustomValidationAttributes;

namespace UsersEdit.Models.ViewModels.Profile
{
    public abstract class AddEditViewModelBase
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Login is required")]
        [StringLength(25, ErrorMessage = "Login length must not exceed 25")]
        [Remote("ValidateLogin", "Profile", ErrorMessage = "Such login already exists")]
        public string Login { get; set; }
     
        public Nullable<byte> Age { get; set; }
        [RegularExpression(@"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$", ErrorMessage = "Incorrect phone number")]
        public string Phone { get; set; }

        [StringLength(50, ErrorMessage = "FirstName length must not exceed 50")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "LastName length must not exceed 50")]
        public string LastName { get; set; }
        public bool IsActive { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Incorrect e-mail")]
        public string Email { get; set; }

        [RequiredIf("IsActive", false, ErrorMessage = "Desciption is required if user is not active")]
        public string BlockDescription { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime BirthDay { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public HttpPostedFileBase Photo { get; set; }

        [Required]
        public int RoleId { get; set; }

    }
}