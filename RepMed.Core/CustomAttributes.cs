using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace RepMed.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Struct)]
    public class IgnoreDapperAttribute : Attribute
    {
    }

    public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            var message = this.ErrorMessageString;
            if (string.IsNullOrEmpty(this.ErrorMessage))
            this.ErrorMessage = message;

            return this.ErrorMessage;
        }
    }

    public class MaxLengthAttribute : System.ComponentModel.DataAnnotations.MaxLengthAttribute
    {
        public MaxLengthAttribute()
        {

        }
        public MaxLengthAttribute(int langth):base(langth)
        {

        }
        public override string FormatErrorMessage(string name)
        {
            var message = this.ErrorMessageString;
            if (string.IsNullOrEmpty(this.ErrorMessage))
                this.ErrorMessage = message;

            return this.ErrorMessage;
        }
    }

    public class MinLengthAttribute : System.ComponentModel.DataAnnotations.MinLengthAttribute
    {
       
        public MinLengthAttribute(int langth) : base(langth)
        {

        }
        public override string FormatErrorMessage(string name)
        {
            var message = this.ErrorMessageString;
            if (string.IsNullOrEmpty(this.ErrorMessage))
                this.ErrorMessage = message;

            return this.ErrorMessage;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _dependentProperty;
        private readonly string _conditionMethod;

        public RequiredIfAttribute(string dependentProperty, string conditionMethod)
        {
            _dependentProperty = dependentProperty;
            _conditionMethod = conditionMethod;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();

            var dependentValue = type.GetProperty(_dependentProperty)?.GetValue(instance);

            var method = type.GetMethod(_conditionMethod, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null)
            {
                return new ValidationResult($"Method '{_conditionMethod}' not found.");
            }

            var shouldRequire = (bool)method.Invoke(instance, new object[] { dependentValue });

            if (shouldRequire && value == null)
            {
                return new ValidationResult($"{validationContext.DisplayName} is required because {_dependentProperty} meets condition.");
            }

            return ValidationResult.Success;
        }
    }


}
