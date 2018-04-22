using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {

    private const int MAX_GREEN_LIFE = 60;
    private const int MAX_YELLOW_LIFE = 20;

    private Color32 YELLOW = new Color32(247, 221, 85, 255);
    private Color32 RED = new Color32(255, 0, 85, 255);

    public Text valueHealth;
    public Text bulletsLabel;
    public Text totalBulletsLabel;

    private int bullets;
    private int totalBullets;

    [SerializeField]
    Slider healthBar;

    public GameObject fillHealth;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeHealthBar(int value)
    {
        healthBar.value = value;
        valueHealth.text = value.ToString();

        if (value < MAX_YELLOW_LIFE)
        {
            fillHealth.GetComponent<Image>().color = RED;
        }
        else if (value < MAX_GREEN_LIFE)
        {
            fillHealth.GetComponent<Image>().color = YELLOW;
        }
    }

    public void setAmmo(int value)
    {
        bullets = value;
        bulletsLabel.text = bullets.ToString();
    }

}
