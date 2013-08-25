using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {

    private Dictionary<string, Sprite> sprites = new Dictionary<string,Sprite>();

	// Use this for initialization
	void Start () {
        Camera.main.camera.isOrthoGraphic = true;
        Camera.main.orthographicSize = Screen.height / 2;
        
        Sprite mainMenu = AddSprite ( 100f, 100f, new Vector3(60f, 60f, 1f), Quaternion.Euler(-90f, 0f, 0f) , "MainMenu" );
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.orthographicSize = Screen.height / 2;

        foreach ( var item in sprites ) {
            item.Value.Update ();
        }
	}

    public Sprite AddSprite ( float w, float h, Vector3 pos, Quaternion rot, string name ) {
        Sprite dialog = new Sprite (w , h, pos, rot, name, Camera.main.transform);
        sprites.Add ( name, dialog );
        return dialog;
    }

    public void RemoveSprite ( string name ) {
        GameObject.Destroy ( sprites[ name ].go );
        sprites.Remove ( name );
    }

}
