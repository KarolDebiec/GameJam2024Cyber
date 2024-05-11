using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationVariablesChange : MonoBehaviour
{

    [SerializeField] Animator animator;
    //[SerializeField] GameController Player;

    public void StopAttacking()
    {
        animator.SetBool("isAttacking", false);
    }


}
