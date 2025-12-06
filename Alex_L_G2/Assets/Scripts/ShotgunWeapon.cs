using UnityEngine;

public class ShotgunWeapon : WeaponBase
{
    [Header("Shotgun")]
    [Min(1)] public int pellets = 10;   // pellets per shot
    [Range(0f, 15f)] public float spread = 7f; // degrees cone
    public int pelletDamage = 8;        // damage applied per pellet

    public override void Use()
    {
        if (!CanFire()) return;

        // Temporarily override base.damage so each pellet uses pelletDamage
        int old = damage;
        damage = pelletDamage;
        SpawnFromCameraForward(pellets, spread);  
        damage = old;
    }
}
