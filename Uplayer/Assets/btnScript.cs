using UnityEngine;
using System.Collections;

public class btnScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playTheVideo(){
		PlayMovie.theMovie.Play();
		PlayMovie.theAudio.Play ();


	}
}
