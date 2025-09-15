using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmo : MonoBehaviour
{
    public Text textammo;
    public WeaponData weaponData;

    void Update()
    {
        if (textammo != null && weaponData != null)
        {
            textammo.text = "Ammo " + weaponData.bullet;
        }
    }
}
