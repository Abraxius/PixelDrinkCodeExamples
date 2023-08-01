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

namespace HigherLower
{
    public class HigherLowerGameplay : MonoBehaviour
    {
        #region Variables
        //Player Counter
        [HideInInspector] public int m_PlayersInt;
        [HideInInspector] public string[] m_PlayerNames;
        [HideInInspector] public int PlayerCount = 0;

        //GameObjects
        [SerializeField] GameObject m_PlayerRowPrefab;
        [SerializeField] GameObject m_GameRowsParent;
        [SerializeField] Button[] m_Buttons;

        //Card and Player Rows Arrays
        int[] m_MasterCards;
        [SerializeField] GameObject m_MasterRow;
        [SerializeField] GameObject[] m_PlayerRows = new GameObject[4];

        //Stacks
        Stack<int> PlayerOne = new Stack<int>();
        Stack<int> PlayerTwo = new Stack<int>();
        Stack<int> PlayerThree = new Stack<int>();
        Stack<int> PlayerFour = new Stack<int>();

        Stack<int> CurrentPlayer = new Stack<int>();
                                                               
        //Other Scripts
        Cards m_Cards;
        [SerializeField] Core.Animation m_Animation;

        [SerializeField] GameObject m_Marker;
        [SerializeField] GameObject m_MarkerMasterline;

        //Background Images
        [SerializeField] GameObject m_BackgroundImageLines;

        //Touch Skip Animation and MsgBox
        bool m_GameEndSequence;
        #endregion


        #region Main Methods
        private void Update()
        {
            if (m_GameEndSequence && (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)))
            {
                GameEndSequence();
            }
        }

        private void OnEnable()
        {
            InitGame();
        }

        #endregion
        #region GameCore

        public void InitGame()
        {
            m_Cards = Cards.instance;
            m_PlayersInt = GameMaster.instance.m_PlayersInt;
            m_PlayerNames = GameMaster.instance.m_PlayerNames;

            InitPlayerRows();

            InitMasterRow();
        }

        void InitPlayerRows()
        {
            m_MasterCards = new int[6];

            for (int i = 0; i < m_PlayersInt; i++)
            {
                //Init Name Player
                m_PlayerRows[i].SetActive(true);
                m_PlayerRows[i].transform.GetChild(0).GetComponent<Text>().text = m_PlayerNames[i];
                m_BackgroundImageLines.transform.GetChild(i + 1).gameObject.SetActive(true);
            }
        }

        void InitMasterRow()
        {
            m_MasterRow.transform.GetChild(0).GetComponent<Text>().text = "";
            for (int i = 1; i < 6; i++)
            {
                m_MasterCards[i] = m_Cards.PullNewCard();

                if (i == 5)
                    m_MasterRow.transform.GetChild(i).GetComponent<Image>().sprite = m_Cards.m_CardQuestionMark;
                else
                    m_MasterRow.transform.GetChild(i).GetComponent<Image>().sprite = m_Cards.m_PicCards[m_MasterCards[i]];
            }

            m_BackgroundImageLines.transform.GetChild(0).gameObject.SetActive(true);

            m_MarkerMasterline.transform.position = m_MasterRow.transform.GetChild(0 + 1).transform.position + new Vector3(0, 130, 0);
        }

        /// <summary>
        /// Zählt den nächsten Spieler auf, maximal aber die max Spieleranzahl
        /// </summary>
        void NextPlayerCount()
        {
            if (PlayerCount < m_PlayersInt - 1)
            {
                PlayerCount += 1;
            }
            else
            {
                PlayerCount = 0;
            }

            //Viseulle Hilfe wer an der Reihe ist
            for (int i = 0; i < m_PlayersInt; i++)
                m_PlayerRows[i].transform.GetChild(0).GetComponent<Text>().fontStyle = FontStyle.Normal;

            m_PlayerRows[PlayerCount].transform.GetChild(0).GetComponent<Text>().fontStyle = FontStyle.Bold;
            SetPlayerMarker(PlayerCount); 
        }

        void GameEndSequence()
        {
            m_GameEndSequence = false;

            StopAllCoroutines();
            m_Animation.StopAllCoroutines();

            m_Animation.ResetChloe();

            MsgBox.instance.StopMessagePermanent();

            ResetGame();
            ChangeButtonInteractable(false);
        }
        #endregion

