using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public WeaponInventory inventory;
    public CrosshairUI crosshair;
    int index;

    void Awake()
    {
        if (!inventory) inventory = GetComponentInChildren<WeaponInventory>(true);
    }

    // Initialize starting weapon
    void Start()
    {
        inventory?.RefreshOwned("Switcher.Start");
        if (inventory && inventory.owned.Count > 0) EquipIndex(0);
    }

    public void EquipNext() => EquipIndex(index + 1);
    public void EquipPrev() => EquipIndex(index - 1);

    // Equip weapon by inventory index
    public void EquipIndex(int idx)
    {
        if (inventory == null || inventory.owned.Count == 0) return;

        var old = Current(); if (old != null) old.OnFired = null;

        if (idx < 0) idx = inventory.owned.Count - 1;
        if (idx >= inventory.owned.Count) idx = 0;
        index = idx;

        for (int i = 0; i < inventory.owned.Count; i++)
        {
            var w = inventory.owned[i];
            if (w) w.gameObject.SetActive(i == index);
        }

        var wnew = Current();
        if (wnew != null && crosshair != null) wnew.OnFired += () => crosshair.Kick();

    }

    // Get currently equipped weapon
    public WeaponBase Current()
    {
        if (inventory == null || inventory.owned.Count == 0) return null;
        return inventory.owned[Mathf.Clamp(index, 0, inventory.owned.Count - 1)];
    }
}
