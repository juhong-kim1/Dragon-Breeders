using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState
{
    Start,
    PlayerTurn,
    EnemyTurn,
    Won,
    Lost
}

public class BattleManager : MonoBehaviour
{
    public Button stopButton;
    public GameObject stopPanel;
    public Button attackButton;
    public Button skillButton;

    public Slider playerHPSlider;
    public Slider enemyHPSlider;

    public BattleState currentState;
    private DragonHealth playerDragon;
    public MonsterHealth monster;
    private SkillTableData dragonAttackSkill;
    private SkillTableData dragonSpecialSkill;
    private SkillTableData enemyAttackSkill;
    private SkillTableData enemySpecialSkill;

    public TextMeshProUGUI alarmText;
    public TextMeshProUGUI battleStartText;

    private TrainingPlace TrainingPlace;
    private Difficulty Difficulty;

    public GameObject[] monsterPrefabs;

    private int skillCooldown = 0;
    [SerializeField] private float displayTime = 2f;

    private Coroutine currentAlarmCoroutine;
    private Coroutine battleStartCoroutine;

    private void Start()
    {
        stopButton.onClick.AddListener(() => ToggleStopButton());
        stopPanel.gameObject.SetActive(false);

        attackButton.onClick.AddListener(OnPlayerAttack);
        skillButton.onClick.AddListener(OnPlayerSkill);

        TrainingPlace = GameManager.Instance.TrainingPlace;
        Difficulty = GameManager.Instance.Difficulty;

        InitializeBattle();

        Debug.Log($"배틀 준비, 플레이어 현재 HP {playerDragon.stats.stamina}/{playerDragon.stats.maxStamina}, 드래곤 현재 HP {monster.stamina}/{monster.maxStamina}");
    }

    private void InitializeBattle()
    {
        currentState = BattleState.Start;

        playerDragon = GameManager.Instance.dragonHealth;
        if (playerDragon == null)
        {
            Debug.LogError("드래곤이 널입니다.");
            return;
        }

        RandomMonsterInstantiate();
        if (monster == null)
        {
            Debug.LogError("몬스터가 널입니다.");
            return;
        }


        LoadSkills();

        UpdateUI();

        ShowAlarm("배틀 준비 완료!");

        ShowBattleStart();

        StartCoroutine(BattleSequence());
    }

    private void LoadSkills()
    {
        dragonAttackSkill = DataTableManger.SkillTable.Get(playerDragon.currentTableData.SKILL1_ID);
        dragonSpecialSkill = DataTableManger.SkillTable.Get(playerDragon.currentTableData.SKILL2_ID);

        enemyAttackSkill = DataTableManger.SkillTable.Get(monster.monsterTableData.MONSKILL1_ID);
        enemySpecialSkill = DataTableManger.SkillTable.Get(monster.monsterTableData.MONSKILL2_ID);

        if (dragonAttackSkill == null || dragonSpecialSkill == null)
        {
            Debug.LogError("Dragon skills not found!");
        }

        if (enemyAttackSkill == null || enemySpecialSkill == null)
        {
            Debug.LogError("Enemy skills not found!");
        }
    }

    private IEnumerator BattleSequence()
    {
        yield return new WaitForSeconds(2f);

        currentState = BattleState.PlayerTurn;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        if (skillCooldown > 0)
            skillCooldown--;

        ShowAlarm("내 차례!");
        SetButtonsInteractable(true);
    }

