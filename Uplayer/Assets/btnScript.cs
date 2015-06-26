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

	WWW www;
	string urlOfTheWebPage;
	string linksFilted;
	List<string> videoLinks;     //stores a list of video links collected from html source of a web page
	List<string> videoNameDisplayGroups;    //stores a list of file names of the videos
	List<string> downloadedVideoNames;     //stores a list of file names that have already been downloaded
//	PlayMovie objectFromPlayMovieClass;
	Canvas canvasA;

	bool isFirstWatch;

	// Use this for initialization
	IEnumerator Start () {
		isFirstWatch = true;
		Text infoText = GameObject.Find ("CanvasB").transform.Find("InfoText").GetComponent<Text> ();
		infoText.text = "Download Folder: \tfile://" + Application.persistentDataPath + "/";
		 canvasA = GameObject.Find ("CanvasA").GetComponent<Canvas> ();

	
		//initialize the Lists
		videoLinks = new List<string> ();
		videoNameDisplayGroups = new List<string> ();
		downloadedVideoNames = new List<string> ();

			Text currentText=GameObject.Find("item0_text").GetComponent<Text>();
			currentText.text="File Name: Empty";
			currentText.transform.FindChild("Button").gameObject.SetActive(false);

		string[] downloadedVideoArray=Directory.GetFiles(Application.persistentDataPath);
		
	
		//initialize the downloaded list
		foreach (string d in downloadedVideoArray) {
			downloadedVideoNames.Add(Path.GetFileName(d));
		}


		yield return new WaitForEndOfFrame();
	}

	IEnumerator Download(string _url,string _fileName,Text _tempFileNameText) {
		// Start a download of the given URL

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

		//write the file downloaded to the disk
		string fileName = Application.persistentDataPath+"/" + _fileName;
		Debug.Log (fileName);
		System.IO.File.WriteAllBytes (fileName, www.bytes);

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
			
			PlayMovie tempObj =GameObject.Find("Plane").GetComponent<PlayMovie>();
			
			tempObj.loadVideoAfterDownloading("file://" + Application.persistentDataPath + "/"+videoNameDisplayGroups [0]);
			
			if (tempObj.theMovie.isReadyToPlay) {
				tempObj.theMovie.Play();
				tempObj.theAudio.Play ();
			}
		}
	
		
	}
	

	// Update is called once per frame
	void Update () {

	}
	

	public void analyizeUrlBtnFunc(){
		videoLinks.Clear();     
		videoNameDisplayGroups.Clear();    
		
		if (GameObject.Find("InputField").GetComponent<InputField>().text!="") {
			//analyze the link inputed/ simplely store the link inputed for this time
			urlOfTheWebPage=GameObject.Find("InputField").GetComponent<InputField>().text;

			videoLinks.Add(urlOfTheWebPage);
		
			//convert the links to file names and display them on the list
			putLinksToTheList();
			Debug.Log("successfully analyzed the link");
		}
	}

	void putLinksToTheList(){
		foreach (string v in videoLinks) {
			//get the file name from the link. supports multiple videos soon
			string fileName=Path.GetFileName(v);
			videoNameDisplayGroups.Add(fileName);
		}
		
		Debug.Log ("videoNameDisplayGroups Count:" + videoNameDisplayGroups.Count);
		//update the display list

				Text currentText=GameObject.Find("item0_text").GetComponent<Text>();
				currentText.text=". "+videoNameDisplayGroups[0];
				
				//dynamic text display on download button
				currentText.transform.FindChild("Button").gameObject.SetActive(true);
				Text textOnDownloadBtn=currentText.transform.FindChild("Button").transform.FindChild("Text").GetComponent<Text>();
				if (downloadedVideoNames.Contains(videoNameDisplayGroups[0])) {
					textOnDownloadBtn.text="Watch";
					Debug.Log("once watched");
				}else{
					textOnDownloadBtn.text="Download";
				}
			
		
		
		
	}

	public void panelSwitchBtnFunc(){

		if (canvasA.gameObject.activeInHierarchy) {
			canvasA.gameObject.SetActive(false);
			GameObject.Find("panelSwitch").GetComponentInChildren<Text>().text="Panel On" ;
		

		}else{
			canvasA.gameObject.SetActive(true);
			GameObject.Find("panelSwitch").GetComponentInChildren<Text>().text="Panel Off" ;
	
		}


	}

	public void restartBtnFunc(){
		Material videoMaterial = Resources.Load ("videoMaterial") as Material;
		videoMaterial.color = Color.white;
		Application.LoadLevel (0);
	}





}
