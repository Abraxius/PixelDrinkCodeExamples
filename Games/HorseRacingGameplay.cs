using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Core;
using Game;
using UI;
using Languages;

namespace HorseRacing
{
    public class HorseRacingGameplay : MonoBehaviour
    {
        #region Variables
        //Player Counter
        [HideInInspector] public int m_PlayersInt;
        [HideInInspector] public string[] m_PlayerNames;

        int[] m_Playerposition = new int[4];

        int m_LastMysteryCard = 6;

        int m_MaxCardValue;

        //GameObjects
        [SerializeField] Button[] m_Buttons;
        [SerializeField] Image m_PulledDeck;

        //Card and Player Rows Arrays
        [SerializeField] GameObject[] m_PlayerRows = new GameObject[4];
        [SerializeField] GameObject m_PenaltyRowGameObject;

        int[] m_PenaltyRow = new int[5];

        //Other Scripts
        Cards m_Cards;
        [SerializeField] Core.Animation m_Animation;

        //Sprites
        [SerializeField] Sprite[] m_HorseSprite;
        [SerializeField] Sprite m_Empty;

        //Touch Skip Animation and MsgBox
        bool m_GameEndSequence;

        bool m_Busy;

        #endregion

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
        public void InitGame()
        {
            m_Cards = Cards.instance;
            m_PlayersInt = GameMaster.instance.m_PlayersInt;
            m_PlayerNames = GameMaster.instance.m_PlayerNames;

            m_LastMysteryCard = 6;
            m_MaxCardValue = 0;
            InitPlayerRows();

            InitPenaltyRow();

            ShowGameStakeMessage();
        }

        void InitPlayerRows()
        {
            for (int i = 0; i < m_PlayersInt; i++)
            {
                m_Playerposition[i] = 7;

                m_MaxCardValue += 13;

                //Init Name Player
                m_PlayerRows[i].transform.GetChild(0).GetComponent<Text>().text = m_PlayerNames[i];
                m_PlayerRows[i].transform.GetChild(7).GetComponent<Image>().sprite = m_HorseSprite[i];
            }

            //Sperrt die Asse
            m_Cards.SetSpecialCardInUse(12);
            m_Cards.SetSpecialCardInUse(25);
            m_Cards.SetSpecialCardInUse(38);
            m_Cards.SetSpecialCardInUse(51);
        }

        void InitPenaltyRow()
        {
            for (int i = 2; i < 7; i++)
            {
                m_PenaltyRowGameObject.transform.GetChild(i).GetComponent<Image>().sprite = m_Cards.m_CardQuestionMark;
            }

            for (int i = m_PenaltyRow.Length - 1; i >= 0; i--)
            {
                m_PenaltyRow[i] = m_Cards.PullNewCardFromSpecialInterval(m_MaxCardValue);
            }
        }

        public void PutCardAndMoveTheHorse()
        {
            int cardNumber = m_Cards.PullNewCardFromSpecialInterval(m_MaxCardValue);
            int player = WhereIsTheRightPlayer(cardNumber);

            m_PulledDeck.sprite = m_Cards.GetCardSprite(cardNumber);

            m_PlayerRows[player].transform.GetChild(m_Playerposition[player]).GetComponent<Image>().sprite = m_Empty;
            m_Playerposition[player] -= 1;
            m_PlayerRows[player].transform.GetChild(m_Playerposition[player]).GetComponent<Image>().sprite = m_HorseSprite[player];

            //Wenn alle Spieler die letzte Randkarte erreicht haben wird diese umgedreht
            if (m_Playerposition.All(n => n <= m_LastMysteryCard))
            {
                m_PenaltyRowGameObject.transform.GetChild(m_LastMysteryCard).GetComponent<Image>().sprite = m_Cards.GetCardSprite(m_PenaltyRow[m_LastMysteryCard - 2]);

                StartCoroutine(MovePlayerBackCoroutine(m_PenaltyRow[m_LastMysteryCard - 2]));

                m_LastMysteryCard -= 1;
            } 
            else if (m_Playerposition[player] <= 1)
            {
                StartCoroutine(WaitAndResetGame(player));
            }
            else
            {
                if (!m_Busy)
                    StartCoroutine(AutoPlayCoroutine());
            }
        }

