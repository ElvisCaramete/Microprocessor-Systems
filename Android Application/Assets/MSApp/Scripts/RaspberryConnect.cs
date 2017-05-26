using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System;

public class RaspberryConnect : MonoBehaviour {

	public Image myImageComponent;
	public Sprite succesConnect;
	public Sprite succesDisconnect;
	public Sprite standBy;
	private bool socketReady = false;
	private bool isCoroutineExecuting = false;
	TcpClient mySocket;
	public NetworkStream theStream;
	StreamWriter theWriter;
	StreamReader theReader;
	public String Host = "10.42.0.111";
	public Int32 Port = 50001;


	void Start(){
		myImageComponent.sprite = standBy;
	}
	void Update() {
		if(socketReady == true)
			while (theStream.DataAvailable) {                  // if new data is recieved from Arduino
				string recievedData = readSocket();            // write it to a string
					
				if (recievedData == "Ceva") {            // compare that string and adjust the current light status accordingly
					Debug.Log("Ceva");
				}
					
			}
		if(myImageComponent.sprite == succesDisconnect)
			StartCoroutine (ExecuteAfterTime (5));
	}

	IEnumerator ExecuteAfterTime(float time){

		if (isCoroutineExecuting)
			yield break;

		isCoroutineExecuting = true;

		yield return new WaitForSeconds (time);
		myImageComponent.sprite = standBy;

		isCoroutineExecuting = false;
	}

	public void setupSocket() {                            // Socket setup here
		try {
			if(socketReady == false){
				mySocket = new TcpClient();
				IAsyncResult result = mySocket.BeginConnect(Host,Port,null,null);

				if(!result.AsyncWaitHandle.WaitOne(2000,false)){
					myImageComponent.sprite = succesDisconnect;
					Debug.Log("Socket On Error Connection");
				}else{
					theStream = mySocket.GetStream();
					theWriter = new StreamWriter(theStream);
					theReader = new StreamReader(theStream);
					socketReady = true;
					myImageComponent.sprite = succesConnect;
				}
			}else{
				Debug.Log("Connection already established");
			}
		}
		catch (Exception e) {
			//myImageComponent.sprite = succesConnect; //temporary
			Debug.Log("Socket error:" + e);                // catch any exceptions
			myImageComponent.sprite = succesDisconnect;
		}
	}

	public void writeSocket() {            // function to write data out
		if (!socketReady){
			Debug.Log ("Connect first");
			return;
		}
		String tmpString = "Ceva";
		theWriter.Write(tmpString);
		theWriter.Flush();
	}

	private String readSocket() {                        // function to read data in
		if (!socketReady)
			return "";
		if (theStream.DataAvailable)
			return theReader.ReadLine();
		return "NoData";
	}

	public void closeSocket() {                            // function to close the socket
		if (!socketReady) {
			//myImageComponent.sprite = succesDisconnect; // temporary
			Debug.Log ("No connection found");
			return;
		}
		theWriter.Close();
		theReader.Close();
		mySocket.Close();
		socketReady = false;
		myImageComponent.sprite = succesDisconnect;
		Debug.Log ("Disconnect from the server");
	}

	//It may be usefull sometime
	private void maintainConnection(){                    // function to maintain the connection (not sure why! but Im sure it will become a solution to a problem at somestage)
		if(!theStream.CanRead) {
			setupSocket();
		}
	}

}
