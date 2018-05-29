using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCanon : Weapon {

    [Header("Canon Settings")]
    public float overheatMaxTime;

    private float overheatTime;
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
        if (firing)
        {
            overheatTime += Time.deltaTime;
            checkMouseInput();
        }
    }

    protected override void checkMouseInput()
    {
        if (overheatTime >= overheatMaxTime || (!Input.GetButton("Fire1") && Input.GetAxis("AxisRT") < 0.5f) || ammunition == 0)
        {
            animator.SetBool("shooting", false);
            firing = false;
            overheatTime = 0f;
        }
    }
}
