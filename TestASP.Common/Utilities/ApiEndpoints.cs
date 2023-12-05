using System;
namespace TestASP.Common.Utilities
{
	public class ApiEndpoints
	{
        //public const string RootUrl = "https://localhost:7069/";

        public const string V1API = "v1/api";

        /// <summary> GET </summary>
        public const string LoginAuthUrl = V1API+"/Authentication/sign_in";
        /// <summary> POST ContentType: application/json,multipart/form-data </summary>
        public const string SignupAuthUrl = V1API + "/Authentication/sign_up";
        /// <summary> POST ContentType: multipart/form-data </summary>
        public const string SignupFormAuthUrl = V1API + "/Authentication/sign_up/form";
        /// <summary> GET </summary>
        public const string WeatherForecastUrl = V1API + "/WeatherForecast";
    
        /// <summary> GET </summary>
        public const string UserUrl = V1API+"/User";
    }
}

