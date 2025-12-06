using UnityEngine;

public class PistolWeapon : WeaponBase
{
    // Implements firing logic for the pistol weapon
    public override void Use()
    {
        if (!CanFire()) return;
        SpawnFromCameraForward(pellets: 1, spreadDeg: 0f);
    }
}
