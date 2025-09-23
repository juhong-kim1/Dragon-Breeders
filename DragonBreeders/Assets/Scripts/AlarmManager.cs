using UnityEngine;
using TMPro;

public class AlarmManager : MonoBehaviour
{
    public static AlarmManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI alarmText;
    [SerializeField] private float displayTime = 2f;

    private Coroutine currentAlarmCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowAlarm(string message)
    {
        if (currentAlarmCoroutine != null)
            StopCoroutine(currentAlarmCoroutine);

        currentAlarmCoroutine = StartCoroutine(ShowAlarmCoroutine(message));
    }

    private System.Collections.IEnumerator ShowAlarmCoroutine(string message)
    {
        if (alarmText == null) yield break;

        alarmText.text = message;
        alarmText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        alarmText.gameObject.SetActive(false);
        alarmText.text = "";
        currentAlarmCoroutine = null;
    }
}

