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


    [SerializeField] private CanvasGroup chatGroup;
    [SerializeField] private Image[] chatBgImgs;
    [SerializeField] private Mask chatMask;
    [SerializeField] private RectTransform chatRect;
    [SerializeField] private Text chatText;
    [SerializeField] private InputField chatInput;
    private int chatLength = 1;

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
        if (!chatGroup.interactable && Input.GetKeyDown(KeyCode.T)) {
            setChatActive(!chatGroup.interactable);
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

    private void setChatActive(bool active) {
        chatGroup.interactable = active;
        foreach (Image img in chatBgImgs) {
            img.enabled = active;
        }
        chatMask.enabled = !active;
    }

    public static void AddChatMsg(string msg) {
        UIManager.Instance.addChatMsg(msg);
    }

    public void addChatMsg(string msg) {
        chatText.text += $"\n{msg}";
        chatRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ++chatLength * 34);
    }

    public void SendChatMsg() {
        if (chatInput.text.Length >= 1) {
            addChatMsg(chatInput.text);
            Networking.SendMsg(MSG_TYPE.CHAT, chatInput.text);
        }
        chatInput.text = "";
        setChatActive(false);
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

            int health = 4;
            switch (cmd[0]) {
                case "help":
                    LogMsg("tp <x> <y>, fill-all <TILE_TYPE> <HEALTH = 4>; fill <TILE_TYPE> <x> <y> <HEALTH = 4>, info");
                    break;
                case "tp":
                    playerController.transform.position = new Vector3(int.Parse(cmd[1]), int.Parse(cmd[2]), 0);
                    break;
                case "fill-all":
                    if (cmd.Length > 2) {
                        health = int.Parse(cmd[2]);
                    }
                    for (int i = 0; i < 100; i++) {
                        for (int j = 0; j < 100; j++) {
                            Networking.SendMsg(MSG_TYPE.ADD_RESOURCE, int.Parse(cmd[1]) + " " + i + " " + j + " " + health);
                        }
                    }
                    break;
                case "fill":
                    if (cmd.Length > 2) {
                        health = int.Parse(cmd[2]);
                    }
                    Networking.SendMsg(MSG_TYPE.ADD_RESOURCE, int.Parse(cmd[1]) + " " + int.Parse(cmd[2]) + " " + int.Parse(cmd[3]) + " " + health);
                    break;
                case "info":
                    string log = "I: ";
                    foreach (KeyValuePair<string, NetworkCharacter> item in Networking.Instance.NetworkCharacters) {
                        log += item.Value.name + "(" + item.Key + ") " + item.Value.transform.position;
                    }
                    LogMsg(log);
                    break;
            }

            inputField.text = "";
            return;
        }
        LogMsg("Send: " + inputField.text);
        Networking.SendMsg(MSG_TYPE.CHAT, inputField.text);
    }
}
