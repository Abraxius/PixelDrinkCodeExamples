using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class WarningScreen : MonoBehaviour
    {
        bool busy;

        private void Start()
        {
            StartCoroutine(m_Wait());
        }

        void Update()
        {
            if ((Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)))
            {
                if (!busy)
                    Confirm();
            }
        }

        private void Confirm()
        {
            busy = true;

            GetComponent<SceneLoader>().LoadScene();
        }

        IEnumerator m_Wait()
        {
            yield return new WaitForSeconds(5f);
            
            if (!busy)
                Confirm();
        }
    }

}
