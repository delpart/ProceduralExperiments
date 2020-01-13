using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Range(0,100)]
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, speed*Time.deltaTime, Space.Self);
    }
}
