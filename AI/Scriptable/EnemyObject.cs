using System.Collections;
using System.Collections.Generic;
using BlackPearl;
using UnityEngine;
[CreateAssetMenu(menuName = "BlackPearl/AI/Create Enemy")]
public class EnemyObject : ScriptableObject
{
    [Header("Attributes")]
    public float maxHealth = 100;
    public float damage;
    public float wanderSpeed;
    public float chaseSpeed;
    public float rotationSpeed;

    [Header("Animation")]
    public string deathAnimation = "Death";
    public string HitAnimation = "Hit";
    public AnimationClip[] attackAnimPossibility;

    [Header("Prefab")]
    public GameObject hitPrefab = null;
    public GameObject projectilePrefab = null;
    public GameObject UiObject;



}

public enum EnemyType
{
    Bear,
    Human,
    Monster,
    Skeleton,
    Wizard
}

public enum DamageType
{
    Range,
    Melee,
    SwordAndShield
}
