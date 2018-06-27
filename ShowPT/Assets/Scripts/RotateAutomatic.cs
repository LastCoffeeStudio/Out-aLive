using UnityEngine;
using System.Collections;

//*El objeto que tenga este script rotará de forma automática a una velocidad determinada.*

public class RotateAutomatic : MonoBehaviour {

	public float rotationSpeed = 1f;

	//Si changeDirectionTime es 0, la torreta no cambiará de dirección.
	public float changeDirectionTime;
	private float changeDirectionCounter = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
		if(changeDirectionTime != 0){
			changeDirectionCounter += Time.deltaTime;
			if(changeDirectionCounter >= changeDirectionTime){
				changeDirectionCounter = 0;
				rotationSpeed = -rotationSpeed;
			}
		}
	}
}
