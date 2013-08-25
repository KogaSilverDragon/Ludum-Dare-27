using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {

    private Dictionary<string, Sprite> sprites = new Dictionary<string,Sprite>();
    public Texture2D mainMenu, transTex;

	// Use this for initialization
	void Start () {
        Camera.main.camera.isOrthoGraphic = true;
        Camera.main.orthographicSize = Screen.height / 2;

        AddSprite ( 512f, 512f, new Vector3 ( Screen.width / 2, 220f, 1.1f ), Quaternion.Euler ( 0f, 0f, 0f ), "MainMenu", mainMenu, null );
        AddSprite ( 100f, 40f, new Vector3 ( Screen.width / 2, 180f, 1f ), Quaternion.Euler ( 0f, 0f, 0f ), "Start", transTex, "MainMenuActions" );
        AddSprite ( 100f, 40f, new Vector3 ( Screen.width / 2, 260f, 1f ), Quaternion.Euler ( 0f, 0f, 0f ), "Help", transTex, "MainMenuActions" );
        AddSprite ( 100f, 40f, new Vector3 ( Screen.width / 2, 340f, 1f ), Quaternion.Euler ( 0f, 0f, 0f ), "About", transTex, "MainMenuActions" );
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.orthographicSize = Screen.height / 2;

        foreach ( var item in sprites ) {
            item.Value.Update ();
        }
	}

    public Sprite AddSprite ( float w, float h, Vector3 pos, Quaternion rot, string name, Texture2D tex,string script ) {
        Sprite dialog = new Sprite ( w, h, pos, rot, name, Camera.main.transform, tex, script );
        sprites.Add ( name, dialog );
        return dialog;
    }

    public void RemoveSprite ( string name ) {
        GameObject.Destroy ( sprites[ name ].go );
        sprites.Remove ( name );
    }

}
