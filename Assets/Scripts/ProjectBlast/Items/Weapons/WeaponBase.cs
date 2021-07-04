using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class WeaponBase : ModdableItemBase
{
    [Header("Weapon Data")]
    [SerializeField] float roundsPerSecond = 3f;
    [SerializeField] float handling = 50f;
    [SerializeField] float reloadTime = 3f;
    [SerializeField] float damage = 50f;

    public float BaseDamage => damage;
    public float ReloadTime => reloadTime;
    public float Handling => handling;
    public float RoundsPerSecond => roundsPerSecond;

}
