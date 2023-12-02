using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TestASP.Common.Helpers
{
	public static class FormatValidator
	{
        public const string ValidEmail = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        public const string ValidPhoneNumber = "@\"^(0|\\+63)9\\d{9}$\")";

        public static bool IsValidEmail(string email)
        {
            bool invalid = false;
            if (String.IsNullOrEmpty(email))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                //strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                email = Regex.Replace(email, @"(@)(.+)$",
                    /*DomainMapper*/(match) =>
                                    {
                                        // IdnMapping class with default property values.
                                        IdnMapping idn = new IdnMapping();

                                        string domainName = match.Groups[2].Value;
                                        try
                                        {
                                            domainName = idn.GetAscii(domainName);
                                        }
                                        catch (ArgumentException)
                                        {
                                            invalid = true;
                                        }
                                        return match.Groups[1].Value + domainName;
                                    }
                , RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(email, ValidEmail, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }



        public static bool IsValidPhoneNumber(this string phoneEntry)
        {
            return Regex.IsMatch(phoneEntry, @"^(0|\+63)9\d{9}$");
        }
    }
}

