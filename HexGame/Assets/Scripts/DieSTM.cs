using System;
using UnityEngine;

public class DieSTM : StateMachineBehaviour
{
    public event Action<Animator> DieAnimationComplete;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DieAnimationComplete?.Invoke(animator);
    }
}
