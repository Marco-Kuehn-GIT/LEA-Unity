using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour{

    public static UIManager Instance;

    [SerializeField] private Text debugText;
    [SerializeField] private CanvasGroup debugConsole;

    private string debugOpenCode = "bmarvinb";
    private int curDebugOpenCodePos = 0;

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
        LogMsg("Send: " + inputField.text);
        WebSocketDemo.SendMsg(MSG_TYPE.CHAT, inputField.text);
        inputField.text = "";
    }
}
