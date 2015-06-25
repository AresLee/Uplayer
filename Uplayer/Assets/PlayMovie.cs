using UnityEngine;
using System.Collections;



public class PlayMovie : MonoBehaviour {
	public static MovieTexture theMovie;
	public static AudioSource theAudio;

	// Use this for initialization
	void Start () {
		//load the video and its audio component

//		MeshRenderer mr = GetComponent<MeshRenderer> ();
//
//		mr.materials [0].color = Color.blue;

		StartCoroutine("downloadVideo");
		//loadVideo ();
		loadAudio (theMovie);

		//play the video with its audio
//		theMovie.Play();
//		GetComponent<AudioSource>().Play();
	}

	IEnumerator downloadVideo(){
		//for loading video source dynamically
		//WWW www = new WWW ("file://" + Application.persistentDataPath + "/Xbox_Elite_Wireless_Controller.ogv");
		WWW www = new WWW ("http://www.mariowiki.com/images/a/aa/NSMBintro.ogv");
	
		Debug.Log ("file://" + Application.persistentDataPath + "/677PHPforAzureM01_high.mp4");
		Material videoMaterial = Resources.Load ("videoMaterial") as Material;
		videoMaterial.color = Color.white;
		//videoMaterial.color = new Color(1,0,1);

		theMovie = www.movie;

		while (!theMovie.isReadyToPlay) {
			yield return null;
		}

		videoMaterial.mainTexture = theMovie;



		MeshRenderer mr = GetComponent<MeshRenderer> ();
		mr.materials [0] = videoMaterial;
		
		

		yield return null;
	
	}


	// Update is called once per frame
	void Update () {

	}

	void loadVideo(){
		Renderer r = GetComponent<Renderer>();
		theMovie = (MovieTexture)r.material.mainTexture;
		GameObject.Find("Plane").AddComponent<AudioSource>();
	}


	void loadAudio(MovieTexture _videoTexture){

		theAudio=GameObject.Find("Plane").GetComponent<AudioSource>();
		theAudio.clip=_videoTexture.audioClip;

	}
}
