using UnityEngine;
using System.Collections.Generic;

public class WorldSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public EnemyBase[] enemyPrefabs;
    public WeaponPickup[] weaponPickupPrefabs;

    [Header("Spawn Points")]
    public SpawnPoint[] enemySpawns;
    public SpawnPoint[] weaponSpawns;

    [Header("Counts")]
    [Min(1)] public int enemiesPerType = 1;     // e.g., 1–3
    [Min(1)] public int pickupsPerType = 1;     // e.g., 1

    [Header("Tracking (polymorphic)")]
    public List<EnemyBase> liveEnemies = new();
    public List<WeaponPickup> livePickups = new();

    void Start()
    {
        SpawnAll();
    }

    public void SpawnAll()
    {
        // Enemies
        foreach (var prefab in enemyPrefabs)
        {
            for (int i = 0; i < enemiesPerType; i++)
            {
                var sp = enemySpawns[Random.Range(0, enemySpawns.Length)];
                var pos = sp ? sp.GetRandomPoint() : Vector3.zero;
                var rot = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);
                var e = Instantiate(prefab, pos, rot);
                liveEnemies.Add(e);
            }
        }

        // Weapon pickups
        foreach (var prefab in weaponPickupPrefabs)
        {
            for (int i = 0; i < pickupsPerType; i++)
            {
                var sp = weaponSpawns[Random.Range(0, weaponSpawns.Length)];
                var pos = sp ? sp.GetRandomPoint() : Vector3.zero;
                var rot = Quaternion.identity;
                var p = Instantiate(prefab, pos, rot);
                livePickups.Add(p);
            }
        }

        Debug.Log($"[WorldSpawner] Spawned {liveEnemies.Count} enemies, {livePickups.Count} pickups.");
    }
}
