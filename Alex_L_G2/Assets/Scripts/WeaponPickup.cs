using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponBase weaponPrefab;

    // When player enters trigger, give them the weapon
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var root      = other.transform.root;
        var inventory = root.GetComponentInChildren<WeaponInventory>(true);
        var switcher  = root.GetComponentInChildren<WeaponSwitcher>(true);

        if (!inventory || !weaponPrefab) return;

        // Instantiate under the inventory's mount 
        Transform parent = inventory.weaponMount ? inventory.weaponMount : inventory.transform;
        var newWpn = Instantiate(weaponPrefab, parent);
        newWpn.transform.localPosition = Vector3.zero;
        newWpn.transform.localRotation = Quaternion.identity;

        // Track it
        if (!inventory.owned.Contains(newWpn))
            inventory.owned.Add(newWpn);

        // Select it
        if (switcher) switcher.EquipIndex(inventory.owned.Count - 1);

        Destroy(gameObject);
    }
}
