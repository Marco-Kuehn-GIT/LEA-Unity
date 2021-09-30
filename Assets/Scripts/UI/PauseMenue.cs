using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenue : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Canvas;
    public void PauseQuit() {
        Networking.Instance.ws.Close();
        SceneManager.LoadScene(0);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Canvas.SetActive(!Canvas.activeSelf);
        }
    }
}
