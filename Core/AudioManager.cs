using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public AudioSource m_Music;

        void Awake()
        {
            m_Music = GetComponent<AudioSource>();

            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != null)
            {
                Destroy(gameObject);
            }
        }
    }
}