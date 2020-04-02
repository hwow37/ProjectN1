using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public Transform attackPoint;
    public GameObject attackEffect;
    public GameObject attackEffect_L;

    public void Attack()
    {
        GameObject effect = Instantiate(attackEffect, attackPoint.position, attackPoint.rotation);
    }

    public void Attack_L()
    {
        GameObject effect = Instantiate(attackEffect_L, attackPoint.position, attackPoint.rotation);
    }
}
