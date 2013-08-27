using UnityEngine;
using System.Collections;

public class MainMenuActions : MonoBehaviour {

    private GameScript GS;

    void Start () {
        GS = GameObject.Find ( "GameLogic" ).GetComponent<GameScript> ();
    }

    void OnMouseUpAsButton () {
        switch ( gameObject.name ) {
            case "StartBtn":
                GS.setState ( GameScript.GameState.GetReady );
                GS.HideFadeMainMenu();
                break;
            case "AboutBtn":
                GS.setState ( GameScript.GameState.About );
                GS.HideMainMenu();
                GS.ShowCredits();
                break;
            case "BackBtn":
                GS.setState ( GameScript.GameState.MainMenu );
                GS.HideCredits();
                GS.ShowMainMenu();
                break;
            case "ExitBtn":
                Application.Quit();
                break;
        }
    }
}
