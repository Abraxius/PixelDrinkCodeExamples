using Languages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ConfirmMsgBox : MonoBehaviour
    {
        public static ConfirmMsgBox instance;

        [SerializeField] Text m_Text;
        [SerializeField] Button m_ConfirmButton;
        [SerializeField] Button m_BackButton;

        public enum ConfirmFunctions
        {
            MainMenu,
            Default
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                gameObject.SetActive(false);
            }
            else if (instance != null)
            {
                Destroy(gameObject);
            }
        }

        public void SetConfirmBox(string txt, ConfirmFunctions function)
        {
            gameObject.SetActive(true);

            m_Text.text = txt;
            m_Text.gameObject.GetComponent<LocalizedText>().Translate(LanguageManager.instance);

            m_BackButton.onClick.AddListener(BackFunction);

            switch (function)
            {
                case ConfirmFunctions.MainMenu:
                    m_ConfirmButton.onClick.AddListener(BackToHomeMenu);
                    break;
                default:
                    Debug.Log("Für diese Funktion ist nichs definiert");
                    break;
            }
        }

        void BackFunction()
        {
            gameObject.SetActive(false);

            //ToDo: Entferne auch wieder den hinzugefügten AddListener - variabel gestalten, wenn es mehrere gibt
            m_ConfirmButton.onClick.RemoveListener(BackToHomeMenu);
        }

        void BackToHomeMenu()
        {
            var component = gameObject.AddComponent<SceneLoader>();

            component.m_Scene = "MenuScene";
            component.LoadScene();

            m_ConfirmButton.onClick.RemoveListener(BackToHomeMenu);
            Destroy(component);

            gameObject.SetActive(false);
        }
    }

}
