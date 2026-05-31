using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerState
{
    private int comboCount = 0;

    private bool canCombo = false;
    private bool wantsToCombo = false;// 连招意愿，攻击预输入
    
    public AttackState(Player _player, PlayerStateMachine _stateMachine) : base(_player, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.animator.SetTrigger("Attack");
        
        player.animator.SetInteger("ComboCount", comboCount);
        
        canCombo = false;
        wantsToCombo = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        Debug.Log("处于AttackState！！！");
        
        // 禁用移动
        player.cc.SimpleMove(Vector3.zero);
        

        // 检测攻击预输入
        if (player.playerInput.IsAttacking)
        {
            wantsToCombo = true;
            if(canCombo)
                TriggerNextCombo();
        }
    }

    // 开启连击窗口
    public void OnEnableComboWindow()
    {
        canCombo = true;
        if (wantsToCombo)
        {
            TriggerNextCombo();
        }
    }

    // 关闭连击窗口
    public void OnDisableComboWindow()
    {
        comboCount = 0;
        stateMachine.ChangeState(player.idleState);
    }

    public void TriggerNextCombo()
    {
        comboCount = (comboCount + 1) % 3;
        stateMachine.ChangeState(player.attackState);
    }
}
