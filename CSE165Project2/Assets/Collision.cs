﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    public GameObject empty;
    public Material activeMaterial; //green
    public Material defaultMaterial;
    public Material collisionMaterial; //red

    public LineRenderer line;



    void OnTriggerEnter(Collider trigger)
    {

        if (empty.GetComponent<RaycastHand>().falling)
        {


            //turn trigger off and falling off

            this.GetComponent<Collider>().isTrigger = false;
            empty.GetComponent<RaycastHand>().falling = false;


            //if we hit floor do nothing
            if (trigger.gameObject.CompareTag("Floor"))
            {


            }
            //if we landed on another object
            //reset position to last valid position
            else
            {
                this.transform.position = empty.GetComponent<RaycastHand>().initialPosition;

            }

        }




    }




        void OnTriggerStay(Collider trigger)
    {

        Debug.Log("OnTriggerStay OUTSIDE Group" + this.name);
        if (!empty.GetComponent<RaycastHand>().groupSelected && (empty.GetComponent<RaycastHand>().grabbed == this.gameObject))
        {

            Debug.Log("Grabbable in collision");
            empty.GetComponent<RaycastHand>().collisionGrab = true;

        }
         else if (!empty.GetComponent<RaycastHand>().groupSelected && (empty.GetComponent<RaycastHand>().lastHit == this.gameObject))
        {
            Debug.Log("OnTriggerStay No Group" + this.name);

            //other.gameObject.transform.position = empty.GetComponent<RaycastHand>().lastValidPosition;

            empty.GetComponent<RaycastHand>().collision = true;

            //change to green
            //this.GetComponent<MeshRenderer>().material = activeMaterial;
            //GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
            line.material.color = Color.red;


        }
        else if (empty.GetComponent<RaycastHand>().groupSelected)
        {

            Debug.Log("OnTriggerStay Group" + this.name);

            GameObject parent = empty.GetComponent<RaycastHand>().lastHit;

            for (int i = 0; i < parent.transform.childCount; i++)
            {

                if (parent.transform.GetChild(i).gameObject == this.gameObject)
                {
                    empty.GetComponent<RaycastHand>().collision = true;
                    line.material.color = Color.red;
                    return;

                }

            }


        }
    }

    //not colliding 
    private void OnTriggerExit(Collider other)
    {

        Debug.Log("OnTriggerEXIT OUTSIDE Group" + this.name);


        if (!empty.GetComponent<RaycastHand>().groupSelected && (empty.GetComponent<RaycastHand>().grabbed == this.gameObject))
        {

            Debug.Log("Grabbable exit collision");
            empty.GetComponent<RaycastHand>().collisionGrab = false;

        }
        else if (!empty.GetComponent<RaycastHand>().groupSelected && (empty.GetComponent<RaycastHand>().lastHit == this.gameObject))
        {
            Debug.Log("OnTriggerExit No Group" + this.name);

            //other.gameObject.transform.position = empty.GetComponent<RaycastHand>().lastValidPosition;

            empty.GetComponent<RaycastHand>().collision = false;

            //change to green
            //this.GetComponent<MeshRenderer>().material = activeMaterial;
            //GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
            line.material.color = Color.green;


        }
        else if (empty.GetComponent<RaycastHand>().groupSelected)
        {
            Debug.Log("OnTriggerExit Group" + this.name);

            GameObject parent = empty.GetComponent<RaycastHand>().lastHit;

            for (int i = 0; i < parent.transform.childCount; i++)
            {

                if (parent.transform.GetChild(i).gameObject == this.gameObject)
                {
                    empty.GetComponent<RaycastHand>().collision = false;
                    line.material.color = Color.green;
                    return;

                }

            }


        }


    }
}
