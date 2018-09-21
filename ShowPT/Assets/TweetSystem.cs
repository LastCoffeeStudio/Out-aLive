using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweetSystem : MonoBehaviour 
{
	[SerializeField]
	float timeBetweenTweets = 60f;
	float tweetTimer = 0f;

	[SerializeField]
	float timeOnScreen = 6f;
	float onScreenTimer = 0f;

	bool tweetShowing = false;

	[SerializeField]
	RectTransform tweet;
	[SerializeField]
	RectTransform outsidePoint;
	[SerializeField]
	RectTransform insidePoint;

	CtrlAudio audioCtrl;
	[SerializeField]
	AudioClip tweetAudio;

	// Use this for initialization
	void Start () 
	{
		audioCtrl = FindObjectOfType<CtrlAudio> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ((Input.GetKeyDown (KeyCode.I) && tweetShowing == false) || tweetTimer > timeBetweenTweets) 
		{
			tweetShowing = true;
			audioCtrl.playOneSound("UI", tweetAudio, transform.position, 0.5f, 0f, 150);
		}

		if (tweetShowing == true) 
		{
			tweet.transform.position = Vector2.Lerp (tweet.transform.position, insidePoint.transform.position, Time.deltaTime);
			onScreenTimer += Time.deltaTime;
			if (onScreenTimer > timeOnScreen) 
			{
				tweetShowing = false;
				onScreenTimer = 0f;
				tweetTimer = 0f;
			}
		} 
		else 
		{
			tweetTimer += Time.deltaTime;
			tweet.transform.position = Vector2.Lerp (tweet.transform.position, outsidePoint.transform.position, Time.deltaTime);
		}
	}
}
