using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.IO;

public class CameraController : MonoBehaviour {

	public MeshRenderer frame;    //Mesh for displaying video

	//Set the value from unity also if needed
	public string sourceURL = "http://10.42.0.111:8081/axis-cgi/mjpg/video.cgi";
	private Texture2D texture; 
	private Stream stream;



	public void GetVideo(){
		try{
			texture = new Texture2D(2, 2); 
			// create HTTP request
			HttpWebRequest req = (HttpWebRequest) WebRequest.Create( sourceURL );
			req.Timeout=2000;
			//Optional (if authorization is Digest)
			//req.Credentials = new NetworkCredential("username", "password");
			// get response
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			// get response stream
			stream = resp.GetResponseStream();
			StartCoroutine (GetFrame ());
		}
		catch (Exception e) {
			Debug.Log("Socket error:" + e);
		}
	}

	public void StopCamera(){
		stream = null;
		StartCoroutine (GetFrame ());
	}

	IEnumerator GetFrame (){
		Byte [] JpegData = new Byte[65536];//100000

		while(true) {
			int bytesToRead = FindLength(stream);
			print (bytesToRead);
			if (bytesToRead == -1) {
				print("End of stream");
				yield break;
			}

			int leftToRead=bytesToRead;

			while (leftToRead > 0) {
				leftToRead -= stream.Read (JpegData, bytesToRead - leftToRead, leftToRead);
				yield return null;
			}

			MemoryStream ms = new MemoryStream(JpegData, 0, bytesToRead, false, true);

			texture.LoadImage (ms.GetBuffer ());
			frame.material.mainTexture = texture;
			stream.ReadByte(); // CR after bytes
			stream.ReadByte(); // LF after bytes
		}
	}

	int FindLength(Stream stream)  {
		int b;
		string line="";
		int result=-1;
		bool atEOL=false;

		while ((b=stream.ReadByte())!=-1) {
			if (b==10) continue; // ignore LF char
			if (b==13) { // CR
				if (atEOL) {  // two blank lines means end of header
					stream.ReadByte(); // eat last LF
					return result;
				}
				if (line.StartsWith("Content-Length:")) {
					result=Convert.ToInt32(line.Substring("Content-Length:".Length).Trim());
				} else {
					line="";
				}
				atEOL=true;
			} else {
				atEOL=false;
				line+=(char)b;
			}
		}
		return -1;
	}
}
