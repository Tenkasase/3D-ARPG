using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private PlayerInput playerInput;
    private Animator animator;
    private Transform cameraTransform;

    private Vector3 inputDir;

    [Header("SpeedInfo")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 5f;

    [Header("RotationInfo")]
    public float rotationSmoothTime = 0.1f;
    private float currentRotationVelocity;

    [Header("DodgeInfo")]
    public float dodgeSpeed = 10f;    // 闪避时的瞬时速度
    public float dodgeDuration = 1.2f; // 闪避持续时间（需要和动画长度大致匹配）
    private bool isDodging = false;    // 闪避状态锁

    void Start()
    {
        cc = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // 获取输入方向
        inputDir = new Vector3(playerInput.MoveInput.x, 0, playerInput.MoveInput.y).normalized;

        // 如果正在闪避中，不执行常规移动和旋转逻辑（状态锁定）
        if (isDodging) return;

        // 处理闪避触发
        if (playerInput.IsDodge)
        {
            StartCoroutine(DodgeRoutine());
            return; // 开启闪避协程后，本帧不再往下走
        }


        HandleMovement();
    }

    // 移动和旋转逻辑
    private void HandleMovement()
    {        
        float targetSpeed = playerInput.IsRunning ? runSpeed : walkSpeed;

        if (inputDir.magnitude >= 0.1f)
        {
            // 计算目标角度：根据输入方向和相机朝向来决定角色面向哪个方向
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            // 平滑旋转到目标角度
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cc.SimpleMove(moveDir * targetSpeed);

            animator.SetFloat("horizontalSpeed", 0f, 0.1f, Time.deltaTime);
            animator.SetFloat("verticalSpeed", targetSpeed, 0.1f, Time.deltaTime);
        }
        else
        {
            cc.SimpleMove(Vector3.zero);
            animator.SetFloat("horizontalSpeed", 0f, 0.1f, Time.deltaTime);
            animator.SetFloat("verticalSpeed", 0f, 0.1f, Time.deltaTime);
        }
    }

    // 闪避逻辑协程
    private IEnumerator DodgeRoutine()
    {
        isDodging = true;

        // 确定闪避方向：如果有按键，往按键方向闪；如果没按键，向后闪避
        Vector3 dodgeDir;

        if (inputDir.magnitude > 0.1f)
        {
            // 基于相机视角计算闪避方向
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            dodgeDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // 闪避瞬间也可以让角色直接转过去
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        }
        else
        {
            dodgeDir = -transform.forward;
        }

        // 2. 设置 Animator 参数，告知 2D 混合树闪避的方向
        // 闪避混合树根据 horizontalSpeed 和 verticalSpeed 来决定前后左右闪
        // 这里需要把输入转化为相对于角色本地的方向
        Vector3 localInputDir = transform.InverseTransformDirection(dodgeDir);
        animator.SetFloat("horizontalSpeed", localInputDir.x);
        animator.SetFloat("verticalSpeed", localInputDir.z);

        // 3. 触发闪避动画
        animator.SetTrigger("Dodge");

        // 4. 在持续时间内执行位移
        float elapsed = 0f;
        while (elapsed < dodgeDuration)
        {
            // 使用 Move 而不是 SimpleMove，因为闪避通常不需要自动处理重力
            cc.Move(dodgeDir * dodgeSpeed * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null; // 等待下一帧
        }

        isDodging = false;
    }
}