        #region Buttons
        void ButtonCore()
        {
            ChangeButtonInteractable(true);

            CurrentPlayer = LinkCurrentPlayer();

            CurrentPlayer.Push(m_Cards.PullNewCard());
            m_PlayerRows[PlayerCount].transform.GetChild(CurrentPlayer.Count).GetComponent<Image>().sprite = m_Cards.m_PicCards[CurrentPlayer.Peek()];

            //Markiert die nächste relevante Karte in der Master Reihe
            var NextPlayer = LinkNextPlayer();
            m_MarkerMasterline.transform.position = m_MasterRow.transform.GetChild(NextPlayer.Count + 1).transform.position + new Vector3(0, 130, 0);

            //Abfrage ob die geheime Zahl erreicht wurde
            if (CurrentPlayer.Count == 5)
            {
                m_MasterRow.transform.GetChild(5).GetComponent<Image>().sprite = m_Cards.m_PicCards[m_MasterCards[5]];
            }
        }

        void RightCard(int player)
        {
            m_Animation.StartHappyChloe();

            if (CurrentPlayer.Count == 5)
            {
                    StartCoroutine(WaitAndResetGame(player));
            }
            else
            {
                ChangeButtonInteractable(false);
            }
        }

        public void ButtonHigher()
        {
            ButtonCore();

            if (!IfCardHigher(CurrentPlayer.Peek(), m_MasterCards[CurrentPlayer.Count]))
            {
                StartCoroutine(WaitAndRemove(PlayerCount));
            }
            else
            {
                RightCard(PlayerCount);
            }

            NextPlayerCount();
        }

        public void ButtonLower()
        {
            ButtonCore();

            if (!IfCardLower(CurrentPlayer.Peek(), m_MasterCards[CurrentPlayer.Count]))
            {
                StartCoroutine(WaitAndRemove(PlayerCount));
            }
            else
            {
                RightCard(PlayerCount);
            }

            NextPlayerCount();
        }

        public void ButtonSame()
        {
            ButtonCore();

            if (!IfCompareCards(CurrentPlayer.Peek(), m_MasterCards[CurrentPlayer.Count]))
            {
                StartCoroutine(WaitAndRemove(PlayerCount));
            }
            else
            {
                RightCard(PlayerCount);
            }

            NextPlayerCount();
        }
        #endregion

        #region Helper Methods
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

        ref Stack<int> LinkCurrentPlayer()
        {
            switch (PlayerCount)
            {
                case 0:
                    return ref PlayerOne;
                case 1:
                    return ref PlayerTwo;
                case 2:
                    return ref PlayerThree;
                case 3:
                    return ref PlayerFour;
                default:
                    Debug.Log("Error to high PlayerCount");
                    return ref PlayerOne;
            }
        }

        //ToDo: die beiden Stack Methoden in eine Kombinieren
        ref Stack<int> LinkNextPlayer()
        {
            int _tmpPlayerCount = 0;

            if ((PlayerCount + 1) < m_PlayersInt)
                _tmpPlayerCount = PlayerCount + 1;

            switch (_tmpPlayerCount)
            {
                case 0:
                    return ref PlayerOne;
                case 1:
                    return ref PlayerTwo;
                case 2:
                    return ref PlayerThree;
                case 3:
                    return ref PlayerFour;
                default:
                    Debug.Log("Error to high PlayerCount");
                    return ref PlayerOne;
            }
        }

        void ClearRow(int player)
        {
            for (int i = 1; i < 6; i++)
            {
                m_PlayerRows[player].transform.GetChild(i).GetComponent<Image>().sprite = m_Cards.m_CardTransparent;
            }

            //Fügt die Karte dem Stabel wieder hinzu
            while (CurrentPlayer.Count > 0)
            {
                m_Cards.PutCardBackToStack(CurrentPlayer.Pop());
            }

            if (m_PlayersInt == 1)
            {
                //Markiert die nächste relevante Karte in der Master Reihe (Bei einem Spieler extra Regeln, sonst Fehler)
                var NextPlayer = LinkNextPlayer();
                m_MarkerMasterline.transform.position = m_MasterRow.transform.GetChild(NextPlayer.Count + 1).transform.position + new Vector3(0, 130, 0);

            }
        }

