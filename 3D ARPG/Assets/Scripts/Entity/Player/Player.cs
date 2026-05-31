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

    [Header(("Attack Info"))] 
    public float comboDuration = 1f; // 连击持续时间，超过这个时间没有继续攻击就重置连击计数

    #endregion

    #region States

    public PlayerStateMachine  stateMachine { get; private set; }
    
    public IdleState  idleState { get; private set; }
    public MoveState  moveState { get; private set; }
    public DodgeState  dodgeState { get; private set; }
    public AttackState attackState  { get; private set; }

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
        attackState = new AttackState(this, stateMachine);
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

    #region 动画事件

    
    public void AnimEvent_EnableCombo()
    {
        if (stateMachine.currentState is AttackState attack)
        {
            attack.OnEnableComboWindow();
        }
    }

    public void AnimEvent_DisableCombo()
    {
        if (stateMachine.currentState is AttackState attack)
        {
            attack.OnDisableComboWindow();
        }
    }

    #endregion
}
