using UnityEngine;
using System.Collections;

public class Sprite {

    public float width, height = 0;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 position = Vector3.zero;
    public string name = "";
    public bool visible = true;
    public GameObject go;
    
    public Sprite ( float w, float h, Vector3 pos, Quaternion rot, string name, Transform parent ) {
        this.position = pos;
        this.width = w;
        this.height = h;
        this.rotation = rot;
        this.name = name;

        this.go = GameObject.CreatePrimitive ( PrimitiveType.Plane );
        this.go.transform.parent = parent;
        this.go.transform.localPosition = new Vector3 ( (float)-Screen.width / 2 + this.position.x, (float)Screen.height / 2 - this.position.y, (float)this.position.z );
        this.go.transform.localScale = new Vector3 ( (float)this.width / 10, 1f, (float)this.width / 10 );
        this.go.transform.localRotation = this.rotation;
        this.go.name = this.name;
    }
	
	public void Update () {
        this.go.transform.localPosition = new Vector3 ( (float)-Screen.width / 2 + this.position.x, (float)Screen.height / 2 - this.position.y, (float)this.position.z );
        this.go.transform.localScale = new Vector3 ( (float)this.width / 10, 1f, (float)this.width / 10 );
        this.go.transform.localRotation = this.rotation;
        this.go.name = this.name;
	}
}
