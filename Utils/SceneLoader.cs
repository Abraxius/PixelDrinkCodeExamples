using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;
using UI;
using Languages;

public class SceneLoader : MonoBehaviour
{
    public string m_Scene;
    [SerializeField] bool m_DisableScene;
    [SerializeField] bool m_WarningScreen;

    bool m_busy;

    public void MoreThan2Players()
    {
        if (GameMaster.instance.m_PlayersInt < 2)
        {
            
            string tmpTranslation = LanguageManager.instance.GetTranslation("MORE_PLAYER");
            MsgBox.instance.SendMessage("", tmpTranslation, 1.5f);    
        } else
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        if (m_DisableScene)
        {
            GetComponent<ComingSoon>().ComingSoonMsg();
        }
        else if (!m_busy)
        {
            m_busy = true;
            StopAllCoroutines();

            if (!m_WarningScreen)
                MsgBox.instance.StopMessagePermanent();

            SceneManager.LoadScene(m_Scene);
        }
    }
}
