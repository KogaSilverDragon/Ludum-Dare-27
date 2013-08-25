using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

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

    private GameObject Player, Enemy, BG;
    private SpriteManager spriteManager;

    public GUIText StateText;
    public GUIText TimeText;

    public Texture2D playerIdle, playerPrepare, playerDash, playerDef, playerHit, playerJump, playerDying, playerDead,
                     enemyIdle, enemyPrepare, enemyDash, enemyDef, enemyHit, enemyJump, enemyDying, enemyDead,
                     bg, sword;

	private void Start ()
	{
        spriteManager = GetComponent<SpriteManager> ();
        
        Player = spriteManager.AddSprite( 120f, 160f, new Vector3( 200f, Screen.height * 0.8f, 1f), Quaternion.identity, "Player", playerIdle, "PlayerScript", true ).go;
        Enemy = spriteManager.AddSprite ( 120f, 160f, new Vector3 ( Screen.width - 200f, Screen.height * 0.8f, 1f ), Quaternion.identity, "Enemy", playerIdle, "EnemyScript", true ).go;
        BG = spriteManager.AddSprite ( Screen.height * 6f, Screen.height, new Vector3 ( Screen.height * 6f * 0.5f, Screen.height * 0.5f, 2f ), Quaternion.identity, "Background", bg, null, false ).go;

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
                StateText.text = "Battle!!!";
                break;

            case GameState.Battle:
                timer -= Time.deltaTime;

                TimeText.text = ((int)timer + 1).ToString();

                if (timer <= 0.0f &&
                    Player.GetComponent<PlayerScript>().GetState() != PlayerScript.PlayerState.Prepare &&
                    Player.GetComponent<PlayerScript>().GetState() != PlayerScript.PlayerState.Attack)
                {
                    state = GameState.GameOver;
                    TimeText.gameObject.SetActive(false);
                    StateText.text = "GAME OVER!";
                }
                else
                {
                    if (enemyAttackTimes.Count > 0)
                    {
                        float time = enemyAttackTimes[0];

                        if (timer <= time &&
                            numEnemyAttacks < maxEnemyAttacks &&
                            Enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Guard)
                        {
                            Enemy.GetComponent<EnemyScript>().Attack();
                            enemyAttackTimes.RemoveAt(0);
                            numEnemyAttacks++;
                        }
                    }

                    if (Input.GetButtonDown("Attack") &&
                        hitted == false &&
                        gotHit == false &&
                        Player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Idle)
                    {
                        Player.GetComponent<PlayerScript>().Attack();
                    }

                    // Change to an collision detect
                    if ( hitted == false &&
                        Player.GetComponent<PlayerScript> ().GetState () == PlayerScript.PlayerState.Attack &&
                        Enemy.GetComponent<EnemyScript>().GetState () == EnemyScript.EnemyState.Prepare ) {
                        hitted = true;
                    } 
                    
                    if ( hitted == false &&
                        Player.GetComponent<PlayerScript> ().GetState () == PlayerScript.PlayerState.Attack &&
                        Enemy.GetComponent<EnemyScript> ().GetState () == EnemyScript.EnemyState.Idle )
                    {
                        Enemy.GetComponent<EnemyScript>().Guard();
                    }

                    // Change to an collision detect
                    if (gotHit == false &&
                        Player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Prepare &&
                        Enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Attack)
                    {
                        gotHit = true;
                    }
                    
                    if (gotHit == false &&
                        Player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Idle &&
                        Enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Attack)
                    {
                        Player.GetComponent<PlayerScript>().Guard();
                    }

                    if (hitted == true &&
                        Player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Idle &&
                        Enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Idle)
                    {
                        Enemy.GetComponent<EnemyScript>().Die();
                        state = GameState.Victory;
                        TimeText.gameObject.SetActive(false);
                        StateText.text = "VICTORY!";
                    }

                    if (gotHit == true &&
                        Player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Idle &&
                        Enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Idle)
                    {
                        Player.GetComponent<PlayerScript>().Die();
                        state = GameState.GameOver;
                        TimeText.gameObject.SetActive(false);
                        StateText.text = "GAME OVER!";
                    }
                }

                break;

            case GameState.Victory:
                if (Input.GetButtonDown("Reset"))
                {
                    state = GameState.GetReady;
                    StateText.text = "Get Ready!";
                }
                break;

            case GameState.GameOver:
                if ( Input.GetButtonDown("Reset"))
                {
                    state = GameState.GetReady;
                    StateText.text = "Get Ready!";
                }
                break;
        }
    }

    private void ResetBattle()
    {
        timer = TURN_TIME;

        maxEnemyAttacks = (int)UnityEngine.Random.Range(1, 3);
        numEnemyAttacks = 0;
        hitted = false;
        gotHit = false;

        int interval = (int) TURN_TIME / maxEnemyAttacks;

        for (int i = maxEnemyAttacks; i > 0 ; i--)
        {
            enemyAttackTimes.Add(UnityEngine.Random.Range(0, interval) + interval * (i - 1) + 1);
        }
    }

    public void setState ( GameState state ) {
        this.state = state;
    }
}
