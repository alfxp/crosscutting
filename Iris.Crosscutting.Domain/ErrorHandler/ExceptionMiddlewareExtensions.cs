using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Iris.Crosscutting.Domain.ErrorHandler
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var serviceProvider = context.Features.Get<IServiceProvidersFeature>();

                    var host = context.Request.Host;
                    var scheme = context.Request.Scheme;
                    var path = context.Request.Path;
                    var query = context.Request.QueryString;

                    var logger = serviceProvider.RequestServices.GetService<ILogger<ErrorDetails>>();

                    //TODO: CRIAR INSTANCIA DO SERVICO DE BUS
                    //var bus = serviceProvider.RequestServices.GetService<IServiceBus>();

                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        var errorResult = new ErrorResult
                        {
                            Url = $"{scheme}://{host}{path}{query}",
                            Errors = new[] {
                                "Erro ao processar requisição. Verifique os valores passados e tente novamente."
                            }
                        };

                        //TODO: CRIAR ROTINA PARA PUBLICAÇÃO NA FILA DE LOGS DE ERRO
                        //await bus.SendLog(
                        //    new ErrorDetails(errorResult, contextFeature.Error),
                        //    convertToJson: false
                        //);

                        await context.Response.WriteAsync(errorResult.ToString());
                    }
                });
            });
        }
    }
}