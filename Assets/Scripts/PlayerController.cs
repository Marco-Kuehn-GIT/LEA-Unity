using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour{

    [SerializeField] private TileController tileController;
    [SerializeField] private Camera cam;
    [SerializeField] private Animator animator;

    [SerializeField] private float movementSpeed = 1f;

    [SerializeField] private List<TILE_TYPE> inventory;

    public int curInventorySpace = 0;

    private Rigidbody2D rgBody;

    private Vector2 movingDir;

    private void Awake() {
        rgBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        movingDir.x = Input.GetAxisRaw("Horizontal");
        movingDir.y = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0)) {
            Vector3 vec = cam.ScreenToWorldPoint(Input.mousePosition);

            int x = (int)vec.x;
            int y = (int)vec.y;

            Debug.Log("x" + x + " y" + y);

            if (vec.x < 0) {
                x--;
            }
            if (vec.y < 0) {
                y--;
            }

            Networking.SendMsg(MSG_TYPE.ADD_RESOURCE, (int)inventory[curInventorySpace] + " " + x + " " + y);
        }
    }

    void FixedUpdate() {
        setAnimation();
        rgBody.MovePosition(rgBody.position + movingDir.normalized * movementSpeed * Time.fixedDeltaTime);
        Networking.SendMsg(MSG_TYPE.MOVE, transform.position.x + " " + transform.position.y);
    }

    private void setAnimation() {
        if (Mathf.Abs(movingDir.x) > Mathf.Abs(movingDir.y)) {
            if (movingDir.x > 0) {
                animator.SetInteger("WalkDir", 2);
            } else if (movingDir.x < 0) {
                animator.SetInteger("WalkDir", 4);
            } else {
                animator.SetInteger("WalkDir", 0);
            }
        } else {
            if (movingDir.y > 0) {
                animator.SetInteger("WalkDir", 1);
            } else if (movingDir.y < 0) {
                animator.SetInteger("WalkDir", 3);
            } else {
                animator.SetInteger("WalkDir", 0);
            }
        }
    }
}
