using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Text;

public class Server : MonoBehaviour
{
    public string myip="192.168.43.231";
    public int port = 5005;

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
        if(udp.Available != 0)
        {
            byte[] packet = new byte[128];
            EndPoint sender = new IPEndPoint(IPAddress.Any, port);

            int rec = udp.ReceiveFrom(packet, ref sender);
            string info = Encoding.Default.GetString(packet);

            Debug.Log("Server received: " + info);
        }
        else
            Debug.Log("no");
    }
    void SendPacket(string str, EndPoint addr)
    {
        byte[] arr = Encoding.ASCII.GetBytes(str);
        udp.SendTo(arr, addr);
    }
}