using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObjectsManager : MonoBehaviour {

    public Text actionText;
    public Text keyText;
    public Text objectNameText;

    public struct InteractableInfo {
        public string keyCode;
        public string action;
        public string objectName;

        public InteractableInfo(string keyCode, string action, string objectName)
        {
            this.keyCode = keyCode;
            this.action = action;
            this.objectName = objectName;
        }
    }

    private Hashtable interactableObjects;
    private InteractableObject objectActive;
    private static InteractableObjectsManager interactableObjectsMapper;

    public static InteractableObjectsManager instance
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

    public static void addInteractableObject(string name, string keyCode, string type, string objectName)
    {
        instance.interactableObjects.Add(name, new InteractableInfo(keyCode, type, objectName));
    }

    public static void showInteractableObject(string name)
    {
        InteractableInfo interactableInfo = (InteractableInfo)instance.interactableObjects[name];
        instance.actionText.text = interactableInfo.action;
        instance.keyText.text = interactableInfo.keyCode;
        instance.objectNameText.text = interactableInfo.objectName;
    }

    public static bool equalsObjectActive(InteractableObject interactableObject)
    {
        return interactableObject.name == instance.objectActive.name;
    }

    public static void hideInteractableObject()
    {
        instance.actionText.text = "";
        instance.keyText.text = "";
        instance.objectNameText.text = "";
    }
}
