/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable]
public class TerrainQualityManagerWindow : EditorWindow 
{
    public const string version = "1.1.0.1";
    private const int maxExecutionTime = 1000000;

    private int[] availableHeights = { 33, 65, 129, 257, 513, 1025, 2049, 4097 };
    private int[] availableResolutions = { 32, 64, 128, 256, 512, 1024, 2048 };

    private string[] availableResolutionsStr;
    private string[] availableHeightsStr;

    private int alphamapResolution = 512;
    private int baseMapResolution = 1024;
    private int detailResolution = 1024;
    private int heightmapResolution = 513;
    private int resolutionPerPatch = 8;

    private int defAlphamapResolution = 512;
    private int defBaseMapResolution = 1024;
    private int defDetailResolution = 1024;
    private int defHeightmapResolution = 513;
    private int defResolutionPerPatch = 8;

    private TerrainQualityManagerModes mode = TerrainQualityManagerModes.singleTerrain;

    [SerializeField]
    private Terrain terrain;

    private TerrainQualityManagerState state = TerrainQualityManagerState.idle;
    private List<TerrainQualityManagerQuery> queries;

    private int queryIndex = 0;
    private int activeQueryIndex = 0;
    private Vector2 scrollPosition;

    private TerrainQualityManagerQuery activeQuery
    {
        get
        {
            if (queries == null) return null;
            if (queries.Count <= queryIndex) return null;
            return queries[queryIndex];
        }
    }

    private TerrainQualityManagerQuery AddQuery(string name, TerrainData terrainData = null)
    {
        if (queries == null) queries = new List<TerrainQualityManagerQuery>();
        TerrainQualityManagerQuery query = new TerrainQualityManagerQuery(terrainData, name);
        queries.Add(query);
        return query;
    }

    private void ApplyNewHeightmap(TerrainData terrainData, float[,] heightmap)
    {
        Vector3 size = terrainData.size;
        terrainData.heightmapResolution = heightmapResolution;
        terrainData.SetHeights(0, 0, heightmap);
        terrainData.size = size;
    }

    private void DownScaleAlphamap(TerrainQualityManagerQuery query)
    {
        TerrainData terrainData = query.terrainData;
        int splatCount = terrainData.splatPrototypes.Length;

        if (query.data == null)
        {
            object[] data = new object[3];
            data[0] = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
            data[1] = new float[alphamapResolution, alphamapResolution, splatCount];
            query.data = data;
        }

        object[] queryData = query.data as object[];
        float[,,] alphamaps = queryData[0] as float[,,];
        float[,,] newAlphamaps = queryData[1] as float[,,];

        int step = terrainData.alphamapResolution / alphamapResolution;
        int halfStep = step / 2;

        long startTime = DateTime.Now.Ticks;

        for (int sp = activeQueryIndex; sp < splatCount; sp++)
        {
            query.progress = (float)sp / splatCount;
            activeQueryIndex = sp;
            if (DateTime.Now.Ticks - startTime > maxExecutionTime) return;

            for (int y = 0; y < alphamapResolution; y++)
            {
                int sy = (y == 0) ? 0 : y * step - halfStep;
                int ly = (y > 0 && y < alphamapResolution - 1) ? step : halfStep;

                for (int x = 0; x < alphamapResolution; x++)
                {
                    int sx = (x == 0) ? 0 : x * step - halfStep;
                    int lx = (x > 0 && x < alphamapResolution - 1) ? step : halfStep;

                    float v = 0;

                    for (int py = 0; py < ly; py++)
                    {
                        for (int px = 0; px < lx; px++) v += alphamaps[sx + px, sy + py, sp];
                    }

                    newAlphamaps[x, y, sp] = v / (lx * ly);
                }
            }
        }

        query.terrainData.alphamapResolution = alphamapResolution;
        query.terrainData.SetAlphamaps(0, 0, newAlphamaps);

        NextQuery();
    }

