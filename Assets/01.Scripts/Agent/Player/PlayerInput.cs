using UnityEngine;
using UnityEngine.Events;

// 움직이는 방향으로 회전하는데 
// 만약 움직이지 않는다면 반대로 회전
public class PlayerInput : MonoBehaviour
{
    [SerializeField] Transform _tmr = null;
    [Header("플레이어 설정")]
    [SerializeField] private KeyCode rev_key = KeyCode.G; //키
    [SerializeField] private float rev_coolTime = 3f; //쿨타임

    [Header("연결 이벤트")]
    [SerializeField] private UnityEvent<KeyCode> OnReverseGravityEvent = null;
    [SerializeField] private UnityEvent<float, float> OnRotateEvent = null;
    [SerializeField] private UnityEvent<Vector3> OnMovementEvent = null;
    [SerializeField] private UnityEvent OnJumpEvent = null;

    #region 프로퍼티 변수
    #endregion

    #region 일반 변수
    public Vector2Int ipt_value = Vector2Int.zero;
    public KeyCode iptKey = KeyCode.None;

    private float rev_curTime = 0f;
    private Vector3 moveDirection = Vector3.zero;
    #endregion

    private void Update()
    {
        //점프 처리
        PlayerJump();
        //플레이어 중력 반전
        PlayerReverseGravity();
        //움직임 처리
        PlayerMovement();
        // 회전 처리
        PlayerRotate();
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpEvent?.Invoke();
        }
    }

    private void PlayerReverseGravity()
    {
        if (Mathf.Abs(ipt_value.x) != Mathf.Abs(ipt_value.y))
        {
            if (0 < ipt_value.y) //앞
            {
                iptKey = KeyCode.W;
            }
            else if (0 > ipt_value.y) //뒤
            {
                iptKey = KeyCode.S;

            }
            else if (0 < ipt_value.x) //오
            {
                iptKey = KeyCode.D;
            }
            else if (0 > ipt_value.x) //왼
            {
                iptKey = KeyCode.A;
            }
        }
        else if (Mathf.Abs(ipt_value.x) + Mathf.Abs(ipt_value.y) == 0) //가만히 있을때
            iptKey = KeyCode.None;
        else //대각선으로 이동할때는 작동안하게
            iptKey = KeyCode.PageUp;

        rev_curTime += Time.deltaTime;
        if (rev_curTime > rev_coolTime
            && Input.GetKeyDown(rev_key))
        {
            OnReverseGravityEvent?.Invoke(iptKey);
            rev_curTime = 0f;
        }
    }

    private void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        ipt_value = new Vector2Int((int)horizontal, (int)vertical);

        // 이동 방향 설정 (카메라 보는 기준)
        moveDirection =
            (_tmr.right * horizontal
            + _tmr.forward * vertical).normalized;

        OnMovementEvent?.Invoke(moveDirection);
    }

    private void PlayerRotate()
    {
        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        OnRotateEvent?.Invoke(mouseX, mouseY);
    }

}
