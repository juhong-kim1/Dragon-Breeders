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
    private MonsterHealth monster;
    private MonsterTableData currentEnemy;
    private SkillTableData dragonAttackSkill;
    private SkillTableData dragonSpecialSkill;
    private SkillTableData enemyAttackSkill;
    private SkillTableData enemySpecialSkill;

    private float enemyCurrentHP;
    private int skillCooldown = 0;

    private void Start()
    {
        stopButton.onClick.AddListener(() => ToggleStopButton());
        stopPanel.gameObject.SetActive(false);

        attackButton.onClick.AddListener(OnPlayerAttack);
        skillButton.onClick.AddListener(OnPlayerSkill);

        InitializeBattle();
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

        currentEnemy = DataTableManger.MonsterTable.Get(90101);
        if (currentEnemy == null)
        {
            Debug.LogError("몬스터가 널입니다.");
            return;
        }

        enemyCurrentHP = currentEnemy.MONHP;

        LoadSkills();

        UpdateUI();

        StartCoroutine(BattleSequence());
    }

    private void LoadSkills()
    {
        dragonAttackSkill = DataTableManger.SkillTable.Get(playerDragon.currentTableData.SKILL1_ID);
        dragonSpecialSkill = DataTableManger.SkillTable.Get(playerDragon.currentTableData.SKILL2_ID);

        enemyAttackSkill = DataTableManger.SkillTable.Get(currentEnemy.MONSKILL1_ID);
        enemySpecialSkill = DataTableManger.SkillTable.Get(currentEnemy.MONSKILL2_ID);

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

        float baseDamage = dragonAttackSkill.SKILL_POWER;
        float damage = baseDamage * playerDragon.currentTableData.ATT_MULT * playerDragon.currentTableData.GROWTH_MULT;

        float finalDamage = damage - (currentEnemy.MONDEF * 0.5f);
        finalDamage = Mathf.Max(1, finalDamage);

        enemyCurrentHP -= finalDamage;
        enemyCurrentHP = Mathf.Max(0, enemyCurrentHP);

        string dragonName = DataTableManger.StringTable.Get(playerDragon.currentTableData.DRAGON_NAME);

        UpdateUI();

        if (enemyCurrentHP <= 0)
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

        float baseDamage = dragonSpecialSkill.SKILL_POWER;
        float damage = baseDamage * playerDragon.currentTableData.ATT_MULT * playerDragon.currentTableData.GROWTH_MULT;

        float finalDamage = damage - (currentEnemy.MONDEF * 0.3f);
        finalDamage = Mathf.Max(1, finalDamage);

        enemyCurrentHP -= finalDamage;
        enemyCurrentHP = Mathf.Max(0, enemyCurrentHP);

        skillCooldown = dragonSpecialSkill.SKILL_CD;
        playerDragon.stats.ChangeStat(StatType.Fatigue, 15);

        UpdateUI();

        if (enemyCurrentHP <= 0)
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
        yield return new WaitForSeconds(1f);

        SkillTableData chosenSkill = Random.Range(0f, 1f) > 0.7f ? enemySpecialSkill : enemyAttackSkill;

        float baseDamage = chosenSkill.SKILL_POWER;
        float damage = baseDamage + (currentEnemy.MONATT * 0.3f);

        float finalDamage = damage / playerDragon.currentTableData.DEF_MULT;
        finalDamage = Mathf.Max(1, finalDamage);

        playerDragon.stats.ChangeStat(StatType.Stamina, -finalDamage);

        UpdateUI();

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

    private IEnumerator BattleEnd(bool playerWon)
    {
        if (playerWon)
        {
            currentState = BattleState.Won;

            int expReward = 25 * currentEnemy.MON_TYPE;
            playerDragon.GainExperience(expReward);

            int coinReward = currentEnemy.MONHP;
            GameManager.Instance.playerManager.coin += coinReward;
            GameManager.Instance.playerManager.UpdateCoinUI();
        }
        else
        {
            currentState = BattleState.Lost;

            playerDragon.stats.ChangeStat(StatType.Fatigue, 20);
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

        if (currentEnemy != null)
        {
            enemyHPSlider.value = enemyCurrentHP / currentEnemy.MONHP;
        }

        //if (skillCooldown > 0)
        //{
        //    skillButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{dragonSpecialSkill.SKILL_NAME} ({skillCooldown})";
        //}
        //else
        //{
        //    skillButton.GetComponentInChildren<TextMeshProUGUI>().text = dragonSpecialSkill.SKILL_NAME;
        //}
    }

    private float PlayerDamage(float skill)
    {
        float combatPower = playerDragon.stats.experience * playerDragon.currentTableData.GROWTH_MULT;

        float damage = combatPower * playerDragon.currentTableData.ATT_MULT;

        return damage * skill;
    }

    private float MonsterDamage(float skill)
    {
        return monster.attack * skill;
    }


    private void SetButtonsInteractable(bool interactable)
    {
        attackButton.interactable = interactable;
        skillButton.interactable = interactable && skillCooldown <= 0 && currentState == BattleState.PlayerTurn;
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