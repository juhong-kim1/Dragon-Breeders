using TMPro;
using UnityEngine;
using System.Collections;

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

    public static readonly string Play = "Play";
    public static readonly string Rest = "Rest";

    public TextMeshProUGUI dragonFeedback;
    public string[] touchMessage = { "±×¸£¸ª", "¸Û¸Û", "²¿¸£¸¤", "³¢À×", "¾È³çÇÏ¼¼¿ä\n µå·¡°ïÀÔ´Ï´Ù", "ÄÝ·Ï", "¢½" };

    private float lastTouchTime = 0f;
    public float touchCooldown = 3f;


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

    public void SetTouchUI(TextMeshProUGUI uiText)
    {
        dragonFeedback = uiText;

        if (dragonFeedback != null)
            dragonFeedback.gameObject.SetActive(false);
    }

    private void TouchAction()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (Time.time - lastTouchTime < touchCooldown)
                    return;

                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        int random = Random.Range(0, Action.Length);

                        animator.SetTrigger(Action[random]);
                        dragonHealth.stats.ChangeStat(StatType.Intimacy, 1);
                    }

                    string message = touchMessage[Random.Range(0, touchMessage.Length)];
                    dragonFeedback.text = message;


                    RectTransform rect = dragonFeedback.GetComponent<RectTransform>();
                    float x = Random.Range(-15f, 15f);
                    float y = Random.Range(-20f, 20f);
                    rect.anchoredPosition = new Vector2(x, y);


                    dragonFeedback.gameObject.SetActive(true);
                    StartCoroutine(TextAnimation());
                }

                lastTouchTime = Time.time;
            }
        }
    }

    private IEnumerator TextAnimation()
    {
        yield return new WaitForSeconds(2f);

        if (dragonFeedback != null)
            dragonFeedback.gameObject.SetActive(false);
    }

    private void TouchGrowth()
    {
        if ((Input.touchCount == 3 && Input.GetTouch(0).phase == TouchPhase.Began) ||
            Input.GetKeyDown(KeyCode.G))
        {
            dragonHealth.GrowUp();
        }
    }

    public void PlayRestAnimation()
    {
        animator.SetTrigger("Rest");
    }

    public void PlayPlayAnimation()
    {
        animator.SetTrigger("Play");
    }
}
