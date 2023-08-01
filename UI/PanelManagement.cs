using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManagement : MonoBehaviour
{
    public void NextScreen(GameObject nextScreen)
    {
        nextScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