    private void DownScaleHeightmap(TerrainQualityManagerQuery query)
    {
        TerrainData terrainData = query.terrainData;

        if (query.data == null)
        {
            object[] data = new object[3];
            data[0] = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
            data[1] = new float[heightmapResolution, heightmapResolution];
            data[2] = (terrainData.heightmapResolution - 1) / (heightmapResolution - 1);
            query.data = data;
        }

        object[] queryData = query.data as object[];

        float[,] heights = queryData[0] as float[,];
        float[,] newHeights = queryData[1] as float[,];
        int step = (int)queryData[2];
        int halfStep = step / 2;

        long startTime = DateTime.Now.Ticks;

        for (int y = activeQueryIndex; y < heightmapResolution; y++)
        {
            query.progress = (float)y / heightmapResolution;
            activeQueryIndex = y;
            if (DateTime.Now.Ticks - startTime > maxExecutionTime) return;

            int sy = (y == 0) ? 0 : y * step - halfStep;
            int ly = (y > 0 && y < heightmapResolution - 1) ? step : halfStep;

            for (int x = 0; x < heightmapResolution; x++)
            {
                int sx = (x == 0) ? 0 : x * step - halfStep;
                int lx = (x > 0 && x < heightmapResolution - 1) ? step : halfStep;

                float v = 0;

                for (int py = 0; py < ly; py++)
                {
                    for (int px = 0; px < lx; px++)
                        v += heights[sy + py, sx + px];
                }

                newHeights[y, x] = v / (lx * ly);
            }
        }

        ApplyNewHeightmap(query.terrainData, newHeights);

        NextQuery();
    }

    private void DrawNeighborWarning()
    {
        EditorGUILayout.HelpBox("If you change Heightmap, can get the gap between the terrains.\nIf you are using Terrains with Neighbors, use mode All Terrains In Scene.", MessageType.Warning);
    }

