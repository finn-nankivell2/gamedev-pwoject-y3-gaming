using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis { x, y, z };

public class BasicRotation : MonoBehaviour
{

    public Axis rotationAxis;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // 
    }

    // Update is called once per frame
    void Update()
    {
        switch (rotationAxis) 
        {
            case Axis.x:
                transform.Rotate(rotationSpeed*Time.deltaTime, 0, 0, Space.World);
                break;
            case Axis.y:
                transform.Rotate(0, rotationSpeed*Time.deltaTime, 0, Space.World);
                break;
            case Axis.z:
                transform.Rotate(0, 0, rotationSpeed*Time.deltaTime, Space.World);
                break;
        }
    }
}
