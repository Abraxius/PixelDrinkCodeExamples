using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using Core;

namespace Languages
{
    /// <summary>
    /// This class is used as a persistent object. It loads a language CSV file that can be accessed to fetch translations
    /// </summary>
    public class LanguageManager : MonoBehaviour
    {
        #region Variables

        private Dictionary<string, Translation> m_LocalizedStrings = new Dictionary<string, Translation>();
        [SerializeField] string m_TranslationDir = "Languages/translations";

        LocalizedText[] m_TranslateTargets;

        // Set default language
        [SerializeField] Language m_DefaultLanguage = Language.EN;  //EN
        [SerializeField] Language m_CurrentLanguage = Language.EN;  //EN

        private static readonly Regex m_NewLineCleanup = new Regex(@"\\n+");
        private static readonly Regex m_WhitespaceRegEx = new Regex(@"\s+");

        public string[] m_TextLines;

        public static LanguageManager instance;
        [HideInInspector] public bool m_IsReady = false;

        #endregion

        #region Main Methods
        /// <summary>
        /// Starts loading translation file. 
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != null)
            {
                Destroy(gameObject);
            }

            FetchSystemLanguage();

            TextAsset translationFile = Resources.Load(m_TranslationDir) as TextAsset;
            Stream file = new MemoryStream(translationFile.bytes);
            if (file == null)
            {
                Debug.LogError(translationFile.name + "not found or not readable");
                return;
            }
            ProcessFile(file);

        }
        #endregion

        #region Helper Methods
        private void ProcessFile(Stream stream)
        {
            StreamReader fileReader = new StreamReader(stream);

            string line;
            int lineNum = 0;

            while ((line = fileReader.ReadLine()) != null)
            {
                lineNum++;
                if (lineNum == 1)
                    continue; // skip the first line with the headers

                ProcessTranslation(line);
            }

            fileReader.Dispose();
            m_IsReady = true;
            InitScene();
        }

        public void InitScene()
        {
            m_TranslateTargets = FindObjectsOfType<LocalizedText>();
            foreach (LocalizedText target in m_TranslateTargets)
            {
                target.Translate(this);
            }
        }

        public void ChangeLanguage(Language newLang)
        {
            m_CurrentLanguage = newLang;
            InitScene();
        }

        private void FetchSystemLanguage()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.German:
                    m_CurrentLanguage = Language.DE;
                    break;
                case SystemLanguage.English:
                    m_CurrentLanguage = Language.EN;
                    break;
                default:
                    m_CurrentLanguage = Language.EN;
                    break;
            }

            Debug.Log("Fetch language: " + m_CurrentLanguage);
        }


        private void ProcessTranslation(string line)
        {
            string[] lineData = line.Split(';');
            string key = lineData[0];

            try
            {
                // Add translations
                Translation translation = new Translation();
                translation.AddTranslation(Language.EN, Unescape(lineData[1]));
                translation.AddTranslation(Language.DE, Unescape(lineData[2]));

                m_LocalizedStrings.Add(key, translation);
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.Log("Error with " + key);
            }
        }

        private string Unescape(string v)
        {
            return m_NewLineCleanup.Replace(v, "\n");
        }

        public string GetTranslation(string key)
        {

            key = m_WhitespaceRegEx.Replace(key, "");
            string translation;

            try
            {
                translation = m_LocalizedStrings[key].GetTranslation(m_CurrentLanguage);
                if (string.IsNullOrEmpty(translation))
                    translation = m_LocalizedStrings[key].GetTranslation(m_DefaultLanguage);
            }
            catch (KeyNotFoundException e)
            {
                translation = "n/a";
            }

            return translation;
        }
        #endregion
    }
}