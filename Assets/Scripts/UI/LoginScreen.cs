using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour {
    // Start is called before the first frame update

    private void Awake() {

        SetCarretVisible(0);
    }
    void SetCarretVisible(int pos) {

        InputField inputField = GetComponent<InputField>();
        inputField.caretPosition = pos; // desired cursor position

        inputField.GetType().GetField("m_AllowInput", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(inputField, true);
        inputField.GetType().InvokeMember("SetCaretVisible", BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance, null, inputField, null);

    }

    
}
