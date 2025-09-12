using UnityEngine;

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
    public DragonBehaviorState currentBehavior;
    public DragonGrowthState currentGrowth;
    private Animator animator;

    public static readonly string[] Action = { "Action1", "Action2", "Action3", "Action4", "Action5" };


    void Start()
    {
        animator = GetComponent<Animator>();
        currentBehavior = DragonBehaviorState.Idle1State;
        currentGrowth = DragonGrowthState.Infancy;
    }

    void Update()
    {
        TouchAction();
    }

    private void TouchAction()
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
        }
    }
}
