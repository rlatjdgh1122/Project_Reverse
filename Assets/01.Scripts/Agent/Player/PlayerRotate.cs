using System;
using System.Collections;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [Header("�÷��̾� �� �Ʒ� ����")]
    [Range(30, 85)] public int AngleXUpLimit = 80;
    [Range(30, 85)] public int AngleXDownLimit = 45;
    [Header("�÷��̾� ȭ�� ����")]
    [SerializeField] private float sensitivity = 2.0f; // ���콺 ����
    [SerializeField] private Transform _playerTrm;

    private float _eulerAngleX = 0;
    private float _eulerAngleY = 0;
    private float EulerAngleY
    {
        get => _eulerAngleY;
        set
        {
            _eulerAngleY = value % 360; // ���� 360���� ���� �������� ����Ͽ� ������ 0���� 359������ ������ ����ȭ�մϴ�.
            if (_eulerAngleY > 180)
                _eulerAngleY -= 360; // ���� 180�� �Ѿ�� -180���� �����ϵ��� �����մϴ�.
            else if (_eulerAngleY < -180)
                _eulerAngleY += 360; // ���� -180���� �۾����� 180���� �����ϵ��� �����մϴ�.
        }
    }

    private Coroutine _rotCorou = null;

    public void OnRotateEvent(float mouseX, float mouseY)
    {
        _eulerAngleX -= mouseY * sensitivity;
        EulerAngleY += mouseX * sensitivity;

        _eulerAngleX = ClampAngle(_eulerAngleX);

        _playerTrm.localRotation = Quaternion.Euler(0, EulerAngleY, 0); //�÷��̾�� �¿�
        transform.localRotation = Quaternion.Euler(_eulerAngleX, EulerAngleY, 0); //ī�޶� ����
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
