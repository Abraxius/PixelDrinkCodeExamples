using Languages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ComingSoon : MonoBehaviour
    {
        public void ComingSoonMsg()
        {
            string tmpTranslation = LanguageManager.instance.GetTranslation("COMING_SOON");
            MsgBox.instance.SendMessage("", tmpTranslation, 1.5f);
        }
    }

}
