using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{    
    public Vector2 MoveInput { get; private set; }
    public bool IsRunning { get; private set; }    

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        MoveInput = new Vector2(horizontal, vertical);
        IsRunning = Input.GetKey(KeyCode.LeftShift);
    }
}
