using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadStateBehaviour : StateMachineBehaviour {

    private Transform particlesTransform;       // Reference to the instantiated prefab's transform.
    private ParticleSystem particleSystem;      // Reference to the instantiated prefab's particle system.

    private Gun gun;

    // This will be called when the animator first transitions to this state.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
    }

    // This will be called once the animator has transitioned out of the state.
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // When leaving the special move state, stop the particles.
        gun.endReload();
    }

    // This will be called every frame whilst in the state.
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
