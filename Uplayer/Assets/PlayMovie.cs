using UnityEngine;
using System.Collections;



public class PlayMovie : MonoBehaviour {
	public static MovieTexture theMovie;
	public static AudioSource theAudio;

	// Use this for initialization
	void Start () {
		//load the video and its audio component
		loadVideo ();
		loadAudio (theMovie);

		//play the video with its audio
//		theMovie.Play();
//		GetComponent<AudioSource>().Play();
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
