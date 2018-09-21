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

	[SerializeField]
	RectTransform tweet;
	[SerializeField]
	Image tweetAvatar;
	[SerializeField]
	Text tweetName;
	[SerializeField]
	Text tweetDir;
	[SerializeField]
	Text tweetText;
	[SerializeField]
	RectTransform outsidePoint;
	[SerializeField]
	RectTransform insidePoint;

	CtrlAudio audioCtrl;
	[SerializeField]
	AudioClip tweetAudio;

	enum state
	{
		TWEET_HIDDEN,
		TWEET_RUNNING_IN,
		TWEET_SHOWING,
		TWEET_RUNNING_OUT
	}

	state tweetState = state.TWEET_HIDDEN;

	[SerializeField]
	Tweet[] randomTweetList;

	// Use this for initialization
	void Start () 
	{
		audioCtrl = FindObjectOfType<CtrlAudio> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (tweetState) 
		{
		case state.TWEET_HIDDEN:
			tweetTimer += Time.deltaTime;
			if ((Input.GetKeyDown (KeyCode.I)) || tweetTimer > timeBetweenTweets) 
			{
				tweetTimer = 0f;
				audioCtrl.playOneSound("UI", tweetAudio, transform.position, 0.5f, 0f, 150);
				generateTweet (chooseRandomTweet ());
				tweetState = state.TWEET_RUNNING_IN;
			}
			break;

		case state.TWEET_RUNNING_IN:
			tweet.transform.position = Vector2.Lerp (tweet.transform.position, insidePoint.transform.position, Time.deltaTime);
			if (Vector2.Distance (tweet.transform.position, insidePoint.transform.position) < 0.5f) {
				tweetState = state.TWEET_SHOWING;
			}
			break;

		case state.TWEET_SHOWING:
			onScreenTimer += Time.deltaTime;
			if (onScreenTimer > timeOnScreen) 
			{
				onScreenTimer = 0f;
				tweetState = state.TWEET_RUNNING_OUT;
			}
			break;

		case state.TWEET_RUNNING_OUT:
			tweet.transform.position = Vector2.Lerp (tweet.transform.position, outsidePoint.transform.position, Time.deltaTime);
			if (Vector2.Distance (tweet.transform.position, outsidePoint.transform.position) < 0.5f) {
				tweetState = state.TWEET_HIDDEN;
			}
			break;
		}
	}

	Tweet chooseRandomTweet()
	{
		int tweetNumber = Random.Range (0, randomTweetList.Length - 1);
		return randomTweetList [tweetNumber];
	}

	public void generateTweet(Tweet tweetData)
	{
		tweetAvatar.sprite = tweetData.tweeter.tweeterAvatar;
		tweetName.text = tweetData.tweeter.tweeterName;
		tweetDir.text = tweetData.tweeter.tweeterDir;
		tweetText.text = tweetData.tweetText;
	}
}
