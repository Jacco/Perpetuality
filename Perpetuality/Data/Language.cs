using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Perpetuality.Data
{
    public class Language
    {
        public string Abbreviation;
        public string LocalName;

        public static Language[] languages;
        public static Language[] Languages
        {
            get
            {
                if (languages == null)
                {
                    languages = new[] {
                            new Language { Abbreviation = "en", LocalName = "English" }
                        ,   new Language { Abbreviation = "nl", LocalName = "Nederlands" }
                        ,   new Language { Abbreviation = "de", LocalName = "Deutsch" }
                        ,   new Language { Abbreviation = "it", LocalName = "Italiano" }
                        ,   new Language { Abbreviation = "es", LocalName = "Español" }
                        ,   new Language { Abbreviation = "fr", LocalName = "Français" }
                        ,   new Language { Abbreviation = "pt", LocalName = "Português" }
                    };
                }
                return languages;
            }
        }

        public static Language LanguageByAbbreviation(string abbreviation)
        {
            return Languages.Where(x => x.Abbreviation == abbreviation).SingleOrDefault();
        }
    }
}