    public void OnPlayerAttack()
    {
        if (currentState != BattleState.PlayerTurn) return;

        SetButtonsInteractable(false);

        float skillDamage = dragonAttackSkill.SKILL_POWER;
        float damage = PlayerAttack(skillDamage);

        float finalDamage = damage / monster.defense;
        finalDamage = Mathf.Max(1, finalDamage);

        monster.stamina -= finalDamage;
        monster.stamina = Mathf.Max(0, monster.stamina);

        string dragonName = DataTableManger.StringTable.Get(playerDragon.currentTableData.DRAGON_NAME);

        UpdateUI();

        if (monster.stamina <= 0)
        {
            StartCoroutine(BattleEnd(true));
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnPlayerSkill()
    {
        if (currentState != BattleState.PlayerTurn || skillCooldown > 0) return;

        SetButtonsInteractable(false);

        float skillDamage = dragonSpecialSkill.SKILL_POWER;
        float damage = PlayerAttack(skillDamage);

        float finalDamage = damage / monster.defense;
        finalDamage = Mathf.Max(1, finalDamage);


        skillCooldown = dragonSpecialSkill.SKILL_CD;

        UpdateUI();

        if (monster.stamina <= 0)
        {
            StartCoroutine(BattleEnd(true));
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator EnemyTurn()
    {
        ShowAlarm("상대 차례!");

        currentState = BattleState.EnemyTurn;
        yield return new WaitForSeconds(2f);

        SkillTableData chosenSkill = Random.Range(0f, 1f) > 0.7f ? enemySpecialSkill : enemyAttackSkill;

        float baseDamage = chosenSkill.SKILL_POWER;
        float damage = MonsterAttack(baseDamage);

        float finalDamage = damage / PlayerDefense();
        finalDamage = Mathf.Max(1, finalDamage);

        playerDragon.stats.ChangeStat(StatType.Stamina, -finalDamage);

        UpdateUI();

        yield return new WaitForSeconds(2f);

        if (playerDragon.stats.stamina <= 0)
        {
            StartCoroutine(BattleEnd(false));
        }
        else
        {
            currentState = BattleState.PlayerTurn;
            PlayerTurn();
        }
    }

    private IEnumerator BattleEnd(bool playerWon)
    {
        if (playerWon)
        {
            currentState = BattleState.Won;

            var Nuture = DataTableManger.NurtureTable;

            switch (Difficulty)
            { 
                case Difficulty.None:
                    Debug.Log("잘못 된 난이도입니다.");
                    break;
                case Difficulty.Low:
                    GameManager.Instance.dragonHealth.stats.ChangeStat(StatType.Experience, Nuture.Get(4000501).EXPGROWTH);
                    break;
                case Difficulty.Medium:
                    GameManager.Instance.dragonHealth.stats.ChangeStat(StatType.Experience, Nuture.Get(4000502).EXPGROWTH);
                break;
                case Difficulty.High:
                    GameManager.Instance.dragonHealth.stats.ChangeStat(StatType.Experience, Nuture.Get(4000503).EXPGROWTH);
                break;
            }


            GameManager.Instance.playerManager.UpdateCoinUI();
            ShowAlarm("승리하였습니다!");
        }
        else
        {
            currentState = BattleState.Lost;
            ShowAlarm("패배하였습니다!");
        }

        yield return new WaitForSeconds(2f);

        OnClickQuitOut();
    }

    private void UpdateUI()
    {
        if (playerDragon != null)
        {
            playerHPSlider.value = playerDragon.stats.stamina / playerDragon.stats.maxStamina;
        }

        if (monster != null)
        {
            enemyHPSlider.value = monster.stamina / monster.maxStamina;
        }
    }

    private float PlayerAttack(float skill)
    {
        float combatPower = playerDragon.stats.experience * playerDragon.currentTableData.GROWTH_MULT;

        float damage = combatPower * playerDragon.currentTableData.ATT_MULT;

        return damage * skill;
    }

    private float MonsterAttack(float skill)
    {
        return monster.attack * skill;
    }

    private float PlayerDefense()
    {
        float combatPower = playerDragon.stats.experience * playerDragon.currentTableData.GROWTH_MULT;

        return combatPower * playerDragon.currentTableData.DEF_MULT;
    
    }

    private void SetButtonsInteractable(bool interactable)
    {
        attackButton.interactable = interactable;
        skillButton.interactable = interactable && skillCooldown <= 0 && currentState == BattleState.PlayerTurn;
    }

    private void RandomMonsterInstantiate()
    {
        List<GameObject> candidates = new List<GameObject>();
        foreach (var prefab in monsterPrefabs)
        {
            MonsterHealth mh = prefab.GetComponent<MonsterHealth>();
            Debug.Log($"monsterType: {mh.monsterType}, MonsterTableData: {mh.monsterTableData}");
            if (mh != null && mh.difficulty == (int)Difficulty && mh.regions.Contains((int)TrainingPlace))
            {
                candidates.Add(prefab);
            }

        }

        if (candidates.Count == 0)
        {
            Debug.LogError($"[BattleManager] 조건에 맞는 몬스터 없음! Difficulty: {Difficulty}, TrainingPlace: {TrainingPlace}");
            return;
        }

        GameObject chosenPrefab = candidates[Random.Range(0, candidates.Count)];
        GameObject monsterObj = Instantiate(chosenPrefab, transform);

        monsterObj.transform.localPosition = new Vector3(-10.14f,5f,-8.9f);
        monsterObj.transform.localRotation = Quaternion.Euler(0f, -48.74f, 0f);
        monsterObj.transform.localScale = new Vector3(2f, 2f, 2f);

        monster = monsterObj.GetComponent<MonsterHealth>();

        if (monster != null)
        {
            monster.InitializeMonsterData();
            Debug.Log($"[BattleManager] 몬스터 초기화 완료 - HP: {monster.stamina}/{monster.maxStamina}");
        }
        else
        {
            Debug.LogError("[BattleManager] 생성된 몬스터에서 MonsterHealth 컴포넌트를 찾을 수 없습니다!");
        }

    }


    public void OnClickQuitOut()
    {
        GameManager.Instance.isFighting = false;
        GameManager.Instance.dragonHealth.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        GameManager.Instance.dragonHealth.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        GameManager.Instance.MoveSceneOnOff();
        SceneManager.UnloadSceneAsync("BattleScene");
    }

    public void ToggleStopButton()
    {
        stopPanel.SetActive(!stopPanel.activeSelf);
    }

    public void ShowAlarm(string message)
    {
        if (currentAlarmCoroutine != null)
            StopCoroutine(currentAlarmCoroutine);

        currentAlarmCoroutine = StartCoroutine(ShowAlarmCoroutine(message));
    }

    private IEnumerator ShowAlarmCoroutine(string message)
    {
        if (alarmText == null) yield break;

        alarmText.text = message;
        alarmText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        alarmText.gameObject.SetActive(false);
        alarmText.text = "";
        currentAlarmCoroutine = null;
    }

    public void ShowBattleStart()
    {
        if (battleStartCoroutine != null)
            StopCoroutine(battleStartCoroutine);

        battleStartCoroutine = StartCoroutine(ShowBattleStartCoroutine());
    }

    private IEnumerator ShowBattleStartCoroutine()
    {
        if (alarmText == null) yield break;

        battleStartText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        battleStartText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);
        battleStartText.gameObject.SetActive(false);
    }
}