using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float defaultShakeIntensity = 0.3f; // ��Ҥ����ç�������
    public float defaultShakeDuration = 0.5f;  // ������������������

    private Vector3 originalPosition; // ���˹�������鹢ͧ���ͧ
    private float shakeTimer = 0f;    // ��ǨѺ����
    private float currentShakeIntensity; // �����ç�Ѩ�غѹ

    private void Start()
    {
        // �纵��˹�������鹢ͧ���ͧ
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            // ������â�Ѻ�᡹ X ��� Y ��������ç�ͧ������
            float offsetX = Random.Range(-1f, 1f) * currentShakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * currentShakeIntensity;

            // �ѻവ���˹觡��ͧ
            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            // Ŵ���ҷ�������
            shakeTimer -= Time.deltaTime;

            // ������������ �׹���˹觡��ͧ��Ѻ
            if (shakeTimer <= 0)
            {
                transform.localPosition = originalPosition;
            }
        }
    }

    // �ѧ��ѹ����������� (Ẻ��˹������)
    public void StartShake(float intensity, float duration)
    {
        currentShakeIntensity = intensity; // ��˹������ç
        shakeTimer = duration;             // ��˹���������
    }

    // �ѧ��ѹ����������� (Ẻ�����������)
    public void StartShake()
    {
        StartShake(defaultShakeIntensity, defaultShakeDuration);
    }
}
