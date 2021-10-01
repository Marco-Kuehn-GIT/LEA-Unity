using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void ExitGame() {
        Application.Quit();
        Debug.Log("Game closed");
    }
}
