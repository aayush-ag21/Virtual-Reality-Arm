using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float dx =0f;
    public float dy =0f;
    public float dz =0f;
    private float x;
    private float y;
    private float z;
    public float gx=0f;
    public float gy=0f;
    public float gz=0f;
    public bool first=true;
    //private int i=0;
    //private bool flag =true;
    // Start is called before the first frame update
    void Start()
    {

        x=transform.eulerAngles.x;
        y=transform.eulerAngles.y;
        z=transform.eulerAngles.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(x+dx/3000, y+dy/3000, z+dz/3000);
        /*if(flag)
        i=i+1;
        else
        {
            i=i-1;
        }
        if(i==90)
        flag=false;
        if(i==-90)
        flag=true;
        */
        x=transform.eulerAngles.x;
        y=transform.eulerAngles.y;
        z=transform.eulerAngles.z;
    }
}
