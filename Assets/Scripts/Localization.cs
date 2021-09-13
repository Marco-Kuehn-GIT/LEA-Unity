using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour {

    public static Localization Instance;

    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private string languageName = "English";

    private Phrase[] phrases;
    private Language[] languages;

    public Language[] Languages { get => languages; }
    public Phrase[] Phrases { get => phrases; }

    private void Awake() {
        Instance = this;

        Languages languagesJSON = JsonUtility.FromJson<Languages>(jsonFile.text);
        languages = languagesJSON.languages;
        setLanguage(languageName);
    }

    public void setLanguage(string languageName) {
        foreach (Language language in Languages) {
            if (languageName.Equals(language.name)) {
                phrases = language.phrases;
            }
        }
    }

    /// <summary>
    /// Get the phrase from a key in the current language.
    /// </summary>
    /// <param name="key">The key for a phrase</param>
    /// <param name="args">Optional add argument to include in the phrase</param>
    /// <returns>The phrase</returns>
    public static string GetPhrase(string key, params string[] args) {
        foreach (Phrase phrase in Localization.Instance.Phrases) {
            if (key.Equals(phrase.key)) {
                if(args.Length == 0) {
                    return phrase.value;
                } else {
                    return phrase.Get(args);
                }
            }
        }

        return "NOT FOUND";
    }
}


[System.Serializable]
public class Languages {
    public Language[] languages;
}

[System.Serializable]
public class Language {
    public string name;
    public Phrase[] phrases;
}

[System.Serializable]
public class Phrase {
    public string key;
    public string value;

    /// <summary>
    /// Get a phrase with included arguments
    /// </summary>
    /// <param name="args">An array of strings that get included in the phrase</param>
    /// <returns>The combination of args and the phrase</returns>
    public string Get(params string[] args) {
        string[] splittedValue = value.Split('@');
        string returnStr = "";
        for (int i = 0; i < splittedValue.Length; i++) {
            returnStr += splittedValue[i];
            if(args.Length > i) returnStr += args[i];
        }

        return returnStr;
    }
}
