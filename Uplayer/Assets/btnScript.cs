using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class btnScript : MonoBehaviour {
	string htmlCode;
	WWW www;
	string url = "http://compass.xbox.com/assets/bb/f4/bbf40fe6-3e14-470b-92fb-71a6734eb715.mp4?n=Troy_WebVideo_Paddles.mp4";
	
//	IEnumerator Start() {
//
//
//		yield return StartCoroutine("Download");
//		init ();
//	}

	IEnumerator Download() {
//		// Start a download of the given URL
//
		bool isDownloaded = false; 
		www = new WWW(url);


		while (!isDownloaded) {
			Debug.Log((int)(www.progress*100)+"%");


			if (www.progress==1) {
				isDownloaded=true;
			}

			yield return new WaitForEndOfFrame();
		}
//
//
//		// Wait for download to complete
//		yield return www;


		//yield return www;
		yield return new WaitForEndOfFrame ();
		string fileName = Application.persistentDataPath+"/" + "video.mp4";
		Debug.Log (fileName);
		System.IO.File.WriteAllBytes (fileName, www.bytes);



	
	}



	// Use this for initialization
	IEnumerator Start () {
		//download ();
	//	StartCoroutine("download");
//yield WaitForEndOfFrame
	

		using (WebClient client = new WebClient())
		{
			htmlCode = client.DownloadString("http://www.xbox.com/en-US/xbox-one/accessories/controllers/elite-wireless-controller");


		}


		Regex linkParser=new Regex(@"\b(?:https?://|www\.)\S+.(avi|AVI|wmv|WMV|flv|FLV|mpg|MPG|mp4|MP4)+\b");



	

		using (StreamWriter sw = new StreamWriter("Assets/htmlSource.txt")) 
		{
			Debug.Log("StartToWrite");
			// Add some text to the file.
			sw.WriteLine("HTML Source Log");
			sw.WriteLine("-------------------");

			foreach (Match m in linkParser.Matches(htmlCode)) {
				sw.WriteLine(m.Value);	
			}

			//sw.WriteLine(htmlCode);
			yield return StartCoroutine("Download");
			Debug.Log("finishWriting");
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void playTheVideo(){
		PlayMovie.theMovie.Play();
		PlayMovie.theAudio.Play ();


	}

	void selectVideoLinksFromSource(string _htmlSource){


	}
}
