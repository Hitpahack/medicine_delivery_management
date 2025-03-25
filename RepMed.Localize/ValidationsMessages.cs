using Microsoft.Extensions.Localization;

namespace Bestshifts.Localize
{
    public static class ValidationsMessages
    {
        public static readonly IStringLocalizer<DataAnnotationsResource> localizer;

        private static string currentCulture { get; set; }
        public static void SetCulture(string culture)
        {
            currentCulture = culture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parms"></param>
        /// <returns>{0} doesn't exist</returns>
        public static string NotExist(params string[] parms)
        {
            if (parms == null || parms.Length == 0)
                parms = new string[] { currentCulture == "ar" ? "المستعمل" : "User" };

            switch (currentCulture)
            {
                case "ar":
                    return string.Format("{0} لم يتم العثور على", parms);
                default:
                    return string.Format("{0} doesn't exist", parms);

            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parms"></param>
        /// <returns>{0} doesn't exist</returns>
        public static string AlreadyExist(params string[] parms)
        {
            if (parms == null || parms.Length == 0)
                parms = new string[] { currentCulture == "ar" ? "المستعمل" : "User" };

            switch (currentCulture)
            {
                case "ar":
                    return string.Format("{0} موجود مسبقا", parms);
                default:
                    if (parms.Length > 1)
                        return string.Format("{0} already exist. {1}", parms);
                    else
                        return string.Format("{0} already exist.", parms);

            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parms"></param>
        /// <returns>{0} account not activated!</returns>
        public static string NotActivate(params string[] parms)
        {
            if (parms == null || parms.Length == 0)
                parms = new string[] { currentCulture == "ar" ? "المستعمل" : "User" };

            switch (currentCulture)
            {
                case "ar":
                    return string.Format("{0} الحساب غير مفعل!", parms);
                default:
                    return string.Format("{0} account not activated!", parms);

            }

        }

        public static string EmailNotConfirm()
        {


            switch (currentCulture)
            {
                case "ar":
                    return string.Format("لم يتم التحقق من بريدك الإلكتروني ، يرجى التحقق أولاً.");
                default:
                    return string.Format("Your email is not verify, please verify first!");

            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parms"></param>
        /// <returns>{0} invalid username or password!</returns>
        public static string InvalidCredential(params string[] parms)
        {
            if (parms == null || parms.Length == 0)
                parms = new string[] { currentCulture == "ar" ? "أنت قد دخلت" : "You have entered" };

            switch (currentCulture)
            {
                case "ar":
                    return string.Format("{0} خطأ في اسم المستخدم أو كلمة مرور!", parms);
                default:
                    return string.Format("{0} invalid username or password!", parms);

            }

        }

        public static string Invalid(this string paramName)
        {
            switch (currentCulture)
            {
                case "ar":
                    return string.Format("{0} اغير صالحة", paramName);
                default:
                    return string.Format("{0} invalid!", paramName);

            }

        }

        public static string ParmaRequired(this string paramName)
        {
            switch (currentCulture)
            {
                case "ar":
                    return string.Format("{0} المعلمات المطلوبة!", paramName);
                default:
                    return string.Format("{0} parameters required!", paramName);

            }

        }
    }
    public class ResponseMessage
    {
        private static string currentCulture { get; set; }
        public static void SetCulture(string culture)
        {
            currentCulture = culture;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parms"></param>
        /// <returns>{0} invalid username or password!</returns>
        public static string Success(params string[] parms)
        {
            if (parms == null || parms.Length == 0)
                parms = new string[] { currentCulture == "ar" ? "طلب" : "Request" };

            switch (currentCulture)
            {
                case "ar":
                    return string.Format("{0} كان ناجحا!", parms);
                default:
                    return string.Format("{0} has been success!", parms);

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parms"></param>
        /// <returns>{0} invalid username or password!</returns>
        public static string Invalid(params string[] parms)
        {
            if (parms == null || parms.Length == 0)
                parms = new string[] { currentCulture == "ar" ? "طلب" : "Request" };

            switch (currentCulture)
            {
                case "ar":
                    return string.Format("{0} غير صالح!", parms);
                default:
                    return string.Format("{0} is not valid!", parms);

            }

        }
    }
    public class ErrorMessages
    {
        private  readonly IStringLocalizer<DataAnnotationsResource> _localizer;

        public ErrorMessages(IStringLocalizer<DataAnnotationsResource> localizer)
        {
            _localizer = localizer;
        }

        public string NotExist(string parma)
        {
            return _localizer["{0} doesn't exist.", parma];
        }
    }



}
