using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    public GameObject empty;
    public Material activeMaterial; //green
    public Material defaultMaterial;
    public Material collisionMaterial; //red

    public LineRenderer line;

    //colliding 
    private void OnTriggerEnter(Collider other)
    {

        if (empty.GetComponent<RaycastHand>().lastHit == this.gameObject)
        {

            //Debug.Log("OnTriggerEnter" + this.name);

            //other.gameObject.transform.position = empty.GetComponent<RaycastHand>().lastValidPosition;

            //empty.GetComponent<RaycastHand>().collision = true;

            //change material to red
            //this.GetComponent<MeshRenderer>().material = collisionMaterial;


        }

    }

    void OnTriggerStay(Collider trigger)
    {
        if (empty.GetComponent<RaycastHand>().lastHit == this.gameObject)
        {

            empty.GetComponent<RaycastHand>().collision = true;

            //change material to red
            //this.GetComponent<MeshRenderer>().material = collisionMaterial;
            //GetComponent<Renderer>().material.shader = Shader.Find("Collision");
            line.material.color = Color.red;

        }
    }

    //not colliding 
    private void OnTriggerExit(Collider other)
    {
        if (empty.GetComponent<RaycastHand>().lastHit == this.gameObject)
        {
            //Debug.Log("OnTriggerExit" + this.name);

            //other.gameObject.transform.position = empty.GetComponent<RaycastHand>().lastValidPosition;

            empty.GetComponent<RaycastHand>().collision = false;

            //change to green
            //this.GetComponent<MeshRenderer>().material = activeMaterial;
            //GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
            line.material.color = Color.green;





        }


    }
}
