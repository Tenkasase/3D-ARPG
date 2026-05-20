using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Components
    [HideInInspector] public CharacterController cc;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform cameraTransform;

    #endregion
    
    #region Infos

    [Header("Move Info")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 5f;
    public float rotationSmoothTime = 0.1f;
    [HideInInspector] public float currentRotationVelocity;

    [Header("Dodge Info")]
    public float dodgeSpeed = 10f;    // 闪避时的瞬时速度
    public float dodgeDuration = 1.2f; // 闪避持续时间（需要和动画长度大致匹配）

    #endregion

    #region States

    public PlayerStateMachine  stateMachine { get; private set; }
    
    public IdleState  idleState { get; private set; }
    public MoveState  moveState { get; private set; }
    public DodgeState  dodgeState { get; private set; }

    #endregion

    private void Awake()
    {
        PlayerManager.Instance.player = this;// 自注册
        
        // 初始化获取组件
        cc = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        cameraTransform = Camera.main.transform;

        stateMachine = new PlayerStateMachine();
        
        // 实例化状态
        idleState = new IdleState(this, stateMachine);
        moveState = new MoveState(this, stateMachine);
        dodgeState = new DodgeState(this, stateMachine);
    }

    private void Start()
    { 
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;// 游戏暂停时不更新状态
        
        stateMachine.currentState.Update();
    }
    
    /// <summary>
    /// 动画触发器接口，供状态调用以触发动画
    /// </summary>
    /// <param name="animationTrigger"></param>
    public void AnimationTrigger(string animationTrigger) => animator.SetTrigger(animationTrigger);
}
