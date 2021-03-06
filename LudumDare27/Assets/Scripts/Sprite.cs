﻿using UnityEngine;
using System.Collections;

public class Sprite {

    public float width, height = 0;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 position = Vector3.zero;
    public Texture2D texture;
    public string name = "";
    public bool visible = true;
    public bool flipH = true;
    public GameObject go;

    public Sprite ( float w, float h, Vector3 pos, Quaternion rot, string name, Transform parent, Texture2D tex, string script, bool transparency, bool flipH ) {
        this.position = pos;
        this.width = w;
        this.height = h;
        this.rotation = rot * Quaternion.Euler ( 90f, 180f, 0f );
        this.name = name;
        this.texture = tex;
        this.flipH = flipH;

        this.go = GameObject.CreatePrimitive ( PrimitiveType.Plane );
        this.go.transform.parent = parent;
        if ( transparency ) {
            this.go.renderer.material = (Material)Resources.Load ( "Material/Transparency", typeof ( Material ) );
            this.go.renderer.material.color = Color.white;
        }
        this.go.renderer.material.mainTexture = this.texture;
        this.go.transform.localPosition = new Vector3 ( (float)-Screen.width / 2 + this.position.x, (float)Screen.height / 2 - this.position.y, (float)this.position.z );
        this.go.transform.localScale = new Vector3 ( (float)this.width / 10 * ( this.flipH ? -1f : 1f ), 1f, (float)this.height / 10 );
        this.go.transform.localRotation = this.rotation;
        this.go.name = this.name;
        if ( script != null ) {
            this.go.AddComponent ( script );
        }
    }
	
	public void Update () {
        this.go.transform.localPosition = new Vector3 ( (float)-Screen.width / 2 + this.position.x, (float)Screen.height / 2 - this.position.y, (float)this.position.z );
        this.go.transform.localScale = new Vector3 ( (float)this.width / 10 * ( this.flipH ? -1f : 1f ), 1f, (float)this.height / 10 );
        this.go.transform.localRotation = this.rotation;
        this.go.name = this.name;
        this.go.renderer.material.mainTexture = this.texture;
	}

}
