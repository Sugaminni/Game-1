using System.Collections.Generic;
using UnityEngine;

public class WorldRegistry : MonoBehaviour
{
    public static WorldRegistry I;
    public readonly List<EnemyBase> enemies = new();
    public readonly List<WeaponBase> worldWeapons = new();

    // Initializes world registry
    void Awake(){ I = this; }
    public void Register(EnemyBase e)   { if (!enemies.Contains(e)) enemies.Add(e); }
    public void Unregister(EnemyBase e) { enemies.Remove(e); }
    public void Register(WeaponBase w)  { if (!worldWeapons.Contains(w)) worldWeapons.Add(w); }
    public void Unregister(WeaponBase w){ worldWeapons.Remove(w); }
}
