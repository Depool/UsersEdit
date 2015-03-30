using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsersEdit.ModelBinders;


namespace UsersEdit.Models.ViewModels.Profile
{
    [ModelBinder(typeof(DateTimeCustomBinder))]
    public class EditUserViewModel : AddEditViewModelBase
    {
        public bool RemovePhoto { get; set; }
    }
}