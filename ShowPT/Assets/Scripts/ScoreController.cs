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

    public Text pistolShotsUsedText;
    public Text shotgunShotsUsedText;
    public Text SMGShotsUsedText;
    public Text canonShotsUsedText;
    public Text pistolShotsTouchedText;
    public Text shotgunShotsTouchedText;
    public Text SMGShotsTouchedText;
    public Text canonShotsTouchedText;

    public Text pistolAverage;
    public Text shotgunAverage;
    public Text SMGAverage;
    public Text canonAverage;

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

    private static bool updateHud = false;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Tab) || CtrlGameState.getGameState() == CtrlGameState.gameStates.DEATH || CtrlGameState.getGameState() == CtrlGameState.gameStates.WIN)
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
        if(updateHud)
        {
            updateHud = false;
            pistolShotsUsedText.text = pistolShotsUsed.ToString();
            pistolShotsTouchedText.text = pistolShotsTouched.ToString();

            SMGShotsUsedText.text = SMGShotsUsed.ToString();
            SMGShotsTouchedText.text = SMGShotsTouched.ToString();

            shotgunShotsUsedText.text = shotgunShotsUsed.ToString();
            shotgunShotsTouchedText.text = shotgunShotsTouched.ToString();

            canonShotsUsedText.text = canonShotsUsed.ToString();
            canonShotsTouchedText.text = canonShotsTouched.ToString();

            pistolAverage.text = (( pistolShotsTouched * 100 ) / pistolShotsUsed).ToString();
            SMGAverage.text = ((SMGShotsTouched * 100) / SMGShotsUsed).ToString();
            shotgunAverage.text = ((shotgunShotsTouched * 100) / shotgunShotsUsed).ToString();
            canonAverage.text = ((canonShotsTouched * 100) / canonShotsUsed).ToString();
        }
    }

    public static void weaponUsed(Inventory.WEAPON_TYPE weaponType)
    {
        switch (weaponType)
        {
            case Inventory.WEAPON_TYPE.GUN:
                ++pistolShotsUsed;
                break;
            case Inventory.WEAPON_TYPE.SHOTGUN:
                ++shotgunShotsUsed;
                break;
            case Inventory.WEAPON_TYPE.RIFLE:
                ++SMGShotsUsed;
                break;
            case Inventory.WEAPON_TYPE.CANON:
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
                ++shotgunShotsTouched;
                break;
            case Inventory.WEAPON_TYPE.RIFLE:
                ++SMGShotsTouched;
                break;
            case Inventory.WEAPON_TYPE.CANON:
                ++canonShotsTouched;
                break;
            default:
                break;
        }
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
