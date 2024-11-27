using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicOscillation : MonoBehaviour
{
    private float time = 0;
    public float oscillationSpeed = 1;
    public float oscillationDistance = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        transform.position = new Vector3(
            transform.position.x, 
            transform.position.y + (Mathf.Sin(time * oscillationSpeed) * oscillationDistance * Time.deltaTime), 
            transform.position.z
            );
    }
}
