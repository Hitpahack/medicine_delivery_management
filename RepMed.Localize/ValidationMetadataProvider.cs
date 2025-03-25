using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RepMed.Localize
{
    public class ValidationMetadataProvider : IValidationMetadataProvider
    {
        private IStringLocalizer<DataAnnotationsResource> _localizer;

        public ValidationMetadataProvider(IStringLocalizer<DataAnnotationsResource> localizer)
        {
            _localizer = localizer;
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (var metadata in context.ValidationMetadata.ValidatorMetadata)
            {
                if (metadata is ValidationAttribute attribute)
                {
                    var sdfs= CultureInfo.CurrentCulture;
                    string message = attribute.FormatErrorMessage(context.Key.Name);
                    var propertyName = context.Key.Name;

                    switch (metadata.GetType().Name)
                    {
                        case nameof(RequiredAttribute):
                            attribute.ErrorMessage = _localizer.GetFormatedString(message, context.Key.Name);
                            break;
                        case nameof(MinLengthAttribute):
                            var minAttr = attribute as MinLengthAttribute;
                            attribute.ErrorMessage = _localizer.GetFormatedString(message, context.Key.Name, minAttr.Length);
                            break;
                        case nameof(MaxLengthAttribute):
                            var mxAttr = attribute as MaxLengthAttribute;
                            attribute.ErrorMessage = _localizer.GetFormatedString(message, context.Key.Name, mxAttr.Length);
                            break;


                    }
                }
            }
        }






    }


    
    public static class LocalizedStringExtenstions 
    {
        public static string GetFormatedString(this IStringLocalizer localizer, string key, params object[] arguments)
        {
            if (arguments.Length > 0)
            {
                var formatedString = arguments.Select(r => localizer[r.ToString()].Value).ToArray();
                return localizer[key, formatedString];

            }
            else
            {
                return localizer[key].Value;
            }
            
        }



        
    }

    public class CUAValidationAttribute : ValidationAttribute
    {
        public CUAValidationAttribute()
        {

        }
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }

    


}
