using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : MonoBehaviour {
    public bool isRotating = false;
    public Vector3 rotationAngle;
    public float rotationSpeed;

    void Start()
    {

    }

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
        }
    }
    
}

