using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Core
{
    public class Cards : MonoBehaviour
    {
        public static Cards instance;

        //Any Card only one
        bool[] m_CardInUseArray = new bool[52];

        //Kartendecks
        public Sprite[] m_PicCards;
        public Sprite m_CardTransparent;
        public Sprite m_CardQuestionMark;

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

        /// <summary>
        /// Für das normale strukturierte Random Karten ziehen bei zB Höher Niedriger Gleich
        /// </summary>
        /// <returns></returns>
        public int PullNewCard()
        {
            int reviewedCardInt = -1;

            //Check is the card used
            while (reviewedCardInt == -1)
            {
                int randomCardInt = RandomUtils.RandomNumber(0, 13);
                reviewedCardInt = IsTheCardFree(randomCardInt);
            }

            m_CardInUseArray[reviewedCardInt] = true;
            return reviewedCardInt;
        }

        public int PullNewCardFromAll()
        {
            int reviewedCardInt = -1;

            //Check is the card used
            while (reviewedCardInt == -1)
            {
                int randomCardInt = RandomUtils.RandomNumber(0, 52);

                if (!m_CardInUseArray[randomCardInt])
                    reviewedCardInt = randomCardInt;
            }

            m_CardInUseArray[reviewedCardInt] = true;
            return reviewedCardInt;
        }

        public int PullNewCardFromSpecialInterval(int maxValue)
        {
            int reviewedCardInt = -1;

            //Check is the card used
            while (reviewedCardInt == -1)
            {
                int randomCardInt = RandomUtils.RandomNumber(0, maxValue);

                if (!m_CardInUseArray[randomCardInt])
                    reviewedCardInt = randomCardInt;
            }

            m_CardInUseArray[reviewedCardInt] = true;
            return reviewedCardInt;
        }

        public void ResetAllCards()
        {
            for (int i = 0; i < m_CardInUseArray.Length; i++)
            {
                m_CardInUseArray[i] = false;
            }
        }

        public void SetSpecialCardInUse(int cardValue)
        {
            m_CardInUseArray[cardValue] = true;
        }

        public void PutCardBackToStack(int cardValue)
        {
            m_CardInUseArray[cardValue] = false;
        }

        public Sprite GetCardSprite(int cardValue)
        {
            return m_PicCards[cardValue];
        }

        /// <summary>
        /// Rechnet die Karten auf ihre eigentliche Größe runter, nicht auf ihre Array Position
        /// </summary>
        /// <returns></returns>
        public int ReduceCardInt(int cardNumber)
        {

            if (cardNumber > 38)
            {
                cardNumber -= 39;
            }
            else if (cardNumber > 25)
            {
                cardNumber -= 26;
            }
            else if (cardNumber > 12)
            {
                cardNumber -= 13;
            }

            return cardNumber;
        }

        public int IsTheCardFree(int cardValue)
        {
            if (m_CardInUseArray[cardValue])
            {
                if (m_CardInUseArray[cardValue + 13])
                {
                    if (m_CardInUseArray[cardValue + 13 * 2])
                    {
                        if (m_CardInUseArray[cardValue + 13 * 3])
                        {
                            return -1;
                        }
                        else
                        {
                            return cardValue + 13 * 3;
                        }
                    }
                    else
                    {
                        return cardValue + 13 * 2;
                    }
                }
                else
                {
                    return cardValue + 13;
                }
            }
            else
            {
                return cardValue;
            }
        }
    }

}
