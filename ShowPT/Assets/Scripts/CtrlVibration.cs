using UnityEngine;
using XInputDotNetPure;

public class CtrlVibration : MonoBehaviour {

    public static void playVibration(float bigVibration, float smallVibration)
    {
        //Supose that only have one controller
        GamePad.SetVibration(PlayerIndex.One, bigVibration, smallVibration);
    }

    public static void stopVibration()
    {
        //Supose that only have one controller
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }
}
