using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetNet : MonoBehaviour
{
    private NetManager lobby;
	// Use this for initialization
	void Start ()
	{
	    lobby = GameObject.Find("Net").GetComponent<NetManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void bt1()
    {
        lobby.btn1(); 
    }

    public void bt2()
    {
        lobby.btn2();
    }
}
