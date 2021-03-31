using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyInfo", menuName = "ScriptableObject/New Enemy Info")]
public class RogueSO : ScriptableObject
{
    public float health;
    public float attack;
    public float moveSpeed;
    public float chaseSpeed;
    public Animator animator;
}
