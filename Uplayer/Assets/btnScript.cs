using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class btnScript : MonoBehaviour {
	string htmlCode;
	// Use this for initialization
	void Start () {
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
