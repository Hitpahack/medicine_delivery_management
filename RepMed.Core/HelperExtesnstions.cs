using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace RepMed.Core
{
    

    public static class HelperExtesntions
    {
        public static string GetActualError(this Exception exception)
        {
            string message_ = string.Empty;
            if (exception != null)
            {
                while (exception.InnerException != null)
                    exception = exception.InnerException;

                if (exception.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    string patern_ = "(\"dbo.)([A-Za-z0-9_.])+(\")";
                    Regex re = new Regex(patern_, RegexOptions.IgnoreCase);
                    Match m = re.Match(exception.Message);
                    if (re.IsMatch(exception.Message))
                    {
                        message_ = $@"This record is associate with {m.Value.Replace("dbo.", string.Empty)}";
                    }
                }
                else
                {
                    message_ = exception.Message;
                }
            }
            return message_;
        }

        public static string GetError(this ModelStateDictionary modelState)
        {

            var erroneousFields = modelState.FirstOrDefault(ms => ms.Value.Errors.Any());
            if (erroneousFields.Value != null)
                return erroneousFields.Value.Errors.FirstOrDefault().ErrorMessage;

            return "";
        }

        public static string GenerateRandomOTP(this int iOTPLength)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = string.Empty;
            var rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                string sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

        public static ICollection<ValidationResult> Validate(this object model, out string message)
        {
            ICollection<ValidationResult> results = null;
            message = "";
            if (!Validate(model, out results))
            {
                message = String.Join("\n", results.Select(o => o.ErrorMessage));
            }

            return results;
        }
        private static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
        }
        public static bool ModelStateIsValid(this object model, out string message)
        {
            return model.Validate(out message)?.Count == 0;
        }

        public static string GetXmlNode(this XmlDocument xml, string xmlNode)
        {
            XmlNode node = xml.DocumentElement.SelectSingleNode(xmlNode);
            XmlNode childNode = node.ChildNodes[0];
            string strValue;
            if (childNode is XmlCDataSection)
            {
                XmlCDataSection cdataSection = childNode as XmlCDataSection;
                strValue = cdataSection.Value.ToString();
            }
            else
            {
                strValue = childNode.Value.ToString();
            }

            return strValue;
        }

        public static Dictionary<string, object> GetPropertiesKeyValue(this object _Class)
        {
            if (_Class == null) return null;
            Type TheType = _Class.GetType();
            PropertyInfo[] Properties = TheType.GetProperties();
            Dictionary<string, object> PropertiesMap = new Dictionary<string, object>();
            foreach (PropertyInfo Prop in Properties)
            {
                PropertiesMap.Add(Prop.Name, Prop.GetValue(_Class, null));
            }
            return PropertiesMap;
        }

        public static bool IsValidPassword(this string password, out string message)
        {
            message = string.Empty;
            char[] SPECIAL_CHARACTERS = { '!', '#', '$', '%', '&', '*',  '/'
                                        , '\u005c', '-', '?', '<', '>','.',':',';','^',
                                        '(','_',')','+','=','_','|','`','~' };

            if (string.IsNullOrWhiteSpace(password))
            {
                message = "Password cannot contain blank or white spaces.";
                return false;
            }
            if (password.Length > 16 || password.Length < 8)
            {
                message = "Password length must be between 8 and 16.";
                return false;
            }
            if (password.Contains(' '))
            {
                message = "Password cannot contain white spaces.";
                return false;
            }
            if (!password.Any(x => char.IsDigit(x)) || !password.Any(x => char.IsLetter(x)))
            {
                message = "Password should be alphanumeric.";
                return false;
            }

            return true;
        }

    }
}
