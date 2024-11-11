using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerGravity : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private float gravityPower;

    [SerializeField] private float rev_castingTime = 1f;
    [SerializeField] private UnityEvent<float> OnRotateEvent = null;

    private Coroutine rotCoroutine = null;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = transform.root.GetComponent<Rigidbody>();

        OnRegister();
    }


    private void Start()
    {
        _rb.useGravity = false;
    }

    private void OnRegister()
    {
        SignalHub.OnJumpEventHandled += OnJump;
    }

    private void OnDestroy()
    {
        SignalHub.OnJumpEventHandled -= OnJump;
    }

    private void FixedUpdate()
    {
        if (_rb != null)
        {
            _rb.AddForce(-transform.root.up * gravityPower, ForceMode.Acceleration);
        }
    }

    private void OnJump(float power)
    {
        _rb.AddForce(transform.root.up * (power), ForceMode.Impulse); // 순간적인 힘
    }

    /// <summary>
    /// 이벤트 함수
    /// </summary>
    /// <param name="value"></param>
    public void OnPlayerReverseGravity(KeyCode value)
    {
        Quaternion getDir = GetReverseDirection(value);

        if (rotCoroutine != null)
        {
            StopCoroutine(rotCoroutine);
        }
        rotCoroutine = StartCoroutine(RotateCoroutine(getDir));
        OnRotateEvent?.Invoke(rev_castingTime);
    }

    private IEnumerator RotateCoroutine(Quaternion endPivotRot)
    {
        float t = 0;
        Quaternion startPivotRot = pivot.rotation;

        while (t < rev_castingTime)
        {
            t += Time.deltaTime;
            float progress = t / rev_castingTime;
            pivot.rotation = Quaternion.Slerp(startPivotRot, endPivotRot, progress);
            yield return null;
        }

        pivot.rotation = endPivotRot;
    }

    private Quaternion GetReverseDirection(KeyCode value)
    {
        Vector3Int getRotDir = GetRotationDirection();
        Quaternion rotY = Quaternion.Euler(getRotDir);
        Quaternion targetRotation = (pivot.rotation * rotY);

        transform.localRotation = Quaternion.Euler(0, 0, 0);

        Vector3 localRight = Vector3.right;
        Vector3 localForward = Vector3.forward;

        targetRotation *= value switch
        {
            KeyCode.W => Quaternion.AngleAxis(-90, localRight),
            KeyCode.A => Quaternion.AngleAxis(-90, localForward),
            KeyCode.S => Quaternion.AngleAxis(90, localRight),
            KeyCode.D => Quaternion.AngleAxis(90, localForward),
            KeyCode.None => Quaternion.AngleAxis(180, Vector3.right),
            _ => default,
        };

        return targetRotation;
    }

    private List<int> values = new List<int>()
    {0, 90, 180, 270, 360 };
    private Vector3Int GetRotationDirection()
    {
        int playerY = (int)(transform.localRotation.eulerAngles.y);

        if (playerY < 0)
        {
            playerY = 360 - playerY;
        }
        int value = values.OrderBy(x => Mathf.Abs(playerY - x)).First();

        if (value == 360)
            value = 0;

        return new Vector3Int(0, value, 0);
    }
}