using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour{

    [SerializeField] private float movementSpeed = 1f;

    private Rigidbody2D rgBody;

    private Vector2 movingDir;

    private void Awake() {
        rgBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        movingDir.x = Input.GetAxisRaw("Horizontal");
        movingDir.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate(){
        rgBody.MovePosition(rgBody.position + movingDir.normalized * movementSpeed * Time.fixedDeltaTime);
        Networking.SendMsg(MSG_TYPE.MOVE, transform.position.x + " " + transform.position.y);
    }
}
