using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    [SerializeField] private Sprite FullHeart;
    [SerializeField] private Sprite EmptyHeart;
    [SerializeField] private Image[] Hearts;

    private void Start() {
    }
    public void LifeChange(int health) {
        for (int i = 0; i < Hearts.Length; i++) {
            if (i < health) {
                Hearts[i].sprite = FullHeart;
            } else {
                Hearts[i].sprite = EmptyHeart;
            }
        }
    }
}
