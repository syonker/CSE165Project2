using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHand : MonoBehaviour {

    public GameObject rightHand;
    public GameObject leftHand;

    private GameObject lastHit = null;

    private LineRenderer line;

    private Collider currCollider;


    public Vector3 prevRHpos;
    public Vector3 prevHitPos;
    public Vector3 lastValidPosition;

    public bool collision = false;

    public bool SecIndOn = false;


    // Use this for initialization
    void Start () {

        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        

        //if right hand trigger is pressed
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {

            //if there is no collision occuring
            //if (!collision)
            //{



                line.enabled = true;
                line.SetPosition(0, rightHand.transform.position);
                line.SetPosition(1, rightHand.transform.position + rightHand.transform.forward * 100);

                //held down
                if (SecIndOn)
                {

                    if (lastHit != null)
                    {



                        lastValidPosition = prevHitPos;


                        float offset = (prevHitPos - prevRHpos).magnitude;

                        lastHit.transform.position = rightHand.transform.position + rightHand.transform.forward * offset;

                        lastHit.transform.rotation = rightHand.transform.rotation;


                        //movement forward/backward
                        Vector2 rightStick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

                        lastHit.transform.position = lastHit.transform.position + rightHand.transform.forward * (rightStick.y / 100);



                        prevHitPos = lastHit.transform.position;
                        prevRHpos = rightHand.transform.position;





                        line.SetPosition(1, lastHit.transform.position);





                    }




                }
                //first click
                else
                {
                    SecIndOn = true;

                    RaycastRightHand();

                }




            //}

        }
        //if right hand trigger is not pressed
        else
        {

            SecIndOn = false;
            line.enabled = false;

            if (lastHit != null)
            {
                    //turn physics on
                    lastHit.GetComponent<Rigidbody>().useGravity = true;
                    lastHit.GetComponent<Rigidbody>().isKinematic = false;
                    lastHit = null;
                    currCollider.isTrigger = false;

            }



        }



        

		
	}

    //when trigger is first pressed
    private void RaycastRightHand()
    {

        RaycastHit hit;
        GameObject curr;

        //hit something
        if (Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hit))
        {

            curr = hit.collider.gameObject;

            if (curr.CompareTag("Floor"))
            {

                lastHit = null;

            }
            //hit object elligible for transform
            else
            {

                //turn physics off
                curr.GetComponent<Rigidbody>().useGravity = false;
                curr.GetComponent<Rigidbody>().isKinematic = true;

                //save previous locations
                prevRHpos = rightHand.transform.position;

                prevHitPos = curr.transform.position;


                lastHit = curr;
                currCollider = lastHit.GetComponent<Collider>();
                //currCollider.isTrigger = true;


            }


        }
        //didnt hit anything
        else
        {
            lastHit = null;

        }



    }



    


}
