using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class move : NetworkBehaviour {
    [SerializeField]
    Camera fpscamera;
    [SerializeField]
    AudioListener audioplayer;
    private NetworkStartPosition[] spawnPoints;
    // Use this for initialization
    void Start ()
    {
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();

    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (isLocalPlayer)
	    {
	        charatermove();
        }
	}

    private void charatermove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
        {
            transform.Translate(new Vector3(h, 0, v) * 3*Time.deltaTime, Space.World);

            transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));
        }
    }
}