    private void DrawProps()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(TerrainQualityManagerGUIUtils.labelWidth + 20));
        EditorGUILayout.LabelField((terrain != null) ? "Current value:" : "Default value", GUILayout.MinWidth(TerrainQualityManagerGUIUtils.minColWidth));
        EditorGUILayout.LabelField("New value:", GUILayout.MinWidth(TerrainQualityManagerGUIUtils.minColWidth));
        GUILayout.Box("", GUIStyle.none, GUILayout.Width(25));
        EditorGUILayout.EndHorizontal();

        const string detailTooltip = "The resolution of the map that controls grass and detail meshes. For performance reasons (to save on draw calls) the lower you set this number the better.";
        const string resolutionPerPatchTooltip = "Specifies the size in pixels of each individually rendered detail patch. A larger number reduces draw calls, but might increase triangle count since detail patches are culled on a per batch basis. A recommended value is 16. If you use a very large detail object distance and your grass is very sparse, it makes sense to increase the value.";
        const string basemapTooltip = "Resolution of the composite texture used on the terrain when viewed from a distance greater than the Basemap Distance.";
        const string heightmapTooltip = "Pixel resolution of the terrains heightmap.";
        const string alphamapTooltip = "Resolution of the splatmap that controls the blending of the different terrain textures.";
        const string helpHref = "http://docs.unity3d.com/Manual/terrain-OtherSettings.html";

        TerrainQualityManagerGUIUtils.DrawField("Heightmap Resolution:", defHeightmapResolution, ref heightmapResolution, heightmapTooltip, helpHref, availableHeightsStr, availableHeights);
        TerrainQualityManagerGUIUtils.DrawField("Detail Resolution:", defDetailResolution, ref detailResolution, detailTooltip, helpHref);
        TerrainQualityManagerGUIUtils.DrawField("Control Texture Resolution:", defAlphamapResolution, ref alphamapResolution, alphamapTooltip, helpHref, availableResolutionsStr, availableResolutions);
        TerrainQualityManagerGUIUtils.DrawField("Base Texture Resolution:", defBaseMapResolution, ref baseMapResolution, basemapTooltip, helpHref, availableResolutionsStr, availableResolutions);
        TerrainQualityManagerGUIUtils.DrawField("Resolution Per Patch:", defResolutionPerPatch, ref resolutionPerPatch, resolutionPerPatchTooltip, helpHref);

        detailResolution = detailResolution / resolutionPerPatch * resolutionPerPatch;
    }

    private void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        GUILayout.Label("", EditorStyles.toolbar, GUILayout.ExpandWidth(true));

        if (GUILayout.Button("Help", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Documentation"), false, OnViewDocs);
            menu.AddItem(new GUIContent("Product Page"), false, OnProductPage);
            menu.AddItem(new GUIContent("Forum"), false, OnForum);
            menu.AddItem(new GUIContent("Support"), false, OnSupport);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Check Updates"), false, OnCheckUpdates);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("About"), false, TerrainQualityManagerAboutWindow.OpenWindow);
            menu.ShowAsContext();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void Finish()
    {
        state = TerrainQualityManagerState.idle;

        if (queries != null)
        {
            foreach (TerrainQualityManagerQuery query in queries) query.Dispose();
            queries = null;
        }

        queryIndex = 0;

        if (mode == TerrainQualityManagerModes.singleTerrain) UpdateDefaultProps();
        else
        {
            defAlphamapResolution = alphamapResolution;
            defBaseMapResolution = baseMapResolution;
            defDetailResolution = detailResolution;
            defHeightmapResolution = heightmapResolution;
            defResolutionPerPatch = resolutionPerPatch;
        }

        GC.Collect();
        Repaint();
    }

    private void NextQuery()
    {
        queryIndex++;
        activeQueryIndex = 0;
        if (queryIndex >= queries.Count) state = TerrainQualityManagerState.finish;
        Repaint();
    }

    private void OnCheckUpdates()
    {
        TerrainQualityManagerUpdater.OpenWindow();
    }

    private void OnForum()
    {
        Process.Start("http://forum.infinity-code.com");
    }

    private void OnGUI()
    {
        if (state == TerrainQualityManagerState.idle) OnIdleGUI();
        else OnWorkGUI();
    }

    private void OnIdleGUI()
    {
        if (availableResolutionsStr == null || availableHeightsStr == null)
        {
            availableHeightsStr = availableHeights.Select(h => h.ToString()).ToArray();
            availableResolutionsStr = availableResolutions.Select(r => r.ToString()).ToArray();
        }

        DrawToolbar();

        EditorGUI.BeginChangeCheck();
        mode = (TerrainQualityManagerModes)EditorGUILayout.EnumPopup("Mode: ", mode);
        if (EditorGUI.EndChangeCheck())
        {
            if (mode != TerrainQualityManagerModes.singleTerrain)
            {
                terrain = null;
                UpdateDefaultProps();
            }
        }

        if (GUILayout.Button("Refresh Default Values"))
        {
            UpdateDefaultProps();
            GUI.FocusControl(null);
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        if (mode == TerrainQualityManagerModes.singleTerrain) OnGUISingle();
        else OnGUIMulti();

        if (mode != TerrainQualityManagerModes.allTerrainsInScene) DrawNeighborWarning();

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Apply"))
        {
            state = TerrainQualityManagerState.prepare;
        }
    }

    private void OnGUIMulti()
    {
        DrawProps();
    }

    private void OnGUISingle()
    {
        EditorGUI.BeginChangeCheck();
        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain: ", terrain, typeof (Terrain), true);
        if (EditorGUI.EndChangeCheck())
        {
            UpdateDefaultProps();
        }

        if (terrain == null)
        {
            GUILayout.Label("Please select the terrain.");
            return;
        }

        DrawProps();
    }

    private void OnProductPage()
    {
        Process.Start("http://infinity-code.com/products/terrain-quality-manager");
    }

    private void OnSupport()
    {
        Process.Start("mailto:support@infinity-code.com?subject=Terrain Quality Manager");
    }

    private void OnWorkGUI()
    {
        string label = null;
        if (state == TerrainQualityManagerState.prepare) label = "Preparing.";
        else if (state == TerrainQualityManagerState.working)
        {
            if (activeQuery == null) label = "Error.";
            else if (activeQuery.terrainData != null)
                label = string.Format("{0}. {1}", activeQuery.terrainData.name, activeQuery.name);
            else label = activeQuery.name;
        }
            
        else if (state == TerrainQualityManagerState.finish) label = "Finishing.";

        GUILayout.Label(label);
        if (state == TerrainQualityManagerState.working && activeQuery != null)
        {
            float progress = (queryIndex + activeQuery.progress) / queries.Count;
            int iProgress = (int)(progress * 100);
            Rect r = EditorGUILayout.BeginVertical();
            EditorGUI.ProgressBar(r, progress, iProgress + "%");
            GUILayout.Space(16);
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Cancel"))
        {
            Finish();
        }
    }

    private void OnViewDocs()
    {
        Process.Start("http://infinity-code.com/docs/terrain-quality-manager");
    }

    [MenuItem("Window/Infinity Code/Terrain Quality Manager/Open Manager", false, -1)]
    private static void OpenWindow()
    {
        GetWindow<TerrainQualityManagerWindow>(false, "Terrain Quality Manager", true);
    }

    private void Prepare()
    {
        TerrainData[] terrainDatas = null;
        if (mode == TerrainQualityManagerModes.singleTerrain)
        {
            if (terrain == null)
            {
                EditorUtility.DisplayDialog("Error", "Terrain not selected.", "OK");
                state = TerrainQualityManagerState.idle;
                return;
            }
            terrainDatas = new[] { terrain.terrainData };
        }
        else if (mode == TerrainQualityManagerModes.allTerrainsInScene)
        {
            Terrain[] terrains = FindObjectsOfType(typeof(Terrain)) as Terrain[];
            if (terrains != null) terrainDatas = terrains.Select(t => t.terrainData).ToArray();
        }
        else if (mode == TerrainQualityManagerModes.allTerrainsInProject)
        {
            string[] terrainsGUID = AssetDatabase.FindAssets("t:terrain");
            terrainDatas = new TerrainData[terrainsGUID.Length];
            for (int i = 0; i < terrainsGUID.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(terrainsGUID[i]);
                terrainDatas[i] = AssetDatabase.LoadMainAssetAtPath(assetPath) as TerrainData;
            }
        }

        if (terrainDatas == null)
        {
            state = TerrainQualityManagerState.idle;
            return;
        }

        bool changeHeightmap = false;

        foreach (TerrainData t in terrainDatas)
        {
            if (t.heightmapResolution != heightmapResolution)
            {
                changeHeightmap = true;
                TerrainQualityManagerQuery query = AddQuery("Resize heightmap.", t);
                if (t.heightmapResolution > heightmapResolution) query.method = DownScaleHeightmap;
                else query.method = UpScaleHeightmap;
            }
            if (t.alphamapResolution != alphamapResolution)
            {
                TerrainQualityManagerQuery query = AddQuery("Resize alphamap.", t);
                if (t.alphamapResolution > alphamapResolution) query.method = DownScaleAlphamap;
                else query.method = UpScaleAlphamap;
            }
            if (t.detailResolution != detailResolution)
            {
                AddQuery("Resize detailmap.", t).method = ScaleDetailmap;
            }
            if (t.baseMapResolution != baseMapResolution) t.baseMapResolution = baseMapResolution;
        }

        if (queries == null || queries.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "Nothing to do.", "OK");
            Finish();
            return;
        }

        Undo.RegisterCompleteObjectUndo(terrainDatas, "Change Terrain Maps Resolution");

        if (changeHeightmap && mode == TerrainQualityManagerModes.allTerrainsInScene)
        {
            AddQuery("Update Neighbors").method = UpdateNeighbors;
        }

        activeQueryIndex = 0;
        queryIndex = 0;
        state = TerrainQualityManagerState.working;
    }

    private void ScaleDetailmap(TerrainQualityManagerQuery query)
    {
        TerrainData terrainData = query.terrainData;

        if (query.data == null) query.data = new List<int[,]>();

        List<int[,]> detailLayers = query.data as List<int[,]>;

        int countProts = terrainData.detailPrototypes.Length;
        int detailWidth = terrainData.detailWidth;
        int detailHeight = terrainData.detailHeight;

        long startTime = DateTime.Now.Ticks;

        for (int p = activeQueryIndex; p < countProts; p++)
        {
            query.progress = (float)p / countProts;
            activeQueryIndex = p;
            if (DateTime.Now.Ticks - startTime > maxExecutionTime) return;

            int[,] detailLayer = terrainData.GetDetailLayer(0, 0, detailWidth, detailHeight, p);
            int[,] newDetailLayer = new int[detailResolution, detailResolution];

            for (int x = 0; x < detailResolution; x++)
            {
                float rx = (float)x / detailResolution * terrainData.detailResolution;
                int x1 = (int)rx;
                int x2 = x1 + 1;
                if (x2 >= terrainData.detailResolution) x2 = terrainData.detailResolution - 1;

                for (int y = 0; y < detailResolution; y++)
                {
                    float ry = (float)y / detailResolution * terrainData.detailResolution;

                    int y1 = (int)ry;
                    int y2 = y1 + 1;
                    if (y2 >= terrainData.detailResolution) y2 = terrainData.detailResolution - 1;

                    float p1 = Mathf.RoundToInt(Mathf.Lerp(detailLayer[x1, y1], detailLayer[x2, y1], rx - x1));
                    float p2 = Mathf.RoundToInt(Mathf.Lerp(detailLayer[x1, y2], detailLayer[x2, y2], rx - x1));

                    newDetailLayer[x, y] = Mathf.RoundToInt(Mathf.Lerp(p1, p2, ry - y1));
                }
            }

            detailLayers.Add(newDetailLayer);
        }

        terrainData.SetDetailResolution(detailResolution, resolutionPerPatch);

        for (int i = 0; i < detailLayers.Count; i++) terrainData.SetDetailLayer(0, 0, i, detailLayers[i]);

        NextQuery();
    }

    private void UpdateDefaultProps()
    {
        bool initDefValues = false;

        if (mode == TerrainQualityManagerModes.singleTerrain)
        {
            if (terrain != null)
            {
                defHeightmapResolution = heightmapResolution = terrain.terrainData.heightmapResolution;
                defDetailResolution = detailResolution = terrain.terrainData.detailResolution;
                defAlphamapResolution = alphamapResolution = terrain.terrainData.alphamapResolution;
                defBaseMapResolution = baseMapResolution = terrain.terrainData.baseMapResolution;
            }
            else initDefValues = true;
        }
        else
        {
            TerrainData terrainData = null;
            if (mode == TerrainQualityManagerModes.allTerrainsInScene)
            {
                Terrain t = FindObjectOfType<Terrain>();
                if (t != null) terrainData = t.terrainData;
            }
            else if (mode == TerrainQualityManagerModes.allTerrainsInProject)
            {
                string[] GUIDs = AssetDatabase.FindAssets("t:terrain");
                if (GUIDs != null && GUIDs.Length > 0)
                {
                    string firstGUID = GUIDs[0];
                    string path = AssetDatabase.GUIDToAssetPath(firstGUID);
                    terrainData = AssetDatabase.LoadMainAssetAtPath(path) as TerrainData;
                } 
            }

            if (terrainData != null)
            {
                defHeightmapResolution = heightmapResolution = terrainData.heightmapResolution;
                defDetailResolution = detailResolution = terrainData.detailResolution;
                defAlphamapResolution = alphamapResolution = terrainData.alphamapResolution;
                defBaseMapResolution = baseMapResolution = terrainData.baseMapResolution;
            }
            else initDefValues = true;
        }

        if (initDefValues)
        {
            defHeightmapResolution = heightmapResolution = 513;
            defDetailResolution = detailResolution = 1024;
            defAlphamapResolution = alphamapResolution = 512;
            defBaseMapResolution = baseMapResolution = 1024;
        }

        defResolutionPerPatch = resolutionPerPatch = 8;
    }

    private void UpdateNeighbors(TerrainQualityManagerQuery query)
    {
        Terrain[] terrains = FindObjectsOfType<Terrain>();
        if (terrains == null || terrains.Length == 0) return;

        foreach (Terrain t in terrains)
        {
            TerrainData terrainData = t.terrainData;
            Vector3 rightNeighborPosition = t.transform.position + new Vector3(terrainData.size.x, 0, 0);
            Vector3 bottomNeighborPosition = t.transform.position + new Vector3(0, 0, terrainData.size.z);

            Terrain rightNeighbor = terrains.FirstOrDefault(tr => (tr.transform.position  - rightNeighborPosition).magnitude < 10);
            Terrain bottomNeighbor = terrains.FirstOrDefault(tr => (tr.transform.position - bottomNeighborPosition).magnitude < 10);

            if (rightNeighbor != null)
            {
                float[,] th = terrainData.GetHeights(heightmapResolution - 1, 0, 1, heightmapResolution);
                float[,] rh = rightNeighbor.terrainData.GetHeights(0, 0, 1, heightmapResolution);

                for (int y = 0; y < heightmapResolution; y++) th[y, 0] = (th[y, 0] + rh[y, 0]) / 2;

                terrainData.SetHeights(heightmapResolution - 1, 0, th);
                rightNeighbor.terrainData.SetHeights(0, 0, th);
            }

            if (bottomNeighbor != null)
            {
                float[,] th = terrainData.GetHeights(0, heightmapResolution - 1, heightmapResolution, 1);
                float[,] rh = bottomNeighbor.terrainData.GetHeights(0, 0, heightmapResolution, 1);

                for (int x = 0; x < heightmapResolution; x++) th[0, x] = (th[0, x] + rh[0, x]) / 2;

                terrainData.SetHeights(0, heightmapResolution - 1, th);
                bottomNeighbor.terrainData.SetHeights(0, 0, th);
            }
        }

        NextQuery();
    }

    private void UpScaleAlphamap(TerrainQualityManagerQuery query)
    {
        TerrainData terrainData = query.terrainData;
        int splatCount = terrainData.splatPrototypes.Length;

        if (query.data == null)
        {
            object[] data = new object[2];
            data[0] = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
            data[1] = new float[alphamapResolution, alphamapResolution, splatCount];
            query.data = data;
        }

        object[] queryData = query.data as object[];
        float[, ,] alphamaps = queryData[0] as float[, ,];
        float[, ,] newAlphamaps = queryData[1] as float[, ,];

        long startTime = DateTime.Now.Ticks;

        for (int sp = activeQueryIndex; sp < splatCount; sp++)
        {
            query.progress = (float)sp / splatCount;
            activeQueryIndex = sp;
            if (DateTime.Now.Ticks - startTime > maxExecutionTime) return;

            for (int y = 0; y < alphamapResolution; y++)
            {
                float ry = (float)y / alphamapResolution * terrainData.alphamapResolution;

                int y1 = (int)ry;
                int y2 = y1 + 1;
                if (y2 >= terrainData.alphamapResolution) y2 = terrainData.alphamapResolution - 1;

                for (int x = 0; x < alphamapResolution; x++)
                {
                    float rx = (float)x / alphamapResolution * terrainData.alphamapResolution;

                    int x1 = (int)rx;
                    int x2 = x1 + 1;
                    if (x2 >= terrainData.alphamapResolution) x2 = terrainData.alphamapResolution - 1;

                    float p1 = Mathf.Lerp(alphamaps[x1, y1, sp], alphamaps[x2, y1, sp], rx - x1);
                    float p2 = Mathf.Lerp(alphamaps[x1, y2, sp], alphamaps[x2, y2, sp], rx - x1);

                    newAlphamaps[x, y, sp] = Mathf.Lerp(p1, p2, ry - y1);
                }
            }
        }

        query.terrainData.alphamapResolution = alphamapResolution;
        query.terrainData.SetAlphamaps(0, 0, newAlphamaps);

        NextQuery();
    }

    private void UpScaleHeightmap(TerrainQualityManagerQuery query)
    {
        if (query.data == null)
        {
            object[] data = new object[1];
            data[0] = new float[heightmapResolution, heightmapResolution];
            query.data = data;
        }

        object[] queryData = query.data as object[];
        float[,] newHeights = queryData[0] as float[,];
        TerrainData terrainData = query.terrainData;

        long startTime = DateTime.Now.Ticks;

        for (int x = activeQueryIndex; x < heightmapResolution; x++)
        {
            query.progress = (float)x / heightmapResolution;
            activeQueryIndex = x;
            if (DateTime.Now.Ticks - startTime > maxExecutionTime) return;

            float rx = (float)x / (heightmapResolution - 1);
            for (int y = 0; y < heightmapResolution; y++)
            {
                float ry = (float)y / (heightmapResolution - 1);
                newHeights[y, x] = terrainData.GetInterpolatedHeight(rx, ry) / terrainData.size.y;
            }
        }

        ApplyNewHeightmap(query.terrainData, newHeights);

        NextQuery();
    }

    private void Update()
    {
        if (state == TerrainQualityManagerState.idle) return;

        if (state == TerrainQualityManagerState.prepare)
        {
            Prepare();
        }
        else if (state == TerrainQualityManagerState.working)
        {
            if (activeQuery != null) activeQuery.Invoke();
            Repaint();
        }
        else if (state == TerrainQualityManagerState.finish)
        {
            Finish();
        }
    }
}

internal class TerrainQualityManagerQuery
{
    public Action<TerrainQualityManagerQuery> method;
    public TerrainData terrainData;
    public string name;
    public float progress;
    public object data;

    public TerrainQualityManagerQuery(TerrainData terrainData, string name, Action<TerrainQualityManagerQuery> method = null)
    {
        this.method = method;
        this.terrainData = terrainData;
        this.name = name;
        progress = 0;
        data = null;
    }

    public void Dispose()
    {
        method = null;
        terrainData = null;
        name = null;
        progress = 0;
        data = null;
    }

    public void Invoke()
    {
        method(this);
    }
}