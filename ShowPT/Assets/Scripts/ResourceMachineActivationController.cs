using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceMachineActivationController : MonoBehaviour
{
    public bool machineActive;
    public Image background;

    public struct BackgroundUI
    {
        public GameObject UIElement;
        public float turnOnDelayTime;
        public float turnOnTime;
        public float flickSpeed;

        [HideInInspector]
        public bool isActive;
    }

    public BackgroundUI[] elements;
    
	void Start ()
    {
        setElements(machineActive);
    }

    private void setElements(bool active)
    {
        for (int i = 0; i < elements.Length; ++i)
        {
            Debug.Log("a");
            Color color = elements[i].UIElement.GetComponent<MaskableGraphic>().color;
            color.a = (active) ? 1f : 0f;
            elements[i].UIElement.GetComponent<MaskableGraphic>().color = color;
        }
    }

    IEnumerator onEnable()
    {
        //Enable background with custom animation



        //Enable all the elements
        bool allActive = false;

        while (!allActive)
        {
            allActive = true;

            for (int i = 0; i < elements.Length; ++i)
            {
                if (!elements[i].isActive)
                {
                    allActive = false;

                    //Update elements
                }
            }

            yield return null;
        }
    }
}
