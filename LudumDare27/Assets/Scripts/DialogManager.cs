using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour {

    public Texture2D mainMenu, transTex;
    private SpriteManager spriteManager;

	// Use this for initialization
	void Start () {
        spriteManager = GetComponent<SpriteManager> ();

        spriteManager.AddSprite ( 512f, 512f, new Vector3 ( Screen.width / 2, 220f, 1.1f ), Quaternion.Euler ( 0f, 0f, 0f ), "MainMenu", mainMenu, null, true );
        spriteManager.AddSprite ( 100f, 40f, new Vector3 ( Screen.width / 2, 180f, 1f ), Quaternion.Euler ( 0f, 0f, 0f ), "Start", transTex, "MainMenuActions", true );
        spriteManager.AddSprite ( 100f, 40f, new Vector3 ( Screen.width / 2, 260f, 1f ), Quaternion.Euler ( 0f, 0f, 0f ), "Help", transTex, "MainMenuActions", true );
        spriteManager.AddSprite ( 100f, 40f, new Vector3 ( Screen.width / 2, 340f, 1f ), Quaternion.Euler ( 0f, 0f, 0f ), "About", transTex, "MainMenuActions", true );
	}
	
	// Update is called once per frame
	void Update () {
	}


}
