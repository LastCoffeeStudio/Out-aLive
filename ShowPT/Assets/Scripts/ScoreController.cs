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
        LIL,
        TV,
        CAMERA,

        UNDEFINED
    }

    public enum EnemyScore
    {
        WAZ = 10,
        TURRET = 20,
        DRON = 30,
        KAMIKAZE = 40,
        LIL = 50,
        TV = 10,
        CAMERA = 10
    }

    public enum ActionScore
    {
        JUMP = 100,
        DEAD = 2000
    }

    private const float SECONDS_TO_BORING = 20;
    private const int POINTS_OF_BORING = 1;

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

    public Text tvDeadsLabel;
    public Text cameraDeadsLabel;
    public Text boringTimeLabel;
    public Text lifeLostLabel;
    
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

    //Dislikes
    private static int tvDeads;
    private static int cameraDeads;
    private static int boringTime;
    private static int lifeLost;

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
    public static float timeSinceLastAction = 0.0f;

    // Use this for initialization
    void Start () {}
	
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
        else
        {
            stadistics.SetActive(false);
            hud.SetActive(true);
        }

        totalScoreLabel.text = totalScoreInt.ToString();
        
        if (Input.GetKey(KeyCode.Z))
        {
            totalScoreInt += 500;
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
            case Inventory.WEAPON_TYPE.CANON:
                ++canonShotsTouched;
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
            case Enemy.TV:
                ++tvDeads;
                addScore((int)EnemyScore.TV);
                break;
            case Enemy.CAMERA:
                ++cameraDeads;
                addScore((int)EnemyScore.CAMERA);
                break;
        }
        updateHud = true;
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

            tvDeadsLabel.text = tvDeads.ToString();
            cameraDeadsLabel.text = cameraDeads.ToString();
            boringTimeLabel.text = boringTime.ToString();
            lifeLostLabel.text = lifeLost.ToString();
        }
    }
}
