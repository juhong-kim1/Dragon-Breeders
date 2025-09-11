using UnityEngine;

public enum DragonBehaviorState
{ 
    Idle1State,
    Idle2State,
    HappyState,
    HurtState,
    AngryState
}

public enum DragonGrowthState
{ 
    Infancy,
    GrowingUp,
    Maturity,
    Adult,
}

public class DragonBehavior : MonoBehaviour
{
    public DragonBehaviorState currentBehavior;
    public DragonGrowthState currentGrowth;
    private Animator animator;
    private float growSpeed = 5f;

    private Vector3 targetScale;

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

        switch (currentGrowth)
        {
            case DragonGrowthState.Infancy:
                targetScale = new Vector3(0.2f, 0.2f, 0.2f);
                if (Touch3() || Input.GetKeyDown(KeyCode.Alpha1))
                {
                    currentGrowth = DragonGrowthState.GrowingUp;
                }
                break;
            case DragonGrowthState.GrowingUp:
                targetScale = new Vector3(0.4f, 0.4f, 0.4f);
                if (Touch3() || Input.GetKeyDown(KeyCode.Alpha1))
                {
                    currentGrowth = DragonGrowthState.Maturity;
                }
                break;
            case DragonGrowthState.Maturity:
                targetScale = new Vector3(0.6f, 0.6f, 0.6f);
                if (Touch3() || Input.GetKeyDown(KeyCode.Alpha1))
                {
                    currentGrowth = DragonGrowthState.Adult;
                }
                break;
            case DragonGrowthState.Adult:
                targetScale = new Vector3(1f, 1f, 1f);
                if (Touch3() || Input.GetKeyDown(KeyCode.Alpha1))
                {
                    currentGrowth = DragonGrowthState.Infancy;
                }
                break;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * growSpeed);
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

    private bool Touch3()
    {
        if (Input.touchCount == 3)
        { 
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            Touch touch2 = Input.GetTouch(2);

            if (touch0.phase == TouchPhase.Began && touch1.phase == TouchPhase.Began && touch2.phase == TouchPhase.Began)
            {
                return true;
            }
        }

        return false;
    }




}
