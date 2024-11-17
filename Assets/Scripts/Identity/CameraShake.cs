using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float defaultShakeIntensity = 0.3f; // ค่าความแรงเริ่มต้น
    public float defaultShakeDuration = 0.5f;  // ค่าระยะเวลาเริ่มต้น

    private Vector3 originalPosition; // ตำแหน่งเริ่มต้นของกล้อง
    private float shakeTimer = 0f;    // ตัวจับเวลา
    private float currentShakeIntensity; // ความแรงปัจจุบัน

    private void Start()
    {
        // เก็บตำแหน่งเริ่มต้นของกล้อง
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            // สุ่มการขยับในแกน X และ Y ตามความแรงของการสั่น
            float offsetX = Random.Range(-1f, 1f) * currentShakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * currentShakeIntensity;

            // อัปเดตตำแหน่งกล้อง
            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            // ลดเวลาที่เหลือ
            shakeTimer -= Time.deltaTime;

            // เมื่อหมดเวลา คืนตำแหน่งกล้องกลับ
            if (shakeTimer <= 0)
            {
                transform.localPosition = originalPosition;
            }
        }
    }

    // ฟังก์ชันเริ่มการสั่น (แบบกำหนดค่าได้)
    public void StartShake(float intensity, float duration)
    {
        currentShakeIntensity = intensity; // กำหนดความแรง
        shakeTimer = duration;             // กำหนดระยะเวลา
    }

    // ฟังก์ชันเริ่มการสั่น (แบบใช้ค่าเริ่มต้น)
    public void StartShake()
    {
        StartShake(defaultShakeIntensity, defaultShakeDuration);
    }
}
