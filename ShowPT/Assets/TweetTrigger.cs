using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweetTrigger : MonoBehaviour {

	[SerializeField]
	Tweet tweet;

	TweetSystem tweetSystem;

	void Start()
	{
		tweetSystem = FindObjectOfType<TweetSystem> ();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") 
		{
			tweetSystem.requestTweet (tweet);
			gameObject.SetActive (false);
		}
	}
}
