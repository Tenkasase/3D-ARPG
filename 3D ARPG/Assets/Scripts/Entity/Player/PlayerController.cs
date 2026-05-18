using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private PlayerInput playerInput;
    private Animator animator;
    private Transform cameraTransform; 

    [Header("Speed")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 5f;

    [Header("Rotation")]
    public float rotationSmoothTime = 0.1f;
    private float currentRotationVelocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Vector3 inputDir = new Vector3(playerInput.MoveInput.x, 0, playerInput.MoveInput.y).normalized;

        float targetSpeed = playerInput.IsRunning ? runSpeed : walkSpeed;

        // 如果玩家有键盘输入，才进行转向和移动；否则直接停下来
        if (inputDir.magnitude >= 0.1f)
        {
            // 计算目标角度：根据输入方向和摄像机朝向计算出角色应该面向的世界角度
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            // 丝滑旋转
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
}
