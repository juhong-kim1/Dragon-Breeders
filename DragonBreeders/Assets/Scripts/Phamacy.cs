using UnityEngine;

public class Pharmacy : MonoBehaviour
{
    public PharmacySlot[] pharmacySlots = new PharmacySlot[2]; // 감기약, 붕대

    private const int MEDICINE_ID = 5030601; // 감기약
    private const int BANDAGE_ID = 5031101;  // 붕대

    private const int MEDICINE_PRICE = 1000;
    private const int BANDAGE_PRICE = 1000;

    private void Start()
    {
        SetupPharmacy();
    }

    private void SetupPharmacy()
    {
        // 슬롯 0: 감기약 (질병 치료)
        var medicineItem = CreatePharmacyItem(MEDICINE_ID);
        if (medicineItem != null && pharmacySlots[0] != null)
        {
            pharmacySlots[0].SetItem(medicineItem, MEDICINE_PRICE, StatusType.Disease);
        }

        // 슬롯 1: 붕대 (부상 치료)
        var bandageItem = CreatePharmacyItem(BANDAGE_ID);
        if (bandageItem != null && pharmacySlots[1] != null)
        {
            pharmacySlots[1].SetItem(bandageItem, BANDAGE_PRICE, StatusType.Injury);
        }

        Debug.Log("약국 설정 완료!");
    }

    private IItem CreatePharmacyItem(int itemId)
    {
        var itemData = DataTableManger.ItemTable.Get(itemId);
        if (itemData == null)
        {
            Debug.LogError($"ItemTable에 ID {itemId} 없음");
            return null;
        }

        Item item = new Item
        {
            itemID = itemData.ITEM_ID,
            itemName = itemData.StringName,
            icon = itemData.SpriteIcon,
            description = itemData.StringDescription,
            itemType = itemData.ITEM_TYPE,
        };

        return item;
    }
}