using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    public GameObject empty;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("OnTriggerEnter");

        //other.gameObject.transform.position = empty.GetComponent<RaycastHand>().lastValidPosition;

        //empty.GetComponent<RaycastHand>().collision = true;

    }

    private void OnTriggerExit(Collider other)
    {

        Debug.Log("OnTriggerExit");

        //other.gameObject.transform.position = empty.GetComponent<RaycastHand>().lastValidPosition;

        //empty.GetComponent<RaycastHand>().collision = false;

    }
}
