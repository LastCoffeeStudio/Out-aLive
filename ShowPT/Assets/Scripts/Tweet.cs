using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tweet : ScriptableObject 
{
	public Tweeter tweeter;
	public string tweetText;
}
