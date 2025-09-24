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
    public int difficulty;
    public List<int> regions;

    public float maxStamina;
    public float stamina;
    public float attack;
    public float defense;

   public MonsterTableData monsterTableData;

    public MonsterType monsterType;

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
}

