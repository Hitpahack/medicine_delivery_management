using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RepMed.Localize
{
    public static class LocalizationExtension
    {
        public static IServiceCollection Localization(this IServiceCollection services, IMvcBuilder builder)
        {

            services.AddLocalization(opt => opt.ResourcesPath = "Resources");
            services.AddTransient<IValidationMetadataProvider, ValidationMetadataProvider>();
            services.AddSingleton<IValidationMessagesServices, ValidationMessagesServices>();
            

            builder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
              .AddDataAnnotationsLocalization(options =>
              {
                  options.DataAnnotationLocalizerProvider = (type, factory) =>
                  {
                      var assemblyName = new AssemblyName(typeof(DataAnnotationsResource).GetTypeInfo().Assembly.FullName);
                      return factory.Create(nameof(DataAnnotationsResource), assemblyName.Name);
                  };
                  
              })
              .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddOptions<MvcOptions>().Configure<IValidationMetadataProvider>((options, provider) =>
            {
                
                options.ModelMetadataDetailsProviders.Add(provider);
            });
            
            return services;
        }
    }
}
