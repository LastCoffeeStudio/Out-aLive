/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEditor;
using UnityEngine;

public class TerrainQualityManagerAboutWindow : EditorWindow
{
    [MenuItem("Window/Infinity Code/Terrain Quality Manager/About")]
    public static void OpenWindow()
    {
        TerrainQualityManagerAboutWindow window = GetWindow<TerrainQualityManagerAboutWindow>(true, "About", true);
        window.minSize = new Vector2(200, 100);
        window.maxSize = new Vector2(200, 100);
    }

    public void OnGUI()
    {
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
        titleStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle textStyle = new GUIStyle(EditorStyles.label);
        textStyle.alignment = TextAnchor.MiddleCenter;

        GUILayout.Label("Terrain Quality Manager", titleStyle);
        GUILayout.Label("version " + TerrainQualityManagerWindow.version, textStyle);
        GUILayout.Label("created Infinity Code", textStyle);
        GUILayout.Label("2013-2016", textStyle);
    }
}