using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Slider volumeSlider;


    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume")) {

            PlayerPrefs.SetFloat("musicVolume", 0.5f);
            Load();
        } else {
            Load();
        }
        
    }
    
    public void ChangeVolume() {

        AudioListener.volume = volumeSlider.value;
        Debug.Log(volumeSlider.value);
        Save();
    }

    private void Load() {
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save() {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
