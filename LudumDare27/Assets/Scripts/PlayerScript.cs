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

    private const float PREPARE_TIME = 0.25f;
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
                }
                break;

            case PlayerState.Prepare:
                gameObject.renderer.material.mainTexture = gs.playerPrepare;
                timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    state = PlayerState.Attack;
                    timer = ATTACK_TIME;
                }
                break;

            case PlayerState.Attack:
                gameObject.renderer.material.mainTexture = gs.playerDash;
                timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    state = PlayerState.Idle;
                }
                break;

            case PlayerState.Dying:
                gameObject.renderer.material.mainTexture = gs.playerDying;
                timer -= Time.deltaTime;
                if ( timer <= 0.0f ) {
                    state = PlayerState.Dead;
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
    }

    public void Guard () {
        state = PlayerState.Guard;
        timer = GUARD_TIME;
    }

    public void Die () {
        state = PlayerState.Dying;
        timer = DYING_TIME;
    }

}
