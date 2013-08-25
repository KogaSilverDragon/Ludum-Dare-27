using UnityEngine;
using System.Collections;

public class OrthoCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Camera.main.camera.isOrthoGraphic = true;
        Camera.main.orthographicSize = Screen.height / 2;
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.orthographicSize = Screen.height / 2;
	}
}
