﻿/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Xml;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TerrainQualityManagerUpdater : EditorWindow 
{
    private TerrainQualityManagerUpdateChannel channel = TerrainQualityManagerUpdateChannel.stable;
    private string invoiceNumber;
    private Vector2 scrollPosition;
    private List<TerrainQualityManagerUpdateItem> updates;

    private void CheckNewVersions()
    {
        if (string.IsNullOrEmpty(invoiceNumber))
        {
            EditorUtility.DisplayDialog("Error", "Please enter the Invoice Number.", "OK");
            return;
        }

        int inum;

        if (!int.TryParse(invoiceNumber, out inum))
        {
            EditorUtility.DisplayDialog("Error", "Wrong Invoice Number.", "OK");
            return;
        }

        SavePrefs();

        string updateKey = GetUpdateKey();
        GetUpdateList(updateKey);
    }

    private string GetUpdateKey()
    {
        WebClient client = new WebClient();
        client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        string updateKey = client.UploadString("http://infinity-code.com/products_update/getupdatekey.php",
            "key=" + invoiceNumber + "&package=" + WWW.EscapeURL("Terrain Quality Manager"));

        return updateKey;
    }

    private void GetUpdateList(string updateKey)
    {
        WebClient client = new WebClient();
        client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

        string response;

        try
        {
            response = client.UploadString("http://infinity-code.com/products_update/checkupdates.php",
            "k=" + WWW.EscapeURL(updateKey) + "&v=" + TerrainQualityManagerWindow.version + "&c=" + (int)channel);
        }
        catch
        {
            return;
        }
        
        XmlDocument document = new XmlDocument();
        document.LoadXml(response);

        XmlNode firstChild = document.FirstChild;
        updates = new List<TerrainQualityManagerUpdateItem>();

        foreach (XmlNode node in firstChild.ChildNodes) updates.Add(new TerrainQualityManagerUpdateItem(node));
    }

    private void OnEnable()
    {
        if (EditorPrefs.HasKey("TerrainQualityManagerInvoiceNumber"))
            invoiceNumber = EditorPrefs.GetString("TerrainQualityManagerInvoiceNumber");
        else invoiceNumber = "";

        if (EditorPrefs.HasKey("TerrainQualityManagerUpdateChannel"))
            channel = (TerrainQualityManagerUpdateChannel)EditorPrefs.GetInt("TerrainQualityManagerUpdateChannel");
        else channel = TerrainQualityManagerUpdateChannel.stable;
    }

    private void OnDestroy()
    {
        SavePrefs();
    }

    private void OnGUI()
    {
        invoiceNumber = EditorGUILayout.TextField("Invoice Number:", invoiceNumber).Trim(new[] { ' ' });
        channel = (TerrainQualityManagerUpdateChannel) EditorGUILayout.EnumPopup("Channel:", channel);

        if (GUILayout.Button("Check new versions")) CheckNewVersions();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (updates != null)
        {
            foreach (TerrainQualityManagerUpdateItem update in updates) update.Draw();
            if (updates.Count == 0) GUILayout.Label("No updates");
        }

        EditorGUILayout.EndScrollView();
    }

    [MenuItem("Window/Infinity Code/Terrain Quality Manager/Check Updates", false, 2)]
    public static void OpenWindow()
    {
        GetWindow<TerrainQualityManagerUpdater>(false, "Terrain Quality Manager Updater", true);
    }

    private void SavePrefs()
    {
        EditorPrefs.SetString("TerrainQualityManagerInvoiceNumber", invoiceNumber);
        EditorPrefs.SetInt("TerrainQualityManagerUpdateChannel", (int)channel);
    }
}

public class TerrainQualityManagerUpdateItem
{
    private string version;
    private int type;
    private string changelog;
    private string download;
    private string date;

    private static GUIStyle _changelogStyle;
    private static GUIStyle _titleStyle;

    private static GUIStyle changelogStyle
    {
        get { return _changelogStyle ?? (_changelogStyle = new GUIStyle(EditorStyles.label) {wordWrap = true}); }
    }

    private static GUIStyle titleStyle
    {
        get
        {
            return _titleStyle ??
                   (_titleStyle = new GUIStyle(EditorStyles.boldLabel) {alignment = TextAnchor.MiddleCenter});
        }
    }

    public TerrainQualityManagerUpdateItem(XmlNode node)
    {
        version = node.SelectSingleNode("Version").InnerText;
        type = int.Parse(node.SelectSingleNode("Type").InnerText);
        changelog = node.SelectSingleNode("ChangeLog").InnerText;
        download = node.SelectSingleNode("Download").InnerText;
        date = node.SelectSingleNode("Date").InnerText;

        string[] vars = version.Split(new[] {'.'});
        string[] vars2 = new string[4];
        vars2[0] = vars[0];
        vars2[1] = int.Parse(vars[1].Substring(0, 2)).ToString();
        vars2[2] = int.Parse(vars[1].Substring(2, 2)).ToString();
        vars2[3] = int.Parse(vars[1].Substring(4, 4)).ToString();
        version = string.Join(".", vars2);
    }

    public void Draw()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Version: " + version + " (" + typeStr + "). " + date, titleStyle);

        GUILayout.Label(changelog, changelogStyle);

        if (GUILayout.Button("Download"))
        {
            Process.Start("http://infinity-code.com/products_update/download.php?k=" + download);
        }

        GUILayout.EndVertical();
    }

    public string typeStr
    {
        get { return Enum.GetName(typeof (TerrainQualityManagerUpdateChannel), type); }
    }
}

public enum TerrainQualityManagerUpdateChannel
{
    stable = 10,
    releaseCandidate = 20,
    beta = 30,
    alpha = 40,
    working = 50
}
