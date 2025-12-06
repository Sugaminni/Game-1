using UnityEngine;
using System.IO;

public class Vec3 { public float x, y, z; }

public class PlayerStart
{
    public Vec3 pos;
    public float yaw;
}

public class WeaponStart
{
    public string name;
    public Vec3 pos;
}

public class StartConfig
{
    public PlayerStart player;
    public WeaponStart[] weapons;
}

public class StartConfigLoader : MonoBehaviour
{
    public Transform playerRoot;      // assign Player object
    public GameObject[] pickupPrefabs; // assign Pistol/Rifle/Shotgun pickups

    public void ApplyFromFile()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "start_config.json");

        try
        {
            string json = File.ReadAllText(path);
            var cfg = JsonUtility.FromJson<StartConfig>(json);

            if (cfg == null)
                throw new System.Exception("Config parsed as null.");

            // Moves player
            playerRoot.position =
                new Vector3(cfg.player.pos.x, cfg.player.pos.y, cfg.player.pos.z);
            playerRoot.rotation =
                Quaternion.Euler(0f, cfg.player.yaw, 0f);

            // Spawns weapon pickups
            foreach (var w in cfg.weapons)
            {
                var prefab = System.Array.Find(
                    pickupPrefabs,
                    p => p.name.StartsWith(w.name)
                );

                if (prefab == null)
                {
                    Debug.LogWarning($"[StartConfig] No prefab found for {w.name}");
                    continue;
                }

                Vector3 pos = new Vector3(w.pos.x, w.pos.y, w.pos.z);
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"[StartConfig] Failed to load JSON: {ex.Message}");
        }
    }
}
