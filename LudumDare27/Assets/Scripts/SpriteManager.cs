using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour {

    private Dictionary<string, Sprite> sprites = new Dictionary<string,Sprite>();

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        foreach ( var item in sprites ) {
            item.Value.Update ();
        }
	}

    public Sprite AddSprite ( float w, float h, Vector3 pos, Quaternion rot, string name, Texture2D tex, string script, bool transparency, bool flipH ) {
        Sprite sprite = new Sprite ( w, h, pos, rot, name, Camera.main.transform, tex, script, transparency, flipH );
        sprites.Add ( name, sprite );
        return sprite;
    }

    public Sprite GetSprite ( string name ) {
        return sprites[name];
    }

    public void RemoveSprite ( string name ) {
        GameObject.Destroy ( sprites[ name ].go );
        sprites.Remove ( name );
    }

}
