using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    public GameManager gameManager;

    public GameObject mainWindow;
    public GameObject mapWindow;
    public GameObject eggVaultWindow;
    public GameObject nestWindow;
    public GameObject retryButton;

    private int currentStep = 0;
    private bool tutorialActive = false;
    private bool isTutorialClear = false;

    private void Start()
    {
        tutorialPanel.SetActive(false);
        isTutorialClear = PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;
    }
    public void OnGameStart()
    {
        if (!tutorialActive && !isTutorialClear)
        {
            tutorialActive = true;
            currentStep = 1;
            tutorialPanel.SetActive(true);
            ShowStep(currentStep);
        }
    }

    //private void Update()
    //{
    //    if (!tutorialActive) return;

    //    switch (currentStep)
    //    {
    //        case 1:
    //            if (mainWindow.activeSelf)
    //                NextStep();
    //            break;
    //        case 2: 
    //            if (mapWindow.activeSelf)
    //                NextStep();
    //            break;
    //        case 3:
    //            if (eggVaultWindow.activeSelf)
    //                NextStep();
    //            break;
    //        case 4:
    //            if (nestWindow.activeSelf)
    //                NextStep();
    //            break;
    //    }
    //}

    public void OnWindowOpened(GameObject window)
    {
        if (!tutorialActive) return;

        switch (currentStep)
        {
            case 1:
                if (window == mapWindow)
                    NextStep();
                break;
            case 2:
                if (window == eggVaultWindow)
                    NextStep();
                break;
            case 3:
                if(gameManager.dragonHealth != null)
                    NextStep();
                break;
            case 4:
                if (window == nestWindow)
                    NextStep();
                break;
            case 5:
                if (window == mapWindow)
                    NextStep();
                break;
            case 6:
                if (window == mainWindow)
                    NextStep();
                break;
            case 7:
                if (window == mapWindow)
                    NextStep();
                break;
        }
    }

    private void ShowStep(int step)
    {
        switch (step)
        {
            case 1:
                tutorialText.text = "드래곤을 키우려면 먼저 알을 부화시켜야합니다. 먼저 지도 버튼을 눌러 지도로 이동하세요";
                break;
            case 2:
                tutorialText.text = "지도로 왔습니다. 알보관소로 가서 알을 확인하세요.";
                break;
            case 3:
                tutorialText.text = "알 이미지를 5초간 눌러 알을 부화시키세요.";
                break;
            case 4:
                tutorialText.text = "알 부화에 성공했습니다. 뒤로 이동후 둥지로 돌아가세요";
                break;
            case 5:
                tutorialText.text = "이제부터 드래곤 육성이 가능합니다. 목욕, 먹이 등의 버튼으로 스탯을 관리할수 있습니다.\n메뉴의 스탯버튼으로 현재 스탯 확인가능하며 도움버튼으로 버튼정보 확인할 수 있습니다.\n 지도로 이동하세요";
                break;
            case 6:
                tutorialText.text = "탐험으로 랜덤 알을 얻을 수 있으며 첫번째 드래곤을 방생 시킨 후 다른 알을 부화시킬 수 있습니다.\n훈련 버튼으로 훈련이 가능합니다(추후 전투시스템 추가예정입니다).\n둥지로 돌아가세요.";
                Debug.Log(tutorialText.text);
                break;
            case 7:
                tutorialText.text = "드래곤이 휴식할때 나머지 스탯을 더하여 경험치를 얻습니다.\n경험치가 쌓일 수록 유아기를 거쳐 성체까지 진화하며, 성체일때 방생하여 명성을 획득합니다.(추후에 시스템 추가 예정입니다.)\n 지도로 이동하면 안내가 종료됩니다.";
                break;
        }
    }

    public void NextStep()
    {
        currentStep++;

        if (currentStep > 7)
        {
            EndTutorial();
            return;
        }

        ShowStep(currentStep);
    }

    private void EndTutorial()
    {
        tutorialActive = false;
        isTutorialClear = true;
        tutorialPanel.SetActive(false);
        Debug.Log("튜토리얼 완료");
    }

}
