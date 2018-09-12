using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjectsManager : MonoBehaviour {

    public Text actionText;
    public Text keyText;
    public Text objectNameText;
    public Image line;
    public Image keyCircle;
    public Image background;

    public struct InteractableInfo {
        public string keyCode;
        public string action;
        public string objectName;
        public bool showKey;
        public bool showAction;
        public bool showName;

        public InteractableInfo(string keyCode, string action, string objectName, bool showKey, bool showAction, bool showName)
        {
            this.keyCode = keyCode;
            this.action = action;
            this.objectName = objectName;
            this.showKey = showKey;
            this.showAction = showAction;
            this.showName = showName;
        }
    }

    private Hashtable interactableObjects;
    private InteractableObject objectActive;
    private static InteractableObjectsManager interactableObjectsMapper;

    public static InteractableObjectsManager interactableObjectsManagerinstance
    {
        get
        {
            if (!interactableObjectsMapper)
            {
                interactableObjectsMapper = FindObjectOfType(typeof(InteractableObjectsManager)) as InteractableObjectsManager;

                if (!interactableObjectsMapper)
                {
                    Debug.LogError("There needs to be one active InteractableObjectsMapper script on a GameObject in your scene.");
                }
                else
                {
                    interactableObjectsMapper.init();
                }
            }

            return interactableObjectsMapper;
        }
    }

    public InteractableObject ObjectActive
    {
        get
        {
            return objectActive;
        }

        set
        {
            objectActive = value;
        }
    }

    private void init()
    {
        if (interactableObjects == null)
        {
            interactableObjects = new Hashtable();
        }
    }

    public static void addInteractableObject(string name, string keyCode, string type, string objectName, bool showKey, bool showAction, bool showName)
    {
        interactableObjectsManagerinstance.interactableObjects.Add(name, new InteractableInfo(keyCode, type, objectName, showKey, showAction, showName));
    }

    public static void showInteractableObject(string name)
    {
        InteractableInfo interactableInfo = (InteractableInfo)interactableObjectsManagerinstance.interactableObjects[name];
        if (interactableInfo.showAction)
        {
            interactableObjectsManagerinstance.actionText.text = interactableInfo.action;
            interactableObjectsManagerinstance.background.gameObject.SetActive(true);
        }
        if (interactableInfo.showKey)
        {
            interactableObjectsManagerinstance.keyText.text = interactableInfo.keyCode;
            interactableObjectsManagerinstance.keyCircle.gameObject.SetActive(true);
            interactableObjectsManagerinstance.background.gameObject.SetActive(true);
        }
        if (interactableInfo.showName)
        {
            interactableObjectsManagerinstance.objectNameText.text = interactableInfo.objectName;
        }
        interactableObjectsManagerinstance.line.gameObject.SetActive(true);
    }

    public static bool equalsObjectActive(InteractableObject interactableObject)
    {
        return interactableObject.name == interactableObjectsManagerinstance.objectActive.name;
    }

    public static void hideInteractableObject()
    {
        interactableObjectsManagerinstance.actionText.text = "";
        interactableObjectsManagerinstance.keyText.text = "";
        interactableObjectsManagerinstance.objectNameText.text = "";
        interactableObjectsManagerinstance.background.gameObject.SetActive(false);
        interactableObjectsManagerinstance.line.gameObject.SetActive(false);
        interactableObjectsManagerinstance.keyCircle.gameObject.SetActive(false);
    }
}
