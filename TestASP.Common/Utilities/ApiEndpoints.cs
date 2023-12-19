using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

namespace TestASP.Common.Utilities;

public struct ApiVersion
{
    ///<summary> v1/api </summary>
    public static ApiVersion Version1API = new ApiVersion("v1/api");
    ///<summary> v2/api </summary>
    public static ApiVersion Version2API = new ApiVersion("v2/api");

    private string Value { get; set; }
    private ApiVersion(string value)
    {
        Value = value;
    }
    public static explicit operator string(ApiVersion value) => value.Value;
    public static explicit operator ApiVersion(string value) => new ApiVersion(value);
}
[DebuggerDisplay($"{nameof(Value)}: {{{nameof(Value)}}}")]
public struct ApiEndpoint
{
    ///<summary> POST: Authentication/sign_in </summary>
    public static ApiEndpoint Login => new("Authentication/sign_in");

    ///<summary> PUT,POST: Authentication/sign_up </summary>
    public static ApiEndpoint Signup => new("Authentication/sign_up");

    ///<summary> POST: Authentication/sign_up/form | ContentType: multipart/form-data  </summary>
    public static ApiEndpoint SignupForm => new("Authentication/sign_up/form");

    ///<summary> GET: WeatherForecast </summary>
    public static ApiEndpoint WeatherForecastUrl => new("WeatherForecast");

    ///<summary> GET: User </summary>
    public static ApiEndpoint User => new("User");

    ///<summary> GET,POST: Questionnaire </summary>
    public static ApiEndpoint AdminQuestionnaire => new("Questionnaire");

    ///<summary> GET,PUT,DELETE: Questionnaire/{id} </summary>
    public static ApiEndpoint AdminQuestionnaireItem => new("Questionnaire/{id}");

    ///<summary> GET: User/{userId}/Questionnaire </summary>
    public static ApiEndpoint UserQuestionnaires => new("User/{userId}/Questionnaire");

    ///<summary> GET: User/{userId}/Questionnaire/Answer/{userQuestionnaireId} </summary>
    //public static ApiEndpoint UserQuestionnaireAnswer => new("User/{userId}/Questionnaire/Answer/{userQuestionnaireId}");

    ///<summary> GET,POST: User/{userId}/Questionnaire/{questionnaireId}/Answer </summary>
    public static ApiEndpoint SaveUserQuestionnaire => new("User/{userId}/Questionnaire/{questionnaireId}/Answer");
    ///<summary> GET,PUT,DELETE: User/{userId}/Questionnaire/{questionnaireId}/Answer/{userQuestionnaireId} </summary>
    public static ApiEndpoint UserQuestionnaireAnswer => new("User/{userId}/Questionnaire/{questionnaireId}/Answer/{userQuestionnaireId}");

    ///<summary> GET: Temp </summary>
    public static ApiEndpoint Temp => new("Temp");

    private string Value { get; set; }
    private ApiEndpoint(string value)
    {
        Value = value;
    }


    public static string FromFormat(ApiEndpoint url, params object[] objects)
    {
        return string.Format(ReFormatFormmatedUrl((string)url), objects);
    }

    public static string FromV1Format(ApiEndpoint url, params object[] objects)
    {
        return FromFormat(ApiVersion.Version1API, url, objects);
    }

    public static string FromFormat(ApiVersion version, ApiEndpoint url, params object[] urlObjects)
    {
        return string.Format(ReFormatFormmatedUrl((string)version+"/"+(string)url), urlObjects);
    }

    private static string ReFormatFormmatedUrl(string url)
    {
        string newUrl = "";
        var urls = Regex.Split(url, @"(?:{\w+})");
        for (int index = 0; index < urls.Length; index++)
        {
            string extension = index + 1 < urls.Length ? $"{{{index}}}" : "";
            newUrl += $"{urls[index]}{extension}";
        }
        return newUrl;
    }

    public static explicit operator string(ApiEndpoint value) => value.Value;
    public static explicit operator ApiEndpoint(string value) => new ApiEndpoint(value);
}   

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
    public const string AdminQuestionnaire = $"{V1API}/Questionnaire";
    /// <summary> GET </summary>
    public const string UserQuestionnaire = $"{V1API}/Questionnaire/Answer";
    /// <summary> GET </summary>
    public const string UserQuestionnaireAnswer = $"{V1API}/Questionnaire/Answer/{{userQuestionnaireId}}";
    /// <summary> POST </summary>
    public const string SaveUserQuestionnaire = $"{V1API}/Questionnaire/{{questionId}}/Answer";
    /// <summary> PUT,DELETE </summary>
    public const string UpdateUserQuestionnaire = $"{SaveUserQuestionnaire}/{{userQuestionnaireId}}";
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

