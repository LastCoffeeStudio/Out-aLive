using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    public enum Enemy
    {
        WAZ,
        TURRET,
        DRON,
        KAMIKAZE,
        LIL
    }

    public enum EnemyScore
    {
        WAZ = 10,
        TURRET = 20,
        DRON = 30,
        KAMIKAZE = 40,
        LIL = 50
    }

    public enum ActionScore
    {
        JUMP = 100,
        DEAD = 2000
    }

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

    public Text gunShotsUsedText;
    public Text rifleShotsUsedText;
    public Text weaponShotsUsedText;
    public Text gunShotsTouchedText;
    public Text rifleShotsTouchedText;
    public Text weaponShotsTouchedText;

    //Public Gui objects
    private static int likesInt;
    private static int disLikesInt;
    private static int totalScoreInt;

    private static int wazDeads;
    private static int torretDeads;
    private static int dronsDeads;
    private static int kamikazeDeads;
    private static int LilDeads;
    private static int totalEnemies;

    private static int gunShotsUsed;
    private static int rifleShotsUsed;
    private static int weaponShotsUsed;

    private static int gunShotsTouched;
    private static int rifleShotsTouched;
    private static int weaponShotsTouched;

    public static void gunUsed() { ++gunShotsUsed;  }
    public static void rifleUsed() { ++rifleShotsUsed; }
    public static void weaponUsed() { ++weaponShotsUsed; }

    public static void gunHit() { ++gunShotsTouched; }
    public static void rifleHit() { ++rifleShotsTouched; }
    public static void weaponHit() { ++weaponShotsTouched; }

    public GameObject stadistics;
    public GameObject hud;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            stadistics.SetActive(true);
            hud.SetActive(false);
        }
        else
        {
            stadistics.SetActive(false);
            hud.SetActive(true);
        }

        totalScoreLabel.text = totalScoreInt.ToString();
    }
    


    public static void addDead(Enemy enemy)
    {
        switch (enemy)
        {
            case Enemy.WAZ:
                ++wazDeads;
                addScore((int)EnemyScore.WAZ);
                break;
            case Enemy.DRON:
                ++torretDeads;
                addScore((int)EnemyScore.DRON);
                break;
            case Enemy.TURRET:
                ++torretDeads;
                addScore((int)EnemyScore.TURRET);
                break;
            case Enemy.KAMIKAZE:
                ++kamikazeDeads;
                addScore((int)EnemyScore.KAMIKAZE);
                break;
            case Enemy.LIL:
                ++LilDeads;
                addScore((int)EnemyScore.LIL);
                break;
        }
    }

    public void addLikes(int likes)
    {
        likesInt += likes;
    }

    public void addDislikes(int likes)
    {
        likesInt += likes;
    }

    public static void addScore(int score)
    {
        totalScoreInt += score;
    }

    void gameFinished()
    {
        totalScoreLabel.text = "Score: " + totalScoreInt.ToString();
    }
}
