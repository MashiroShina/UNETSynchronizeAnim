using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetManager : NetworkManager {
    public int chosenCharacter = 0;
    private NetworkStartPosition[] spawnPoints;
    private NetworkManager na;
    // Use this for initialization
    private static Button bt11;

    private static Button bt22;
    void Start()
    {
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }
    public class NetworkMessage : MessageBase
    {
        public int chosenClass;
    }

    public int selectedClass=0;
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId,
        NetworkReader extraMessageReader)
    {
        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int selectedClass = message.chosenClass;
        Debug.Log("server add with message " + selectedClass);
        if (selectedClass == 0)
        {    
            GameObject player = Instantiate(Resources.Load("Wolf", typeof(GameObject))) as GameObject;//加载角色
            player.transform.position=new Vector3(0,0,0);
           // gamePlayerPrefab = Resources.Load("Wolf", typeof(GameObject)) as GameObject;
            //Debug.Log("wolf");
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);//添加角色
            //NetworkServer.ReplacePlayerForConnection(conn, gamePlayerPrefab, playerControllerId);    
        }

        if (selectedClass == 1)
        {
             GameObject player = Instantiate(Resources.Load("Yuka2", typeof(GameObject))) as GameObject;
             player.transform.position = new Vector3(1, 1, 1);
           // gamePlayerPrefab = Resources.Load("Yuka2", typeof(GameObject)) as GameObject;
            //Debug.Log("Yuka2");
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);//添加角色
            //NetworkServer.ReplacePlayerForConnection(conn, gamePlayerPrefab, playerControllerId);
        }
        // base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("点击连接按钮时LANCLient调用");
        NetworkMessage test = new NetworkMessage();
        test.chosenClass = chosenCharacter;
        ClientScene.AddPlayer(conn, 0, test);//把自己的conn传递给服务端
        //AddPlayer 这会将AddPlayer消息发送到服务器，并调用NetworkManager.OnServerAddPlayer。  
    }
    public void btn1()
    {
        chosenCharacter = 0;
        Debug.Log(0);
    }

    public void btn2()
    {
        chosenCharacter = 1;
        Debug.Log(1);
    }
    
}
