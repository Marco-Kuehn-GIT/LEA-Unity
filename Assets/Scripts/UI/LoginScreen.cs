using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginScreen : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] private InputField username;
    [SerializeField] private InputField password;

    public void LoginSave() {
        if (username.text.Equals("")  || password.text.Equals("")  ) {
            return;
        }
        PlayerPrefs.SetString("login", username.text + " " + password.text);
        SceneManager.LoadScene(1);
    }

}
