using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHand : MonoBehaviour
{

    public GameObject rightHand;
    public GameObject leftHand;

    public GameObject Furniture;

    public Vector3[] initialPositions;

    public Vector3[] initialPositions2;


    public bool falling = false;



    public GameObject Group;
    private GameObject newGroup;

    private GameObject selectedChild;

    private Component[] children;

    private bool aPressed = false;

    public bool groupSelected;

    public GameObject grabbed;

    public bool collisionGrab = false;

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
    public Vector3 initialPosition2;





    public Quaternion initialRotation;

    public bool collision = false;

    public bool SecIndOn = false;


    // Use this for initialization
    void Start()
    {

        line = GetComponent<LineRenderer>();
        line.material.color = Color.white;
        line.enabled = false;

        initialPositions = new Vector3[100];
    }




    void GroupUpdate()
    {

        //if just right hand trigger is pressed
        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {


            line.enabled = true;
            //line.material.color = Color.white;
            line.SetPosition(0, rightHand.transform.position);
            line.SetPosition(1, rightHand.transform.position + rightHand.transform.forward * 100);

            //held down
            if (SecIndOn)
            {

                float offset = (prevHitPos - prevRHpos).magnitude;

                //Vector3 dist = selectedChild.transform.position - prevHitPos;//rightHand.transform.position + rightHand.transform.forward * offset;

                Vector3 dist = (rightHand.transform.position + rightHand.transform.forward * offset) - prevHitPos;

                for (int i = 0; i < lastHit.transform.childCount; i++)
                {
                    lastHit.transform.GetChild(i).transform.position = lastHit.transform.GetChild(i).transform.position + dist;
                }

                //movement forward/backward and rotation
                Vector2 rightStick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

                float vertical = rightStick.y;
                float horizontal = rightStick.x;

                if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
                {
                    //lastHit.transform.position = lastHit.transform.position + rightHand.transform.forward * (rightStick.y / 100);
                    for (int i = 0; i < lastHit.transform.childCount; i++)
                    {
                        lastHit.transform.GetChild(i).transform.position = lastHit.transform.GetChild(i).transform.position + rightHand.transform.forward * (rightStick.y / 50);
                    }
                }
                else
                {
                    //rotates individually
                    //lastHit.transform.Rotate(0, (rightStick.x), 0);


                    //calculate center position and rotate around it

                    Vector3 center = new Vector3(0, 0, 0);

                    for (int i = 0; i < lastHit.transform.childCount; i++)
                    {
                        center += lastHit.transform.GetChild(i).transform.position;
                    }
                    center = center / (lastHit.transform.childCount);


                    for (int i = 0; i < lastHit.transform.childCount; i++)
                    {
                        lastHit.transform.GetChild(i).transform.RotateAround(center, new Vector3(0, 1, 0), rightStick.x);
                    }
                }

                prevHitPos = selectedChild.transform.position;
                prevRHpos = rightHand.transform.position;


                line.SetPosition(1, selectedChild.transform.position);


            }

            //first click
            else
            {
                Debug.Log("SHOULDNT BE HERE");

            }

        }
        //if no trigger is pressed
        else
        {


            ReadyToTeleport = true;
            SecIndOn = false;
            line.enabled = false;


            //turn physics on
            //lastHit.GetComponent<Rigidbody>().useGravity = true;
            //lastHit.GetComponent<Rigidbody>().isKinematic = false;
            children = lastHit.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody comp in children)
            {
                comp.useGravity = true;
                comp.isKinematic = false;
            }

            //lastHit.GetComponent<MeshRenderer>().material = lastHit.GetComponent<Collision>().defaultMaterial; //reset material
            //lastHit.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
            children = lastHit.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in children)
            {
                rend.material.shader = Shader.Find("Standard");
            }

            line.material.color = Color.white;

            //lastHit.GetComponent<Collider>().isTrigger = false;
            children = lastHit.GetComponentsInChildren<Collider>();
            foreach (Collider comp in children)
            {
                comp.isTrigger = false;
            }

            
            //let go in collision
            if (collision)
            {
                //destroy all children
                if (copyPasteOn)
                {

                    Destroy(lastHit);

                }
                //reset positions
                else
                {

                    for (int i = 0; i < lastHit.transform.childCount; i++)
                    {
                        lastHit.transform.GetChild(i).transform.position = initialPositions[i];
                    }
                }
                
            } else
            {

                children = lastHit.GetComponentsInChildren<Collider>();
                foreach (Collider comp in children)
                {
                    comp.isTrigger = true;
                }

                falling = true;

                initialPositions2 = initialPositions;


            }


            



            lastHit = null;

            groupSelected = false;

        }            

    }



    // Update is called once per frame
    void Update()
    {


        if (groupSelected) {

            GroupUpdate();
            return;

        }



        //create object
        if (newObjectOn && (lastHit == null))
        {

            GameObject newObj = Instantiate(NewArray.transform.GetChild(indexNew).gameObject, Furniture.transform, true);

            lastHit = newObj;

            newObj.SetActive(true);

            lastHit.GetComponent<Collider>().isTrigger = true;


            lastHit.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");


            children = lastHit.GetComponentsInChildren<Renderer>();

            foreach (Renderer rend in children)
            {
                rend.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
            }

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


                        //GameObject newObj = Instantiate(NewArray.transform.GetChild(indexNew).gameObject, transform.parent, true);
                        GameObject newObj = Instantiate(NewArray.transform.GetChild(indexNew).gameObject, Furniture.transform, true);

                        lastHit = newObj;

                        newObj.SetActive(true);

                        lastHit.GetComponent<Collider>().isTrigger = true;


                        lastHit.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");

                        children = lastHit.GetComponentsInChildren<Renderer>();

                        foreach (Renderer rend in children)
                        {
                            rend.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                        }

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
                        lastHit.transform.position = lastHit.transform.position + rightHand.transform.forward * (rightStick.y / 50);
                    }
                    else
                    {
                        lastHit.transform.Rotate(0, 0, (rightStick.x));
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



            //just released
            if (lastHit != null)
            {

                
                //was curr
                if (lastHit.CompareTag("TapePoint"))
                {


                    lastHit.GetComponent<Renderer>().material.shader = Shader.Find("Standard");

                    children = lastHit.GetComponentsInChildren<Renderer>();

                    foreach (Renderer rend in children)
                    {
                        rend.material.shader = Shader.Find("Standard");
                    }

                    line.material.color = Color.white;

                    lastHit = null;

                    return;


                }

                //turn physics on
                lastHit.GetComponent<Rigidbody>().useGravity = true;




                
                lastHit.GetComponent<Rigidbody>().isKinematic = false;
                //lastHit.GetComponent<Rigidbody>().isKinematic = true;




                //lastHit.GetComponent<MeshRenderer>().material = lastHit.GetComponent<Collision>().defaultMaterial; //reset material
                lastHit.GetComponent<Renderer>().material.shader = Shader.Find("Standard");

                children = lastHit.GetComponentsInChildren<Renderer>();

                foreach (Renderer rend in children)
                {
                    rend.material.shader = Shader.Find("Standard");
                }

                line.material.color = Color.white;


                
                lastHit.GetComponent<Collider>().isTrigger = false;
                


                //reset rotation
                //float zRot = lastHit.transform.rotation.eulerAngles.z;


                //lastHit.transform.rotation = Quaternion.Euler(-90, 0, zRot);





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
                } else
                {


                    lastHit.GetComponent<Collider>().isTrigger = true;
                    falling = true;
                    initialPosition2 = initialPosition;

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

                    //newGroup.GetComponent<Renderer>().material.shader = Shader.Find("Standard");

                    children = newGroup.GetComponentsInChildren<Renderer>();

                    foreach (Renderer rend in children)
                    {
                        rend.material.shader = Shader.Find("Standard");
                    }

                    return;

                }
                else
                {

                    groupOn = true;

                    curr.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = buttonOn;


                    //make a new group
                    newGroup = Instantiate(Group, Group.transform.parent, true);
                    


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


            else if (groupOn)
            {
                

                //highlight it
                curr.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");

                children = curr.GetComponentsInChildren<Renderer>();

                foreach (Renderer rend in children)
                {
                   rend.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                }

                curr.transform.SetParent(newGroup.transform);

                lastHit = null;

                return;

            }

            //hit object elligible for transform
            else
            {


                //if its a part of a group
                if (curr.transform.parent.gameObject.CompareTag("Group"))
                {

                    groupSelected = true;


                    //duplicate group
                    if (copyPasteOn)
                    {

                        newGroup = Instantiate(Group, Group.transform.parent, true);



                        int size = curr.transform.parent.childCount;

                        GameObject oldCurr = curr;

                        for (int i = 0; i < size; i++)
                        {

                            GameObject newObject = Instantiate(oldCurr.transform.parent.GetChild(i).gameObject, newGroup.transform, true);

                            newObject.GetComponent<Collider>().isTrigger = true;


                            if (oldCurr.transform.parent.GetChild(i).gameObject == oldCurr)
                            {
                                curr = newObject;

                            }

                        }

                    }



                    selectedChild = curr;

                    //save previous locations
                    prevRHpos = rightHand.transform.position;
                    prevHitPos = curr.transform.position;


                    for (int i = 0; i < curr.transform.parent.childCount; i++)
                    {

                        if (!copyPasteOn)
                        {

                            initialPositions[i] = curr.transform.parent.GetChild(i).transform.position;
                        } else
                        {

                            initialPositions[i].x = -1000;
                            initialPositions[i].y = -1000;
                            initialPositions[i].z = -1000;

                        }
                    }

                    initialPosition = curr.transform.position;
                    initialRotation = curr.transform.rotation;


                    curr = curr.transform.parent.gameObject;

                    //highlight them all
                    children = curr.GetComponentsInChildren<Renderer>();

                    foreach (Renderer rend in children)
                    {
                        rend.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                    }


                    


                    //curr.GetComponent<Collider>().isTrigger = true;
                    children = curr.GetComponentsInChildren<Collider>();
                    foreach (Collider comp in children)
                        comp.isTrigger = true;


                    line.material.color = Color.green;



                    //turn physics off
                    //curr.GetComponent<Rigidbody>().useGravity = false;
                    //curr.GetComponent<Rigidbody>().isKinematic = true;
                    children = curr.GetComponentsInChildren<Rigidbody>();
                    foreach (Rigidbody comp in children) { 
                        comp.useGravity = false;
                        comp.isKinematic = true;
                    }


                    lastHit = curr;



                    return;



                }



                //duplicate object
                if (copyPasteOn)
                {

                    if (!(curr.CompareTag("TapePoint")))
                    {

                        GameObject newCopy = Instantiate(curr, curr.transform.parent, true);
                        curr = newCopy;

                        newCopy.GetComponent<Collider>().isTrigger = true;

                        initialPosition.x = -1000;
                        initialPosition.y = -1000;
                        initialPosition.z = -1000;

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

                children = curr.GetComponentsInChildren<Renderer>();

                foreach (Renderer rend in children)
                {
                    rend.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
                }


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

                if (!copyPasteOn)
                {

                    initialPosition = curr.transform.position;
                    initialRotation = curr.transform.rotation;

                }


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
