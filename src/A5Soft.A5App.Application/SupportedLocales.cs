using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace A5Soft.A5App.Application
{
    public static class SupportedLocales
    {
        /// <summary>
        /// A default locale for the application.
        /// </summary>
        public static readonly LocaleInfo Default = new LocaleInfo(new CultureInfo("en"), "us");

        /// <summary>
        /// All locales supported by the application.
        /// </summary>
        public static readonly IReadOnlyCollection<LocaleInfo> All = new List<LocaleInfo>()
        {
            Default,
            new LocaleInfo(new CultureInfo("lt"), "lt")
        }.AsReadOnly();

        /// <summary>
        /// Gets a locale for supported culture (of <see cref="Default"/> if the culture specified is not supported).
        /// </summary>
        /// <param name="culture"></param>
        public static LocaleInfo ToSupportedLocale(this CultureInfo culture)
        {
            if (null == culture) throw new ArgumentNullException(nameof(culture));

            var result = All.FirstOrDefault(l => 
                culture.Name.StartsWith(l.Name, StringComparison.OrdinalIgnoreCase));

            return result ?? Default;
        }

        /// <summary>
        /// Data about a locale that is supported by the application.
        /// </summary>
        public class LocaleInfo
        {
            internal LocaleInfo(CultureInfo culture, string stateCode)
            {
                Culture = culture;
                StateCode = stateCode;
            }

            /// <summary>
            /// a <see cref="CultureInfo"/> for the locale
            /// </summary>
            public CultureInfo Culture { get; }

            /// <summary>
            /// a two letter country code for the locale
            /// </summary>
            public string StateCode { get; }

            /// <summary>
            /// <see cref="CultureInfo.Name"/>
            /// </summary>
            public string Name 
                => Culture.Name;

            /// <summary>
            /// <see cref="CultureInfo.NativeName"/>
            /// </summary>
            public string NativeName
                => Culture.NativeName;

        }

    }
}
