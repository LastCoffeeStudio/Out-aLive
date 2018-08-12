using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Scene data")]
    public GameObject minLimit;
    public GameObject maxLimit;

    [Header("Boss components")]
    public Arm[] arms;
    public GameObject platform1;
    public GameObject platform2;
    public GameObject body;
    public GameObject head;
    public GameObject spawnSelector;
    public GameObject arrow;

    [Header("Boss attributes")]
    public float diameter;
    public int idleDamage;
    public int rotationDamage;
    public int rollDamage;

    [Header("Rotate attack attributes")]
    public int totalRepeatNumber;
    public int rollRepeatNumber;
    public float maxSpeed;
    public float acceleration;
    public float minPlayerDistance;
    public AnimationClip rotateAnim;
    public AnimationCurve rollDisplacementCurve;

    [Header("Arms attack attributes")]
    public float recoverArmsStartTime;

    [Header("Roulette attack attributes")]
    public EnemySpawn[] enemies;
    //TODO: change spawnpoints for an area when the boss room is flat.
    public GameObject[] points;
    
    private GameObject player;
    private Bounds combatZone;
    private Animator bossAnimator;
    private Vector3 playerPosition;
    private STATES state = STATES.ROTATE_ATTACK;
    private int rollRepeat = 0;
    private int totalRepeat = 0;
    private int armsStopped = 0;
    private int armsDead = 0;
    private float chaseSpeed = 0f;
    private float timeElapsed = 0f;
    private bool armsActive = false;
    private bool chasePlayer = false;

    private enum STATES
    {
        ROTATE_ATTACK,
        ARMS_ATTACK,
        ROULETTE_ATTACK,
        DEFEAT,
        END_STATE
    }

    [System.Serializable]
    public struct Arm
    {
        [HideInInspector]
        public int id;
        public GameObject arm;
        public int health;
        public int damage;
        public float attackDistance;
        public float speed;
        [HideInInspector]
        public bool dead;
        [HideInInspector]
        public bool stopped;
        [HideInInspector]
        public float delayLaunchTime;
        [HideInInspector]
        public Vector3 initialPosition;
        [HideInInspector]
        public Vector3 endingPosition;
    }

    [System.Serializable]
    public struct EnemySpawn
    {
        public Enemy.EnemyType type;
        public GameObject enemy;
        public int minNumEnemies;
        public int maxNumEnemies;
    }
    
    
    /**Boss**/

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bossAnimator = GetComponent<Animator>();
        Vector3 combatZoneSize = (minLimit.transform.position - maxLimit.transform.position);
        combatZoneSize = new Vector3(Mathf.Abs(combatZoneSize.x), Mathf.Abs(combatZoneSize.y), Mathf.Abs(combatZoneSize.z));
        combatZone = new Bounds((maxLimit.transform.position - minLimit.transform.position) / 2f + minLimit.transform.position, combatZoneSize);
        Debug.Log(combatZone.min + " " + combatZone.max);
        Debug.Log(combatZone.Contains(new Vector3(-10f, 5f, 76f)));
        setArms();
        createRotationCurve();
    }
	
	void Update ()
    {
        switch (state)
        {
            case STATES.ROTATE_ATTACK:

                if (chasePlayer)
                {
                    if (Vector3.Distance(transform.position, playerPosition) >= minPlayerDistance && combatZone.Contains(transform.position))
                    {
                        chaseSpeed += acceleration * Time.deltaTime;
                        chaseSpeed = Mathf.Clamp(chaseSpeed, 0, maxSpeed);
                        Vector3 dir = (playerPosition - transform.position).normalized;
                        dir.y = 0f;
                        Debug.DrawRay(transform.position, (playerPosition - transform.position), new Color(0f, 1f, 0f, 1f));
                        transform.Translate(dir * chaseSpeed * Time.deltaTime, Space.World);
                    }
                    else
                    {
                        chaseSpeed = 0f;
                        chasePlayer = false;
                        bossAnimator.SetBool("PlayerCaught", true);
                    }
                }
                break;
            case STATES.ARMS_ATTACK:
                if (armsActive)
                {
                    if (armsDead == arms.Length)
                    {
                        bossAnimator.SetInteger("State", 3);
                        state = STATES.DEFEAT;
                        break;
                    }

                    timeElapsed += Time.deltaTime;
                    if (armsStopped < (arms.Length - armsDead))
                    {
                        for (int i = 0; i < arms.Length; ++i)
                        {
                            if (!arms[i].dead && !arms[i].stopped && arms[i].delayLaunchTime <= timeElapsed)
                            {
                                float displacement = arms[i].speed * Time.deltaTime;
                                arms[i].arm.transform.Translate(arms[i].arm.transform.forward * displacement, Space.World);

                                if (Vector3.Distance(arms[i].initialPosition, arms[i].arm.transform.position) >= arms[i].attackDistance)
                                {
                                    stopArm(i);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (timeElapsed <= recoverArmsStartTime)
                        {
                            for (int i = 0; i < arms.Length; ++i)
                            {
                                if (!arms[i].dead)
                                {
                                    float displacement = arms[i].speed * Time.deltaTime;
                                    arms[i].arm.transform.position = Vector3.Slerp(arms[i].endingPosition, arms[i].initialPosition, timeElapsed / recoverArmsStartTime);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < arms.Length; ++i)
                            {
                                if (!arms[i].dead)
                                {
                                    arms[i].arm.transform.position = arms[i].initialPosition;
                                    arms[i].arm.GetComponent<BossArmController>().vulnerable = false;
                                }
                            }
                            timeElapsed = 0f;
                            armsStopped = 0;
                            bossAnimator.SetInteger("State", 2);
                            state = STATES.ROULETTE_ATTACK;
                            armsActive = false;
                        }
                    }
                }
                break;
            default:
                break;
        }
	}

    public int getBossDamage(int armId = -1)
    {
        switch (state)
        {
            case STATES.ROTATE_ATTACK:
                if (bossAnimator.GetBool("Roll"))
                {
                    return rollDamage;
                }
                else
                {
                    return rotationDamage;
                }
            case STATES.ARMS_ATTACK:
                if (armId != -1)
                {
                    return arms[armId].damage;
                }
                break;
        }

        return idleDamage;
    }

    public void stopArm(int id)
    {
        arms[id].stopped = true;
        arms[id].endingPosition = arms[id].arm.transform.position;
        ++armsStopped;
        if (armsStopped == (arms.Length - armsDead))
        {
            timeElapsed = 0f;
        }
    }

    public void getHitArm(int id, int damage)
    {
        arms[id].health -= damage;
        Debug.Log(arms[id].health);
        if (arms[id].health <= 0)
        {
            arms[id].dead = true;
            ++armsDead;
            arms[id].arm.SetActive(false);
            //Activate death effect (activate another "Arm" broken, stays on the ground)
        }
    }

    
    /**Animation calls**/

    public void startChasingPlayer()
    {
        playerPosition = player.transform.position;
        chasePlayer = true;
        bossAnimator.SetBool("PlayerCaught", false);
    }

    public void calculateRollData()
    {
        calculateDirectionFromPlayerPosition();
        checkRoll();
        StartCoroutine("rollBoss");
    }

    public void setRoll()
    {
        ++rollRepeat;
        if (rollRepeat >= rollRepeatNumber)
        {
            rollRepeat = 0;
            bossAnimator.SetBool("Roll", false);
        }

        if (totalRepeat >= totalRepeatNumber)
        {
            totalRepeat = 0;
            bossAnimator.SetInteger("State", 1);
            state = STATES.ARMS_ATTACK;
        }
    }

    public void prepareArms(AnimationEvent animationEvent)
    {
        int boolParam = animationEvent.intParameter;

        for (int i = 0; i < arms.Length; ++i)
        {
            if (!arms[i].dead)
            {
                arms[i].arm.SetActive(boolParam != 0);
                arms[i].initialPosition = arms[i].arm.transform.position;
                arms[i].stopped = false;
            }
        }
    }

    public void activateArms()
    {
        armsActive = true;
        for (int i = 0; i < arms.Length; ++i)
        {
            if (!arms[i].dead)
            {
                arms[i].arm.GetComponent<BossArmController>().vulnerable = true;
            }
        }
    }

    public void activatePlatform(AnimationEvent animationEvent)
    {
        int boolParam = animationEvent.intParameter;
        platform1.SetActive(boolParam != 0);
        platform2.SetActive(boolParam != 0);

        if (boolParam != 0)
        {
            ++totalRepeat;
        }
        else
        {
            bossAnimator.SetBool("Roll", true);
        }
    }

    public void activateRoulette(AnimationEvent animationEvent)
    {
        int boolParam = animationEvent.intParameter;
        head.SetActive(boolParam != 0);
        spawnSelector.SetActive(boolParam != 0);
        arrow.SetActive(boolParam != 0);

        if (boolParam != 0)
        {
            bossAnimator.SetInteger("RouletteRoll", Random.Range(1, 4));
            calculateDirectionFromPlayerPosition();
        }
        else
        {
            state = STATES.ROTATE_ATTACK;
            bossAnimator.SetInteger("State",0);
        }
    }

    public void summonEnemies(AnimationEvent animationEvent)
    {
        int typeParam = animationEvent.intParameter;
        Enemy.EnemyType type = Enemy.EnemyType.NONE;
        switch (typeParam)
        {
            case 1:
                type = Enemy.EnemyType.LILONE;
                break;
            case 2:
                type = Enemy.EnemyType.TURRET;
                break;
            case 3:
                type = Enemy.EnemyType.KAMIKAZE;
                break;
        }

        bool[] spawns = new bool[points.Length];
        for (int i = 0; i < spawns.Length; ++i)
        {
            spawns[i] = false;
        }

        EnemySpawn enemySpawn = findEnemySpawn(type);

        if (enemySpawn.type != Enemy.EnemyType.NONE)
        {
            int numberSpawns = Random.Range(enemySpawn.minNumEnemies, enemySpawn.maxNumEnemies+1);

            for (int i = 0; i < numberSpawns; ++i)
            {
                int randomPoint = Random.Range(0,spawns.Length);
                //Number of spawnpoints must be greater than maximum num enemies of any enemy!
                while (spawns[randomPoint])
                {
                    ++randomPoint;
                    if (randomPoint >= spawns.Length)
                    {
                        randomPoint = 0;
                    }
                }

                if (!spawns[randomPoint] && !body.GetComponent<BoxCollider>().bounds.Contains(points[randomPoint].transform.position))
                {
                    spawns[randomPoint] = true;
                    Instantiate(enemySpawn.enemy, points[randomPoint].transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void calculateDirectionFromPlayerPosition()
    {
        float angle = Vector3.Angle(transform.forward, (player.transform.position - transform.position).normalized);
        //Negative left, positive right
        Vector3 playerVec = (player.transform.position - transform.position);
        playerVec.y = 0;
        float dot = Vector3.Dot(transform.right, playerVec.normalized);
        
        if (dot >= 0)
        {
            if (angle > 45 && angle <= 135)
            {
                transform.Rotate(0f, 90f, 0f);
            }
            else if (angle > 135)
            {
                transform.Rotate(0f, 180f, 0f);
            }
        }
        else
        {
            if (angle > 45 && angle <= 135)
            {
                transform.Rotate(0f, -90f, 0f);
            }
            else if (angle > 135)
            {
                transform.Rotate(0f, -180f, 0f);
            }
        }
    }

    private void checkRoll()
    {
        Vector3 destinationPoint = transform.position + transform.forward * body.GetComponent<Renderer>().bounds.size.z;
        int count = 0;
        while (count <= 4 && !combatZone.Contains(destinationPoint))
        {
            transform.Rotate(0f, 90f, 0f);
            destinationPoint = transform.position + transform.forward * body.GetComponent<Renderer>().bounds.size.z;
            ++count;
        }
    }

    private IEnumerator rollBoss()
    {
        float time = 0f;
        while (time * 60 < rollDisplacementCurve.length)
        {
            float prevTime = time;
            time += Time.deltaTime;
            if (time * 60 > rollDisplacementCurve.length)
            {
                time = rollDisplacementCurve.length / 60f;
            }
            for (int i = ((int)(prevTime * 60)); i < (int)(time * 60); ++i)
            {
                transform.Translate(transform.forward * rollDisplacementCurve.keys[i].value, Space.World);
            }
            yield return null;
        }
    }

    private EnemySpawn findEnemySpawn(Enemy.EnemyType type)
    {
        for (int i = 0; i < enemies.Length; ++i)
        {
            if (enemies[i].type == type)
            {
                return enemies[i];
            }
        }

        EnemySpawn spawn = new EnemySpawn();
        spawn.type = Enemy.EnemyType.NONE;

        return spawn;
    }
    
    /**Initialization**/

    private void setArms()
    {
        float delayTime = 0f;
        for (int i = 0; i < arms.Length; ++i)
        {
            arms[i].id = i;
            arms[i].arm.GetComponent<BossArmController>().id = i;
            arms[i].dead = false;
            arms[i].delayLaunchTime = delayTime;
            delayTime += 0.3f;
        }
    }

    private void createRotationCurve()
    {
        Keyframe[] frames = new Keyframe[(int)rotateAnim.length * 60];
        rotateAnim.SampleAnimation(gameObject, rotateAnim.length);
        float lastPos = body.transform.rotation.x;

        for (int i = 0; i < frames.Length; ++i)
        {
            rotateAnim.SampleAnimation(gameObject, i/60f);
            float rotationX = body.transform.rotation.x;
            
            if (i > 0)
            {
                rotateAnim.SampleAnimation(gameObject, (i - 1) / 60f);
                float lastRotationX = body.transform.rotation.x;
                frames[i] = new Keyframe(i, ((diameter * rotationX) / lastPos) - ((diameter * lastRotationX) / lastPos));
            }
            else
            {
                frames[i] = new Keyframe(i, (diameter * body.transform.rotation.x) / lastPos);
            }
        }
        rollDisplacementCurve = new AnimationCurve(frames);
    }

    
    /**Utils**/

    void OnDrawGizmosSelected()
    {
        Vector3 bSize = (minLimit.transform.position - maxLimit.transform.position);
        bSize = new Vector3(Mathf.Abs(bSize.x), Mathf.Abs(bSize.y), Mathf.Abs(bSize.z));
        Bounds b = new Bounds((maxLimit.transform.position - minLimit.transform.position) / 2f + minLimit.transform.position, bSize);
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(b.center, b.size);
    }
}
