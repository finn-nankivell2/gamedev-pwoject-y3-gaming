using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BatteryDisplay : MonoBehaviour
{
    private struct Battery
    {
        public GameObject obj;
        public Image img;

        public Battery(GameObject batObj, Image image)
        {
            obj = batObj;
            img = image;
        }
    }

    private List<Battery> batteries = new List<Battery>();
    public GameObject batteryPrefab;
    public float batteryOffset = 100;
    private int uiMaxAirJumps = 0;
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

        for(int i=0; i<uiMaxAirJumps; i++)
        {
            if(i < playerScript.airJumps) {
                batteries[i].img.color = Color.white;
            } else {
                batteries[i].img.color = Color.black;
            }
        }

    }

    void AddBatterySprite()
    {
        var batteryObject = Instantiate(batteryPrefab);
        batteryObject.transform.SetParent(transform);

        var rect = batteryObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0f, 0f); 
        batteryObject.transform.position += new Vector3(batteries.Count*batteryOffset, 0, 0);
        Image batteryImage = batteryObject.GetComponent<Image>();
        Battery battery = new Battery(batteryObject, batteryImage);
        batteries.Add(battery);
    }
}
