using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objAddForce : MonoBehaviour
{
    public GameObject obj;

    private Rigidbody objrigi;
    public float forces=5;
    public Transform jetport;
	// Use this for initialization
	void Start ()
	{
	    objrigi = obj.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float h = Input.GetAxis("Horizontal");
	    float v = Input.GetAxis("Vertical");
	    if (Input.GetKey(KeyCode.W))
	    {
	        objrigi.AddForceAtPosition(Vector3.up * forces, jetport.position);
        }
	   
	    //objrigi.AddForceAtPosition(Vector3.forward * forces, jetport.position);


    }
}
