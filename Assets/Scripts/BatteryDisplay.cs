using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BatteryDisplay : MonoBehaviour
{
    private List<GameObject> batterySprites = new List<GameObject>();
    public GameObject batteryPrefab;
    public float batteryOffset = 100;
    private int uiAirJumps = 0;
    private PlayerMovementFreecam playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameManager.Instance.playerScript;
        uiAirJumps = playerScript.maxAirJumps;
        for (int i=0; i < uiAirJumps; i++) {
            AddBatterySprite();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.maxAirJumps > uiAirJumps) {
            int diff = playerScript.maxAirJumps - uiAirJumps;
            for (int i=0; i < diff; i++) {
                AddBatterySprite();
            }
        }
        uiAirJumps = playerScript.maxAirJumps;
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
