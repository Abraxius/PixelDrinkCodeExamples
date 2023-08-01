using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameMaster : MonoBehaviour
    {
        public int m_PlayersInt;
        public string[] m_PlayerNames;

        public static GameMaster instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else if (instance != null)
            {
                Destroy(this);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
