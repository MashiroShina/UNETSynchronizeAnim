using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveGUN : MonoBehaviour
{

    //   public float horSetive = 10;

    //   public float verSetive = 10;

    //   public float mouseX;
    //   public float mouseY;
    //   public float horAngle=0;
    //   public float verAngle = 0;
    //   // Use this for initialization
    //   void Start () {

    //}

    //// Update is called once per frame
    //void Update ()
    //{
    //    mouseX = Input.GetAxis("Mouse X");
    //    mouseY = Input.GetAxis("Mouse Y");
    //    horAngle += mouseX * horSetive * Time.deltaTime * 10;
    //    verAngle += mouseY * verSetive * Time.deltaTime * 10;
    //    transform.localRotation = Quaternion.Euler(0, horAngle, 0);
    //}
    private Transform camTrans;
    private float camHeight = 0.3f;
    private Vector3 camAng;
    public Image image;
    void Start()
    {
        camTrans = Camera.main.transform;

        Vector3 startPos = transform.position;

        startPos.y += camHeight;

        camTrans.position = startPos;

        camTrans.rotation = transform.rotation;

        camAng = camTrans.eulerAngles;
        // camTrans.position=new Vector3(transform.position.x,0.3f,transform.position.z);
    }

    void Update()
    {
        image.transform.position = Input.mousePosition;
        cameraView();

        float h = Input.GetAxis("Vertical");
        float v = Input.GetAxis("Horizontal");
        if (h != 0 || v != 0)
        {
            transform.Translate(new Vector3(v*Time.deltaTime*50f, 0, h*Time.deltaTime*50f));
            //transform.Translate(new Vector3(h, 0, v) * Time.deltaTime, Space.World);
        }
    }

    private void cameraView()
    {
        float y = Input.GetAxis("Mouse X");

        float x = Input.GetAxis("Mouse Y");

        camAng.x -= x;

        camAng.y += y;

        camTrans.eulerAngles = camAng;

        float camy = camAng.y;

        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, camy, this.transform.eulerAngles.z);

        // Vector3 startPos = transform.position;

        //startPos.y += camHeight;

        //camTrans.position = startPos;
    }

}
