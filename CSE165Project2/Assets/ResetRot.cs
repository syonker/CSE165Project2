using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRot : MonoBehaviour {

    private Vector3 prevPos;

    private float initialRotX;
    private float initialRotZ;

    // Use this for initialization
    void Start () {


        prevPos = transform.position;

        initialRotX = transform.rotation.x;
        initialRotZ = transform.rotation.z;

    }
	
	// Update is called once per frame
	void Update () {


        if (prevPos == transform.position)
        {
            Quaternion temp = new Quaternion(initialRotX,transform.rotation.y, initialRotZ, 1.0f);
            //reset x and z orientation
            //transform.rotation.x = initialRotX;
            //transform.rotation.z = initialRotZ;
            transform.rotation = temp;



        } else
        {

            prevPos = transform.position;


        }
		
	}
}
