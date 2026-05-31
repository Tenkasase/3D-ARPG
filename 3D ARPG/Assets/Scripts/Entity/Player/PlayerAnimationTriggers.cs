using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => PlayerManager.Instance.player;

    /// <summary>
    /// 禁用移动
    /// </summary>
    private void DisableMove()
    {
        player.cc.SimpleMove(Vector3.zero);
    }

    private void AnimEvent_EnableCombo()
    {
        // 调用player的OnEnableComboWindow方法
        player.AnimEvent_EnableCombo();
    }

    private void AnimEvent_DisableCombo()
    {
        player.AnimEvent_DisableCombo();
    }
}
