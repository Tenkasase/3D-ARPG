using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    private Vector2 moveInput;
    public IdleState(Player _player, PlayerStateMachine _stateMachine) : base(_player, _stateMachine)
    {
    }

    public override void Enter()
    {
        
        base.Enter();
        player.cc.SimpleMove(Vector3.zero);
        player.animator.SetFloat("verticalSpeed", 0f);
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public override void Update()
    {
        base.Update();
        
        moveInput = player.playerInput.MoveInput;
        
        // 闪避
        if (player.playerInput.IsDodge)
        {
            stateMachine.ChangeState(player.dodgeState);
            return;
        }
        
        // 检测到移动输入
        if (moveInput.magnitude >= 0.1f)
        {
            stateMachine.ChangeState(player.moveState);
            return;
        }

    }

}
