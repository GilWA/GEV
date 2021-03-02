using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace GestionEvenements.Services.App
{
    public class Parametres
    {
        private static IConfiguration _config;
        public static void Init(IConfiguration configuration)
        {
            _config = configuration;
            PathApp = _config.GetValue<string>(WebHostDefaults.ContentRootKey);
        }
        public static bool EnvironnementDev { get; set; }
        public static string UserIdDev => "UserIdDev";
        public static string PathApp { private set; get; }
        static string _pathVirtuelData;
        static string _pathData;

        public static string PathData
        {
            get
            {
                if (string.IsNullOrEmpty(_pathVirtuelData))
                {
                    _pathVirtuelData = _config.GetSection("Parametres").GetValue(typeof(string), "PathVirtuelData").ToString();
                }
                if (string.IsNullOrEmpty(_pathData))
                {
                    _pathData = Path.Combine(PathApp, "wwwroot", _pathVirtuelData);
                }
                return _pathData;
            }
        }

        static string _fichierLogEx;
        public static string FichierLogEx
        {
            get
            {
                if (string.IsNullOrEmpty(_fichierLogEx))
                {
                    _fichierLogEx = _config.GetSection("Parametres").GetValue(typeof(string), "FichierLogEx").ToString();
                }

                _fichierLogEx = Path.Combine(PathData, _fichierLogEx);

                return _fichierLogEx;
            }
        }
    }
}
