//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]

public class controller : NetworkBehaviour 
{
	
	public float animSpeed = 1.5f;				
	public float lookSmoother = 3.0f;			
	public bool useCurves = true;				
	public float useCurvesHeight = 0.5f;		
	public float forwardSpeed = 7.0f;
	public float backwardSpeed = 2.0f;
	public float rotateSpeed = 2.0f;
	public float jumpPower = 3.0f; 
	private CapsuleCollider col;
	private Rigidbody rb;
	private Vector3 velocity;
	private float orgColHight;
	private Vector3 orgVectColCenter;
	private Animator anim;						
	private AnimatorStateInfo currentBaseState;			
	private GameObject cameraObject;	
	
	

	static int idle_cState = Animator.StringToHash("Base Layer.Idle_C");
	static int idle_aState = Animator.StringToHash("Base Layer.Idle_A");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int cute1State = Animator.StringToHash("Base Layer.Cute1");
	

	
	void Start ()
	{
		anim = GetComponent<Animator>();
		col = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();
		cameraObject = GameObject.FindWithTag("MainCamera");
		orgColHight = col.height;
		orgVectColCenter = col.center;

	    //if (myTransform.name == "" || myTransform.name == "Player(Clone)")
	    //{
	    //    SetIdentity(); //这里设置唯一name的是本机客户端中的其他玩家，本机玩家已经在OnStartLocalPlayer设置好了
	    //}
    }
    //[SyncVar]
    //private string playerUniqueIdentity;
    //private NetworkInstanceId playerNetId;
    //private Transform myTransform;
    //void GetNetIdentity()
    //{
    //    playerNetId = GetComponent<NetworkIdentity>().netId;
    //    CmdTellServerMyIdentity(MakeUniqueIdentiy());
    //}
    //void SetIdentity()
    //{
    //    if (!isLocalPlayer)
    //    {
    //        myTransform.name = playerUniqueIdentity;
    //    }
    //    else
    //    {
    //        myTransform.name = MakeUniqueIdentiy();
    //    }
    //}
    //string MakeUniqueIdentiy()
    //{
    //    string uniqueName = "Player_" + playerNetId.ToString();
    //    return uniqueName;
    //}
    //[Command]
    //void CmdTellServerMyIdentity(string name)
    //{
    //    playerUniqueIdentity = name;
    //}
    //public override void OnStartLocalPlayer()
    //{
    //    GetNetIdentity();
    //    SetIdentity();
    //}
    //public void Awake()
    //{
    //    myTransform = transform;
    //}


    [SyncVar]
    private Vector3 v3_PlayerPos;

    [SyncVar]
    private Quaternion qua_PlayerRotate;

    [Command]
    public void CmdSendServerPos(Vector3 pos, Quaternion rotate)
    {
        v3_PlayerPos = pos;
        qua_PlayerRotate = rotate;
    }

    public GameObject prefabs;
    [Command]// called in client, run in server
    void CmdFire()//这个方法需要在server里面调用
    {
        // 子弹的生成 需要server完成，然后把子弹同步到各个client
        GameObject bullet = Instantiate(prefabs, transform.position+new Vector3(1,1,1), transform.rotation) as GameObject;
        //Destroy(bullet, 2);

        NetworkServer.Spawn(bullet);
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetMouseButton(1))
            {
                CmdFire();
            }
        }
       
    }

    public override void OnStartAuthority()
    {
      
        base.OnStartAuthority();
    }

    void FixedUpdate()
	{
        if (!hasAuthority)
	    {
	        transform.position = Vector3.Lerp(transform.position, v3_PlayerPos, 5 * Time.fixedDeltaTime);
	        transform.rotation = Quaternion.Lerp(transform.rotation, qua_PlayerRotate, 5 * Time.fixedDeltaTime);
            return;
	    }      
	    CmdSendServerPos(transform.position, transform.rotation);



        float h = Input.GetAxis("Horizontal");				
		float v = Input.GetAxis("Vertical");				
		anim.SetFloat("Speed", v);							
		anim.SetFloat("Direction", h); 						
		anim.speed = animSpeed;								
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	
		rb.useGravity = true;
		
		
		
		velocity = new Vector3(0, 0, v);
		velocity = transform.TransformDirection(velocity);
		if (v > 0.1) {
			velocity *= forwardSpeed;	
		} else if (v < -0.1) {
			velocity *= backwardSpeed;
		}
	
		

		
		if (Input.GetButtonDown("Jump")) {	
			if (currentBaseState.nameHash == locoState){
				if(!anim.IsInTransition(0))
				{
					rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
					anim.SetBool("Jump", true);
				}
			}
		}
		
		
		transform.localPosition += velocity * Time.fixedDeltaTime;
		transform.Rotate(0, h * rotateSpeed, 0);	
		if (currentBaseState.nameHash == locoState){
			if(useCurves){
				resetCollider();
			}
		}
		if(currentBaseState.nameHash == jumpState)
		{
			cameraObject.SendMessage("setCameraPositionJumpView");	
			if(!anim.IsInTransition(0))
			{
				
				if(useCurves){
					float jumpHeight = anim.GetFloat("JumpHeight");
					float gravityControl = anim.GetFloat("GravityControl"); 
					if(gravityControl > 0)
						rb.useGravity = false;	
					
					Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
					RaycastHit hitInfo = new RaycastHit();
					if (Physics.Raycast(ray, out hitInfo))
					{
						if (hitInfo.distance > useCurvesHeight)
						{
							col.height = orgColHight - jumpHeight;			
							float adjCenterY = orgVectColCenter.y + jumpHeight;
							col.center = new Vector3(0, adjCenterY, 0);
						}
						else{
							
							resetCollider();
						}
					}
				}
				anim.SetBool("Jump", false);
			}
		}
		

		
		else if (currentBaseState.nameHash == idle_cState)
		{
			if(useCurves){
				resetCollider();
			}
			
			if (Input.GetButtonDown("Jump")) {
				anim.SetBool("Cute1", true);
			}

			
			
		}
		else if (currentBaseState.nameHash == idle_aState)
		{
			if(useCurves){
				resetCollider();
			}
			
			if (Input.GetButtonDown("Jump")) {
				anim.SetBool("Cute1", true);
			}

		}
		else if (currentBaseState.nameHash == cute1State)
		{
			
			if(!anim.IsInTransition(0))
			{
				anim.SetBool("Cute1", false);
			}
		}


	}
	
	void OnGUI()
	{
		GUI.Box(new Rect(Screen.width -260, 10 ,250 ,150), "Interaction");
		GUI.Label(new Rect(Screen.width -245,30,250,30),"Up/Down Arrow : Go Forwald/Go Back");
		GUI.Label(new Rect(Screen.width -245,50,250,30),"Left/Right Arrow : Turn Left/Turn Right");
		GUI.Label(new Rect(Screen.width -245,70,250,30),"Hit Space key while Running : Jump");
		GUI.Label(new Rect(Screen.width -245,90,250,30),"Hit Spase key while Stopping : Cute1");
		GUI.Label(new Rect(Screen.width -245,110,250,30),"Left Control : Front Camera");
		GUI.Label(new Rect(Screen.width -245,130,250,30),"Alt : LookAt Camera");
	}
	void resetCollider()
	{
		
		col.height = orgColHight;
		col.center = orgVectColCenter;
	}
}

