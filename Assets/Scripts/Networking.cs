using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Use plugin namespace
using HybridWebSocket;
using System;

public enum MSG_TYPE {
    AUTH,
    INIT,
    CHAT,
    SPAWN,
    MOVE,
    CHANGE_TRANSFORM,
    ERR
}

public class Networking : MonoBehaviour {

    static Networking Instance;

    [SerializeField] private string protocol = "ws";
    [SerializeField] private string ip = "localhost";
    [SerializeField] private string port = "4242";

    private WebSocket ws;

    private void Awake() {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        // Create WebSocket instance
        ws = WebSocketFactory.CreateInstance(protocol + "://" + ip + ":" + port);

        // Add OnOpen event listener
        ws.OnOpen += () => {
            UIManager.LogPhrase("connected", ws.GetState().ToString());
        };

        // Add OnMessage event listener
        ws.OnMessage += (byte[] msg) => {
            string stringMsg = Encoding.UTF8.GetString(msg);
            int msgType = (int)stringMsg[0];
            stringMsg = stringMsg.Substring(1);


            // TODO switch over all MSG_TYPES
            switch (msgType) {
                case (int)MSG_TYPE.CHAT:
                    UIManager.LogPhrase("msg", Enum.GetName(typeof(MSG_TYPE), msgType), stringMsg);
                    break;
                case (int)MSG_TYPE.MOVE:
                    string[] arr = stringMsg.Split(' ');
                    NetworkCharacter.Instance.transform.position = new Vector3(float.Parse(arr[0]), float.Parse(arr[1]), 0);
                    break;
            }
        };

        // Add OnError event listener
        ws.OnError += (string errMsg) => {
#if UNITY_EDITOR
            Debug.LogWarning("errMsg: " + errMsg);
#else
            UIManager.LogPhrase("err", errMsg);
#endif
        };

        // Add OnClose event listener
        ws.OnClose += (WebSocketCloseCode code) => {
            UIManager.LogPhrase("closed", code.ToString());
        };

        // Connect to the server
        ws.Connect();

    }

    public static void SendMsg(MSG_TYPE msgType, string msg) {
        Networking.Instance.ws.Send(Encoding.UTF8.GetBytes((char)msgType + msg));
    }
}
