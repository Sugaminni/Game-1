using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public Transform weaponMount;  // where weapons are parented
    public List<WeaponBase> owned = new();

    void Awake()
    {
        if (weaponMount == null) weaponMount = transform;
        RefreshOwned("Awake");
    }

    public void AddAndSelect(WeaponBase w, WeaponSwitcher switcher)
    {
    if (w == null) return;
    owned.Add(w);
    if (switcher != null)
        switcher.EquipIndex(owned.Count - 1); // select the newly added weapon
    }

    // Refresh the list of owned weapons
    public void RefreshOwned(string from = "")
    {
        owned.Clear();
        var root = weaponMount ? weaponMount : transform;
        owned.AddRange(root.GetComponentsInChildren<WeaponBase>(true)); // include inactive
        Debug.Log($"[WeaponInventory] {from}: found {owned.Count} weapon(s) under '{root.name}'.");
    }

    // Registers a weapon into the inventory
    public WeaponBase RegisterWeapon(WeaponBase w)
    {
        if (!owned.Contains(w)) owned.Add(w);
        w.transform.SetParent(weaponMount, false);
        w.gameObject.SetActive(false);   // don't show until equipped
        return w;
    }

    // Adds a weapon to the inventory
    public void Add(WeaponBase prefab)
    {
        var w = Instantiate(prefab, weaponMount.position, weaponMount.rotation, weaponMount);
        w.gameObject.SetActive(false);
        owned.Add(w);
        WorldRegistry.I?.Unregister(w);
    }
}
