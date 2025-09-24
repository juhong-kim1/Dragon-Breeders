using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class RewardItem
{
    public int itemId;
    public int quantity;
    public ItemTableData itemData;

    public RewardItem(int itemId, int quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
        this.itemData = DataTableManger.ItemTable.Get(itemId);
    }

    public override string ToString()
    {
        string itemName = itemData?.StringName ?? "Unknown Item";
        return $"{itemName} x{quantity}";
    }
}

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
    private int enemySkillCooldown = 0;
    [SerializeField] private float displayTime = 2f;

    private Coroutine currentAlarmCoroutine;
    private Coroutine battleStartCoroutine;
    private Coroutine showPlayerDamageCoroutine;
    private Coroutine showDamageCoroutine;

    public TextMeshProUGUI DragonDamage;
    public TextMeshProUGUI MonsterDamage;

    private float attackDelay = 1.3f;

    public GameObject endBattlePanel;

    [SerializeField] private int rewardItemCount = 3;

    private void Start()
    {
        stopButton.onClick.AddListener(() => ToggleStopButton());
        stopPanel.gameObject.SetActive(false);

        attackButton.onClick.AddListener(OnPlayerAttack);
        skillButton.onClick.AddListener(OnPlayerSkill);

        TrainingPlace = GameManager.Instance.TrainingPlace;
        Difficulty = GameManager.Instance.Difficulty;

        DragonDamage.enabled = false;
        MonsterDamage.enabled = false;

        endBattlePanel.gameObject.SetActive(false);

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

        monster.PlayStartAnimation();

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

        StartCoroutine(ApplyDamageDelayed(attackDelay));

        GameManager.Instance.dragonHealth.GetComponent<DragonBehavior>().PlayAttackAnimation();

        Debug.Log("플레이어택애니메이션 실행해야함");
    }

    public void OnPlayerSkill()
    {
        if (currentState != BattleState.PlayerTurn || skillCooldown > 0) return;

        SetButtonsInteractable(false);

        StartCoroutine(ApplySkillDamageDelayed(attackDelay));

        GameManager.Instance.dragonHealth.GetComponent<DragonBehavior>().PlaySkillAnimation();

        Debug.Log("플레이스킬애니메이션 실행해야함");
    }

    private IEnumerator EnemyTurn()
    {
        ShowAlarm("상대 차례!");

        currentState = BattleState.EnemyTurn;
        yield return new WaitForSeconds(2f);

        if (enemySkillCooldown > 0)
            enemySkillCooldown--;

        SkillTableData chosenSkill;
        bool useSpecialSkill = Random.Range(0f, 1f) > 0.7f && enemySkillCooldown <= 0;

        if (useSpecialSkill)
        {
            chosenSkill = enemySpecialSkill;
            enemySkillCooldown = enemySpecialSkill.SKILL_CD;

            monster.PlaySkillAnimation();

            StartCoroutine(ApplyEnemySkillDamageDelayed(attackDelay));
        }
        else
        {
            chosenSkill = enemyAttackSkill;

            monster.PlayAttackAnimation();

            StartCoroutine(ApplyEnemyAttackDamageDelayed(attackDelay));
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

            List<RewardItem> rewards = GenerateBattleRewards();
            GiveBattleRewards(rewards);
            ShowRewardMessage(rewards);

            monster.PlayDieAnimation();
            GameManager.Instance.playerManager.UpdateCoinUI();
            ShowAlarm("승리하였습니다!");
            yield return new WaitForSeconds(5f);
            endBattlePanel.SetActive(true);
        }
        else
        {
            monster.PlayWinAnimation();
            currentState = BattleState.Lost;
            ShowAlarm("패배하였습니다!");
            yield return new WaitForSeconds(5f);
            OnClickQuitOut();
        }
    }

    private List<RewardItem> GenerateBattleRewards()
    {
        List<RewardItem> rewards = new List<RewardItem>();

        List<DropTableData> availableDrops = DataTableManger.DropTable.GetDropsByBiome((int)TrainingPlace);

        if (availableDrops.Count == 0)
        {
            Debug.LogWarning($"바이옴 {TrainingPlace}에서 드롭 가능한 아이템이 없습니다!");
            return rewards;
        }

        Debug.Log($"바이옴 {TrainingPlace}에서 {availableDrops.Count}개 아이템 드롭 가능");

        List<DropTableData> uniqueItems = new List<DropTableData>();
        List<int> addedItemIds = new List<int>();

        foreach (var drop in availableDrops)
        {
            if (!addedItemIds.Contains(drop.ITEM_ID))
            {
                uniqueItems.Add(drop);
                addedItemIds.Add(drop.ITEM_ID);
            }
        }

        if (uniqueItems.Count < rewardItemCount)
        {
            Debug.LogWarning($"바이옴 {TrainingPlace}에 서로 다른 아이템이 {uniqueItems.Count}개밖에 없습니다!");
        }

        List<DropTableData> weightedList = CreateWeightedDropList(uniqueItems);
        List<int> usedItemIds = new List<int>();

        int itemsToGenerate = Mathf.Min(rewardItemCount, uniqueItems.Count);

        for (int i = 0; i < itemsToGenerate; i++)
        {
            DropTableData selectedDrop = SelectRandomDrop(weightedList, usedItemIds);

            if (selectedDrop == null)
            {
                Debug.LogWarning("더 이상 선택할 수 있는 아이템이 없습니다!");
                break;
            }

            int quantity = Random.Range(selectedDrop.MINDROP, selectedDrop.MAXDROP + 1);

            RewardItem reward = new RewardItem(selectedDrop.ITEM_ID, quantity);
            rewards.Add(reward);

            Debug.Log($"보상 {i + 1}: {reward}");

            usedItemIds.Add(selectedDrop.ITEM_ID);
        }

        return rewards;
    }

    private List<DropTableData> CreateWeightedDropList(List<DropTableData> drops)
    {
        List<DropTableData> weightedList = new List<DropTableData>();

        foreach (var drop in drops)
        {
            for (int i = 0; i < drop.DROP_RATE; i++)
            {
                weightedList.Add(drop);
            }
        }

        return weightedList;
    }

    private DropTableData SelectRandomDrop(List<DropTableData> weightedList, List<int> usedItemIds)
    {
        if (weightedList.Count == 0) return null;

        List<DropTableData> availableDrops = new List<DropTableData>();

        foreach (var drop in weightedList)
        {
            if (!usedItemIds.Contains(drop.ITEM_ID))
            {
                availableDrops.Add(drop);
            }
        }

        if (availableDrops.Count == 0) return null;

        int randomIndex = Random.Range(0, availableDrops.Count);
        return availableDrops[randomIndex];
    }

    private void GiveBattleRewards(List<RewardItem> rewards)
    {
        if (GameManager.Instance?.inventoryManager == null)
        {
            Debug.LogError("GameManager 또는 InventoryManager를 찾을 수 없습니다!");
            return;
        }

        foreach (var reward in rewards)
        {
            GameManager.Instance.inventoryManager.AddItem(reward.itemId, reward.quantity);
            Debug.Log($"플레이어가 {reward}를 획득했습니다!");
        }

        Debug.Log($"총 {rewards.Count}개의 보상 아이템이 인벤토리에 추가되었습니다.");
    }

    private void ShowRewardMessage(List<RewardItem> rewards)
    {
        string rewardText = "전투 승리! 획득 아이템:\n";
        foreach (var reward in rewards)
        {
            string itemName = reward.itemData?.StringName ?? "Unknown Item";
            rewardText += $"• {itemName} x{reward.quantity}\n";
        }

        ShowAlarm(rewardText);
        Debug.Log(rewardText);
    }

    private void UpdateUI()
    {
        if (playerDragon != null)
        {
            playerHPSlider.DOValue(playerDragon.stats.stamina / playerDragon.stats.maxStamina, 0.5f);
        }

        if (monster != null)
        {
            enemyHPSlider.DOValue(monster.stamina / monster.maxStamina, 0.5f);
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

        monsterObj.transform.localPosition = new Vector3(-10.14f, 5f, -8.9f);
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


    public void ShowMonsterDamage(int damage)
    {
        if (showDamageCoroutine != null)
            StopCoroutine(showDamageCoroutine);

        showDamageCoroutine = StartCoroutine(ShowMonsterDamageCoroutine(damage));
    }

    public void ShowPlayerDamage(int damage)
    {
        if (showPlayerDamageCoroutine != null)
            StopCoroutine(showPlayerDamageCoroutine);

        showPlayerDamageCoroutine = StartCoroutine(ShowPlayerDamageCoroutine(damage));
    }

    private IEnumerator ShowMonsterDamageCoroutine(int damage)
    {
        MonsterDamage.enabled = true;
        MonsterDamage.text = damage.ToString();

        yield return new WaitForSeconds(displayTime);

        MonsterDamage.enabled = false;
        showDamageCoroutine = null;
    }

    private IEnumerator ShowPlayerDamageCoroutine(int damage)
    {
        DragonDamage.enabled = true;
        DragonDamage.text = damage.ToString();

        yield return new WaitForSeconds(displayTime);

        DragonDamage.enabled = false;
        showPlayerDamageCoroutine = null;
    }

    private IEnumerator ApplyDamageDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        float skillDamage = dragonAttackSkill.SKILL_POWER;
        float damage = PlayerAttack(skillDamage);
        float finalDamage = damage / monster.defense;
        finalDamage = Mathf.Max(1, finalDamage);

        monster.stamina -= finalDamage;
        monster.stamina = Mathf.Max(0, monster.stamina);


        ShowMonsterDamage(Mathf.RoundToInt(finalDamage));
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

    private IEnumerator ApplySkillDamageDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        float skillDamage = dragonSpecialSkill.SKILL_POWER;
        float damage = PlayerAttack(skillDamage);
        float finalDamage = damage / monster.defense;
        finalDamage = Mathf.Max(1, finalDamage);

        monster.stamina -= finalDamage;
        skillCooldown = dragonSpecialSkill.SKILL_CD;

        ShowMonsterDamage(Mathf.RoundToInt(finalDamage));
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

    private IEnumerator ApplyEnemyAttackDamageDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        float baseDamage = enemyAttackSkill.SKILL_POWER;
        float damage = MonsterAttack(baseDamage);
        float finalDamage = damage / PlayerDefense();
        finalDamage = Mathf.Max(1, finalDamage);

        playerDragon.stats.ChangeStat(StatType.Stamina, -finalDamage);
        ShowPlayerDamage(Mathf.RoundToInt(finalDamage));
        UpdateUI();

        Handheld.Vibrate();

        yield return new WaitForSeconds(1f);

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

    private IEnumerator ApplyEnemySkillDamageDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        float baseDamage = enemySpecialSkill.SKILL_POWER;
        float damage = MonsterAttack(baseDamage);
        float finalDamage = damage / PlayerDefense();
        finalDamage = Mathf.Max(1, finalDamage);

        playerDragon.stats.ChangeStat(StatType.Stamina, -finalDamage);
        ShowPlayerDamage(Mathf.RoundToInt(finalDamage));
        UpdateUI();

        Handheld.Vibrate();

        yield return new WaitForSeconds(1f);

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

}