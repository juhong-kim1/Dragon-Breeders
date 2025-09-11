using UnityEngine;
using UnityEngine.AI;

public enum DragonBehaviorState
{ 
    Idle1State,
    Idle2State,
    HappyState,
    HurtState,
    AngryState
}

public class DragonBehavior : MonoBehaviour
{
    public DragonBehaviorState currentState;
    private Animator animator;

    public static readonly string[] Action = { "Action1", "Action2", "Action3", "Action4", "Action5" };

    private float idleTime = 5f;
    private float time = 0f;


    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = DragonBehaviorState.Idle1State;
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        int random = Random.Range(0, 5);

                        animator.SetTrigger(Action[random]);
                    }
                }
            }


            time += Time.deltaTime;

            if (time > idleTime)
            {
                int random = Random.Range(0, 3);

                switch (random)
                {
                    case 0:
                        currentState = DragonBehaviorState.Idle2State;
                        break;
                    case 1:
                        currentState = DragonBehaviorState.HappyState;
                        break;
                    case 2:
                        currentState = DragonBehaviorState.HurtState;
                        break;
                    case 3:
                        currentState = DragonBehaviorState.AngryState;
                        break;
                }

                time = 0f;
            }


            switch (currentState)
            {
                case DragonBehaviorState.Idle1State:
                    UpdateIdle1State();
                    break;
                case DragonBehaviorState.Idle2State:
                    UpdateIdle2State();
                    break;
                case DragonBehaviorState.HappyState:
                    UpdateHappyState();
                    break;
                case DragonBehaviorState.HurtState:
                    UpdateHurtState();
                    break;
                case DragonBehaviorState.AngryState:
                    UpdateAngryState();
                    break;
            }
        }
    }

    private void UpdateIdle1State()
    {
    
    
    }

    private void UpdateIdle2State()
    {
        animator.SetTrigger("Idle");

    }

    private void UpdateHappyState()
    {
        animator.SetTrigger("Happy");

    }

    private void UpdateHurtState()
    {
        animator.SetTrigger("Hurt");

    }

    private void UpdateAngryState()
    {
        animator.SetTrigger("Angry");
    }


}
