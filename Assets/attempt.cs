using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Text;

public class attempt : MonoBehaviour
{
    public string myip="127.0.0.1";
    public int port = 5005;
    
    public rotate upperarmR;
    public rotate forearmR;
    
    Socket udp;
    void Start()
    {
        IPEndPoint myendPoint = new IPEndPoint(IPAddress.Parse(myip), port);
        udp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udp.Bind(myendPoint);
        udp.Blocking = false;
        udp.ReceiveTimeout = 1000;

    }

    void Update()
    {
        /*if(true)
        {
            Debug.Log("sending");
            SendPacket("CHECK",new IPEndPoint(IPAddress.Parse(myip), 5004));
            
        }*/
        if(udp.Available != 0)
        {
            byte[] packet = new byte[64];
            EndPoint sender = new IPEndPoint(IPAddress.Any, port);
            int rec = udp.ReceiveFrom(packet, ref sender);
            string info = Encoding.Default.GetString(packet);
            Debug.Log("Server received: " + info);
            string[] mess=info.Split(' ');
            if(string.Compare(mess[0],"1")==0)
            {
                if(forearmR.first)
                {
                forearmR.gx=float.Parse(mess[1]);
                forearmR.gy=float.Parse(mess[3]);
                forearmR.gz=float.Parse(mess[2]);
                forearmR.first=false;    
                }
                else
                {
                forearmR.dx=float.Parse(mess[1])-forearmR.gx;
                forearmR.dy=float.Parse(mess[3])-forearmR.gy;
                forearmR.dz=float.Parse(mess[2])-forearmR.gz;
                }
            }
            if(string.Compare(mess[0],"2")==0)
            {
                if(upperarmR.first)
                {
                upperarmR.gx=float.Parse(mess[1]);
                upperarmR.gy=float.Parse(mess[3]);
                upperarmR.gz=float.Parse(mess[2]);
                upperarmR.first=false;    
                }
                else{
                upperarmR.dx=float.Parse(mess[1])-upperarmR.gx;
                upperarmR.dy=float.Parse(mess[3])-upperarmR.gy;
                upperarmR.dz=float.Parse(mess[2])-upperarmR.gz;
                }
            }
/*             }
            if(string.Compare(mess[0],"1")==0)
            {  
                forearmR.dx=float.Parse(mess[1]);
                forearmR.dy=float.Parse(mess[2]);
                forearmR.dz=float.Parse(mess[3]);
            }
            if(string.Compare(mess[0],"2")==0)
            {
                upperarmR.dx=float.Parse(mess[1]);
                upperarmR.dy=float.Parse(mess[2]);
                upperarmR.dz=float.Parse(mess[3]);
            }
            */
        }
    }
    void SendPacket(string str, EndPoint addr)
    {
        byte[] arr = Encoding.ASCII.GetBytes(str);
        udp.SendTo(arr, addr);
    }
}