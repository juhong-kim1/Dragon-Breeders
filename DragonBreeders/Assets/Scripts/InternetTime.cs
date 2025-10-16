using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class InternetTime : MonoBehaviour
{
    private const string TIME_API_URL = "http://worldtimeapi.org/api/timezone/Asia/Seoul";

    public DateTime ServerTime { get; private set; }
    public bool IsTimeSynced { get; private set; } = false;
    private float timeSinceSync = 0f;

    void Start()
    {
        StartCoroutine(SyncServerTime());
    }

    void Update()
    {
        if (IsTimeSynced)
        {
            timeSinceSync += Time.deltaTime;
        }
    }

    public IEnumerator SyncServerTime()
    {
        Debug.Log("⏰ 서버 시간 동기화 중...");

        using (UnityWebRequest request = UnityWebRequest.Get(TIME_API_URL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                try
                {
                    WorldTimeResponse response = JsonUtility.FromJson<WorldTimeResponse>(json);
                    ServerTime = DateTime.Parse(response.datetime);
                    timeSinceSync = 0f;
                    IsTimeSynced = true;

                    Debug.Log($"세계시간: {ServerTime}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"파싱 실패: {e.Message}");
                    ServerTime = DateTime.Now;
                    IsTimeSynced = false;
                }
            }
            else
            {
                Debug.LogError($"서버 연결 실패: {request.error}");
                ServerTime = DateTime.Now;
                IsTimeSynced = false;
            }
        }
    }


    public DateTime GetCurrentServerTime()
    {
        if (!IsTimeSynced)
        {
            Debug.LogWarning("서버 시간 미동기화! 로컬 시간 사용");
            return DateTime.Now;
        }

        return ServerTime.AddSeconds(timeSinceSync);
    }
}

[System.Serializable]
public class WorldTimeResponse
{
    public string datetime;
}
