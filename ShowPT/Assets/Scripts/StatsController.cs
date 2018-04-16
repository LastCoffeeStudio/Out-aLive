using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatsController : MonoBehaviour {

    public Text fps;
    public Text triangles;
    public Text vertices;
    public Text drawCalls;
    public Text render;
    public Text vbo;

    private int frames;
    private float dt;
    private float framesPerSecond;
    private float rate;

    // Use this for initialization
    void Start () {
        frames = 0;
        dt = 0f;
        framesPerSecond = 0f;
        rate = 4f;
	}
	
	// Update is called once per frame
	void Update () {

        ++frames;
        dt += Time.deltaTime;
        if (dt > 1f/rate)
        {
            framesPerSecond = frames / dt;
            frames = 0;
            dt -= 1f / rate;
        }

        fps.text = "FPS: " + framesPerSecond.ToString();
        triangles.text = "Triangles: " + UnityEditor.UnityStats.triangles.ToString();
        vertices.text = "Vertices: " + UnityEditor.UnityStats.vertices.ToString();
        drawCalls.text = "Draw Calls: " + UnityEditor.UnityStats.drawCalls.ToString();
        render.text = "Render Time: " + UnityEditor.UnityStats.renderTime.ToString();
        vbo.text = "VBOs: " + UnityEditor.UnityStats.vboTotal.ToString();

    }
}
