using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PlayerController : MonoBehaviour{

    [SerializeField] private TileController tileController;
    [SerializeField] private Camera cam;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource hittingAudioSource;
    [SerializeField] private ParticleSystem hittingParticleSystem;

    [SerializeField] private float movementSpeed = 1f;

    public GameObject[] skin;

    [SerializeField] private List<TILE_TYPE> inventory;
    [SerializeField] private Image[] inventoryBorders;

    [SerializeField] private Color notSelected;
    [SerializeField] private Color selected;

    public int curInventorySpace = 0;

    private Rigidbody2D rgBody;

    private Vector2 movingDir;
    private bool sendNoMsgs = false;

    private Vector2 hittingPosition;
    private float curHitTime = 0;
    [SerializeField] private float hitTime = 1;

    private void Awake() {
        rgBody = GetComponent<Rigidbody2D>();
        inventoryBorders[curInventorySpace].color = selected;
    }

    private void Update() {
        movingDir.x = Input.GetAxisRaw("Horizontal");
        movingDir.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Tab)) {
            inventoryBorders[curInventorySpace].color = notSelected;
            curInventorySpace++;
            curInventorySpace %= inventory.Count;
            inventoryBorders[curInventorySpace].color = selected;
        }

        if (Input.GetMouseButtonDown(0)) {
            Vector3 vec = cam.ScreenToWorldPoint(Input.mousePosition);

            int x = (int)vec.x;
            int y = (int)vec.y;

            if (vec.x < 0) {
                x--;
            }
            if (vec.y < 0) {
                y--;
            }

            if (inventory[curInventorySpace] == TILE_TYPE.WATER) {
                hittingPosition = new Vector2(x, y);
                if(tileController.GetTile(x, y) != TILE_TYPE.WATER) {
                    hittingAudioSource.enabled = true;
                    hittingParticleSystem.transform.position = hittingPosition + new Vector2(0.5f, 0.5f);
                    hittingParticleSystem.gameObject.SetActive(true);
                }
            } else {
                Networking.SendMsg(MSG_TYPE.ADD_RESOURCE, (int)inventory[curInventorySpace] + " " + x + " " + y + " 4");
            }
        }

        if (Input.GetMouseButton(0)) {
            Vector3 vec = cam.ScreenToWorldPoint(Input.mousePosition);

            int x = (int)vec.x;
            int y = (int)vec.y;

            if (vec.x < 0) {
                x--;
            }
            if (vec.y < 0) {
                y--;
            }

            if (inventory[curInventorySpace] == TILE_TYPE.WATER) {
                if(hittingPosition.x == x && hittingPosition.y == y) {
                    curHitTime += Time.deltaTime;
                    if (curHitTime >= hitTime){
                        curHitTime = 0;
                        if (tileController.GetHealth(x, y) <= 1) {
                            hittingAudioSource.enabled = false;
                            hittingParticleSystem.gameObject.SetActive(false);
                        } else {
                            hittingAudioSource.enabled = true;
                            hittingParticleSystem.gameObject.SetActive(true);
                        }

                        Networking.SendMsg(MSG_TYPE.HIT_RESOURCE, x + " " + y);
                    }
                } else {
                    hittingPosition = new Vector2(x, y);
                    curHitTime = 0;
                    if (tileController.GetTile(x, y) != TILE_TYPE.WATER) {
                        hittingAudioSource.enabled = true;
                        hittingParticleSystem.transform.position = hittingPosition + new Vector2(0.5f, 0.5f);
                        hittingParticleSystem.gameObject.SetActive(true);
                    } else {
                        hittingAudioSource.enabled = false;
                        hittingParticleSystem.gameObject.SetActive(false);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            curHitTime = 0;
            hittingAudioSource.enabled = false;
            hittingParticleSystem.gameObject.SetActive(false);
        }
    }

    void FixedUpdate() {
        if (setAnimation()) {
            spriteRenderer.sortingOrder = 150 - (int)transform.position.y;
        }
        Networking.SendMsg(MSG_TYPE.MOVE, transform.position.x + " " + transform.position.y);
        rgBody.MovePosition(rgBody.position + movingDir.normalized * movementSpeed * Time.fixedDeltaTime);
    }

    private bool setAnimation() {
        if (Mathf.Abs(movingDir.x) > Mathf.Abs(movingDir.y)) {
            if (movingDir.x > 0) {
                animator.SetInteger("WalkDir", 2);
                audioSource.enabled = true;
            } else if (movingDir.x < 0) {
                animator.SetInteger("WalkDir", 4);
                audioSource.enabled = true;
            } else {
                animator.SetInteger("WalkDir", 0);
                audioSource.enabled = false;
                if (!sendNoMsgs) {
                    sendNoMsgs = true;
                    return true;
                }
                return false;
            }
        } else {
            if (movingDir.y > 0) {
                animator.SetInteger("WalkDir", 1);
                audioSource.enabled = true;
            } else if (movingDir.y < 0) {
                animator.SetInteger("WalkDir", 3);
                audioSource.enabled = true;
            } else {
                animator.SetInteger("WalkDir", 0);
                audioSource.enabled = false;
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
            audioSource = skin[nr].GetComponent<AudioSource>();
        } catch (System.Exception e) {
            Debug.Log(e);
            Debug.Log(e.StackTrace);
        }
    }
}
