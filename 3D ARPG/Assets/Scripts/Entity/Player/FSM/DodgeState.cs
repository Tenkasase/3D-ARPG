using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : PlayerState
{
    private float startSpeed;
    private float dodgeTimer;
    private Vector3 fixedDodgeDir;
    public DodgeState(Player _player, PlayerStateMachine _stateMachine) : base(_player, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.animator.SetTrigger("Dodge");

        // 冷却开始
        dodgeTimer = player.dodgeDuration;
        startSpeed = player.dodgeSpeed;
        
        // 闪避方向
        Vector2 moveInput = player.playerInput.MoveInput;
        if (moveInput.magnitude >= 0.1f)
        {
            Vector3 inputDir =  new Vector3(moveInput.x, 0, moveInput.y).normalized;
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + player.cameraTransform.eulerAngles.y;
            
            player.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            fixedDodgeDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            fixedDodgeDir = -player.transform.forward;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public override void Update()
    {
        base.Update();
        
        // 冷却时间
        dodgeTimer -= Time.deltaTime;
        
        float normalizedTime = 1f - (dodgeTimer / player.dodgeDuration);// 计算当前时间在闪避持续时间中的归一化值（0到1之间）
        float currentSpeed = Mathf.Lerp(startSpeed, 0f, normalizedTime);// 线性插值计算当前速度，随着时间推移逐渐减慢

        player.cc.SimpleMove(fixedDodgeDir * currentSpeed);
        
        // 闪避结束
        if (dodgeTimer <= 0f)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
        
    }
}
