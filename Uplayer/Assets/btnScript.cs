using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class btnScript : MonoBehaviour {
	//string urlOfTheWebPage;
	WWW www;
	string url = "http://compass.surface.com/assets/67/8a/678abbb4-49dd-4d7d-a50e-0b6c500d06f1.mp4?n=Troy_web_spin_black_md.mp4";

	string urlOfTheWebPage;
	string linksFilted;

	// Use this for initialization
	IEnumerator Start () {

		yield return null;
	}

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

	// Update is called once per frame
	void Update () {

	}

	void collectVideoLinksFromSource(string _htmlSource){
		
		using (WebClient client = new WebClient())
		{
			linksFilted = client.DownloadString(_htmlSource);
		}
		
		
		
		Regex linkParser=new Regex(@"\b(?:https?://|www\.)\S+.(avi|AVI|wmv|WMV|flv|FLV|mpg|MPG|mp4|MP4)+\b");
		
		
		
		
		
		using (StreamWriter sw = new StreamWriter("Assets/htmlSource.txt")) 
		{
			Debug.Log("StartToWrite");
			// Add some text to the file.
			sw.WriteLine("HTML Source Log");
			sw.WriteLine("-------------------");
			
			foreach (Match m in linkParser.Matches(linksFilted)) {
				sw.WriteLine(m.Value);	
			}
			
			//yield return null;
			//	yield return StartCoroutine("Download");
			Debug.Log("finishWriting");
		}
		
	}

	public void playTheVideo(){
		PlayMovie.theMovie.Play();
		PlayMovie.theAudio.Play ();


	}



	public void analyizeUrlBtnFunc(){
		if (GameObject.Find("InputField").GetComponent<InputField>().text!="") {
			urlOfTheWebPage=GameObject.Find("InputField").GetComponent<InputField>().text;
			collectVideoLinksFromSource(urlOfTheWebPage);
			Debug.Log("successfully analyzed the link");
		}



	}


}
