using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PostProcessVolume))]
public class ResetValues : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PostProcessVolume myScript = (PostProcessVolume)target;
        if (GUILayout.Button("ResetValues"))
        {
            myScript.ResetValues();
        }


    }
}