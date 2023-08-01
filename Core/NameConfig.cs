using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using HigherLower;
using UnityEngine.SceneManagement;
using UI;
using Languages;

namespace Core
{
    public class NameConfig : MonoBehaviour
    {
        [HideInInspector] public string[] m_PlayerNames;

        [SerializeField] GameObject[] m_InputFields;

        private void OnEnable()
        {
            for (int i = 0; i < GameMaster.instance.m_PlayersInt; i++)
            {
                m_InputFields[i].SetActive(true);
            }

            m_PlayerNames = new string[GameMaster.instance.m_PlayersInt];

            GetOldNames();
        }

        void GetOldNames()
        {
            for (int i = 0; i < m_PlayerNames.Length; i++)
            {
                string tmpName = PlayerPrefs.GetString("player" + i);

                if (tmpName == "")
                    continue;

                m_PlayerNames[i] = tmpName;
                m_InputFields[i].GetComponent<InputField>().text = tmpName;
            }
        }

        void SetOldNames()
        {
            for (int i = 0; i < m_PlayerNames.Length; i++)
            {
                PlayerPrefs.SetString("player" + i, m_PlayerNames[i]);
            }
        }

        public void ChangePlayerOneName(string text)
        {
            m_PlayerNames[0] = CutString(text);
        }

        public void ChangePlayerTwoName(string text)
        {
            m_PlayerNames[1] = CutString(text);
        }

        public void ChangePlayerThreeName(string text)
        {
            m_PlayerNames[2] = CutString(text);
        }

        public void ChangePlayerFourName(string text)
        {
            m_PlayerNames[3] = CutString(text);
        }

        string CutString(string value)
        {
            if (value.Length > 6)
            {
                char[] source = value.ToCharArray();
                char[] dest = new char[6];
                Array.Copy(source, dest, 6);

                string newStr = new string(dest);

                return newStr;
            }
            else
            {
                return value;
            }
        }

        public void ConfirmNameConfigs()
        {
            if (m_PlayerNames.Any(x => x == null))
            {
                string tmpTranslation = LanguageManager.instance.GetTranslation("NAME_NOT_COMPLETED");
                MsgBox.instance.SendMessage("", tmpTranslation, 2f);
            }
            else
            {
                GameMaster.instance.m_PlayerNames = m_PlayerNames;

                SetOldNames();

                GetComponent<SceneLoader>().LoadScene();
            }
        }
    }
}
