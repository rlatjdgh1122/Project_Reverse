using System;
using System.Collections;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [Header("플레이어 위 아래 조절")]
    [Range(30, 85)] public int AngleXUpLimit = 80;
    [Range(30, 85)] public int AngleXDownLimit = 45;
    [Header("플레이어 화면 감도")]
    [SerializeField] private float sensitivity = 2.0f; // 마우스 감도
    [SerializeField] private Transform _playerTrm;

    private float _eulerAngleX = 0;
    private float _eulerAngleY = 0;
    private float EulerAngleY
    {
        get => _eulerAngleY;
        set
        {
            _eulerAngleY = value % 360; // 값을 360으로 나눈 나머지를 사용하여 각도를 0부터 359까지의 값으로 정규화합니다.
            if (_eulerAngleY > 180)
                _eulerAngleY -= 360; // 값이 180을 넘어가면 -180부터 시작하도록 조정합니다.
            else if (_eulerAngleY < -180)
                _eulerAngleY += 360; // 값이 -180보다 작아지면 180부터 시작하도록 조정합니다.
        }
    }

    private Coroutine _rotCorou = null;

    public void OnRotateEvent(float mouseX, float mouseY)
    {
        _eulerAngleX -= mouseY * sensitivity;
        EulerAngleY += mouseX * sensitivity;

        _eulerAngleX = ClampAngle(_eulerAngleX);

        _playerTrm.localRotation = Quaternion.Euler(0, EulerAngleY, 0); //플레이어는 좌우
        transform.localRotation = Quaternion.Euler(_eulerAngleX, EulerAngleY, 0); //카메라만 상하
    }

    public void OnReverseRot(float rev_castingTime)
    {

        //if (_rotCorou != null)
        //StopCoroutine(_rotCorou);

        //_rotCorou = StartCoroutine(RotateCoroutine(rev_castingTime));
    }

    private float ClampAngle(float angle)
    {
        if (angle < -360) angle += 360f;
        if (angle > 360) angle -= 360f;

        return Mathf.Clamp(angle, -AngleXUpLimit, AngleXDownLimit);
    }

    private IEnumerator RotateCoroutine(float castingTime)
    {
        float t = 0;
        float prevValue = EulerAngleY;
        float value = 0;

        while (t < castingTime)
        {
            t += Time.deltaTime;
            float progress = t / castingTime;
            EulerAngleY = Mathf.Lerp(prevValue, value, progress);
            yield return null;
        }
    }
}
