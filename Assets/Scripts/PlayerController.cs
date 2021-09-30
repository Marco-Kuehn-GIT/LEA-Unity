using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour{

    [SerializeField] private TileController tileController;
    [SerializeField] private Camera cam;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float movementSpeed = 1f;

    public GameObject[] skin;

    [SerializeField] private List<TILE_TYPE> inventory;

    public int curInventorySpace = 0;

    private Rigidbody2D rgBody;

    private Vector2 movingDir;
    private bool sendNoMsgs = false;

    private void Awake() {
        rgBody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        movingDir.x = Input.GetAxisRaw("Horizontal");
        movingDir.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Tab)) {
            curInventorySpace++;
            curInventorySpace %= inventory.Count;
        }

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

            if (inventory[curInventorySpace] == TILE_TYPE.WATER) {
                Networking.SendMsg(MSG_TYPE.HIT_RESOURCE, x + " " + y);
            } else {
                Networking.SendMsg(MSG_TYPE.ADD_RESOURCE, (int)inventory[curInventorySpace] + " " + x + " " + y);
            }
        }
    }

    void FixedUpdate() {
        if (setAnimation()) {
            Networking.SendMsg(MSG_TYPE.MOVE, transform.position.x + " " + transform.position.y);
            spriteRenderer.sortingOrder = 150 - (int)transform.position.y;
        }
        rgBody.MovePosition(rgBody.position + movingDir.normalized * movementSpeed * Time.fixedDeltaTime);
    }

    private bool setAnimation() {
        if (Mathf.Abs(movingDir.x) > Mathf.Abs(movingDir.y)) {
            if (movingDir.x > 0) {
                animator.SetInteger("WalkDir", 2);
            } else if (movingDir.x < 0) {
                animator.SetInteger("WalkDir", 4);
            } else {
                animator.SetInteger("WalkDir", 0);
                if (!sendNoMsgs) {
                    sendNoMsgs = true;
                    return true;
                }
                return false;
            }
        } else {
            if (movingDir.y > 0) {
                animator.SetInteger("WalkDir", 1);
            } else if (movingDir.y < 0) {
                animator.SetInteger("WalkDir", 3);
            } else {
                animator.SetInteger("WalkDir", 0);
                if (!sendNoMsgs) {
                    sendNoMsgs = true;
                    return true;
                }
                return false;
            }
        }
        sendNoMsgs = false;
        return true;
    }

    public void SetSkin(int nr) {
        if (nr > skin.Length) nr = 0;
        Debug.Log("activate skin " + nr + " " + skin.Length);
        try {
            Debug.Log(skin[nr]);
            skin[nr].SetActive(true);
            animator = skin[nr].GetComponent<Animator>();
            spriteRenderer = skin[nr].GetComponent<SpriteRenderer>();
        } catch (System.Exception e) {
            Debug.Log(e);
            Debug.Log(e.StackTrace);
        }
    }
}
