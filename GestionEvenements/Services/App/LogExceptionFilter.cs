using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestionEvenements.Services.App
{
    public class LogExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        public LogExceptionFilter(IWebHostEnvironment hostingEnvironment)
        {
            _env = hostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            string infosLog = $"{DateTime.Now}{Environment.NewLine}{ex.GetType()}{Environment.NewLine}{ex.Message}{Environment.NewLine}";
            if (ex.GetBaseException() != ex)
            {
                infosLog += $",{ex.GetBaseException().GetType()},{ex.GetBaseException().Message}{Environment.NewLine}";
            }

            infosLog += $"Controller/Action : {context.RouteData.Values["controller"]}/{context.RouteData.Values["action"]}{Environment.NewLine}";

            if (ex.StackTrace != null)
            {
                string s = ex.StackTrace;
                int pos = s.IndexOf("at Microsoft", StringComparison.InvariantCultureIgnoreCase);
                infosLog += $"{s.Substring(0, pos - 1).Trim()}{Environment.NewLine}";
            }

            infosLog += "-".PadRight(150, '-') + Environment.NewLine;

            using var sw = new StreamWriter(Parametres.FichierLogEx, true);
            sw.Write(infosLog);
        }
    }
}
