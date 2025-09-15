using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName= "Weapon/Creat Weapon")]


public class WeaponData : ScriptableObject{
    public GameObject prefab_Weapon;
    public int ammo_pack;
    public int bullet;
    public int damage;
}
