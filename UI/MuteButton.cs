using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.UI;

namespace UI
{
    public class MuteButton : MonoBehaviour
    {
        bool m_PlaySound;
        int m_PlaySoundInt;

        [SerializeField] Sprite[] m_UiSprites;

        void Start()
        {
            m_PlaySoundInt = PlayerPrefs.GetInt("music");

            if (m_PlaySoundInt == 0)
            {
                m_PlaySound = true;
                GetComponent<Image>().sprite = m_UiSprites[0];
            } else
            {
                m_PlaySound = false;
                GetComponent<Image>().sprite = m_UiSprites[1];
                AudioManager.instance.m_Music.mute = !AudioManager.instance.m_Music.mute;
            }
        }

        public void EnterMuteButton()
        {
            AudioManager.instance.m_Music.mute = !AudioManager.instance.m_Music.mute;
            m_PlaySound = !m_PlaySound;

            if (m_PlaySound)
                m_PlaySoundInt = 0;
            else
                m_PlaySoundInt = 1;

            GetComponent<Image>().sprite = m_UiSprites[m_PlaySoundInt];
            PlayerPrefs.SetInt("music", m_PlaySoundInt);
        }
    }
}
