using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	public static ScoreController scoreController;

    public enum Enemy
    {
        WAZ,
        TURRET,
        DRON,
        KAMIKAZE,
        LIL,
        TV,
        CAMERA,

        UNDEFINED
    }

    public enum EnemyScore
    {
        WAZ = 10,
        TURRET = 35,
        DRON = 20,
        KAMIKAZE = 50,
        LIL = 10,
        TV = 20,
        CAMERA = 10,
        BOSS_ARM_HIT = 10,
        BOSS_ARM_KEEPED = 50
    }

    public enum ActionScore
    {
        JUMP = 100,
        DEAD = 2000
    }

    private const float SECONDS_TO_BORING = 20;
    private const int POINTS_OF_BORING = 1;
    private const int MULTIUPLAY_DAMAGE_POINTS = 3;

    //Public GUI objects
    public Text likesLabel;
    public Text disLikesLabel;
    public Text totalScoreLabel;

    public Text wazDeadsLabel;
    public Text turretDeadsLabel;
    public Text dronsDeadsLabel;
    public Text kamikazeDeadsLabel;
    public Text lilDeadsLabel;
    public Text totalDeadsLabel;
    public Text bossArmsHittedLabel;

    public Text tvDeadsLabel;
    public Text cameraDeadsLabel;
    public Text boringTimeLabel;
    public Text lifeLostLabel;
    public Text bossAtacksLabel;

    public Text pistolAverage;
    public Text shotgunAverage;
    public Text SMGAverage;
    public Text canonAverage;

    //Public Gui objects
    private static int likesInt;
    private static int disLikesInt;
    private static int totalScoreInt;

    //Likes
    private static int wazDeads;
    private static int torretDeads;
    private static int dronsDeads;
    private static int kamikazeDeads;
    private static int LilDeads;
    private static int bossAtacks;

    //Dislikes
    private static int tvDeads;
    private static int cameraDeads;
    private static int boringTime;
    private static int lifeLost;
    private static int bossArmsHitted;

    private static int pistolShotsUsed;
    private static int SMGShotsUsed;
    private static int shotgunShotsUsed;
    private static int canonShotsUsed;

    private static int pistolShotsTouched;
    private static int SMGShotsTouched;
    private static int shotgunShotsTouched;
    private static int canonShotsTouched;
    
    public static void pistolUsed() { ++pistolShotsUsed;  }
    public static void SMGUsed() { ++SMGShotsUsed; }
    public static void shotgunUsed() { ++SMGShotsUsed; }
    public static void canonUsed() { ++canonShotsUsed; }

    public static void pistolHit() { ++pistolShotsTouched; }
    public static void SMGHit() { ++SMGShotsTouched; }
    public static void shotgunHit() { ++shotgunShotsTouched; }
    public static void canonHit() { ++canonShotsTouched; }

    public GameObject stadistics;
    public GameObject hud;

    public GameObject likeParticles;
    public GameObject dislikeParticles;

    private static bool updateHud = false;
    private static bool shouldShowLikePArticle = false;
    private static bool shouldShowDislikePArticle = false;

    private static bool haveCannonIncremented = false;
    private static bool haveShotgunIncremented = false;

    public static float timeSinceLastAction = 0.0f;

	[Header("Score Tweets")]
	TweetSystem tweetSystem;
	[SerializeField]
	Tweet tenLilsTweet;
	[SerializeField]
	Tweet twentyLilsTweet;
	[SerializeField]
	Tweet fiveTurretsTweet;
	[SerializeField]
	Tweet fiveKamisTweet;
    

    // Use this for initialization
    void Start () 
	{
		scoreController = this;
		tweetSystem = FindObjectOfType<TweetSystem> ();
        haveCannonIncremented = false;
        haveShotgunIncremented = false;

        //Public Gui objects
        likesInt = 0;
        disLikesInt = 0;
        totalScoreInt = 0;

        //Likes
        wazDeads = 0;
        torretDeads = 0;
        dronsDeads = 0;
        kamikazeDeads = 0;
        LilDeads = 0;
        bossArmsHitted = 0;

        //Dislikes
        tvDeads = 0;
        cameraDeads = 0;
        boringTime = 0;
        lifeLost = 0;
        bossAtacks = 0;

        pistolShotsUsed = 0;
        SMGShotsUsed = 0;
        shotgunShotsUsed = 0;
        canonShotsUsed = 0;

        pistolShotsTouched = 0;
        SMGShotsTouched = 0;
        shotgunShotsTouched = 0;
        canonShotsTouched = 0;

}

    // Update is called once per frame
    void Update ()
    {
        timeSinceLastAction += Time.deltaTime;
        if(timeSinceLastAction > SECONDS_TO_BORING)
        {
            updateHud = true;
            timeSinceLastAction = 0;
            boringTime += POINTS_OF_BORING;
        }
        updateLavels();
        if (Input.GetKey(KeyCode.Tab) || CtrlGameState.getGameState() == CtrlGameState.gameStates.DEATH || CtrlGameState.getGameState() == CtrlGameState.gameStates.WIN)
        {
            stadistics.SetActive(true);
            hud.SetActive(false);
        }
        else if(CtrlGameState.getGameState() != CtrlGameState.gameStates.INITIALINTRO)
        {
            stadistics.SetActive(false);
            hud.SetActive(true);
        }

        totalScoreLabel.text = totalScoreInt.ToString();
        
        /*if (Input.GetKey(KeyCode.Z))
        {
            totalScoreInt += 500;
        }*/
    }

    public static void weaponUsed(Inventory.WEAPON_TYPE weaponType)
    {
        switch (weaponType)
        {
            case Inventory.WEAPON_TYPE.GUN:
                ++pistolShotsUsed;
                break;
            case Inventory.WEAPON_TYPE.SHOTGUN:
                haveShotgunIncremented = false;
                ++shotgunShotsUsed;
                break;
            case Inventory.WEAPON_TYPE.CANON:
                haveCannonIncremented = false;
                ++canonShotsUsed;
                break;
            default:
                break;
        }
        updateHud = true;
    }

    public static void weaponHit(Inventory.WEAPON_TYPE weaponType)
    {
        switch (weaponType)
        {
            case Inventory.WEAPON_TYPE.GUN:
                ++pistolShotsTouched;
                break;
            case Inventory.WEAPON_TYPE.SHOTGUN:
                if (!haveShotgunIncremented)
                {
                    ++shotgunShotsTouched;
                    haveShotgunIncremented = true;
                }
                break;
            case Inventory.WEAPON_TYPE.CANON:
                {
                    if (!haveCannonIncremented)
                    {
                        ++canonShotsTouched;
                        haveCannonIncremented = true;
                    }
                }
                haveCannonIncremented = true;
                break;
            default:
                break;
        }
    }

    public static void addDead(Enemy enemy)
    {
        timeSinceLastAction = 0.0f;
        switch (enemy)
        {
            case Enemy.WAZ:
                ++wazDeads;
                addScore((int)EnemyScore.WAZ);
                break;
            case Enemy.DRON:
                ++dronsDeads;
                addScore((int)EnemyScore.DRON);
                break;
            case Enemy.TURRET:
                ++torretDeads;
                addScore((int)EnemyScore.TURRET);
				if (torretDeads == 5)
					scoreController.tweetSystem.requestTweet (scoreController.fiveTurretsTweet);
                break;
            case Enemy.KAMIKAZE:
                ++kamikazeDeads;
                addScore((int)EnemyScore.KAMIKAZE);
				if (kamikazeDeads == 5)
					scoreController.tweetSystem.requestTweet (scoreController.fiveKamisTweet);
                break;
			case Enemy.LIL:
				++LilDeads;
				addScore ((int)EnemyScore.LIL);
				if (LilDeads == 10)
					scoreController.tweetSystem.requestTweet (scoreController.tenLilsTweet);
				else if (LilDeads == 20)
					scoreController.tweetSystem.requestTweet (scoreController.twentyLilsTweet);
                break;
            case Enemy.TV:
                ++tvDeads;
                addScore((int)EnemyScore.TV, false);
                break;
            case Enemy.CAMERA:
                ++cameraDeads;
                addScore((int)EnemyScore.CAMERA, false);
                break;
        }
        updateHud = true;
    }

    public static void boss_finish_arm_atack()
    {
        bossAtacks += 1;
        addScore((int)EnemyScore.BOSS_ARM_KEEPED, false);
    }

    public static void boss_arm_hitted()
    {
        bossArmsHitted += 1;
        addScore((int)EnemyScore.BOSS_ARM_HIT, true);
    }

    public static void addLikes(int likes)
    {
        shouldShowLikePArticle = true;
        likesInt += likes;
    }

    public static void addDislikes(int likes)
    {
        shouldShowDislikePArticle = true;
        disLikesInt += likes;
    }

    public static void addScore(int score, bool isLike = true)
    {
        totalScoreInt += score;
        if (isLike)
        {
            addLikes(score);
        }
        else
        {
            addDislikes(score);
        }
    }

    void gameFinished()
    {
        totalScoreLabel.text = "Score: " + totalScoreInt.ToString();
    }


    public int getTotalScore()
    {
        return totalScoreInt;
    }



    void updateLavels()
    {
        if (updateHud)
        {
            updateHud = false;

            if (pistolShotsUsed > 0)
                pistolAverage.text = ((pistolShotsTouched * 100) / pistolShotsUsed).ToString();
            if (SMGShotsUsed > 0)
                SMGAverage.text = ((SMGShotsTouched * 100) / SMGShotsUsed).ToString();
            if (shotgunShotsUsed > 0)
                shotgunAverage.text = ((shotgunShotsTouched * 100) / shotgunShotsUsed).ToString();
            if (canonShotsUsed > 0)
                canonAverage.text = ((canonShotsTouched * 100) / canonShotsUsed).ToString();

            wazDeadsLabel.text = wazDeads.ToString();
            turretDeadsLabel.text = torretDeads.ToString();
            dronsDeadsLabel.text = dronsDeads.ToString();
            kamikazeDeadsLabel.text = kamikazeDeads.ToString();
            lilDeadsLabel.text = LilDeads.ToString();
            bossArmsHittedLabel.text = bossArmsHitted.ToString();

            tvDeadsLabel.text = tvDeads.ToString();
            cameraDeadsLabel.text = cameraDeads.ToString();
            boringTimeLabel.text = boringTime.ToString();
            lifeLostLabel.text = lifeLost.ToString();
            bossAtacksLabel.text = bossAtacks.ToString();

            likesLabel.text = likesInt.ToString();

            if (shouldShowLikePArticle)
            {
                likeParticles.GetComponent<UIParticleSystem>().Play();
                shouldShowLikePArticle = false;
            }

            if (shouldShowDislikePArticle)
            {
                dislikeParticles.GetComponent<UIParticleSystem>().Play();
                shouldShowDislikePArticle = false;
            }


            disLikesLabel.text = disLikesInt.ToString();
        }
    }

    public static void addLoseLife(int damage)
    {
        lifeLost += damage;
        addScore(damage * MULTIUPLAY_DAMAGE_POINTS, false);
        updateHud = true;
    }
}
