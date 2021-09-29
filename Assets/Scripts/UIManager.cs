using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour{

    public static UIManager Instance;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private Text debugText;
    [SerializeField] private CanvasGroup debugConsole;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        LogPhrase("test");
        LogPhrase( "testWithArgs", "User", "Test");
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Caret) || Input.GetKeyDown(KeyCode.Backslash)) {
            toggleDebugConsole();
        }
    }

    // Show/hide the debug console
    private void toggleDebugConsole() {
        if (debugConsole.alpha < 1) {
            debugConsole.alpha = 1;
        } else {
            debugConsole.alpha = 0;
        }
    }

    public static void LogMsg(string msg) {
        UIManager.Instance.debugText.text = msg + "\n" + UIManager.Instance.debugText.text;
    }

    public static void LogPhrase(string key, params string[] args) {
        LogMsg(Localization.GetPhrase(key, args));
    }

    public void SubmitDebugInput(InputField inputField) {
        if (inputField.text.StartsWith("/")) {
            String[] cmd = inputField.text.Substring(1).Split(' ');
            Debug.Log("cmd " + cmd[0]);
            switch (cmd[0]) {
                case "tp":
                    playerController.transform.position = new Vector3(int.Parse(cmd[1]), int.Parse(cmd[2]), 0);
                    break;
            }

            return;
        }
        LogMsg("Send: " + inputField.text);
        Networking.SendMsg(MSG_TYPE.CHAT, inputField.text);
        inputField.text = "";
    }
}
