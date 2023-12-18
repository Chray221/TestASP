using System;
using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace TestASP.Common.Extensions
{ 
    public static class StringExtension
	{
        #region Strings
        public static bool EqualsLower(this string str, string value, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            return str.ToLower().Equals(value?.ToLower(), stringComparison);
        }
        public static bool ContainsLower(this string str, string value)
        {
            return str.ToLower().Contains(value.ToLower());
        }
        public static bool ContainsLower(this IEnumerable<string> strs, string value)
        {
            return strs.Select( str => str.ToLower()).Contains(value?.ToLower());
        }
        #endregion

        #region Date Time
        const int DaysInAYear = 365;
        const int DaysInAMonth = 30;
        const int HourInADay = 24;
        const int MinuteInAnHour = 60;
        const int SecondInAMinute = 60;
        #endregion

        public static string GetTimelapse(this DateTime dateTime)
        {
            var timelapse = DateTime.Now.Subtract(dateTime);
            int timelapseNum = 0;
            if (timelapse.TotalDays > DaysInAYear)
            {
                timelapseNum = timelapse.Days / DaysInAYear;
                return $"{timelapseNum} year{(timelapseNum > 1 ? "s" : "")} ago";
            }
            else if (timelapse.TotalDays > DaysInAMonth)
            {
                timelapseNum = timelapse.Days / DaysInAMonth;
                return $"{timelapseNum} month{(timelapseNum > 1 ? "s" : "")} ago";
            }
            else if (timelapse.TotalHours > HourInADay)
            {
                timelapseNum = (int)(timelapse.TotalHours / HourInADay);
                return $"{timelapseNum} hour{(timelapseNum > 1 ? "s" : "")} ago";
            }
            else if (timelapse.TotalMinutes > MinuteInAnHour)
            {
                timelapseNum = (int)(timelapse.TotalMinutes / MinuteInAnHour);
                return $"{timelapseNum} min{(timelapseNum > 1 ? "s" : "")} ago";
            }

            return $"{(int)timelapse.TotalSeconds} second ago";
        }

        /// <summary>
        /// append a string at start
        /// <br>e.g</br>
        /// <br>"Arr" => "<paramref name="propertyName"/>.Arr"</br>
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IEnumerable<string> AppendPropertyName(this IEnumerable<string> strings, string propertyName)
        {
            return strings.Select(memName => $"{propertyName}.{memName}");
        }

        /// <summary>
        /// e.g   
        /// MemberNames = "ErrorPropertyName" => "<paramref name="propertyName"/>.ErrorPropertyName"
        /// </summary>
        /// <param name="validationResult"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static ValidationResult ToParentValidationResult(this ValidationResult validationResult, string propertyName)
        {
            return new ValidationResult(validationResult.ErrorMessage,
                                        validationResult.MemberNames.AppendPropertyName(propertyName));
        }
    }
}

