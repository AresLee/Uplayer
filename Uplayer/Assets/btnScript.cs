using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class btnScript : MonoBehaviour {
	string htmlCode;

	string url = "http://compass.surface.com/assets/67/8a/678abbb4-49dd-4d7d-a50e-0b6c500d06f1.mp4?n=Troy_web_spin_black_md.mp4";
	
	IEnumerator Start() {


		yield return StartCoroutine("Download");
	}

	IEnumerator Download() {
		// Start a download of the given URL
		WWW www = new WWW(url);
		
		// Wait for download to complete
		yield return www;
		
		string fileName = @"Assets/" + "video.mp4";
		Debug.Log (fileName);
		System.IO.File.WriteAllBytes (fileName, www.bytes);

		init ();

	
	}



	// Use this for initialization
	void init () {
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
