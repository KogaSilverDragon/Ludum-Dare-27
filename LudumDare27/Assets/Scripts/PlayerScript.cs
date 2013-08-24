using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public enum PlayerState
    {
        Walk, // Scene Transition
        Guard,
        Prepare, // Vunerable
        Attack,
        Die
    }

    private const float PREPARE_TIME = 0.25f;
    private const float ATTACK_TIME = 0.5f; // Change to an animation time

    private PlayerState state = PlayerState.Guard;

    private float timer = 0.0f;

    private void Start()
    {
    }
	
    private void Update()
    {
        switch (state)
        {
            case PlayerState.Walk:
                break;

            case PlayerState.Guard:
                break;

            case PlayerState.Prepare:
                timer -= Time.deltaTime;

                if (timer <= 0.0f)
                {
                    state = PlayerState.Attack;
                    gameObject.renderer.material.color = Color.green;
                    timer = ATTACK_TIME;
                }
                break;

            case PlayerState.Attack:
                timer -= Time.deltaTime;

                if (timer <= 0.0f)
                {
                    state = PlayerState.Guard;
                    gameObject.renderer.material.color = Color.white;
                }
                break;

            case PlayerState.Die:
                break;
        }
    }

    public PlayerState GetState()
    {
        return state;
    }

    public void Attack()
    {
        state = PlayerState.Prepare;
        gameObject.renderer.material.color = Color.yellow;
        timer = PREPARE_TIME;
    }

}
