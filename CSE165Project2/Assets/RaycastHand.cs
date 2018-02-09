using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHand : MonoBehaviour
{

    public GameObject rightHand;
    public GameObject leftHand;

    private bool aPressed = false;

    private int indexNew = 0;

    public GameObject ground;
    public GameObject cam;

    public GameObject lastHit = null;


    public GameObject NewArray;

    private LineRenderer line;


    public bool copyPasteOn = false;
    public bool newObjectOn = false;
    public bool tapeOn = false;
    public bool groupOn = false;

    private bool ReadyToTeleport = true;


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
    void Start()
    {

        line = GetComponent<LineRenderer>();
        line.material.color = Color.white;
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        //create object
        if (newObjectOn && (lastHit == null))
        {

            GameObject newObj = Instantiate(NewArray.transform.GetChild(indexNew).gameObject, transform.parent, true);

            lastHit = newObj;

            newObj.SetActive(true);

            lastHit.GetComponent<Collider>().isTrigger = true;


            lastHit.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
            line.material.color = Color.green;


            //turn physics off
            lastHit.GetComponent<Rigidbody>().useGravity = false;
            lastHit.GetComponent<Rigidbody>().isKinematic = true;


            //save previous locations
            prevRHpos = rightHand.transform.position;

            prevHitPos = lastHit.transform.position;


            initialPosition = lastHit.transform.position;
            initialRotation = lastHit.transform.rotation;

            return;


        }



        //rotate camera
        //Vector2 leftStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        //cam.transform.Rotate(0,(leftStick.x / 100),0);


        //if left hand trigger is pressed
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && !(OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)))
        {

            //show blue line
            line.enabled = true;
            line.material.color = Color.blue;
            line.SetPosition(0, rightHand.transform.position);
            line.SetPosition(1, rightHand.transform.position + rightHand.transform.forward * 100);


        }
        //if left hand trigger is pressed and right hand trigger is pressed
        else if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {

            if (ReadyToTeleport)
            {

                RaycastHit hit;

                Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hit);

                if (hit.collider.gameObject.CompareTag("Floor"))
                {

                    //teleport
                    Debug.Log("Teleport!");

                    float offset = cam.transform.position.y - ground.transform.position.y;
                    Vector3 newPos = new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);
                    cam.transform.position = newPos;

                    ReadyToTeleport = false;
                    line.material.color = Color.white;


                }
            }






        }
        //if just right hand trigger is pressed
        else if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {


            line.enabled = true;
            //line.material.color = Color.white;
            line.SetPosition(0, rightHand.transform.position);
            line.SetPosition(1, rightHand.transform.position + rightHand.transform.forward * 100);

            //held down
            if (SecIndOn)
            {

                if (lastHit != null)
                {



                    if (newObjectOn && OVRInput.GetUp(OVRInput.Button.One))
                    {

                         //change object
                        if (indexNew < 4)
                        {
                            indexNew++;
                        } else
                        {
                            indexNew = 0;
                        }

                        Destroy(lastHit);


                        GameObject newObj = Instantiate(NewArray.transform.GetChild(indexNew).gameObject, transform.parent, true);

                        lastHit = newObj;

                        newObj.SetActive(true);

                        lastHit.GetComponent<Collider>().isTrigger = true;


                        lastHit.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                        line.material.color = Color.green;


                        //turn physics off
                        lastHit.GetComponent<Rigidbody>().useGravity = false;
                        lastHit.GetComponent<Rigidbody>().isKinematic = true;

                        return;

                    }



                    float offset = (prevHitPos - prevRHpos).magnitude;

                    lastHit.transform.position = rightHand.transform.position + rightHand.transform.forward * offset;

                    //lastHit.transform.rotation = rightHand.transform.rotation;


                    //movement forward/backward and rotation
                    Vector2 rightStick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

                    float vertical = rightStick.y;
                    float horizontal = rightStick.x;

                    if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
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

        }
        //if no trigger is pressed
        else
        {
            ReadyToTeleport = true;
            SecIndOn = false;
            line.enabled = false;




            if (lastHit != null)
            {

                
                //was curr
                if (lastHit.CompareTag("TapePoint"))
                {


                    lastHit.GetComponent<Renderer>().material.shader = Shader.Find("Standard");

                    line.material.color = Color.white;

                    lastHit = null;

                    return;


                }

                //turn physics on
                lastHit.GetComponent<Rigidbody>().useGravity = true;
                lastHit.GetComponent<Rigidbody>().isKinematic = false;

                //lastHit.GetComponent<MeshRenderer>().material = lastHit.GetComponent<Collision>().defaultMaterial; //reset material
                lastHit.GetComponent<Renderer>().material.shader = Shader.Find("Standard");

                line.material.color = Color.white;



                lastHit.GetComponent<Collider>().isTrigger = false;

                //reset rotation
                //lastHit.transform.rotation.

                //lastHit.GetComponent<Rigidbody>().isKinematic = false;


                //return to initial position
                if (collision)
                {
                    if (copyPasteOn || newObjectOn)
                    {

                        Destroy(lastHit);

                    }
                    else
                    {

                        lastHit.transform.position = initialPosition;
                        lastHit.transform.rotation = initialRotation;


                    }
                }


                if (newObjectOn)
                {
                    newObjectOn = false;

                }


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
                    //curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOff;

                }
                else
                {

                    newObjectOn = true;

                    //curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOn;


                    lastHit = null;

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

                    Vector3 offset = new Vector3(0, 0.1f, 0);

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
            else if (curr.CompareTag("Room"))
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

                //curr.GetComponent<MeshRenderer>().material = curr.GetComponent<Collision>().activeMaterial; //reset material
                curr.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                line.material.color = Color.green;


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
            line.material.color = Color.white;

        }



    }






}
