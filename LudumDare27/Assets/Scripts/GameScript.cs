using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class GameScript : MonoBehaviour
{

    public enum GameState
    {
        MainMenu,
        Help,
        About,
        Pause,
        Transition,
        GetReady,
        Battle,
        Victory,
        GameOver
    }

    private const float TURN_TIME = 10.0f;

    private GameState state = GameState.MainMenu;
    private float timer = 0.0f;

    private bool hitted = false;
    private bool gotHit = false;
    
    private int maxEnemyAttacks = 3;
    private float numEnemyAttacks = 0;
    private List<float> enemyAttackTimes = new List<float>();

    public Sprite Player, Enemy, BG, BlurBG, Battle, Victory, GameOver, MainMenu, StartGame, About, Exit, Credit, Back;
    public Vector3 startPlayerPos, startEnemyPos, MainMenuPos, StartGameOff, AboutOff, ExitOff, CreditPos, BackOff;
    private SpriteManager spriteManager;

    public GUIText TimeText;

    public Texture2D playerIdle, playerPrepare, playerDash, playerDef, playerHit, playerJump, playerDying, playerDead,
                     enemyIdle, enemyPrepare, enemyDash, enemyDef, enemyHit, enemyJump, enemyDying, enemyDead,
                     bg, blurbg, sword, battleSign, gameOverSign, victorySign, mainMenu, credit, backBtn, transTex;

    public AudioClip menuBGM, creditBGM, startBattleBGM, victoryBGM, gameOverBGM,
                     hitSFX, attackSFX, defSFX, prepareSFX, playerDieSFX, enemyDieSFX, buttonSFX, currentSong;

	private void Start ()
	{
        spriteManager = GetComponent<SpriteManager> ();

        startPlayerPos = new Vector3 ( 200f, Screen.height * 0.8f, 1f );
        startEnemyPos = new Vector3 ( Screen.width - 200f, Screen.height * 0.8f, 1f );

        MainMenuPos = new Vector3 ( Screen.width * 0.5f, 180f, 0.4f );
        StartGameOff = new Vector3 ( 0, -40f, -0.1f);
        AboutOff = new Vector3 ( 0, 40f, -0.1f );
        ExitOff = new Vector3 ( 0, 120f, -0.1f );

        CreditPos = new Vector3 ( Screen.width * 0.5f -10f, 350f, 0.4f );
        BackOff = new Vector3 ( Screen.width * 0.5f - 50f, -320f, 0f );

        Player = spriteManager.AddSprite ( 100f, 160f, startPlayerPos, Quaternion.identity, "Player", playerIdle, "PlayerScript", true, true );
        Enemy = spriteManager.AddSprite ( 100f, 160f, startEnemyPos, Quaternion.identity, "Enemy", enemyIdle, "EnemyScript", true, false );
        BG = spriteManager.AddSprite ( Screen.height * 3.2f, Screen.height, new Vector3 ( Screen.height * 3.2f * 0.5f, Screen.height * 0.5f, 2f ), Quaternion.identity, "Background", bg, null, false, false );
        BlurBG = spriteManager.AddSprite ( Screen.height * 3.2f, Screen.height, new Vector3 ( Screen.height * 3.2f * 0.5f, Screen.height * 0.5f, 0.5f ), Quaternion.identity, "BlurBackground", blurbg, null, true, false );
        Battle = spriteManager.AddSprite ( 512f, 512f, new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ), Quaternion.identity, "BattleSign", battleSign, null, true, false );
        GameOver = spriteManager.AddSprite ( 512f, 512f, new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ), Quaternion.identity, "GameOverSign", gameOverSign, null, true, false );
        Victory = spriteManager.AddSprite ( 512f, 512f, new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ), Quaternion.identity, "VictorySign", victorySign, null, true, false );

        MainMenu = spriteManager.AddSprite ( 512f, 512f, new Vector3 ( Screen.width * 0.5f, -512f, 0.4f ), Quaternion.Euler ( 0f, 0f, 0f ), "MainMenu", mainMenu, null, true, false );
        StartGame = spriteManager.AddSprite ( 100f, 40f, MainMenuPos + StartGameOff, Quaternion.Euler ( 0f, 0f, 0f ), "StartBtn", transTex, "MainMenuActions", true, false );
        About = spriteManager.AddSprite ( 100f, 40f, MainMenuPos + AboutOff, Quaternion.Euler ( 0f, 0f, 0f ), "AboutBtn", transTex, "MainMenuActions", true, false );
        Exit = spriteManager.AddSprite ( 100f, 40f, MainMenuPos + ExitOff, Quaternion.Euler ( 0f, 0f, 0f ), "ExitBtn", transTex, "MainMenuActions", true, false );

        Credit = spriteManager.AddSprite ( 600f, 900f, new Vector3 ( Screen.width * 0.5f, -512f, 0.4f ), Quaternion.Euler ( 0f, 0f, 0f ), "Credit", credit, null, true, false );
        Back = spriteManager.AddSprite ( 100f, 50f, CreditPos + BackOff, Quaternion.Euler ( 0f, 0f, 0f ), "BackBtn", backBtn, "MainMenuActions", true, false );

        currentSong = menuBGM;

        TimeText.gameObject.SetActive(false);
        ShowMainMenu ();
	}
	
    private void Update()
    {
        switch (state)
        {
            case GameState.MainMenu:

            break;

            case GameState.Pause:
            break;

            case GameState.Transition:
                break;

            case GameState.GetReady:
                ResetBattle();
                state = GameState.Battle;
                TimeText.gameObject.SetActive(true);
                ShowBattle ();
                TimeText.color = Color.black;
                HOTween.Kill ( TimeText );
                HOTween.To ( TimeText, 10f, new TweenParms ()
                    .Prop ( "color", Color.red )
                    .Ease ( EaseType.Linear )
                );
                break;

            case GameState.Battle:
                timer -= Time.deltaTime;

                TimeText.text = timer.ToString("0.00");

                if (timer <= 0.0f &&
                    Player.go.GetComponent<PlayerScript>().GetState() != PlayerScript.PlayerState.Prepare &&
                    Player.go.GetComponent<PlayerScript> ().GetState () != PlayerScript.PlayerState.Attack )
                {
                    state = GameState.GameOver;
                    TimeText.gameObject.SetActive(false);
                    HOTween.To ( Battle, 1f, new TweenParms ()
                        .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ) )
                        .Ease ( EaseType.EaseInOutBack )
                        .OnComplete ( ShowGameOver )
                    );
                }
                else
                {
                    if (enemyAttackTimes.Count > 0)
                    {
                        float time = enemyAttackTimes[0];

                        if (timer <= time &&
                            numEnemyAttacks < maxEnemyAttacks &&
                            Enemy.go.GetComponent<EnemyScript> ().GetState () == EnemyScript.EnemyState.Idle )
                        {
                            Enemy.go.GetComponent<EnemyScript> ().Attack ();
                            Enemy.go.audio.PlayOneShot ( prepareSFX );
                            enemyAttackTimes.RemoveAt(0);
                            numEnemyAttacks++;
                        }
                    }

                    if (Input.GetButtonDown("Attack") &&
                        hitted == false &&
                        gotHit == false &&
                        Player.go.GetComponent<PlayerScript> ().GetState () == PlayerScript.PlayerState.Idle )
                    {
                        Player.go.GetComponent<PlayerScript> ().Attack ();
                        Player.go.audio.PlayOneShot ( prepareSFX );
                    }

                    // Change to an collision detect
                    if ( hitted == false &&
                        Player.go.GetComponent<PlayerScript> ().GetState () == PlayerScript.PlayerState.Attack &&
                        Enemy.go.GetComponent<EnemyScript> ().GetState () == EnemyScript.EnemyState.Prepare ) {
                        hitted = true;
                        Player.go.audio.PlayOneShot ( hitSFX );
                    } 
                    
                    if ( hitted == false &&
                        Player.go.GetComponent<PlayerScript> ().GetState () == PlayerScript.PlayerState.Attack &&
                        Enemy.go.GetComponent<EnemyScript> ().GetState () == EnemyScript.EnemyState.Idle )
                    {
                        Enemy.go.GetComponent<EnemyScript> ().Guard ();
                    }

                    // Change to an collision detect
                    if (gotHit == false &&
                        Player.go.GetComponent<PlayerScript> ().GetState () == PlayerScript.PlayerState.Prepare &&
                        Enemy.go.GetComponent<EnemyScript> ().GetState () == EnemyScript.EnemyState.Attack )
                    {
                        gotHit = true;
                        Enemy.go.audio.PlayOneShot ( hitSFX );
                    }
                    
                    if (gotHit == false &&
                        Player.go.GetComponent<PlayerScript> ().GetState () == PlayerScript.PlayerState.Idle &&
                        Enemy.go.GetComponent<EnemyScript> ().GetState () == EnemyScript.EnemyState.Attack )
                    {
                        Player.go.GetComponent<PlayerScript> ().Guard ();
                    }

                    if (hitted == true &&
                        Player.go.GetComponent<PlayerScript> ().GetState () == PlayerScript.PlayerState.Idle &&
                        Enemy.go.GetComponent<EnemyScript> ().GetState () == EnemyScript.EnemyState.Idle )
                    {
                        Enemy.go.GetComponent<EnemyScript> ().Die ();
                        state = GameState.Victory;
                        TimeText.gameObject.SetActive(false);
                        HOTween.To ( Battle, 1f, new TweenParms ()
                            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ) )
                            .Ease ( EaseType.EaseInOutBack )
                            .OnComplete ( ShowVictory )
                        );
                    }

                    if (gotHit == true &&
                        Player.go.GetComponent<PlayerScript> ().GetState () == PlayerScript.PlayerState.Idle &&
                        Enemy.go.GetComponent<EnemyScript> ().GetState () == EnemyScript.EnemyState.Idle )
                    {
                        Player.go.GetComponent<PlayerScript> ().Die ();
                        state = GameState.GameOver;
                        TimeText.gameObject.SetActive(false);
                        HOTween.To ( Battle, 1f, new TweenParms ()
                            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ) )
                            .Ease ( EaseType.EaseInOutBack )
                            .OnComplete( ShowGameOver )
                        );
                    }
                }

                if ( Input.GetButtonDown ( "MainMenu" ) ) {
                    state = GameState.MainMenu;
                    TimeText.gameObject.SetActive ( false );
                    HideBattle ();
                    ShowMainMenu ();
                }

                break;

            case GameState.Victory:
            case GameState.GameOver:
                TimeText.gameObject.SetActive (false);
                if (Input.GetButtonDown("Reset"))
                {
                    state = GameState.GetReady;
                }
                if ( Input.GetButtonDown ( "MainMenu" ) ) {
                    state = GameState.MainMenu;
                    HideGameOver ();
                    HideVictory ();
                    ShowMainMenu ();
                }
                break;
        }
        StartGame.position = MainMenu.position + StartGameOff;
        About.position = MainMenu.position + AboutOff;
        Exit.position = MainMenu.position + ExitOff;
        Back.position = Credit.position + BackOff;
    }

    private void ResetBattle()
    {
        timer = TURN_TIME;

        TimeText.color = Color.black;

        HOTween.To ( GameOver, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ) )
            .Ease ( EaseType.EaseInOutBack )
        );
        HOTween.To ( Victory, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ) )
            .Ease ( EaseType.EaseInOutBack )
        );

        maxEnemyAttacks = (int)UnityEngine.Random.Range(1, 3);
        numEnemyAttacks = 0;
        hitted = false;
        gotHit = false;

        Enemy.go.GetComponent<EnemyScript> ().Reset ();
        Player.go.GetComponent<PlayerScript> ().Reset ();

        int interval = (int) TURN_TIME / maxEnemyAttacks;

        for (int i = maxEnemyAttacks; i > 0 ; i--)
        {
            enemyAttackTimes.Add(UnityEngine.Random.Range(0, interval) + interval * (i - 1) + 1);
        }
    }

    public void setState ( GameState state ) {
        this.state = state;
    }

    public void ShowBattle () {
        HOTween.To ( Battle, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, 80f, 0.5f ) )
            .Ease ( EaseType.EaseOutElastic )
        );
        Camera.main.audio.loop = false;
        ChangeMusic ( startBattleBGM );
    }

    public void HideBattle () {
        HOTween.To ( Battle, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ) )
            .Ease ( EaseType.EaseInOutBack )
        );
    }

    public void ShowVictory () {
        HOTween.To ( Victory, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, Screen.height * 0.5f, 0.5f ) )
            .Ease ( EaseType.EaseOutElastic )
        );
        Camera.main.audio.loop = false;
        ChangeMusic ( victoryBGM );
    }

    public void HideVictory () {
        HOTween.To ( Victory, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ) )
            .Ease ( EaseType.EaseInOutBack )
        );
    }

    public void ShowGameOver () {
        HOTween.To ( GameOver, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, Screen.height * 0.5f, 0.5f ) )
            .Ease ( EaseType.EaseOutElastic )
        );
        Camera.main.audio.loop = false;
        ChangeMusic ( gameOverBGM );
    }

    public void HideGameOver () {
        HOTween.To ( GameOver, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ) )
            .Ease ( EaseType.EaseInOutBack )
        );
    }

    public void ShowMainMenu () {
        HOTween.To ( MainMenu, 1f, new TweenParms ()
            .Prop ( "position", MainMenuPos )
            .Ease ( EaseType.EaseOutElastic )
        );
        HOTween.To ( BlurBG.go.renderer.material, 1f, new TweenParms ()
            .Prop ( "color", Color.white )
            .Ease ( EaseType.Linear )
        );
        HOTween.To ( Camera.main.audio, 0.5f, new TweenParms ()
            .Prop ( "volume", 1f )
            .Ease ( EaseType.Linear )
        );

        Camera.main.audio.loop = true;
        ChangeMusic ( menuBGM );
    }

    public void HideMainMenu () {
        HOTween.To ( MainMenu, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.4f ) )
            .Ease ( EaseType.EaseInOutBack )
        );
    }

    public void HideFadeMainMenu () {
        HOTween.To ( MainMenu, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.4f ) )
            .Ease ( EaseType.EaseInOutBack )
        );
        HOTween.To ( BlurBG.go.renderer.material, 1f, new TweenParms ()
            .Prop ( "color", Color.clear )
            .Ease ( EaseType.Linear )
        );
    }

    public void ShowCredits () {
        HOTween.To ( Credit, 1f, new TweenParms ()
            .Prop ( "position", CreditPos )
            .Ease ( EaseType.EaseOutElastic )
        );

        Camera.main.audio.loop = true;
        ChangeMusic ( creditBGM );
    }

    public void HideCredits () {
        HOTween.To ( Credit, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f - 50f, -512f, 0.4f ) )
            .Ease ( EaseType.EaseInOutBack )
        );
    }

    public void ChangeMusic (AudioClip audio) {
        currentSong = audio;
        Sequence sequence = new Sequence ();
        sequence.Append ( HOTween.To ( Camera.main.audio, 0.5f, new TweenParms ()
            .Prop ( "volume", 0f )
            .Ease ( EaseType.Linear )
            .OnComplete( SetMusic )
        ));
        sequence.Append ( HOTween.To ( Camera.main.audio, 0.5f, new TweenParms ()
            .Prop ( "volume", 1f )
            .Ease ( EaseType.Linear )
        ));
        sequence.Play ();
    }

    public void SetMusic () {
        Camera.main.audio.clip = currentSong;
        Camera.main.audio.Play ();
    }

}
