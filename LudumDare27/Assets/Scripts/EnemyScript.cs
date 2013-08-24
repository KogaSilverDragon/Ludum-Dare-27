using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    public enum EnemyState
    {
        Guard,
        Prepare, // Vunerable
        Attack,
        Die
    }

    private const float PREPARE_TIME = 1.0f;
    private const float ATTACK_TIME = 0.5f; // Change to an animation time

    private EnemyState state = EnemyState.Guard;

    private float timer = 0.0f;

	void Start ()
	{
	}
	
	void Update ()
    {
	    switch (state)
	    {
	        case EnemyState.Guard:
	            break;

	        case EnemyState.Prepare:
	            timer -= Time.deltaTime;

	            if (timer <= 0.0f)
	            {
	                state = EnemyState.Attack;
	                gameObject.renderer.material.color = Color.green;
	                timer = ATTACK_TIME;
	            }
	            break;

	        case EnemyState.Attack:
	            timer -= Time.deltaTime;

	            if (timer <= 0.0f)
	            {
	                state = EnemyState.Guard;
	                gameObject.renderer.material.color = Color.white;
	            }
	            break;

	        case EnemyState.Die:
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
        gameObject.renderer.material.color = Color.yellow;
        timer = PREPARE_TIME;
    }
}
