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
    SET_WORLD,
    ADD_RESOURCE,
    REMOVE_RESOURCE,
    HIT_RESOURCE,
    CHAT,
    SPAWN,
    DESPAWN,
    MOVE,
    ERR
}

public class Networking : MonoBehaviour {

    static Networking Instance;

    [SerializeField] private GameObject[] networkCharacterObj;
    [SerializeField] private TileController tileController;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject loadingCanvas;

    [SerializeField] private string protocol = "ws";
    [SerializeField] private string ip = "localhost";
    [SerializeField] private string port = "4242";

    private WebSocket ws;

    public Dictionary<String, NetworkCharacter> networkCharacters = new Dictionary<string, NetworkCharacter>();

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
            SendMsg(MSG_TYPE.AUTH, "admin admin");
        };

        // Add OnMessage event listener
        ws.OnMessage += (byte[] msg) => {
            string stringMsg = Encoding.UTF8.GetString(msg);
            int msgType = (int)stringMsg[0];
            stringMsg = stringMsg.Substring(1);

            string[] arr;

            // TODO switch over all MSG_TYPES
            switch (msgType) {
                case (int)MSG_TYPE.INIT:
                    arr = stringMsg.Split(' ');
                    Debug.Log("Set skin " + arr[0]);
                    playerController.SetSkin(int.Parse(arr[0]));
                    break;
                case (int)MSG_TYPE.CHAT:
                    UIManager.LogPhrase("msg", Enum.GetName(typeof(MSG_TYPE), msgType), stringMsg);
                    break;
                case (int)MSG_TYPE.MOVE:
                    arr = stringMsg.Split(' ');
                    NetworkCharacter networkCharacter;
                    if (networkCharacters.TryGetValue(arr[0], out networkCharacter)) {
                        networkCharacter.Move(float.Parse(arr[1]), float.Parse(arr[2]));
                    }
                    break;
                case (int)MSG_TYPE.SET_WORLD:
                    tileController.initMap(stringMsg);
                    // Deactivte Loading Canvas
                    loadingCanvas.SetActive(false);
                    break;
                case (int)MSG_TYPE.ADD_RESOURCE:
                    arr = stringMsg.Split(' ');
                    tileController.SetTile(new Vector3Int(int.Parse(arr[1]), int.Parse(arr[2]), 0), (TILE_TYPE)int.Parse(arr[0]), true);
                    break;
                case (int)MSG_TYPE.HIT_RESOURCE:
                
                    break;
                case (int)MSG_TYPE.REMOVE_RESOURCE:
                    arr = stringMsg.Split(' ');
                    tileController.SetTile(new Vector3Int(int.Parse(arr[0]), int.Parse(arr[1]), 0), TILE_TYPE.WATER, true);
                    break;

                case (int)MSG_TYPE.SPAWN:
                    Debug.Log("SPAWN" + stringMsg);
                    arr = stringMsg.Split(' ');
                    GameObject obj = Instantiate(networkCharacterObj[int.Parse(arr[1])]) as GameObject;
                    networkCharacters.Add(arr[0], obj.GetComponent<NetworkCharacter>());
                    break;
                case (int)MSG_TYPE.DESPAWN:
                    Debug.Log("DESPAWN" + stringMsg);
                    arr = stringMsg.Split(' ');
                    NetworkCharacter networkCharacter1;
                    if (networkCharacters.TryGetValue(arr[0], out networkCharacter1)) {
                        networkCharacters.Remove(arr[0]);
                        Destroy(networkCharacter1.gameObject);
                    }
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
