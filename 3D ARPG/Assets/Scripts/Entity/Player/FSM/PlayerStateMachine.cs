using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }
    
    /// <summary>
    /// 初始化状态
    /// </summary>
    /// <param name="_state"></param>
    public void Initialize(PlayerState _state)
    {
        currentState = _state;
        currentState.Enter();
    }
    
    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="_newState"></param>
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
