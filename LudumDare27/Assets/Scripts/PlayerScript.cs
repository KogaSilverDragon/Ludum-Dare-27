using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public enum PlayerState
    {
        Walk, // Scene Transition
        Idle,
        Guard,
        Prepare, // Vunerable
        Attack,
        Dying,
        Dead
    }

    private const float PREPARE_TIME = 0.4f;
    private const float GUARD_TIME = 0.5f;
    private const float ATTACK_TIME = 0.5f; // Change to an animation time
    private const float DYING_TIME = 1.0f;

    private PlayerState state = PlayerState.Idle;
    private GameScript gs;

    private float timer = 0.0f;

    private void Start()
    {
        gs = GameObject.Find("GameLogic").GetComponent<GameScript> ();
    }
	
    private void Update()
    {
        switch (state)
        {
            case PlayerState.Walk:
            
                break;
            case PlayerState.Idle:
                
                break;

            case PlayerState.Guard:
                timer -= Time.deltaTime;
                gameObject.renderer.material.mainTexture = gs.playerDef;
                if ( timer <= 0.0f ) {
                    state = PlayerState.Idle;
                    gs.Player.width = 100f;
                    gs.Player.height = 160f;
                }
                break;

            case PlayerState.Prepare:
                gameObject.renderer.material.mainTexture = gs.playerPrepare;
                timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    state = PlayerState.Attack;
                    timer = ATTACK_TIME;
                    gs.Player.width = 140f;
                    gs.Player.height = 140f;
                }
                break;

            case PlayerState.Attack:
                gameObject.renderer.material.mainTexture = gs.playerDash;
                timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    state = PlayerState.Idle;
                    gs.Player.width = 100f;
                    gs.Player.height = 160f;
                }
                break;

            case PlayerState.Dying:
                gameObject.renderer.material.mainTexture = gs.playerDying;
                timer -= Time.deltaTime;
                if ( timer <= 0.0f ) {
                    state = PlayerState.Dead;
                    gs.Player.width = 200f;
                    gs.Player.height = 100f;
                }
                break;

            case PlayerState.Dead:
                gameObject.renderer.material.mainTexture = gs.playerDead;
                break;
        }
    }

    public PlayerState GetState()
    {
        return state;
    }

    public void Attack () {
        state = PlayerState.Prepare;
        timer = PREPARE_TIME;
        gs.Player.width = 100f;
        gs.Player.height = 160f;
    }

    public void Guard () {
        state = PlayerState.Guard;
        timer = GUARD_TIME;
        gs.Player.width = 100f;
        gs.Player.height = 160f;
    }

    public void Die () {
        state = PlayerState.Dying;
        timer = DYING_TIME;
        gs.Player.width = 160f;
        gs.Player.height = 120f;
    }

    public void Reset () {
        state = PlayerState.Idle;
        gs.Player.width = 100f;
        gs.Player.height = 160f;
    }

}
