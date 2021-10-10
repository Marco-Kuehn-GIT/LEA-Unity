using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacter : MonoBehaviour{

    public string name = "Player_xy";

    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float movingThreshold = 0.01f;

    private void Start() {
        audioSource.enabled = false;
    }

    public void Move(float x, float y) {
        Vector2 posBefore = transform.position;
        transform.position = new Vector2(x, y);
        Vector2 movingDir = (Vector2)transform.position - posBefore;

        if (Mathf.Abs(movingDir.x) > Mathf.Abs(movingDir.y)) {
            if (movingDir.x > movingThreshold) {
                animator.SetInteger("WalkDir", 2);
                audioSource.enabled = true;
            }
            else if (movingDir.x < -movingThreshold) {
                animator.SetInteger("WalkDir", 4);
                audioSource.enabled = true;
            }
            else {
                animator.SetInteger("WalkDir", 0);
                audioSource.enabled = false;
            }
        }
        else {
            if (movingDir.y > movingThreshold) {
                animator.SetInteger("WalkDir", 1);
                audioSource.enabled = true;
            }
            else if (movingDir.y < -movingThreshold) {
                animator.SetInteger("WalkDir", 3);
                audioSource.enabled = true;
            }
            else {
                animator.SetInteger("WalkDir", 0);
                audioSource.enabled = false;
            }
        }

        spriteRenderer.sortingOrder = 150 - (int)transform.position.y;
    }
}
