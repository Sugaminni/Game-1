using UnityEngine;

public class RifleWeapon : WeaponBase
{
    [Range(0f, 5f)]
    public float spread = 0.75f;

    // Implements firing logic for the rifle weapon
    public override void Use()
    {
        if (!CanFire()) return;
        SpawnFromCameraForward(pellets: 1, spreadDeg: spread);
    }
}
