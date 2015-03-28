using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Reflection;

namespace UsersEdit.CustomValidationAttributes
{
    public class RequiredIf : RequiredAttribute, IClientValidatable
    {
        private string propName;
        private object expectedValue;

        public RequiredIf(string fieldName, object expectedValue)
        {
            this.propName = fieldName;
            this.expectedValue = expectedValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object propertyValue = type.GetProperty(propName).GetValue(instance, null);

            if ((expectedValue == null && propertyValue == null) || (expectedValue != null && propertyValue != null && propertyValue.ToString() == expectedValue.ToString()))
            {
                ValidationResult res = base.IsValid(value, context);
                return res;
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
               ErrorMessage = ErrorMessage,
               ValidationType = "requiredif"
            };
        }
    }
}