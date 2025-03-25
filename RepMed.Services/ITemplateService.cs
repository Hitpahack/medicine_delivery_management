using RepMed.Core;
using RepMed.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace RepMed.Services
{
    public interface ITemplatesService
    {
        /// <summary>
        /// Get xml template
        /// </summary>
        /// <returns>XmlDocument</returns>
        Task<XmlDocument> GetTemplate();

        /// <summary>
        /// Get otp html template
        /// </summary>
        /// <param name="templateDto">OtpTemplate</param>
        /// <returns>XmlTemplateResponse</returns>
        Task<XmlTemplateResponse> GetOtpTemplate(OtpTemplate templateDto);
    }

    public class TemplatesService : ITemplatesService
    {
        private readonly XmlDocument xmlDocument;
        public TemplatesService(string xmlTemplatePath)
        {
            xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlTemplatePath);
        }

        public async Task<XmlDocument> GetTemplate()
        {
            return await Task.FromResult(xmlDocument);
        }

        public async Task<XmlTemplateResponse> GetOtpTemplate(OtpTemplate templateDto)
        {
            try
            {
                if (templateDto.ModelStateIsValid(out var message))
                {
                    var emailBody = xmlDocument.GetXmlNode($"ResendOTP/Body");
                    var emailSub = xmlDocument.GetXmlNode($"ResendOTP/Subject");

                    foreach (KeyValuePair<string, object> item in templateDto.GetPropertiesKeyValue())
                    {
                        emailBody = emailBody.Replace(string.Concat("@@@", item.Key), item.Value.ToString());
                        emailSub = emailSub.Replace(string.Concat("@@@", item.Key), item.Value.ToString());
                    }
                    return await Task.FromResult(new XmlTemplateResponse
                    {
                        Body = emailBody,
                        Subject = emailSub,
                        IsSuccess = true,
                        Message = "Success"
                    });
                }
                else
                {
                    return await Task.FromResult(new XmlTemplateResponse(message));
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new XmlTemplateResponse(ex.GetActualError()));
            }
        }

    }

}
