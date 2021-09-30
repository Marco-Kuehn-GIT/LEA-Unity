using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacter : MonoBehaviour{

    public string name = "Player_xy";

    [SerializeField] private Animator animator;
    private float movingThreshold = 0.01f;

    public Vector3 startPosition = Vector3.zero;
    public Vector3 endPosition = Vector3.zero;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private float lerpTime = 0;
    private float lerpScale = 1f;

    private void Awake() {
    }

    private void Update() {
        lerpTime += Time.deltaTime * lerpScale;
        transform.position = Vector3.Lerp(startPosition, endPosition, lerpTime);
    }

    public void Move(float x, float y) {
        spriteRenderer.sortingOrder = 150 - (int)transform.position.y;

        lerpTime = 0;
        startPosition = transform.position;
        endPosition = new Vector3(x, y, 0) - startPosition;
        endPosition = startPosition + endPosition * 2;
        //transform.position = new Vector2(x, y);
        Vector2 movingDir = (Vector2)endPosition - (Vector2)startPosition;
        lerpScale = Vector3.Distance(endPosition, startPosition);

        if (Mathf.Abs(movingDir.x) > Mathf.Abs(movingDir.y)) {
            if (movingDir.x > movingThreshold) {
                animator.SetInteger("WalkDir", 2);
            }
            else if (movingDir.x < -movingThreshold) {
                animator.SetInteger("WalkDir", 4);
            }
            else {
                animator.SetInteger("WalkDir", 0);
            }
        }
        else {
            if (movingDir.y > movingThreshold) {
                animator.SetInteger("WalkDir", 1);
            }
            else if (movingDir.y < -movingThreshold) {
                animator.SetInteger("WalkDir", 3);
            }
            else {
                animator.SetInteger("WalkDir", 0);
            }
        }
    }
}
