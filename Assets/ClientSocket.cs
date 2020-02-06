 using UnityEngine;                        // These are the librarys being used
 using System.Collections;
 using System;
 using System.IO;
 using System.Net.Sockets; 
 
 public class ClientSocket : MonoBehaviour {
 
     public bool trig = false;
     bool socketReady = false;                // global variables are setup here
     TcpClient mySocket;
     public NetworkStream theStream;
     StreamWriter theWriter;
     StreamReader theReader;
     public String Host = "127.0.0.1";
     public Int32 Port = 5001;
 
    public void setupSocket() {                            // Socket setup here
         try {                
             mySocket = new TcpClient(Host, Port);
             theStream = mySocket.GetStream();
             theWriter = new StreamWriter(theStream);
             theReader = new StreamReader(theStream);
             socketReady = true;
         }
         catch (Exception e) {
            Debug.Log("Socket error:" + e);                // catch any exceptions
         }
     }
     
     public void writeSocket(string theLine) {            // function to write data out
         try{
             if (!socketReady)
                return;
            String tmpString = theLine;
            theWriter.Write(tmpString);
            theWriter.Flush();
        }
         catch (Exception e) {
            Debug.Log("Socket error:" + e);                // catch any exceptions
         }
     }
     
     public String readSocket() {                        // function to read data in
             if (!socketReady)
                return "";
            if (theStream.DataAvailable)
                return theReader.ReadLine();
            return "NoData";
     }
     
     public void closeSocket() {                            // function to close the socket
         try{
             if (!socketReady)
                return;
            theWriter.Close();
            theReader.Close();
            mySocket.Close();
            socketReady = false;
            }
         catch (Exception e) {
            Debug.Log("Socket error:" + e);                // catch any exceptions
         }
     }
     
     public void maintainConnection(){                    // function to maintain the connection (not sure why! but Im sure it will become a solution to a problem at somestage)
         try{
             if(!theStream.CanRead) {
             setupSocket();
         }
         }
         catch (Exception e) {
            Debug.Log("Socket error:" + e);                // catch any exceptions
         }
     }
     void Start() {
         try{
             if(trig){
         setupSocket ();
         }
         }
         catch (Exception e) {
            Debug.Log("Socket error:" + e);                // catch any exceptions
         }                        // setup the server connection when the program starts
     }
     
     // Update is called once per frame
     void Update() {
         try{
             if(trig){
            writeSocket("HI");
            while (theStream.DataAvailable) {                  // if new data is recieved from Arduino
             string recievedData = readSocket();            // write it to a string
             Debug.Log("Input"+recievedData);
            }
         }
         }
         catch (Exception e) {
            Debug.Log("Socket error:" + e);                // catch any exceptions
         }
    }
 } // end class ClientSocket