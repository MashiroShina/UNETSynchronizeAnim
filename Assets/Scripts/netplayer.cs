using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class netplayer : NetworkBehaviour {	
	// Update is called once per frame
	void Update () {
	    if (isLocalPlayer)
	    {
	        if (Input.GetMouseButtonDown(1))
	        {
	            CmdSpawn();
	        }
	        if (Input.GetMouseButtonDown(0))
	        {
	            CmdSpawn2();
	        }
        }
	}
    [Command]
    void CmdSpawn()
    {
        GameObject go = Instantiate(Resources.Load("Yuka2", typeof(GameObject))) as GameObject;
        transform.position = new Vector3(0, 0, 0);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        //客户端调用后会得到一个授权哪个客户端调用哪个客户端的hasAuthority会变成true
    }
    [Command]
    void CmdSpawn2()
    {
        
           GameObject go = Instantiate(Resources.Load("Wolf", typeof(GameObject))) as GameObject;
        transform.position = new Vector3(0, 0, 0);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }
}
