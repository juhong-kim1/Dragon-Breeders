using UnityEngine;

public class DragonStatusManager : MonoBehaviour
{
    public StatusType currentStatus = StatusType.Default;
    private DragonHealth dragonHealth;

    private float statusCheckTimer = 0f;
    private float statusCheckMaxTime = 180f;

    private void Update()
    {
        statusCheckTimer += Time.deltaTime;

        if (statusCheckTimer >= statusCheckMaxTime)
        {
            CheckForNewStatus();
            statusCheckTimer = 0f;
        }
    }

    private void CheckForNewStatus()
    {
        var stats = dragonHealth.stats;

        if (stats.fatigue >= 100) SetStatus(StatusType.PassOut);
        else if (stats.hunger < 10) SetStatus(StatusType.FoodPoisoning);
        else if (stats.clean < 15) SetStatus(StatusType.Infection);
        else SetStatus(StatusType.Default);
    }

    public void SetStatus(StatusType newStatus)
    {
        if (currentStatus != newStatus)
        {
            currentStatus = newStatus;
        }
    }
}