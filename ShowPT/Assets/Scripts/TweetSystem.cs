using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweetSystem : MonoBehaviour 
{
	[SerializeField]
	float tweetSpeed = 2f;
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

	bool toldToDeactivate = false;

	enum state
	{
		TWEET_HIDDEN,
		TWEET_RUNNING_IN,
		TWEET_SHOWING,
		TWEET_RUNNING_OUT
	}

	state tweetState = state.TWEET_HIDDEN;

	[SerializeField]
	List<Tweet> randomTweetList;

	List<Tweet> requestedTweetsQueue;

	// Use this for initialization
	void Start () 
	{
		audioCtrl = FindObjectOfType<CtrlAudio> ();
		//randomTweetList = new List<Tweet> ();
		requestedTweetsQueue = new List<Tweet> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (tweetState) 
		{
		case state.TWEET_HIDDEN:
			tweetTimer += Time.deltaTime;
			if (tweetTimer > timeBetweenTweets) 
			{
				if (randomTweetList.Count > 0) 
				{
					generateTweet (chooseRandomTweet ());
				}
			}
			break;

		case state.TWEET_RUNNING_IN:
			tweet.transform.position = Vector2.Lerp (tweet.transform.position, insidePoint.transform.position, Time.deltaTime * tweetSpeed);
			if (Vector2.Distance (tweet.transform.position, insidePoint.transform.position) < 10f) 
			{
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
			tweet.transform.position = Vector2.Lerp (tweet.transform.position, outsidePoint.transform.position, Time.deltaTime * tweetSpeed);
			if (Vector2.Distance (tweet.transform.position, outsidePoint.transform.position) < 10f) 
			{
				if (toldToDeactivate == true) 
				{
					gameObject.SetActive (false);
				} 
				else if (requestedTweetsQueue.Count > 0) 
				{
					generateTweet (requestedTweetsQueue [0]);
					requestedTweetsQueue.RemoveAt (0);
				} 
				else 
				{
					tweetState = state.TWEET_HIDDEN;
				}
			}
			break;
		}
	}

	Tweet chooseRandomTweet()
	{
		int tweetNumber = Random.Range (0, randomTweetList.Count - 1);
		Tweet tweetToReturn = randomTweetList [tweetNumber];
		randomTweetList.RemoveAt (tweetNumber);
		return tweetToReturn;
	}

	void generateTweet(Tweet tweetData)
	{
		tweetTimer = 0f;

		tweetAvatar.sprite = tweetData.tweeter.tweeterAvatar;
		tweetName.text = tweetData.tweeter.tweeterName;
		tweetDir.text = tweetData.tweeter.tweeterDir;
		tweetText.text = tweetData.tweetText;

		audioCtrl.playOneSound("UI", tweetAudio, transform.position, 0.5f, 0f, 150);
		tweetState = state.TWEET_RUNNING_IN;
	}

	public void requestTweet(Tweet requestedTweet)
	{
		if (tweetState == state.TWEET_HIDDEN) 
		{
			generateTweet (requestedTweet);
		} 
		else 
		{
			requestedTweetsQueue.Add (requestedTweet);
		}
	}

	public void deactivateSystem()
	{
		tweetState = state.TWEET_RUNNING_OUT;
		toldToDeactivate = true;
	}
}
