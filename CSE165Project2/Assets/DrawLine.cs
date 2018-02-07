using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

    public GameObject endPoint;
    private LineRenderer line;

	// Use this for initialization
	void Start () {


        line = GetComponent<LineRenderer>();
        line.enabled = false;

    }
	
	// Update is called once per frame
	void Update () {


        if (gameObject.activeSelf)
        {

            line.enabled = true;

            line.SetPosition(0, transform.position);
            line.SetPosition(1, endPoint.transform.position);


        }
        else
        {

            line.enabled = false;

        }
		
	}
}
