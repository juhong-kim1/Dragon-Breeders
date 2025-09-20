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
                tutorialText.text = "�巡���� Ű����� ���� ���� ��ȭ���Ѿ��մϴ�. ���� ���� ��ư�� ���� ������ �̵��ϼ���";
                break;
            case 2:
                tutorialText.text = "������ �Խ��ϴ�. �˺����ҷ� ���� ���� Ȯ���ϼ���.";
                break;
            case 3:
                tutorialText.text = "�� �̹����� 5�ʰ� ���� ���� ��ȭ��Ű����.";
                break;
            case 4:
                tutorialText.text = "�� ��ȭ�� �����߽��ϴ�. �ڷ� �̵��� ������ ���ư�����";
                break;
            case 5:
                tutorialText.text = "�������� �巡�� ������ �����մϴ�. ���, ���� ���� ��ư���� ������ �����Ҽ� �ֽ��ϴ�.\n�޴��� ���ȹ�ư���� ���� ���� Ȯ�ΰ����ϸ� �����ư���� ��ư���� Ȯ���� �� �ֽ��ϴ�.\n ������ �̵��ϼ���";
                break;
            case 6:
                tutorialText.text = "Ž������ ���� ���� ���� �� ������ ù��° �巡���� ��� ��Ų �� �ٸ� ���� ��ȭ��ų �� �ֽ��ϴ�.\n�Ʒ� ��ư���� �Ʒ��� �����մϴ�(���� �����ý��� �߰������Դϴ�).\n������ ���ư�����.";
                Debug.Log(tutorialText.text);
                break;
            case 7:
                tutorialText.text = "�巡���� �޽��Ҷ� ������ ������ ���Ͽ� ����ġ�� ����ϴ�.\n����ġ�� ���� ���� ���Ʊ⸦ ���� ��ü���� ��ȭ�ϸ�, ��ü�϶� ����Ͽ� ���� ȹ���մϴ�.(���Ŀ� �ý��� �߰� �����Դϴ�.)\n ������ �̵��ϸ� �ȳ��� ����˴ϴ�.";
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
        Debug.Log("Ʃ�丮�� �Ϸ�");
    }

}
