using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacter : MonoBehaviour{

    public static NetworkCharacter Instance;

    [SerializeField] private Animator animator;

    private void Awake() {
        Instance = this;
    }


    public void Move(float x, float y) {
        Debug.Log("move " + x + " " + y);
        Vector2 posBefore = transform.position;
        transform.position = new Vector2(x, y);
        Vector2 movingDir = (Vector2)transform.position - posBefore;

        if (Mathf.Abs(movingDir.x) > Mathf.Abs(movingDir.y)) {
            if (movingDir.x > 0) {
                animator.SetInteger("WalkDir", 2);
            }
            else if (movingDir.x < 0) {
                animator.SetInteger("WalkDir", 4);
            }
            else {
                animator.SetInteger("WalkDir", 0);
            }
        }
        else {
            if (movingDir.y > 0) {
                animator.SetInteger("WalkDir", 1);
            }
            else if (movingDir.y < 0) {
                animator.SetInteger("WalkDir", 3);
            }
            else {
                animator.SetInteger("WalkDir", 0);
            }
        }
    }
}
