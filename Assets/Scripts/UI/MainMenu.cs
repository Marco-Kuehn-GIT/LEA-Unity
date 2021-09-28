using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void ExitGame() {
        Application.Quit();
        Debug.Log("Game closed");
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
    }
}
