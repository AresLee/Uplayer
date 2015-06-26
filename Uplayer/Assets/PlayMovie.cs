using UnityEngine;
using System.Collections;



public class PlayMovie : MonoBehaviour {
	public  MovieTexture theMovie;
	public  AudioSource theAudio;
	//public static PlayMovie selfObject;
	Material videoMaterial;

	// Use this for initialization
	void Start () {

	}

	public void loadVideoAfterDownloading(string _url){
		StartCoroutine(loadVideo(_url));
		StartCoroutine (loadAudio (theMovie));
		
	}

	IEnumerator loadAudio(MovieTexture _videoTexture){
		theAudio = null;
		theAudio=GameObject.Find("Plane").GetComponent<AudioSource>();
		theAudio.clip=_videoTexture.audioClip;

		while (!theMovie.isReadyToPlay) {
			yield return null;
		}

		
	}

	IEnumerator loadVideo(string _url){
		//for loading video source dynamically
		WWW www = new WWW (_url);
	
	//	Debug.Log ("file://" + Application.persistentDataPath + "/677PHPforAzureM01_high.mp4");
		videoMaterial = Resources.Load ("videoMaterial") as Material;
		videoMaterial.color = Color.white;
		//videoMaterial.color = new Color(1,0,1);
		theMovie = null;
		theMovie = www.movie;

		while (!theMovie.isReadyToPlay) {
			yield return null;
		}

		videoMaterial.mainTexture = theMovie;

		//yield return new WaitForSeconds (3);
		MeshRenderer mr = gameObject.GetComponent<MeshRenderer> ();
		mr.materials [0] = videoMaterial;
		

	
	}

	IEnumerator wait(){

		yield return new WaitForSeconds(3);
	}


	// Update is called once per frame
	void Update () {

	}


}
