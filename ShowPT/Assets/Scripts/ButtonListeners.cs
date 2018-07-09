using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonListeners : MonoBehaviour {

    public Main.Actions[] actions;

    private Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        addListeners();
    }

    private void addListeners()
    {
        for (int i = 0; i < actions.Length; ++i)
        {
            UnityAction action = Main.instance.getUnityAction(actions[i]);
            if (action != null)
            {
                button.onClick.AddListener(action);
                Debug.Log("Added " + button.onClick.ToString() + " in button " + gameObject.name );
            }
        }
    }
}
