using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHand : MonoBehaviour {

    public GameObject rightHand;
    public GameObject leftHand;

    public GameObject lastHit = null;

    private LineRenderer line;


    public bool copyPasteOn = false;
    public bool newObjectOn = false;
    public bool tapeOn = false;
    public bool groupOn = false;


    public Material buttonOn;
    public Material buttonOff;


    private GameObject curr;

    public GameObject TapeBegin;
    public GameObject TapeEnd;




    public Vector3 prevRHpos;
    public Vector3 prevHitPos;
    public Vector3 initialPosition;
    public Quaternion initialRotation;

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



                        


                        float offset = (prevHitPos - prevRHpos).magnitude;

                        lastHit.transform.position = rightHand.transform.position + rightHand.transform.forward * offset;

                        //lastHit.transform.rotation = rightHand.transform.rotation;


                        //movement forward/backward and rotation
                        Vector2 rightStick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

                        float vertical = rightStick.y;
                        float horizontal = rightStick.x;

                        if(Mathf.Abs(vertical) > Mathf.Abs(horizontal))
                        {
                            lastHit.transform.position = lastHit.transform.position + rightHand.transform.forward * (rightStick.y / 100);
                        }
                        else
                        {
                            lastHit.transform.Rotate(0, (rightStick.x), 0);
                        }




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
              
                //return to initial position
                if(collision)
                {
                    if (copyPasteOn)
                    {

                        Destroy(lastHit);

                    }
                    else
                    {

                        lastHit.transform.position = initialPosition;
                        lastHit.transform.rotation = initialRotation;

                    }
                }

                if (!(curr.CompareTag("TapePoint")))
                {

                    //turn physics on
                    lastHit.GetComponent<Rigidbody>().useGravity = true;
                    lastHit.GetComponent<Rigidbody>().isKinematic = false;

                }
                
                lastHit.GetComponent<MeshRenderer>().material = lastHit.GetComponent<Collision>().defaultMaterial; //reset material

                lastHit.GetComponent<Collider>().isTrigger = false;

                //reset rotation
                //lastHit.transform.rotation.

                //lastHit.GetComponent<Rigidbody>().isKinematic = false;

                lastHit = null;

            }



        }



        

		
	}

    //when trigger is first pressed
    private void RaycastRightHand()
    {

        RaycastHit hit;
        

        //hit something
        if (Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hit))
        {

            curr = hit.collider.gameObject;

            if (curr.CompareTag("Floor"))
            {

                lastHit = null;

                



            }
            else if (curr.CompareTag("CopyPaste"))
            {
                lastHit = null;

                if (copyPasteOn)
                {
                    copyPasteOn = false;
                    curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOff;

                }
                else
                {

                    copyPasteOn = true;

                    curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOn;
                }

            }
            else if (curr.CompareTag("NewObject"))
            {

                lastHit = null;

                if (newObjectOn)
                {
                    newObjectOn = false;
                    curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOff;

                }
                else
                {

                    newObjectOn = true;

                    curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOn;
                }

            }
            else if (curr.CompareTag("Tape"))
            {

                lastHit = null;

                if (tapeOn)
                {
                    tapeOn = false;
                    curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOff;
                    //curr.GetComponentInChildren<MeshRenderer>().material = buttonOff;

                    TapeBegin.SetActive(false);
                    TapeEnd.SetActive(false);
                    
                }
                else
                {

                    tapeOn = true;

                    curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOn;

                    //spawn spheres
                    TapeBegin.SetActive(true);
                    TapeEnd.SetActive(true);

                    Vector3 offset = new Vector3( 0, 0.1f, 0 );

                    TapeBegin.transform.position = rightHand.transform.position + offset;

                    TapeEnd.transform.position = rightHand.transform.position - offset;


                }

            }
            else if (curr.CompareTag("Group"))
            {

                lastHit = null;

                if (groupOn)
                {
                    groupOn = false;
                    curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOff;

                }
                else
                {

                    groupOn = true;

                    curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOn;
                }

            }
            else if (curr.CompareTag("UnTouchable"))
            {
                lastHit = null;

            }
            //hit object elligible for transform
            else
            {
                //duplicate object
                if (copyPasteOn)
                {

                    if (!(curr.CompareTag("TapePoint")))
                    {

                        GameObject newCopy = Instantiate(curr, curr.transform.parent, true);
                        curr = newCopy;

                        newCopy.GetComponent<Collider>().isTrigger = true;

                    }
                   

                }
                else
                {


                    if (!(curr.CompareTag("TapePoint")))
                    {

                        curr.GetComponent<Collider>().isTrigger = true;

                    }
                }

                curr.GetComponent<MeshRenderer>().material = curr.GetComponent<Collision>().activeMaterial; //reset material


                if (!(curr.CompareTag("TapePoint")))
                {

                    //turn physics off
                    curr.GetComponent<Rigidbody>().useGravity = false;
                    curr.GetComponent<Rigidbody>().isKinematic = true;

                }

                
               //curr.GetComponent<Rigidbody>().isKinematic = true;

                //save previous locations
                prevRHpos = rightHand.transform.position;

                prevHitPos = curr.transform.position;


                lastHit = curr;
    
                
                initialPosition = curr.transform.position;
                initialRotation = curr.transform.rotation;


            }


        }
        //didnt hit anything
        else
        {
            lastHit = null;

        }



    }



    


}
