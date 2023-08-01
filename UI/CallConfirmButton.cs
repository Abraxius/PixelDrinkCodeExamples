using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class CallConfirmButton : MonoBehaviour
    {
        [SerializeField] string m_Text = "Bist du dir sicher, dass du zurück zum Hauptmenü möchtest?";
        [SerializeField] ConfirmMsgBox.ConfirmFunctions m_Function;

        public void OnButtonClick()
        {
            ConfirmMsgBox.instance.SetConfirmBox(m_Text, m_Function);
        }
    }

}
