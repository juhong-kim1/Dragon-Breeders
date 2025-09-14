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
    private DragonHealth dragonHealth;

    public static readonly string[] Action = { "Action1", "Action2", "Action3", "Action4", "Action5" };


    private void Start()
    {
        animator = GetComponent<Animator>();
        dragonHealth = GetComponent<DragonHealth>();
    }

    private void Update()
    {
        TouchAction();
        TouchGrowth();
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
                    if (hit.collider.gameObject == gameObject)
                    {
                        int random = Random.Range(0, Action.Length);

                        animator.SetTrigger(Action[random]);
                        dragonHealth.stats.ChangeStat(StatType.Intimacy, 1);
                    }
                }
            }
        }
    }

    private void TouchGrowth()
    {
        if ((Input.touchCount == 3 && Input.GetTouch(0).phase == TouchPhase.Began) ||
            Input.GetKeyDown(KeyCode.G))
        {
            dragonHealth.GrowUp();
        }


    }
}
