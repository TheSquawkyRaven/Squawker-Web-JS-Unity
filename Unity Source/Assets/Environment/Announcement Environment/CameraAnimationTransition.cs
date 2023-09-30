using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationTransition : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SN.@this.ENVIRONMENT is AnnouncementEnvironment AnEnv)
        {
            if (!AnEnv.animate || AnEnv.integers.Count == 0)
            {
                animator.SetInteger("XYZ", -1);
                return;
            }
            animator.SetInteger("XYZ", AnEnv.integers[Random.Range(0, AnEnv.integers.Count)]);
        }
    }

}
