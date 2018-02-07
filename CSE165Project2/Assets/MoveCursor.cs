using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursor : MonoBehaviour
{

    public GameObject cam;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = new Vector3(0,0.15f,0);
        //transform.position = cam.transform.position + cam.transform.forward * 8;
        transform.position = cam.transform.position + offset;

        transform.rotation = cam.transform.rotation;

    }
}