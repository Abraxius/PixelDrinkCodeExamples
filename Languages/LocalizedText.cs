using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Languages
{
    /// <summary>
    /// Attached to each UI Text element to be translated - automatically fetches the correct translation string
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LocalizedText : MonoBehaviour
    {
        #region Variables
        protected Text m_TextComp;
        protected string m_StringKey;
        bool m_IsTranslated = false;
        #endregion

        #region Main Methods
        private void Awake()
        {
            m_TextComp = GetComponent<Text>();
        }

        public void OnEnable()
        {
            if (LanguageManager.instance == null)
                return;

            if (!m_IsTranslated && LanguageManager.instance.m_IsReady)
            {            
                Translate(LanguageManager.instance);
            }
        }
        #endregion

        #region Helper Methods
        public virtual void Init()
        {
            m_StringKey = m_TextComp.text;
        }

        public void Translate(LanguageManager manager)
        {
            if (m_StringKey == null)
                Init();

            if (m_StringKey == "")
                return;

            m_TextComp.text = manager.GetTranslation(m_StringKey);

            m_IsTranslated = true;
        }
        #endregion
    }
}

