using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsDodge { get; private set; }

    [Header("ShiftInfo")]
    [SerializeField] private float sprintHoldThreshold = 0.2f; // 长按超过 0.2 秒算作冲刺
    private float shiftPressedTime; // 记录 Shift 按下的时间点
    private bool hasTriggeredDodge; // 标志位：本次长按是否已经触发了冲刺

    [Header("DodgeInfo")]
    [SerializeField] private float dodgeCooldown = 1.0f; // 闪避冷却时间

    public bool CanDodge { get; private set; } // 暴露给外部看当前能不能闪避
    private float cooldownTimer;

    void Start()
    {
        // 游戏开始时，默认是可以闪避的
        CanDodge = true;
    }

    void Update()
    {
        // 读取基础移动轴
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        MoveInput = new Vector2(horizontal, vertical);
        
        HandleShiftInput();

        HandleDodgeCooldown();
    }

    // 处理 Shift 按键逻辑
    private void HandleShiftInput()
    {
        // 每一帧开始，先重置闪避触发信号
        IsDodge = false;

        // 【按下瞬间】
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftPressedTime = Time.time;
            hasTriggeredDodge = false;
        }

        // 【按住期间】
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // 如果按住的时间超过了阈值，且之前没触发过冲刺，并且玩家正在移动
            if (Time.time - shiftPressedTime >= sprintHoldThreshold && !hasTriggeredDodge && MoveInput.magnitude > 0.1f)
            {
                IsRunning = true;
                hasTriggeredDodge = true; // 锁定，防止重复判定
            }
        }

        // 【抬起瞬间】
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // 如果抬起时是短按（小于阈值），并且当前闪避 CD 已经好了
            if (Time.time - shiftPressedTime < sprintHoldThreshold && CanDodge)
            {
                IsDodge = true;
                CanDodge = false; // 关闭闪避开关，进入 CD
                cooldownTimer = dodgeCooldown; // 重置 CD 计时器
            }

            // 抬起键时，必须解除冲刺状态
            IsRunning = false;
            hasTriggeredDodge = false;
        }

        // 防呆：如果玩家冲刺跑着跑着突然松开了 WASD 停在原地，应该自动取消冲刺状态
        if (IsRunning && MoveInput.magnitude < 0.1f)
        {
            IsRunning = false;
        }
    }

    // 处理闪避冷却倒计时
    private void HandleDodgeCooldown()
    {
        // 如果当前不能闪避，说明正在冷却中，继续倒计时
        if (!CanDodge)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                CanDodge = true; // CD 结束，恢复闪避能力
            }
        }
    }
}