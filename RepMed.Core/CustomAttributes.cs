using System;
using System.Collections.Generic;
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
}
