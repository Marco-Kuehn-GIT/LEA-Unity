using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacter : MonoBehaviour{

    public static NetworkCharacter Instance;

    private void Awake() {
        Instance = this;
    }
}
