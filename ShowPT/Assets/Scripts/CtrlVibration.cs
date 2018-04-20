using UnityEngine;
using XInputDotNetPure;

public class CtrlVibration : MonoBehaviour {

    public void playVibration(float bigVibration, float smallVibration)
    {
        //Supose that only have one controller
        GamePad.SetVibration(PlayerIndex.One, bigVibration, smallVibration);
    }

    public void stopVibration()
    {
        //Supose that only have one controller
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }
}
