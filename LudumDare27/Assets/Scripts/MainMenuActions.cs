using UnityEngine;
using System.Collections;

public class MainMenuActions : MonoBehaviour {

    void OnMouseUpAsButton () {
        switch ( gameObject.name ) {
            case "Start":
            Debug.Log ( "Wololoo" );
            break;
            case "Help":
            Debug.Log ( "HUEHUE" );
            break;
            case "About":
            Debug.Log ( "Yadda yadda" );
            break;
        }
    }
}