        public void ChangePlayerNumber(int value)
        {
            m_PlayersInt = value;
        }

        void ResetGame()
        {
            for (int a = 0; a < m_PlayersInt; a++)
            {
                for (int i = 1; i < 6; i++)
                {
                    m_PlayerRows[a].transform.GetChild(i).GetComponent<Image>().sprite = m_Cards.m_CardTransparent;
                }
            }

            PlayerOne.Clear();
            PlayerTwo.Clear();
            PlayerThree.Clear();
            PlayerFour.Clear();

            m_Cards.ResetAllCards();
            
            InitMasterRow();

            PlayerCount = -1;

            NextPlayerCount();
        }
        
        void SetPlayerMarker(int player)
        {
            m_Marker.transform.parent = m_GameRowsParent.transform.parent;
            m_Marker.transform.position = m_PlayerRows[player].transform.GetChild(0).transform.position + new Vector3(77, 0, 0);
        }
        #endregion

        #region CardCompares
        /// <summary>
        /// Gibt einen bool zurück ob die Karten übereinstimmen oder nicht
        /// </summary>
        /// <returns></returns>
        bool IfCompareCards(int playerCard, int masterCard)
        {
            int _playerCard = m_Cards.ReduceCardInt(playerCard);
            int _masterCard = m_Cards.ReduceCardInt(masterCard);

            Debug.Log(_playerCard + " " + _masterCard);

            if (_playerCard != _masterCard)
                return false;
            else
                return true;
        }

        bool IfCardHigher(int playerCard, int masterCard)
        {
            int _playerCard = m_Cards.ReduceCardInt(playerCard);
            int _masterCard = m_Cards.ReduceCardInt(masterCard);

            Debug.Log(_playerCard + " " + _masterCard);

            if (_playerCard > _masterCard)
                return true;
            else
                return false;
        }

        bool IfCardLower(int playerCard, int masterCard)
        {
            int _playerCard = m_Cards.ReduceCardInt(playerCard);
            int _masterCard = m_Cards.ReduceCardInt(masterCard);

            Debug.Log(_playerCard + " " + _masterCard);

            if (_playerCard < _masterCard)
                return true;
            else
                return false;
        }
        #endregion

        #region IEnumerators
        IEnumerator WaitAndRemove(int player)
        {
            if (CurrentPlayer.Count == 1)
            {
                string tmpTranslation = LanguageManager.instance.GetTranslation("MSG_BOX_4");
                string tmpTranslation2 = LanguageManager.instance.GetTranslation("MSG_BOX_5");
                MsgBox.instance.SendMessage(m_PlayerNames[player], tmpTranslation + " " + CurrentPlayer.Count + tmpTranslation2, 3f);
            }
            else
            {
                string tmpTranslation = LanguageManager.instance.GetTranslation("MSG_BOX_4");
                string tmpTranslation2 = LanguageManager.instance.GetTranslation("MSG_BOX_6");
                MsgBox.instance.SendMessage(m_PlayerNames[player], tmpTranslation + CurrentPlayer.Count + tmpTranslation2, 3f);
            }

            m_Animation.StartDrunkChloe();

            yield return new WaitForSeconds(4f);

            if (CurrentPlayer.Count == 5)
            {
                m_MasterCards[5] = m_Cards.PullNewCard();
                m_MasterRow.transform.GetChild(5).GetComponent<Image>().sprite = m_Cards.m_CardQuestionMark;
            }

            ClearRow(player);

            ChangeButtonInteractable(false);
        }

        IEnumerator WaitAndResetGame(int player)
        {
            string tmpTranslation = LanguageManager.instance.GetTranslation("MSG_BOX_7");
            MsgBox.instance.SendMessage(m_PlayerNames[player], tmpTranslation, 20f);

            yield return new WaitForSeconds(1f);
            m_Animation.StartDrunkChloe();
            yield return new WaitForSeconds(6f);
            m_GameEndSequence = true;
            m_Animation.StartDrunkChloe();
            yield return new WaitForSeconds(6f);
            m_Animation.StartDrunkChloe();
            yield return new WaitForSeconds(6f);
            m_Animation.StartDrunkChloe();
            yield return new WaitForSeconds(5f);

            GameEndSequence();
        }
        #endregion
    }

}
