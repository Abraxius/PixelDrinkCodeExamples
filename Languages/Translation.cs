using System.Collections;
using System.Collections.Generic;

namespace Languages
{
    /// <summary>
    /// 
    /// </summary>
    public enum Language
    {
        EN,
        DE
    }

    /// <summary>
    /// 
    /// </summary>
    public class Translation
    {
        private Dictionary<Language, string> m_Translations;

        public Translation()
        {
            m_Translations = new Dictionary<Language, string>();
        }

        public string GetTranslation(Language lang)
        {
            return m_Translations[lang];
        }

        public void AddTranslation(Language lang, string text)
        {
            m_Translations.Add(lang, text);
        }
    }
}

