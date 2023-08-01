using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class Animation : MonoBehaviour
    {
        [HideInInspector] public bool busyHappy;
        [HideInInspector] public bool busyDrunk;

        //Chloe
        [SerializeField] Image m_Chloe;
        [SerializeField] Sprite m_ChloeHappy1;
        [SerializeField] Sprite m_ChloeHappy2;
        [SerializeField] Sprite m_ChloeDrunk1;
        [SerializeField] Sprite m_ChloeDrunk2;

        public void StartHappyChloe()
        {
            if (!busyHappy)
                StartCoroutine(HappyChloe());
        }

        public void StartDrunkChloe()
        {
            if (!busyDrunk)
                StartCoroutine(DrunkChloe());
        }

        public void ResetChloe()
        {
            m_Chloe.sprite = m_ChloeHappy1;
        }

        IEnumerator HappyChloe()
        {
            busyHappy = true;

            m_Chloe.sprite = m_ChloeHappy2;
            yield return new WaitForSeconds(1f);
            m_Chloe.sprite = m_ChloeHappy1;

            busyHappy = false;
        }

        IEnumerator DrunkChloe()
        {
            busyDrunk = true;

            m_Chloe.sprite = m_ChloeDrunk1;
            yield return new WaitForSeconds(1f);
            m_Chloe.sprite = m_ChloeDrunk2;
            yield return new WaitForSeconds(1f);
            m_Chloe.sprite = m_ChloeDrunk1;
            yield return new WaitForSeconds(1f);
            m_Chloe.sprite = m_ChloeDrunk2;
            yield return new WaitForSeconds(1f);
            m_Chloe.sprite = m_ChloeDrunk1;
            yield return new WaitForSeconds(1f);
            m_Chloe.sprite = m_ChloeHappy1;

            busyDrunk = false;
        }
    }
}

