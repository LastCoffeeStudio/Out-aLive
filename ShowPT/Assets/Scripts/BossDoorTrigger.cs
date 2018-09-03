using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorTrigger : MonoBehaviour {

	[SerializeField]
	BossDoor bossDoor;

	[SerializeField]
	bool opensDoor = true;

	[Header("Audio")]
	public AudioClip doorOpenAudio;
	public AudioClip doorCloseAudio;
	protected CtrlAudio ctrlAudio;

	void Start()
	{
		ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			if (opensDoor == true) 
			{
				bossDoor.OpenSesame ();
				ctrlAudio.playOneSound("Weaponds", doorOpenAudio, bossDoor.transform.position, 0.5f, 0f, 150);
			} 
			else 
			{
				ctrlAudio.playOneSound("Weaponds", doorCloseAudio, bossDoor.transform.position, 0.5f, 0f, 150);
				bossDoor.CloseSesame ();
			}

			gameObject.SetActive (false);
		}
	}
}
