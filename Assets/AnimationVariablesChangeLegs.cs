using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationVariablesChangeLegs : MonoBehaviour
{
    [SerializeField] private Animator animator;


    public void StopAttackingLegs()
    {
        animator.SetBool("isAttacking", false);
    }
}
