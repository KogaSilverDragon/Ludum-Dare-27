using UnityEngine;
using System.Collections;
using Holoville.HOTween;

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
        gameObject.AddComponent ( "AudioSource" );
        gameObject.audio.volume = 1f;
        gameObject.audio.loop = false;
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
                    HOTween.To ( gs.Player, ATTACK_TIME, "position", gs.Enemy.position );
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
                    gs.Player.texture = gs.playerJump;

                    Sequence sequence = new Sequence ();
                    sequence.Append ( HOTween.To ( gs.Player, 0.06f, new TweenParms ()
                        .Prop ( "position", gs.startPlayerPos + new Vector3 ( ( gs.Player.position.x - gs.startPlayerPos.x ) * 0.8f, -50f ) )
                        .Ease ( EaseType.EaseOutQuad ) )
                    );
                    sequence.Append ( HOTween.To ( gs.Player, 0.08f, new TweenParms ()
                        .Prop ( "position", gs.startPlayerPos + new Vector3 ( ( gs.Player.position.x - gs.startPlayerPos.x ) * 0.6f, -100f ) )
                        .Ease ( EaseType.Linear ) )
                    );
                    sequence.Append ( HOTween.To ( gs.Player, 0.1f, new TweenParms ()
                        .Prop ( "position", gs.startPlayerPos + new Vector3 ( ( gs.Player.position.x - gs.startPlayerPos.x ) * 0.4f, -100f ) )
                        .Ease ( EaseType.Linear ) )
                    );
                    sequence.Append ( HOTween.To ( gs.Player, 0.08f, new TweenParms ()
                        .Prop ( "position", gs.startPlayerPos + new Vector3 ( ( gs.Player.position.x - gs.startPlayerPos.x ) * 0.2f, -50f ) )
                        .Ease ( EaseType.Linear ) )
                    );
                    sequence.Append ( HOTween.To ( gs.Player, 0.06f, new TweenParms ()
                        .Prop ( "position", gs.startPlayerPos )
                        .Ease ( EaseType.EaseInQuad )
                        .OnComplete ( SetIdleTexture ) )
                    );
                    sequence.Play ();
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
        gameObject.audio.PlayOneShot (gs.attackSFX);
    }

    public void Guard () {
        state = PlayerState.Guard;
        timer = GUARD_TIME;
        gs.Player.width = 100f;
        gs.Player.height = 160f;
        gameObject.audio.PlayOneShot ( gs.defSFX );
    }

    public void Die () {
        state = PlayerState.Dying;
        timer = DYING_TIME;
        gs.Player.width = 160f;
        gs.Player.height = 120f;
        HOTween.To ( gs.Player, ATTACK_TIME, "position", gs.startPlayerPos - new Vector3 ( 100f, 0, 0 ) );
        gameObject.audio.PlayOneShot ( gs.playerDieSFX );
    }

    public void Reset () {
        state = PlayerState.Idle;
        gs.Player.width = 100f;
        gs.Player.height = 160f;
        gs.Player.position = gs.startPlayerPos;
    }

    private void SetIdleTexture () {
        gs.Player.texture = gs.playerIdle;
        gs.Player.width = 100f;
        gs.Player.height = 160f;
    }

}
