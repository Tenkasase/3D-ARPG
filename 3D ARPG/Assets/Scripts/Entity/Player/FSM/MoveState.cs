using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    private Vector2 moveInput;
    private Vector3 moveDir;
    
    public MoveState(Player _player, PlayerStateMachine _stateMachine) : base(_player, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
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
        if(player.playerInput.IsDodge)
        {
            stateMachine.ChangeState(player.dodgeState);
            return;
        }
        
        // 攻击
        if (player.playerInput.IsAttacking)
        {
            stateMachine.ChangeState(player.attackState);
            return;
        }
        
        // 没有移动输入
        if(moveInput.magnitude < 0.1f)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
        
        Calculate3DMovement();
    }

    /// <summary>
    /// 计算移动
    /// </summary>
    private void Calculate3DMovement()
    {
        float targetSpeed = player.playerInput.IsRunning ? player.runSpeed : player.walkSpeed;
        
        // 将输入转换为世界坐标系中的方向
        Vector3 inputDir = new  Vector3(moveInput.x, 0, moveInput.y).normalized;
        float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + player.cameraTransform.transform.eulerAngles.y;
        
        // 平滑旋转角色朝向目标角度
        float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref player.currentRotationVelocity, player.rotationSmoothTime);
        player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
        // 计算移动方向
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        
        player.cc.SimpleMove(moveDir * targetSpeed);
        
        player.animator.SetFloat("verticalSpeed", targetSpeed, 0.1f, Time.deltaTime);
    }
}
