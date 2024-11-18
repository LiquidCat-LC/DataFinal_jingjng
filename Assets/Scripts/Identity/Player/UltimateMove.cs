using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateMove : MonoBehaviour
{
    public GameObject ultimateEffectPrefab; // Prefab ของ Ultimate Effect
    public GameObject swordEffectPrefab; // Prefab เอฟเฟกต์ฟันดาบ
    public CameraShake cameraShake; // อ้างอิง CameraShake

    public float fadeDuration = 1.0f; // ระยะเวลาในการเฟด 
    public float moveDuration = 1.0f; // ระยะเวลาในการเลื่อน Ultimate Effect
    public float waitBeforeSwordEffect = 3.0f; // เวลาก่อนแสดงฟันดาบ
    public float swordEffectDuration = 5.0f; // ระยะเวลาแสดงเอฟเฟกต์ฟันดาบ
    public Color bossHitColor = Color.red; // สีแดงของบอสเมื่อโดนฟัน

    private GameObject currentUltimateEffect; // ตัวอ้างอิงของ Ultimate Effect
    private GameObject currentSwordEffect; // ตัวอ้างอิงของ Sword Effect
    private List<SpriteRenderer> mapRenderers = new List<SpriteRenderer>(); // รายการ SpriteRenderer ในแผนที่

    private void Start()
    {
        CacheMapObjectsByTags(); // ค้นหาและเก็บวัตถุในแผนที่ตาม Tags
    }

    public void TriggerUltimateMove()
    {
        StartCoroutine(StartUltimateMove());
    }

    private IEnumerator StartUltimateMove()
    {

        if (ultimateEffectPrefab != null)
        {
            // ตั้งตำแหน่ง Ultimate Effect
            Vector3 ultimatePosition = transform.position + new Vector3(0, 0.5f, 0);
            currentUltimateEffect = Instantiate(ultimateEffectPrefab, ultimatePosition, Quaternion.identity);
            currentUltimateEffect.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            SpriteRenderer renderer = currentUltimateEffect.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = new Color(1f, 1f, 1f, 0f); // เริ่มต้นโปร่งใส
            }
        }

        float elapsedTime = 0f;
        Vector3 startPosition = currentUltimateEffect.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 0.5f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            currentUltimateEffect.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            SpriteRenderer renderer = currentUltimateEffect.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = new Color(1f, 1f, 1f, t); // ค่อย ๆ เฟดเข้า
            }

            yield return null;
        }

        // รอ 3 วินาทีก่อนเริ่มฟันดาบ
        yield return new WaitForSeconds(waitBeforeSwordEffect);

        // แสดง Sword Effect และโจมตีหลังจากมันจบ
        yield return StartCoroutine(ShowSwordEffect());

        // จบ Ultimate Move
        EndUltimateMove();
    }

    private void EndUltimateMove()
    {

        if (currentUltimateEffect != null)
        {
            Destroy(currentUltimateEffect);
        }

        if (currentSwordEffect != null)
        {
            Destroy(currentSwordEffect);
        }

        Debug.Log("Ultimate Move Finished!");
    }

    private IEnumerator ShowSwordEffect()
    {
        // ค้นหาบอสทุกตัวในฉาก (ใช้ Tag "Boss")
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");

        foreach (GameObject boss in bosses)
        {
            // สร้าง Sword Effect บนตำแหน่งของบอส
            if (swordEffectPrefab != null)
            {
                GameObject swordEffect = Instantiate(swordEffectPrefab, boss.transform.position, Quaternion.identity);
                Destroy(swordEffect, swordEffectDuration); // ลบ Sword Effect หลังจากหมดเวลา
            }

            // สั่นกล้องขณะโจมตีบอส
            if (cameraShake != null)
            {
                cameraShake.StartShake(0.2f, swordEffectDuration);
            }

            // เปลี่ยนสีบอสเป็นสีแดงเพื่อบ่งบอกว่ากำลังโดนโจมตี
            SpriteRenderer renderer = boss.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                StartCoroutine(ChangeBossColor(renderer));
            }
        }

        // รอจนกว่า Sword Effect จะจบ
        yield return new WaitForSeconds(swordEffectDuration);

        // โจมตีบอสทุกตัวหลัง Sword Effect จบ
        foreach (GameObject boss in bosses)
        {
            var bossScript = boss.GetComponent<OOPBob>();
            if (bossScript != null)
            {
                bossScript.Hit(); // บอสจะได้รับดาเมจหลัง Sword Effect
            }
        }
    }

    // ฟังก์ชันเปลี่ยนสีบอสเป็นแดงชั่วคราว
    private IEnumerator ChangeBossColor(SpriteRenderer renderer)
    {
        Color originalColor = renderer.color;
        renderer.color = bossHitColor; // เปลี่ยนเป็นสีแดง

        yield return new WaitForSeconds(swordEffectDuration); // รอจนกว่า Sword Effect จะจบ

        renderer.color = originalColor; // คืนค่าเป็นสีเดิม
    }

    private void CacheMapObjectsByTags()
    {
        // ค้นหา GameObjects ตาม Tags
        string[] tagsToCache = { "Item", "Map", "Enemy" };

        foreach (string tag in tagsToCache)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    mapRenderers.Add(renderer); // เพิ่ม SpriteRenderer ลงในรายการ
                }
            }
        }
    }
}
