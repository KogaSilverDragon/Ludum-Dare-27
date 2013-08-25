﻿using System;
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

    private bool hitted = false;
    private bool gotHit = false;
    
    private int maxEnemyAttacks = 3;
    private float numEnemyAttacks = 0;
    private List<float> enemyAttackTimes = new List<float>();

    public GameObject Player;
    public GameObject Enemy;

    public GUIText StateText;
    public GUIText TimeText;

	private void Start ()
	{
	    Player = GameObject.Find("Player");
	    Enemy = GameObject.Find("Enemy");

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

                    if (Input.GetKeyDown(KeyCode.Space) &&
                        hitted == false &&
                        gotHit == false &&
                        Player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Guard)
                    {
                        Player.GetComponent<PlayerScript>().Attack();
                    }

                    // Change to an collision detect
                    if (hitted == false &&
                        Player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Attack &&
                        Enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Prepare)
                    {
                        hitted = true;
                    }

                    // Change to an collision detect
                    if (gotHit == false &&
                        Player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Prepare &&
                        Enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Attack)
                    {
                        gotHit = true;
                    }

                    if (hitted == true &&
                        Player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Guard &&
                        Enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Guard)
                    {
                        state = GameState.Victory;
                        TimeText.gameObject.SetActive(false);
                        StateText.text = "VICTORY!";
                    }

                    if (gotHit == true &&
                        Player.GetComponent<PlayerScript>().GetState() == PlayerScript.PlayerState.Guard &&
                        Enemy.GetComponent<EnemyScript>().GetState() == EnemyScript.EnemyState.Guard)
                    {
                        state = GameState.GameOver;
                        TimeText.gameObject.SetActive(false);
                        StateText.text = "GAME OVER!";
                    }
                }

                break;

            case GameState.Victory:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    state = GameState.GetReady;
                    StateText.text = "Get Ready!";
                }
                break;

            case GameState.GameOver:
                if (Input.GetKeyDown(KeyCode.Return))
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
}
