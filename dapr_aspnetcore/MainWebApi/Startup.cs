using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MainWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .AddJsonOptions(options =>
            {
                // �]�mReponse��JSON key�R�W�W�h�ϥ�PascalCase�Ӥ��O�ϥιw�]camelCase(�m�p���R�W)
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            })
            .AddDapr() // ���UDapr Services�JApp Pipline
            ;

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(SWGENOptions =>
            {
                #region �]�mSwagger������󤺮e�A��mAPI����²����T

                SWGENOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    // �����D
                    Title = "Main Web API",

                    // ���ԭz
                    Description = "A simple example ASP.NET Core Web API",

                    // �A�ȱ��ڡA�o�̥����O���}
                    TermsOfService = new Uri("https://example.com/terms"),

                    // �����s��
                    Contact = new OpenApiContact
                    {
                        Name = "Min Web Api",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },

                    // ���v��T
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                #endregion

                // XML ����
                // https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio#xml-comments
                // �o�϶��O�ھڦb�{�����i��XML�榡���ѡA�ͦ��@�������ѷ�XML���
                // �åB�bSwagger UI�������C��API���ئW�٫᭱�|�a�J������XML�������e�ԭz
                #region �t�mXML���ѻ���

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // ���wXML�ɮצW�٥H�M�צW�٨өR�W
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile); // ���w��b�M�ץؿ���m�U
                SWGENOptions.IncludeXmlComments(xmlPath);

                #endregion
            });

            // �ɻ�.net core�r���s�X�����A�קK�X�{�ýX�{�H
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(SWOptions =>
            {
                // �o�O�O�w��Swagger JSON�ɮ׸��Ѽ˦��t�m�A�Ъ`�N�����n�]�t'{documentName}'
                // �p�G���]�m�����ءA�����n�h��s�Aapp.UseSwaggerUI��'SwaggerEndpoint'�̭����Ѽ˦�
                // �p�G�S�]�m�����ءA�w�]���Ѭ� /swagger/{documentName}/swagger.json
                SWOptions.RouteTemplate = "/api-docs/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(SWUIOptions =>
            {
                // JSON�w�]���Ѭ�/swagger/v1/swagger.json
                SWUIOptions.SwaggerEndpoint(
                    "/api-docs/v1/swagger.json",
                    "Main Web Api API V1" // �o�̷|�򤶭��k�W��API�ﶵ����r�����p
                );

                // �[�J���q�N�w�]�ڥؿ��]�m��Swagger UI����
                SWUIOptions.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // �[�JDapr�bPulish/Subscribe Endpoint Handler
                // ���ݩ�Dapr�B�z��pub/sub�ШD�ɦV��Dapr�ۤv��runtime�h�B�z
                endpoints.MapSubscribeHandler();

                endpoints.MapControllers();

                // �]�wApi���Ѽ˦��ǰt�W�h
                endpoints.MapControllerRoute(
                    name: "ApiArea",
                    pattern: "{area:exists}/{controller}/{action}"
                );
            });
        }
    }
}
