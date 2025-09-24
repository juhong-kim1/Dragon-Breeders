using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

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

    public GameObject mosterPrefabs;

    private int skillCooldown = 0;

    private void Start()
    {
        stopButton.onClick.AddListener(() => ToggleStopButton());
        stopPanel.gameObject.SetActive(false);

        attackButton.onClick.AddListener(OnPlayerAttack);
        skillButton.onClick.AddListener(OnPlayerSkill);

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
        yield return new WaitForSeconds(1f);

        currentState = BattleState.PlayerTurn;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        if (skillCooldown > 0)
            skillCooldown--;

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

            //int expReward = 25 * currentEnemy.MON_TYPE;
            //playerDragon.GainExperience(expReward);

            //int coinReward = monster.stamina;
           // GameManager.Instance.playerManager.coin += coinReward;
            GameManager.Instance.playerManager.UpdateCoinUI();
            AlarmManager.Instance.ShowAlarm("승리하였습니다! 지역 선택으로 돌아갑니다.");
        }
        else
        {
            currentState = BattleState.Lost;
            AlarmManager.Instance.ShowAlarm("패배하였습니다! 지역 선택으로 돌아갑니다.");
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
        //for (int i = 1; i < )
    
    
    
    
    }


    public void OnClickQuitOut()
    {
        GameManager.Instance.MoveSceneOnOff();
        SceneManager.UnloadSceneAsync("BattleScene");
    }

    public void ToggleStopButton()
    {
        stopPanel.SetActive(!stopPanel.activeSelf);
    }
}