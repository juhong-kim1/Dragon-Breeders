using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public float stamina;
    public float attack;
    public float defense;

   public MonsterTableData monsterTable;

    private void Start()
    {
        stamina = monsterTable.MONHP;
        attack = monsterTable.MONATT;
        defense = monsterTable.MONDEF;
    }


}
