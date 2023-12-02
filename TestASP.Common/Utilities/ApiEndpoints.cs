using System;
namespace TestASP.Common.Utilities
{
	public class ApiEndpoints
	{
        //public const string RootUrl = "https://localhost:7069/";

        public const string V1API = "v1/api";

        /// <summary> GET </summary>
        public const string LoginAuth = V1API+"/Authentication/sign_in";
        public const string SignupAuth = V1API + "/Authentication/sign_up";
        public const string SignupFormAuth = V1API + "/Authentication/sign_up/form";
        /// <summary> GET </summary>
        public const string WeatherForecast = V1API + "/WeatherForecast";
        
    }
}

