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

    private GameState state = GameState.GetReady;
    private float timer = 0.0f;

    private bool hitted = false;
    private bool gotHit = false;
    
    private int maxEnemyAttacks = 3;
    private float numEnemyAttacks = 0;
    private List<float> enemyAttackTimes = new List<float>();

    public Sprite Player, Enemy, BG, Battle, Victory, GameOver;
    public Vector3 startPlayerPos, startEnemyPos;
    private SpriteManager spriteManager;

    public GUIText TimeText;

    public Texture2D playerIdle, playerPrepare, playerDash, playerDef, playerHit, playerJump, playerDying, playerDead,
                     enemyIdle, enemyPrepare, enemyDash, enemyDef, enemyHit, enemyJump, enemyDying, enemyDead,
                     bg, sword, battleSign, gameOverSign, victorySign;

	private void Start ()
	{
        spriteManager = GetComponent<SpriteManager> ();

        startPlayerPos = new Vector3 ( 200f, Screen.height * 0.8f, 1f );
        startEnemyPos = new Vector3 ( Screen.width - 200f, Screen.height * 0.8f, 1f );

        Player = spriteManager.AddSprite ( 100f, 160f, startPlayerPos, Quaternion.identity, "Player", playerIdle, "PlayerScript", true, true );
        Enemy = spriteManager.AddSprite ( 100f, 160f, startEnemyPos, Quaternion.identity, "Enemy", enemyIdle, "EnemyScript", true, false );
        BG = spriteManager.AddSprite ( Screen.height * 3.2f, Screen.height, new Vector3 ( Screen.height * 3.2f * 0.5f, Screen.height * 0.5f, 2f ), Quaternion.identity, "Background", bg, null, false, false );
        Battle = spriteManager.AddSprite ( 512f, 512f, new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ), Quaternion.identity, "BattleSign", battleSign, null, true, false );
        GameOver = spriteManager.AddSprite ( 512f, 512f, new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ), Quaternion.identity, "GameOverSign", gameOverSign, null, true, false );
        Victory = spriteManager.AddSprite ( 512f, 512f, new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ), Quaternion.identity, "VictorySign", victorySign, null, true, false );

        TimeText.gameObject.SetActive(false);
	}
	
    private void Update()
    {
        switch (state)
        {
            case GameState.Pause:
                break;
            
            case GameState.Transition:
                break;

            case GameState.GetReady:
                ResetBattle();
                state = GameState.Battle;
                TimeText.gameObject.SetActive(true);
                ShowBattle ();
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
                    }

                    // Change to an collision detect
                    if ( hitted == false &&
                        Player.go.GetComponent<PlayerScript> ().GetState () == PlayerScript.PlayerState.Attack &&
                        Enemy.go.GetComponent<EnemyScript> ().GetState () == EnemyScript.EnemyState.Prepare ) {
                        hitted = true;
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

                break;

            case GameState.Victory:
            case GameState.GameOver:
                if (Input.GetButtonDown("Reset"))
                {
                    state = GameState.GetReady;
                }
                break;
        }
    }

    private void ResetBattle()
    {
        timer = TURN_TIME;

        TimeText.color = Color.black;

        HOTween.To ( GameOver, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ) )
            .Ease ( EaseType.EaseInOutBack )
            .OnComplete ( ShowBattle )
        );
        HOTween.To ( Victory, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, -512f, 0.5f ) )
            .Ease ( EaseType.EaseInOutBack )
            .OnComplete ( ShowBattle )
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
    }

    public void ShowVictory () {
        HOTween.To ( Victory, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, Screen.height * 0.5f, 0.5f ) )
            .Ease ( EaseType.EaseInOutBack )
        );
    }

    public void ShowGameOver () {
        HOTween.To ( GameOver, 1f, new TweenParms ()
            .Prop ( "position", new Vector3 ( Screen.width * 0.5f, Screen.height * 0.5f, 0.5f ) )
            .Ease ( EaseType.EaseInOutBack )
        );
    }

}
