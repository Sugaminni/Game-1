using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatInput : MonoBehaviour
{
    public WeaponSwitcher switcher;
    bool fireHeld;

    void Awake()
    {
        if (!switcher) switcher = GetComponentInChildren<WeaponSwitcher>(true);
    }

    // Input polling
    void Update()
    {
        if (Input.GetMouseButton(0)) fireHeld = true;
        else if (Mouse.current != null && !Mouse.current.leftButton.isPressed) fireHeld = false;

        var w = switcher ? switcher.Current() : null;
        if (fireHeld && w != null) w.Use();

        if (Input.GetKeyDown(KeyCode.E))  switcher?.EquipNext();
        if (Input.GetKeyDown(KeyCode.Q))  switcher?.EquipPrev();
        if (Input.GetKeyDown(KeyCode.Alpha1)) switcher?.EquipIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) switcher?.EquipIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) switcher?.EquipIndex(2);

        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0.2f)  switcher?.EquipNext();
        if (scroll < -0.2f) switcher?.EquipPrev();
    }

    // Input System event handlers
    void OnFire(InputValue v)   { fireHeld = v.isPressed; }
    void OnNextWeapon()         { switcher?.EquipNext(); }
    void OnPrevWeapon()         { switcher?.EquipPrev(); }
    void OnEquip1()             { switcher?.EquipIndex(0); }
    void OnEquip2()             { switcher?.EquipIndex(1); }
    void OnEquip3()             { switcher?.EquipIndex(2); }
}
