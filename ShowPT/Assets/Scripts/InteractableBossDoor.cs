using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBossDoor : InteractableObject {

    private BGM bgm;

    protected override void Start()
    {
        bgm = GameObject.FindGameObjectWithTag("CtrlAudio").GetComponent<BGM>();
        base.Start();
    }

    protected override void executeAction()
	{
		GetComponent<BossDoor>().OpenSesame();
        bgm.playMeSomething(3);
		setState(InteractableObjectState.DISABLE);
	}
}
