using UnityEngine;
using System.Collections;
using Holoville.HOTween;

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

    private EnemyState state = EnemyState.Idle;
    private GameScript gs;

    private float timer = 0.0f;

	void Start ()
	{
        gs = GameObject.Find ( "GameLogic" ).GetComponent<GameScript> ();
        gameObject.AddComponent ( "AudioSource" );
        gameObject.audio.volume = 1f;
        gameObject.audio.loop = false;
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
                    gs.Enemy.width = 100f;
                    gs.Enemy.height = 160f;
                }
                break;

	        case EnemyState.Prepare:
	            timer -= Time.deltaTime;
                gameObject.renderer.material.mainTexture = gs.enemyPrepare;
	            if (timer <= 0.0f)
	            {
	                state = EnemyState.Attack;
	                timer = ATTACK_TIME;
                    gs.Enemy.width = 140f;
                    gs.Enemy.height = 140f;
                    HOTween.To ( gs.Enemy, ATTACK_TIME, "position", gs.Player.position );
	            }
	            break;

	        case EnemyState.Attack:
	            timer -= Time.deltaTime;
                gameObject.renderer.material.mainTexture = gs.enemyDash;
	            if (timer <= 0.0f)
	            {
	                state = EnemyState.Idle;
                    gs.Enemy.width = 100f;
                    gs.Enemy.height = 160f;
                    gs.Enemy.texture = gs.enemyJump;

                    Sequence sequence = new Sequence ();
                    sequence.Append ( HOTween.To ( gs.Enemy, 0.06f, new TweenParms ()
                        .Prop ( "position", gs.startEnemyPos + new Vector3 ( ( gs.Enemy.position.x - gs.startEnemyPos.x ) * 0.8f, -50f ) )
                        .Ease ( EaseType.EaseOutQuad ) )
                    );
                    sequence.Append ( HOTween.To ( gs.Enemy, 0.08f, new TweenParms ()
                        .Prop ( "position", gs.startEnemyPos + new Vector3 ( ( gs.Enemy.position.x - gs.startEnemyPos.x ) * 0.6f, -100f ) )
                        .Ease ( EaseType.Linear ) )
                    );
                    sequence.Append ( HOTween.To ( gs.Enemy, 0.1f, new TweenParms ()
                        .Prop ( "position", gs.startEnemyPos + new Vector3 ( ( gs.Enemy.position.x - gs.startEnemyPos.x ) * 0.4f, -100f ) )
                        .Ease ( EaseType.Linear ) )
                    );
                    sequence.Append ( HOTween.To ( gs.Enemy, 0.08f, new TweenParms ()
                        .Prop ( "position", gs.startEnemyPos + new Vector3 ( ( gs.Enemy.position.x - gs.startEnemyPos.x ) * 0.2f, -50f ) )
                        .Ease ( EaseType.Linear ) )
                    );
                    sequence.Append ( HOTween.To ( gs.Enemy, 0.06f, new TweenParms ()
                        .Prop ( "position", gs.startEnemyPos )
                        .Ease ( EaseType.EaseInQuad )
                        .OnComplete ( SetIdleTexture ) )
                    );
                    sequence.Play ();
	            }
	            break;

            case EnemyState.Dying:
                gameObject.renderer.material.mainTexture = gs.enemyDying;
                timer -= Time.deltaTime;
                if ( timer <= 0.0f ) {
                    state = EnemyState.Dead;
                    gs.Enemy.width = 200f;
                    gs.Enemy.height = 100f;
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
        gs.Enemy.width = 100f;
        gs.Enemy.height = 160f;
        gameObject.audio.PlayOneShot ( gs.attackSFX );
    }

    public void Guard () {
        state = EnemyState.Guard;
        timer = GUARD_TIME;
        gs.Enemy.width = 100f;
        gs.Enemy.height = 160f;
        gameObject.audio.PlayOneShot ( gs.defSFX );
    }

    public void Die () {
        state = EnemyState.Dying;
        timer = DYING_TIME;
        gs.Enemy.width = 160f;
        gs.Enemy.height = 120f;
        gameObject.audio.PlayOneShot ( gs.enemyDieSFX );
    }

    public void Reset () {
        state = EnemyState.Idle;
        gs.Enemy.width = 100f;
        gs.Enemy.height = 160f;
        gs.Enemy.position = gs.startEnemyPos;
    }

    private void SetIdleTexture () {
        gs.Enemy.texture = gs.enemyIdle;
        gs.Enemy.width = 100f;
        gs.Enemy.height = 160f;
    }
}
