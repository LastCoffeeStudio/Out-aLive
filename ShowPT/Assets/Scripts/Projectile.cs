using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Properties")]
    public Inventory.WEAPON_TYPE projectileWeaponType = Inventory.WEAPON_TYPE.NONE;
    [SerializeField]
    float speed = 5.0f;
    [SerializeField]
    float timeLife = 10f;

    public int damage = 1;

    public LayerMask layerMask = -1; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 
    [Header("Explosion Properties")]
    public bool invokeExplosion;
    public GameObject explosionType;
    public GameObject ledsDecall;
    public AudioClip explosionSound;

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody myRigidbody;
    private Collider myCollider;
    private CtrlAudio ctrlAudio;
    [HideInInspector]
    public bool toDelete;

    [SerializeField]
    GameObject hitEffect;

    private GameObject player;

    //initialize values 
    void Start()
    {
        Debug.Log("uo");
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        player = GameObject.FindGameObjectWithTag("Player");
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
        toDelete = false;
    }

    void FixedUpdate()
    {
        Debug.Log("fixedUpdate");
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitInfo;

            //check for obstructions we might have missed 
            if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
            {
                if (!hitInfo.collider)
                    return;

                if (hitInfo.collider.isTrigger)
                    hitInfo.collider.SendMessage("OnTriggerEnter", myCollider);

                if (!hitInfo.collider.isTrigger)
                    myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;

            }
        }

        previousPosition = myRigidbody.position;

        if (timeLife > 0f)
        {
            timeLife -= Time.deltaTime;
        }
        else
        {
            toDelete = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        GameObject weapon = player.GetComponent<Inventory>().getCarryingWeapon();

        touchedEnemy(col, weapon);
    }

    public void touchedEnemy(Collider col, GameObject weapon)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Wall") || col.tag == "Sphere" || col.gameObject.layer == LayerMask.NameToLayer("BossWall"))
        {
            destroyMe();
        }
        if (col.tag == "Enemy" || col.tag == "Agent" || col.tag == "Snitch")
        {
            ScoreController.weaponHit(projectileWeaponType);
            float enemyHealth = col.gameObject.GetComponent<Enemy>().getHit(damage);
            if (weapon != null)
            {
                Crosshair crosshair = weapon.GetComponent<Crosshair>();
                updateCrosshair(enemyHealth, crosshair);
            }
            destroyMe();
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("PhysicsObjects"))
        {
            ScoreController.weaponHit(projectileWeaponType);
            Vector4 dataToPass = new Vector4(transform.position.x, transform.position.y, transform.position.z, damage);
            col.gameObject.SendMessage("shotBehavior", dataToPass);
            destroyMe();
        }
        if (col.tag == "BossArm")
        {
            ScoreController.weaponHit(projectileWeaponType);
            bool armActive;
            float armHealth = col.gameObject.GetComponent<BossArmController>().getHit(damage, out armActive);

            if (weapon != null && armActive)
            {
                Crosshair crosshair = weapon.GetComponent<Crosshair>();
                updateCrosshair(armHealth, crosshair);
            }

            destroyMe();
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("LedsWall"))
        {
            Instantiate(ledsDecall, col.transform);
            destroyMe();
        }
    }

    private void destroyMe()
    {
        if (invokeExplosion && explosionType != null)
        {
            ctrlAudio.playOneSound("Player", explosionSound, transform.position, 1.0f, 0.5f, 150);
            explosionType.SetActive(true);
        }
        if (hitEffect)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        toDelete = true;
    }

    private void updateCrosshair(float enemyHealth, Crosshair crosshair)
    {
        if (enemyHealth <= 0f)
        {
            crosshair.enemyDeath();
        }
        else
        {
            crosshair.enemyReached();
        }
    }
}