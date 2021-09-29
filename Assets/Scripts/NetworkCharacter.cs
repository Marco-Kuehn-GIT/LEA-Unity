using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacter : MonoBehaviour{

    public string name = "Player_xy";

    [SerializeField] private Animator animator;
    private float movingThreshold = 0.01f;

    private void Awake() {
        Instance = this;
    }

    public void Move(float x, float y) {
        Vector2 posBefore = transform.position;
        transform.position = new Vector2(x, y);
        Vector2 movingDir = (Vector2)transform.position - posBefore;

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
