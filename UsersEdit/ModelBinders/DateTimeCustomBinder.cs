using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace UsersEdit.ModelBinders
{
    public class DateTimeCustomBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.PropertyType == typeof(DateTime) && propertyDescriptor.Name == "BirthDay")
            {
                object result = null;

                HttpRequestBase request = controllerContext.HttpContext.Request;

                string dayS = request.Form.Get("BirthDay.Day"); int day;
                string monthS = request.Form.Get("BirthDay.Month"); int month;
                string yearS = request.Form.Get("BirthDay.Year"); int year;
                DateTime birthDate;

                if (!int.TryParse(dayS, out day) ||
                    !int.TryParse(monthS, out month) ||
                    !int.TryParse(yearS, out year))
                    result = null;
                else
                {
                    try
                    {
                        birthDate = new DateTime(year, month, day);
                        result = birthDate;
                    }
                    catch
                    {
                        result = null;
                    }
                }

                if (result == null)
                    bindingContext.ModelState.AddModelError("BirthDay", "Invalid date");
                propertyDescriptor.SetValue(bindingContext.Model, 
                        result);
            }
            else
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}