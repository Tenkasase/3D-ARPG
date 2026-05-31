using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态基类接口
/// </summary>
public abstract class PlayerState
{
    protected Player player;
    protected PlayerStateMachine  stateMachine;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine)
    {
        player = _player;
        stateMachine = _stateMachine;
    }
    
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }

    // 动画结束事件触发
    public virtual void AnimationFinishedTrigger() { }
}