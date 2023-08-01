using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Core;
using Game;
using UI;
using Languages;

public class GobletOfFireGameplay : MonoBehaviour
{
    [SerializeField] string[] m_ChalangeList = new string[14];

    [SerializeField] Image m_PulledCardGameObject;
    [SerializeField] Text m_Text;
    [SerializeField] Text m_NameText;
    [SerializeField] Button m_Button;
    [SerializeField] Core.Animation m_Animation;

    int m_CurrentPlayer;
    int m_GobletOfFireValue;
    bool m_GameEndSequence;

    #region Main Methods
    private void OnEnable()
    {
        InitGame();
    }

    private void Update()
    {
        if (m_GameEndSequence && (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)))
        {
            GameEndSequence();
        }
    }
    #endregion

    #region GameCore
    void InitGame()
    {
        m_PulledCardGameObject.sprite = Cards.instance.m_CardQuestionMark;

        m_GobletOfFireValue = 0;
        m_CurrentPlayer = 0;

        string tmpTranslation = LanguageManager.instance.GetTranslation("GAMEMASTER_1");
        m_NameText.text = GameMaster.instance.m_PlayerNames[m_CurrentPlayer] + tmpTranslation;
    }

    int m_Index;

    /// <summary>
    /// Wird per Button aufgerufen
    /// </summary>
    public void PutCardAndSetText()
    {
        int cardValue = Cards.instance.PullNewCardFromAll();

        //Verhindert das 2x hintereinander die selbe Karte kommt
        if (m_Index < 1)
        {
            m_Index += 1;
        } else
        {
            m_Index = 0;
            Cards.instance.ResetAllCards();
        }

        m_PulledCardGameObject.sprite = Cards.instance.GetCardSprite(cardValue);

        cardValue = Cards.instance.ReduceCardInt(cardValue);

        if (cardValue == 12)
        {
            if (m_GobletOfFireValue < 3)
            {
                m_GobletOfFireValue += 1;
            } 
            else
            {
                m_Text.text = "";
                StartCoroutine(WaitAndResetGame());
                return;
            }
        }

        if (m_CurrentPlayer < GameMaster.instance.m_PlayersInt - 1)
        {
            m_CurrentPlayer += 1;
        }
        else
        {
            m_CurrentPlayer = 0;
        }

        m_Text.text = LanguageManager.instance.GetTranslation(m_ChalangeList[cardValue]);

        string tmpTranslation = LanguageManager.instance.GetTranslation("GAMEMASTER_1");
        m_NameText.text = GameMaster.instance.m_PlayerNames[m_CurrentPlayer] + tmpTranslation;
    }

    void GameEndSequence()
    {
        m_GameEndSequence = false;

        MsgBox.instance.StopMessagePermanent();

        StopAllCoroutines();
        m_Animation.StopAllCoroutines();

        m_Animation.ResetChloe();

        InitGame();
        m_Button.interactable = true;
    }
    #endregion

    #region IEnumerators
    IEnumerator WaitAndResetGame()
    {
        m_Button.interactable = false;

        string tmpTranslation = LanguageManager.instance.GetTranslation("MSG_BOX_1");
        MsgBox.instance.StartMessagePermanent(GameMaster.instance.m_PlayerNames[m_CurrentPlayer] + tmpTranslation);

        yield return new WaitForSeconds(1f);
        m_Animation.StartDrunkChloe();
        yield return new WaitForSeconds(6f);
        m_Animation.StartDrunkChloe();
        m_GameEndSequence = true;
        yield return new WaitForSeconds(6f);
        m_Animation.StartDrunkChloe();
        yield return new WaitForSeconds(6f);
        m_Animation.StartDrunkChloe();
        yield return new WaitForSeconds(5f);

        GameEndSequence();
    }
    #endregion
}
