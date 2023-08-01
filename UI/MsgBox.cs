using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class MsgBox : MonoBehaviour
    {
        public static MsgBox instance;

        [SerializeField] Text m_Text;
        bool busy;
        string m_Gaps;

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

        public void StartMessagePermanent(string txt)
        {
            gameObject.SetActive(true);
            m_Text.text = txt;
        }

        public void StopMessagePermanent()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
            busy = false;
        }

        public void SendMessage(string player, string txt , float time)
        {
            gameObject.SetActive(true);
            StartCoroutine(SendMessageCoroutine(player, txt, time));
        }

        IEnumerator SendMessageCoroutine(string player, string txt, float time)
        {
            if (player == "")
                m_Text.text = txt;
            else 
                m_Text.text = player + " " + txt;

            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }
    }

}
