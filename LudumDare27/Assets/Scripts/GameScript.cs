using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour
{

    public enum GameState
    {
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

    private GameObject player;
    private bool hitted = false;
    private bool gotHit = false;

    private GameObject enemy;
    private int maxEnemyAttacks = 3;
    private float numEnemyAttacks = 0;
    private List<int> enemyAttackTimes = new List<int>();

    private Camera cam;

	private void Start ()
	{
	    player = GameObject.Find("Player");
	    enemy = GameObject.Find("Enemy");
	    cam = Camera.main;
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
                cam.backgroundColor = Color.red;
                break;

            case GameState.Battle:
                timer -= Time.deltaTime;

                if (timer <= 0.0f &&
                    player.GetComponent<PlayerScript>().GetState() != PlayerScript.PlayerState.Prepare &&
                    player.GetComponent<PlayerScript>().GetState() != PlayerScript.PlayerState.Attack)
                {
                    state = GameState.GameOver;
                    cam.backgroundColor = Color.black;
                }
                else
                {
                    if (timer <= 5.0f &&
                        numEnemyAttacks < maxEnemyAttacks)
                    {
                        enemy.GetComponent<EnemyScript>().Attack();
                        numEnemyAttacks++;
                    }

                    if (Input.GetKeyDown(KeyCode.Space) &&
                        hitted == false &&
                        gotHit == false &&
                        player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Guard)
                    {
                        player.GetComponent<PlayerScript>().Attack();
                    }

                    // Change to an collision detect
                    if (hitted == false &&
                        player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Attack &&
                        enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Prepare)
                    {
                        hitted = true;
                    }

                    // Change to an collision detect
                    if (gotHit == false &&
                        player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Prepare &&
                        enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Attack)
                    {
                        gotHit = true;
                    }

                    if (hitted == true &&
                        player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Guard &&
                        enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Guard)
                    {
                        state = GameState.Victory;
                        cam.backgroundColor = Color.green;
                    }

                    if (gotHit == true &&
                        player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Guard &&
                        enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Guard)
                    {
                        state = GameState.GameOver;
                        cam.backgroundColor = Color.black;
                    }
                }

                break;

            case GameState.Victory:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    state = GameState.GetReady;
                    cam.backgroundColor = Color.yellow;
                }
                break;

            case GameState.GameOver:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    state = GameState.GetReady;
                    cam.backgroundColor = Color.yellow;
                }
                break;
        }
    }

    private void ResetBattle()
    {
        timer = TURN_TIME;
        maxEnemyAttacks = 1;
        numEnemyAttacks = 0;
        hitted = false;
        gotHit = false;

        System.Random rng = new System.Random();

        int interval = (int) TURN_TIME / maxEnemyAttacks;

        for (int i = maxEnemyAttacks; i > 0 ; i--)
        {
            enemyAttackTimes.Add(rng.Next(interval) + (interval + 1) * i);
        }
    }
}