        void MovePlayerBack(int cardNumber)
        {
            int player = WhereIsTheRightPlayer(cardNumber);

            m_PlayerRows[player].transform.GetChild(m_Playerposition[player]).GetComponent<Image>().sprite = m_Empty;

            //Abfrage damit es nicht über die Startposition hinaus geht
            if (m_Playerposition[player] <= 6)
                m_Playerposition[player] += 1;

            m_PlayerRows[player].transform.GetChild(m_Playerposition[player]).GetComponent<Image>().sprite = m_HorseSprite[player];

            if (!m_Busy)
                StartCoroutine(AutoPlayCoroutine());
        }

        void ResetGame()
        {
            for (int a = 0; a < m_PlayersInt; a++)
            {
                for (int i = 1; i < 7; i++)
                {
                    m_PlayerRows[a].transform.GetChild(i).GetComponent<Image>().sprite = m_Cards.m_CardTransparent;
                }

                m_Playerposition[a] = 7;
            }

            m_PulledDeck.sprite = m_Cards.m_CardQuestionMark;

            m_Cards.ResetAllCards();
        }

        void GameEndSequence()
        {
            m_GameEndSequence = false;

            StopAllCoroutines();
            m_Animation.StopAllCoroutines();

            m_Animation.ResetChloe();

            ResetGame();
            ChangeButtonInteractable(false);

            InitGame();
        }
        #endregion

        #region Buttons
        public void ButtonCore()
        {
            MsgBox.instance.StopMessagePermanent();

            ChangeButtonInteractable(true);

            PutCardAndMoveTheHorse();
        }
        #endregion

        #region Helper Methods
        int WhereIsTheRightPlayer(int cardNumber)
        {
            int player = 0;

            if (cardNumber < 13)
            {
                player = 0;
            }
            else if (cardNumber < 26)
            {
                player = 1;
            }
            else if (cardNumber < 39)
            {
                player = 2;
            }
            else if (cardNumber < 52)
            {
                player = 3;
            }

            return player;
        }

        void ChangeButtonInteractable(bool busy)
        {
            if (busy)
            {
                foreach (Button _button in m_Buttons)
                {
                    _button.interactable = false;
                }
            }
            else
            {
                foreach (Button _button in m_Buttons)
                {
                    _button.interactable = true;
                }
            }
        }

        private void ShowGameStakeMessage()
        {
            string tmpTranslation = LanguageManager.instance.GetTranslation("MSG_BOX_2");
            MsgBox.instance.StartMessagePermanent(tmpTranslation);
        }
        #endregion


        #region IEnumerators
        IEnumerator MovePlayerBackCoroutine(int cardNumber)
        {
            yield return new WaitForSeconds(1f);
            MovePlayerBack(cardNumber);
        }

        IEnumerator AutoPlayCoroutine()
        {
            yield return new WaitForSeconds(1f);
            PutCardAndMoveTheHorse();

            m_Busy = false;
        }

        IEnumerator WaitAndResetGame(int player)
        {
            string tmpTranslation = LanguageManager.instance.GetTranslation("MSG_BOX_3");
            MsgBox.instance.SendMessage(m_PlayerNames[player], tmpTranslation, 20f);
    
            yield return new WaitForSeconds(1f);
            m_Animation.StartDrunkChloe();
            yield return new WaitForSeconds(6f);
            m_GameEndSequence = true;
            m_Animation.StartDrunkChloe();
            yield return new WaitForSeconds(6f);
            m_Animation.StartDrunkChloe();
            yield return new WaitForSeconds(6f);

            yield return new WaitForSeconds(2f);

            GameEndSequence();
        }


        #endregion
    }

}
