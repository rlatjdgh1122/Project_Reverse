using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 3;
    [SerializeField] private float jumpPower = 3;

    private bool IsGround = true;
    public void OnMovementEvent(Vector3 value)
    {
        transform.Translate(value * MoveSpeed * Time.deltaTime, Space.World);
    }

    public void OnJumpEvent()
    {
        if (IsGround == true)
        {
            SignalHub.OnJumpEventHandled?.Invoke(jumpPower);

            IsGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {   
            IsGround = true;
        }
    }
}
