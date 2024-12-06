using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BatteryDisplay : MonoBehaviour
{
    private List<GameObject> batterySprites = new List<GameObject>();
    public GameObject batteryPrefab;
    public float batteryOffset = 100;
    private int uiMaxAirJumps = 0;
    private int uiCurrentAirJumps = 0;
    private PlayerMovementFreecam playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameManager.Instance.playerScript;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.maxAirJumps > uiMaxAirJumps) {
            int diff = playerScript.maxAirJumps - uiMaxAirJumps;
            for (int i=0; i < diff; i++) {
                AddBatterySprite();
            }
            uiMaxAirJumps = playerScript.maxAirJumps;
        }

        if (playerScript.airJumps < uiCurrentAirJumps) {
            int diff = playerScript.maxAirJumps - playerScript.airJumps;
            int idx = batterySprites.Count - diff;

            Image img = batterySprites[idx].GetComponent<Image>();
            img.color = Color.black;
            uiCurrentAirJumps = playerScript.airJumps;
        }

        if (playerScript.airJumps > uiCurrentAirJumps) {
            foreach (GameObject battery in batterySprites) {
                Image img2 = battery.GetComponent<Image>();
                img2.color = Color.white;

            }
            uiCurrentAirJumps = playerScript.airJumps;
        }

        for(int i=0; i<uiMaxAirJumps; i++)
        {
            if(i < playerScript.airJumps)
            {

            }
        }

    }

    void AddBatterySprite()
    {
        var batteryImage = Instantiate(batteryPrefab);
        batteryImage.transform.SetParent(transform);

        var rect = batteryImage.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0f, 0f); 
        batteryImage.transform.position += new Vector3(batterySprites.Count*batteryOffset, 0, 0);
        batterySprites.Add(batteryImage);
    }
}
