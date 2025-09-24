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
    private int difficulty;
    public List<int> regions;

    public float maxStamina;
    public float stamina;
    public float attack;
    public float defense;

   public MonsterTableData monsterTableData;

    public MonsterType monsterType;

    private void Start()
    {
        monsterTableData = DataTableManger.MonsterTable.Get((int)monsterType);

        maxStamina = monsterTableData.MONHP;
        stamina = maxStamina;
        attack = monsterTableData.MONATT;
        defense = monsterTableData.MONDEF;
        difficulty = monsterTableData.MON_TYPE;

    }
}
