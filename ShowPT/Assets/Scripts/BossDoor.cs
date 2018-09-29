using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour {

	[SerializeField]
	GameObject upperPanel;
	[SerializeField]
	GameObject lowerPanel;

	Vector3 upperPanelClosedPosition;
	Vector3 lowerPanelClosedPosition;
	[SerializeField]
	Transform upperPanelOpenPosition;
	[SerializeField]
	Transform lowerPanelOpenPosition;

	[SerializeField]
	GameObject securityWall;

    [Header("Audio")]
    public AudioClip doorOpenAudio;
    protected CtrlAudio ctrlAudio;

    public bool openDoor = false;

	// Use this for initialization
	void Start () 
	{
	    ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        upperPanelClosedPosition = upperPanel.transform.position;
		lowerPanelClosedPosition = lowerPanel.transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		if (openDoor == true) 
		{
			upperPanel.transform.position = Vector3.Lerp (upperPanel.transform.position, upperPanelOpenPosition.position, Time.deltaTime);
			lowerPanel.transform.position = Vector3.Lerp (lowerPanel.transform.position, lowerPanelOpenPosition.position, Time.deltaTime);
		} 
		else 
		{
			upperPanel.transform.position = Vector3.Lerp (upperPanel.transform.position, upperPanelClosedPosition, Time.deltaTime);
			lowerPanel.transform.position = Vector3.Lerp (lowerPanel.transform.position, lowerPanelClosedPosition, Time.deltaTime);
		}
	}

	public void CloseSesame()
	{
		openDoor = false;
		securityWall.SetActive (true);
	}

	public void OpenSesame()
	{
	    ctrlAudio.playOneSound("Weaponds", doorOpenAudio, transform.position, 0.5f, 0f, 150);
        openDoor = true;
	}
}
