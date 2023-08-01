using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HigherLower;

namespace Core
{
    /// <summary>
    /// Script zur Einstellung der Spieler Anzahl
    /// </summary>
    public class GameConfiguration : MonoBehaviour
    {
        [SerializeField] GameObject m_NamePanel;
        [SerializeField] Dropdown m_DropdownPlayerValue;

        int m_PlayerValue;

        private void Start()
        {
            m_PlayerValue = 1;
        }

        public void ChangePlayerValue(int value)
        {
            m_PlayerValue = value + 1;
        }

        public void ConfirmGameConfig()
        {
            GameMaster.instance.m_PlayersInt = m_PlayerValue;

            m_NamePanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
