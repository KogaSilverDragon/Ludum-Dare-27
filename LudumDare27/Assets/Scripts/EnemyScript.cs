using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    public enum EnemyState
    {
        Idle,
        Guard,
        Prepare, // Vunerable
        Attack,
        Dying,
        Dead
    }

    private const float PREPARE_TIME = 0.75f;
    private const float GUARD_TIME = 0.5f;
    private const float ATTACK_TIME = 0.5f; // Change to an animation time
    private const float DYING_TIME = 1.0f;

    private EnemyState state = EnemyState.Guard;
    private GameScript gs;

    private float timer = 0.0f;

	void Start ()
	{
        gs = GameObject.Find ( "GameLogic" ).GetComponent<GameScript> ();
	}
	
	void Update ()
    {
	    switch (state)
	    {
            case EnemyState.Idle:
                break;
            case EnemyState.Guard:
                timer -= Time.deltaTime;
                gameObject.renderer.material.mainTexture = gs.enemyDef;
                if ( timer <= 0.0f ) {
                    state = EnemyState.Idle;
                }
                break;

	        case EnemyState.Prepare:
	            timer -= Time.deltaTime;
                gameObject.renderer.material.mainTexture = gs.enemyPrepare;
	            if (timer <= 0.0f)
	            {
	                state = EnemyState.Attack;
	                timer = ATTACK_TIME;
	            }
	            break;

	        case EnemyState.Attack:
	            timer -= Time.deltaTime;
                gameObject.renderer.material.mainTexture = gs.enemyDash;
	            if (timer <= 0.0f)
	            {
	                state = EnemyState.Idle;
	            }
	            break;

            case EnemyState.Dying:
                gameObject.renderer.material.mainTexture = gs.enemyDying;
                timer -= Time.deltaTime;
                if ( timer <= 0.0f ) {
                    state = EnemyState.Dead;
                }
                break;

            case EnemyState.Dead:
                gameObject.renderer.material.mainTexture = gs.enemyDead;
                break;
	    }
    }

    public EnemyState GetState()
    {
        return state;
    }

    public void Attack()
    {
        state = EnemyState.Prepare;
        timer = PREPARE_TIME;
    }

    public void Guard () {
        state = EnemyState.Guard;
        timer = GUARD_TIME;
    }

    public void Die () {
        state = EnemyState.Dying;
        timer = DYING_TIME;
    }
}
