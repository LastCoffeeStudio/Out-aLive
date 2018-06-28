using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
    [Header("Projectile Properties")]
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
    public AudioClip explosionSound;

	private float minimumExtent; 
	private float partialExtent; 
	private float sqrMinimumExtent; 
	private Vector3 previousPosition; 
	private Rigidbody myRigidbody;
	private Collider myCollider;
    private CtrlAudio ctrlAudio;

	[SerializeField]
	GameObject hitEffect;

	//initialize values 
	void Start() 
	{
        ctrlAudio = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<CtrlAudio>();
        myRigidbody = GetComponent<Rigidbody>();
		myCollider = GetComponent<Collider>();
		previousPosition = myRigidbody.position; 
		minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z); 
		partialExtent = minimumExtent * (1.0f - skinWidth); 
		sqrMinimumExtent = minimumExtent * minimumExtent;
    } 

	void FixedUpdate() 
	{ 
		transform.Translate (Vector3.forward * speed * Time.deltaTime);

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
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider col)
	{
        if (col.gameObject.layer == LayerMask.NameToLayer ("Wall")) 
		{
            destroyMe();
		}
        if (col.tag == "Enemy" || col.tag == "Agent" || col.tag == "Snitch")
        {
            col.gameObject.GetComponent<Enemy>().getHit(damage);
            destroyMe();
        }
		if (col.gameObject.layer == LayerMask.NameToLayer("PhysicsObjects")) 
		{
            Vector4 dataToPass = new Vector4(transform.position.x, transform.position.y, transform.position.z, damage);
			col.gameObject.SendMessage ("shotBehavior", dataToPass);
            destroyMe();
        }
    }

    void destroyMe()
	{
        if (invokeExplosion && explosionType != null)
        {
            ctrlAudio.playOneSound("Player", explosionSound, transform.position, 1.0f, 0.5f, 150);
            explosionType.SetActive(true);
        }
		if (hitEffect) 
		{
			Instantiate (hitEffect, transform.position, Quaternion.identity);
		}
		Destroy (gameObject);
	}
}
