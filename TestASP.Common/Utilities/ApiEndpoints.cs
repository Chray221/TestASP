using System;
using System.Text.RegularExpressions;

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

        #region questionnaire
        /// <summary> GET </summary>
        public const string Questionnaire = V1API + "/Questionnaire";
        /// <summary> POST </summary>
        public const string SaveQuestionnaire = V1API + "/Questionnaire/{questionId}/Save";
        #endregion

        public static string FromFormat(string url, params object[] objects)
        {
            return string.Format(ReFormatFormmatedUrl(url), objects);
        }

        private static string ReFormatFormmatedUrl(string url)
        {
            string newUrl = "";
            var urls = Regex.Split(url, @"(?:{\w+})");
            for (int index = 0; index < urls.Length; index++)
            {
                string extension = index + 1 < urls.Length ?$"{{{index}}}" : "";
                newUrl += $"{urls[index]}{extension}";
            }
            return newUrl;
        }
    }
}

