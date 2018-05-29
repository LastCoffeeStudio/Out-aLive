/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEditor;
using UnityEngine;

public static class TerrainQualityManagerGUIUtils
{
    public const int labelWidth = 160;
    public const int minColWidth = 70;

    private static GUIStyle _changedStyle;
    private static Texture2D _helpIcon;
    private static GUIStyle _helpStyle;

    private static GUIStyle changedStyle
    {
        get
        {
            if (_changedStyle == null)
            {
                _changedStyle = new GUIStyle(EditorStyles.label);
                _changedStyle.normal.textColor = Color.green;
            }
            return _changedStyle;
        }
    }

    public static Texture2D helpIcon
    {
        get
        {
            if (_helpIcon == null)
            {
                string[] guids = AssetDatabase.FindAssets("HelpIcon t:texture");
                if (guids != null && guids.Length > 0)
                {
                    string iconPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _helpIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(iconPath, typeof(Texture2D));
                }
                
            }
            return _helpIcon;
        }
    }

    public static GUIStyle helpStyle
    {
        get
        {
            if (_helpStyle == null)
            {
                _helpStyle = new GUIStyle();
                _helpStyle.margin = new RectOffset(0, 0, 2, 0);
            }
            return _helpStyle;
        }
    }

    public static void DrawField(string label, int defaultValue, ref int value, string tooltip, string href, string[] displayedOptions = null, int[] optionValues = null)
    {
        bool changed = defaultValue != value;

        EditorGUILayout.BeginHorizontal(GUILayout.Height(26));
        GUILayout.Box(GUIContent.none, GUIStyle.none, GUILayout.Width(5));
        DrawHelpButton(tooltip, href);
        EditorGUILayout.LabelField(label, changed ? changedStyle : EditorStyles.label, GUILayout.Width(labelWidth));
        EditorGUILayout.LabelField(defaultValue.ToString(), GUILayout.MinWidth(minColWidth));
        if (displayedOptions != null) value = EditorGUILayout.IntPopup(string.Empty, value, displayedOptions, optionValues, GUILayout.MinWidth(minColWidth));
        else value = EditorGUILayout.IntField("", value, GUILayout.MinWidth(minColWidth));

        if (changed)
        {
            GUIStyle clearButtonStyle = new GUIStyle(GUI.skin.button);
            clearButtonStyle.margin.top = 2;
            clearButtonStyle.margin.bottom = 0;
            if (GUILayout.Button("X", clearButtonStyle, GUILayout.Width(25), GUILayout.Height(15))) value = defaultValue;
        }
        else GUILayout.Box("", GUIStyle.none, GUILayout.Width(25));

        EditorGUILayout.EndHorizontal();
    }

    private static void DrawHelpButton(string tooltip, string href = null)
    {
        if (GUILayout.Button(new GUIContent(helpIcon, tooltip),
            helpStyle, GUILayout.ExpandWidth(false)) && !string.IsNullOrEmpty(href))
            Application.OpenURL(href);
    }

    public static int IntPopup(string label, int value, string[] displayedOptions, int[] optionValues, string tooltip, string href)
    {
        GUILayout.BeginHorizontal();
        DrawHelpButton(tooltip, href);
        value = EditorGUILayout.IntPopup(label, value, displayedOptions, optionValues);

        GUILayout.EndHorizontal();
        return value;
    }
}