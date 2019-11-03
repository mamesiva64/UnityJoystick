using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlpacaTech;

public class JoystickSample : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpVelocity = 20f;


    void Update()
    {
        var rigidbody = GetComponent<Rigidbody>();
        var velocity = rigidbody.velocity;

        //  Input.Axis        
        var leftStick = JoystickManager.GetAxis();
        velocity.x = leftStick.x * moveSpeed * Time.deltaTime;

        //  Input.GetButton
        if (JoystickManager.GetButtonDown(0))
        {
            velocity.y = jumpVelocity;
        }

        rigidbody.velocity = velocity;

        
    }
}
