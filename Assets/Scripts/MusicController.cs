using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour{
    // Start is called before the first frame update
    void Start(){
        if (!PlayerPrefs.HasKey("musicVolume")) {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
            Load();
        } else {
            Load();
        }
    }

    private void Load() {
        AudioListener.volume = PlayerPrefs.GetFloat("musicVolume");
    }
}
