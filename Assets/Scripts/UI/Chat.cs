using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Canvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            Canvas.SetActive(!Canvas.activeSelf);
        }
    }
}
