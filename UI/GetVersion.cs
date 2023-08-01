using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GetVersion : MonoBehaviour
    {
        [SerializeField] Text m_Text;

        private void OnEnable()
        {
            m_Text.text = "Version: " + Application.version;
        }
    }

}
