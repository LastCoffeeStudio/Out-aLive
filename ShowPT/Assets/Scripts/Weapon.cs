using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public AudioSource shotAudio;
    public AudioSource reloadAudio;
    public AudioSource lastReloadAudio;
    public float amount;
    public float maxAmount;
    public float smoothAmount;
    public int damage;
    public Transform shootPoint;
    public Transform camera;
    public uint maxAmmo = 10;
    public uint ammunition;
    public Text text;
    public Text numDronsText;

    private bool firing = false;
    private bool reloading = false;
    private bool aimming = false;
    private Animator animator;
    private Vector3 initialposition;

    public Vector3 aimPosition;
    public Vector3 originalPosition;
    public float aimSpeed;
    public float weaponRange;
    public GameObject esferaVerde;
    public GameObject esferaRoja;
    public GameObject explosion;

    public int numDrons = 0;

    private Inventory inventory;
    public Inventory.AMMO_TYPE typeAmmo;

    public Text wazPuntuation;
    public Text turretPuntuation;
    public Text dronsPuntuation;
    public Text totalEnemies;
    public Text totalScore;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void swagWeaponMovement()
    {
        reloadAudio.Stop();
        float movX = -Input.GetAxis("Mouse X") * amount;
        float movY = -Input.GetAxis("Mouse Y") * amount;
        movX = Mathf.Clamp(movX, -maxAmount, maxAmount);
        movY = Mathf.Clamp(movY, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(movX, movY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialposition, Time.deltaTime * smoothAmount);
    }
}
