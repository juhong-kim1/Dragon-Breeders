using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{ 
    Mushroom = 90101,
    StarTurtle = 90102,
    Aracne = 90103,
    Salamander = 90201,
    MoonBat = 90202,
    DutchManCrap = 90203,
    Manta = 90301,
    Ballock = 90302,
    DesertWarm = 90303,
}

public class MonsterHealth : MonoBehaviour
{
    private static readonly string start = "Start";
    private static readonly string skill1 = "Skill1";
    private static readonly string skill2 = "Skill2";
    private static readonly string die = "Die";
    private static readonly string win = "Win";

    public int difficulty;
    public List<int> regions;

    public float maxStamina;
    public float stamina;
    public float attack;
    public float defense;

   public MonsterTableData monsterTableData;

    private Animator animator;

    public MonsterType monsterType;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InitializeMonsterData();
    }

    public void InitializeMonsterData()
    {
        monsterTableData = DataTableManger.MonsterTable.Get((int)monsterType);
        if (monsterTableData == null)
        {
            Debug.LogError($"[MonsterHealth] MonsterTableData가 없습니다! monsterType: {monsterType}");
            return;
        }

        maxStamina = monsterTableData.MONHP;
        stamina = maxStamina;
        attack = monsterTableData.MONATT;
        defense = monsterTableData.MONDEF;
        difficulty = monsterTableData.MON_TYPE;

        Debug.Log($"[MonsterHealth] 몬스터 데이터 초기화 완료: {monsterType}");
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger(skill1);
    }

    public void PlaySkillAnimation()
    {
        animator.SetTrigger(skill2);
    }

    public void PlayStartAnimation()
    { 
        animator.SetTrigger(start);
    }

    public void PlayDieAnimation()
    {
        animator.SetTrigger(die);
    }

    public void PlayWinAnimation()
    {
        animator.SetTrigger(win);
    }
}