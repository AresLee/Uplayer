using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class btnScript : MonoBehaviour {
	//string urlOfTheWebPage;
	WWW www;
	string url = "http://compass.surface.com/assets/67/8a/678abbb4-49dd-4d7d-a50e-0b6c500d06f1.mp4?n=Troy_web_spin_black_md.mp4";

	string urlOfTheWebPage;
	string linksFilted;
	List<string> videoLinks;     //stores a list of video links collected from html source of a web page
	List<string> videoNameDisplayGroups;    //stores a list of file names of the videos
	List<string> downloadedVideoNames;     //stores a list of file names that have already been downloaded
	PlayMovie objectFromPlayMovieClass;


	// Use this for initialization
	IEnumerator Start () {
		objectFromPlayMovieClass = new PlayMovie ();

		//initialize the Lists
		videoLinks = new List<string> ();
		videoNameDisplayGroups = new List<string> ();
		downloadedVideoNames = new List<string> ();
		for (int i = 0; i < 6; i++) {

			Text currentText=GameObject.Find("item"+i+"_text").GetComponent<Text>();
			currentText.text=(i+1)+". "+"Empty";

			currentText.transform.FindChild("Button").gameObject.SetActive(false);
		}

		string[] downloadedVideoArray=Directory.GetFiles(Application.persistentDataPath);
		
	
		//initialize the downloaded list
		foreach (string d in downloadedVideoArray) {
			downloadedVideoNames.Add(Path.GetFileName(d));
		}


		yield return null;
	}

	IEnumerator Download(string _url,string _fileName,Text _tempFileNameText) {
		//		// Start a download of the given URL
		//
		bool isDownloaded = false; 
		www = new WWW(_url);
		
		// Displaying the downloading progress
		while (!isDownloaded) {
			Debug.Log((int)(www.progress*100)+"%");
			string oldFileName=_tempFileNameText.text;
			_tempFileNameText.text="Downloading: "+(int)(www.progress*100)+"%";

			if (www.progress==1) {


				_tempFileNameText.text="downloaded";
				Text tempBtnText=_tempFileNameText.gameObject.GetComponentInChildren<Button>().GetComponentInChildren<Text>();
				tempBtnText.text="Watch";
				isDownloaded=true;

			}
			
			yield return new WaitForEndOfFrame();
		}


		yield return new WaitForEndOfFrame ();
		string fileName = Application.persistentDataPath+"/" + _fileName;
		Debug.Log (fileName);
		System.IO.File.WriteAllBytes (fileName, www.bytes);
		
		//need to update the downloaded list here
		
		
	}

	public void downloadBtnFunc(){
	
	//	StartCoroutine("Download");

		//detect which download button clicked
		EventSystem eventSystem;
		eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		//value returned based on the index included in the name of the item selected. It is a walk around solusion because Unity doesn't provide listview
		string resultString=Regex.Match(eventSystem.currentSelectedGameObject.gameObject.transform.parent.name, @"\d+").Value;
		int indexOfButtonPressed = int.Parse (resultString);
		Debug.Log (indexOfButtonPressed);

		if (eventSystem.currentSelectedGameObject.gameObject.GetComponentInChildren<Text> ().text == "Download") {
			Debug.Log ("StartDownloading!!!!");

			Text tempFileNameText = eventSystem.currentSelectedGameObject.gameObject.transform.parent.GetComponent<Text> ();

			StartCoroutine (Download (videoLinks [indexOfButtonPressed], videoNameDisplayGroups [indexOfButtonPressed], tempFileNameText));

			if (eventSystem.currentSelectedGameObject.gameObject.transform.parent.gameObject.GetComponent<Text> ().text == "downloaded") {
				eventSystem.currentSelectedGameObject.gameObject.GetComponentInChildren<Text> ().text = "Watch";
			}
		} else {

		//	objectFromPlayMovieClass.loadVideoAfterDownloading("file://" + Application.persistentDataPath + "/"+videoNameDisplayGroups [indexOfButtonPressed]);

			PlayMovie test =PlayMovie.selfObject;

			test.loadVideoAfterDownloading("file://" + Application.persistentDataPath + "/"+videoNameDisplayGroups [indexOfButtonPressed]);
			PlayMovie.theMovie.Play();
			PlayMovie.theAudio.Play ();
		}
		// if 


	}

	// Update is called once per frame
	void Update () {

	}

	void collectVideoLinksFromSource(string _htmlSource){

		//split video links from html source of an unknown webpage.
		using (WebClient client = new WebClient()) {
			linksFilted = client.DownloadString (_htmlSource);
		}

		Regex linkParser = new Regex (@"\b(?:https?://|www\.)\S+.(avi|AVI|wmv|WMV|flv|FLV|mpg|MPG|mp4|MP4|ogv|OGV)+\b");

		//store the video links gotten to an text file for debugging; also store them to the videoLinks List
		using (StreamWriter sw = new StreamWriter("Assets/htmlSource.txt")) 
		{
			Debug.Log("StartToWrite");
			// Add some text to the file.
			sw.WriteLine("HTML Source Log");
			sw.WriteLine("-------------------");
			
			foreach (Match m in linkParser.Matches(linksFilted)) {
				sw.WriteLine(m.Value);	//add a video link to the text file
				if (!videoLinks.Contains(m.Value))
				videoLinks.Add(m.Value);  //add the link to the videoLinks List

			}
			
			string[] downloadedVideoArray=Directory.GetFiles(Application.persistentDataPath);

			sw.WriteLine("Downloaded Videos");
			sw.WriteLine("-------------------");
			foreach (string d in downloadedVideoArray) {
				sw.WriteLine(Path.GetFileName(d));

			}
			//	yield return StartCoroutine("Download");
			Debug.Log("finishWriting");
		}
		
	}

	public void playTheVideo(){
		PlayMovie.theMovie.Play();
		PlayMovie.theAudio.Play ();
	}



	public void analyizeUrlBtnFunc(){
		//release resources
		videoLinks.Clear();     
		videoNameDisplayGroups.Clear();    
		downloadedVideoNames.Clear();     

		if (GameObject.Find("InputField").GetComponent<InputField>().text!="") {
			//analyze the link inputed
			urlOfTheWebPage=GameObject.Find("InputField").GetComponent<InputField>().text;
			collectVideoLinksFromSource(urlOfTheWebPage);

			//convert the links to file names and display them on the list
			putLinksToTheList();
			Debug.Log("successfully analyzed the link");
		}
	}

	void putLinksToTheList(){
		foreach (string v in videoLinks) {
			//get the file name from the link
			string fileName=Path.GetFileName(v);
			videoNameDisplayGroups.Add(fileName);
		}

		Debug.Log ("videoNameDisplayGroups Count:" + videoNameDisplayGroups.Count);
		//update the display list
		for (int i = 0; i < 6; i++) {
			if(i<videoNameDisplayGroups.Count){

				Text currentText=GameObject.Find("item"+i+"_text").GetComponent<Text>();
				currentText.text=(i+1)+". "+videoNameDisplayGroups[i];

				//dynamic text display on download button
				currentText.transform.FindChild("Button").gameObject.SetActive(true);
				Text textOnDownloadBtn=currentText.transform.FindChild("Button").transform.FindChild("Text").GetComponent<Text>();
				if (downloadedVideoNames.Contains(videoNameDisplayGroups[i])) {
					textOnDownloadBtn.text="Watch";
				}else{
					textOnDownloadBtn.text="Download";
				}
			}

		}



	}



}
