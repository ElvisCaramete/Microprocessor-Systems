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
	public String Host = "";
	public Int32 Port = 50001;

	public Text humidityValue;
	private int prevPump = 2;
	float valueHumidity ;

	public Text temperatureValue;
	private int prevTemp = 2;
	float valueTemperature ;

	public Text moistureValue;
	private int prevMoist = 2;

	public Text lightValue;
	private int prevLight   = 2;


	void Start(){
		myImageComponent.sprite = standBy;
		humidityValue.text = "Humidity Value: Connect First";
		temperatureValue.text = "Temperature Value: Connect First";
	}
	void Update() {

		if (socketReady == true && (prevPump == 1 || prevPump == 0)) {
			humidityValue.text = "Humidity value: " + String.Format ("{0:00.00}", valueHumidity).ToString ();
		} else if (socketReady == true)
			humidityValue.text = "Temperature Value: None";

		if (socketReady == true && (prevTemp == 1 || prevTemp == 0)) {
			temperatureValue.text = "Temperature value: "+ String.Format("{0:00.00}", valueTemperature).ToString ();
		} else if (socketReady == true) 
			temperatureValue.text = "Temperature Value: None";


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

	//HUMIDITY Functions
	public void humidityTurnON() { 
		if (!socketReady){
			Debug.Log ("Connect first");
			return;
		}
		prevPump = 1;
		valueHumidity = UnityEngine.Random.Range (44f, 54f);
		String tmpString = "PumpON";
		theWriter.Write(tmpString);
		theWriter.Flush();
	}

	public void humidityTurnOFF() {     
		if (!socketReady){
			Debug.Log ("Connect first");
			return;
		}
		if (prevPump == 1) {
			valueHumidity = UnityEngine.Random.Range (44f, 54f);
			String tmpString = "PumpOFF";
			theWriter.Write (tmpString);
			theWriter.Flush ();
			prevPump = 0;
		}
		Debug.Log ("Turn ON first");
	}

	//TEMPERATURE Functions
	public void temperatureTurnON() { 
		if (!socketReady){
			Debug.Log ("Connect first");
			return;
		}
		prevTemp = 1;
		valueTemperature = UnityEngine.Random.Range (24f, 26f);
		String tmpString = "LedON";
		theWriter.Write(tmpString);
		theWriter.Flush();
	}

	public void temperatureTurnOFF() {     
		if (!socketReady){
			Debug.Log ("Connect first");
			return;
		}
		if (prevTemp == 1) {
			valueTemperature = UnityEngine.Random.Range (24f, 26f);
			String tmpString = "LedOFF";
			theWriter.Write (tmpString);
			theWriter.Flush ();
			prevTemp = 0;
		}
		Debug.Log ("Turn ON first");
	}


	//MOISTURE Functions

	public void moistureTurnON() { 
		if (!socketReady){
			Debug.Log ("Connect first");
			return;
		}
		prevMoist = 1;
		String tmpString = "MoistON";
		theWriter.Write(tmpString);
		theWriter.Flush();
	}

	public void moistureTurnOFF() {     
		if (!socketReady){
			Debug.Log ("Connect first");
			return;
		}
		if (prevMoist == 1) {
			String tmpString = "MoistOFF";
			theWriter.Write (tmpString);
			theWriter.Flush ();
		}
		Debug.Log ("Turn ON first");
	}

	//LIGHT Functions

	public void lightTurnON() { 
		if (!socketReady){
			Debug.Log ("Connect first");
			return;
		}
		prevLight = 1;
		String tmpString = "LightON";
		theWriter.Write(tmpString);
		theWriter.Flush();
	}

	public void lightTurnOFF() {     
		if (!socketReady){
			Debug.Log ("Connect first");
			return;
		}
		if (prevLight == 1) {
			String tmpString = "LightOFF";
			theWriter.Write (tmpString);
			theWriter.Flush ();
		}
		Debug.Log ("Turn ON first");
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
