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
    CHANGE_TRANSFORM,
    ERR
}

public class WebSocketDemo : MonoBehaviour {

    static WebSocketDemo Instance;

    private WebSocket ws;

    private void Awake() {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        // Create WebSocket instance
        ws = WebSocketFactory.CreateInstance("ws://localhost:4242");

        // Add OnOpen event listener
        ws.OnOpen += () => {
            UIManager.LogPhrase("connected", ws.GetState().ToString());
        };

        // Add OnMessage event listener
        ws.OnMessage += (byte[] msg) => {
            string stringMsg = Encoding.UTF8.GetString(msg);
            int msgType = (int)stringMsg[0];
            stringMsg = stringMsg.Substring(1);

            UIManager.LogPhrase("msg", Enum.GetName(typeof(MSG_TYPE), msgType), stringMsg);

            // TODO switch over all MSG_TYPES
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
        WebSocketDemo.Instance.ws.Send(Encoding.UTF8.GetBytes((char)msgType + msg));
    }
}
