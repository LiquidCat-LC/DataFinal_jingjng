using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateMove : MonoBehaviour
{
    public OOPPlayer _player;
    public GameObject ultimateEffectPrefab;
    public GameObject swordEffectPrefab;
    public GameObject blackCirclePrefab; // Prefab สำหรับเอฟเฟกต์วงกลมสีดำ
    public CanvasGroup screenFade; // CanvasGroup สำหรับหน้าจอสีดำ
    public CameraShake cameraShake;

    public Sprite floorSpriteDuringUltimate; // Sprite พิเศษสำหรับ Floor
    public Sprite wallSpriteDuringUltimate; // Sprite พิเศษสำหรับ Wall

    public float circleDuration = 1.5f; // ระยะเวลาในการขยายวงกลมดำ
    public float fadeDuration = 1.5f; // ระยะเวลาในการเฟดสีดำ
    public float waitDuration = 2.0f; // เวลาที่จะค้างจอดำ
    public float moveDuration = 1.0f; // ระยะเวลาในการเลื่อน Ultimate Effect
    public float waitBeforeSwordEffect = 2.5f; // เวลาก่อนแสดงฟันดาบ
    public float swordEffectDuration = 12.0f; // ระยะเวลาแสดงเอฟเฟกต์ฟันดาบ
    public Color bossHitColor = Color.red; // สีแดงเมื่อบอสโดนโจมตี

    private GameObject currentUltimateEffect;
    private GameObject currentBlackCircle; // วงกลมสีดำ
    private Dictionary<GameObject, Sprite> originalSprites = new Dictionary<GameObject, Sprite>(); // เก็บ Sprite เดิมของ Floor และ Wall
    private List<GameObject> removedObjects = new List<GameObject>(); // เก็บวัตถุที่ถูกลบ

    private void Start()
    {
        screenFade.gameObject.SetActive(true);
        if (screenFade != null)
        {
            screenFade.alpha = 0f; // ตั้งค่าเริ่มต้นให้โปร่งใส
        }
    }

    public void TriggerUltimateMove()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.ultimateSound);

        StartCoroutine(StartUltimateMove());
    }

    private IEnumerator StartUltimateMove()
    {
        // 1. แสดงวงกลมสีดำและเฟดหน้าจอพร้อมกัน
        yield return StartCoroutine(ExpandBlackCircleAndFadeScreen());

        // 2. ลบวัตถุและเปลี่ยน Sprite
        ModifySceneForUltimate(true);

        // 3. ค้างหน้าจอดำไว้ 2 วินาที
        yield return new WaitForSeconds(waitDuration);

        // 4. เฟดหน้าจอกลับมาโปร่งใส พร้อมเริ่ม Ultimate Effect ทันที
        StartCoroutine(FadeScreenToBlack(false)); // เฟดหน้าจอกลับโปร่งใสแบบไม่ต้องรอ
        yield return StartCoroutine(StartUltimateEffect());

        // 5. ก่อนคืนค่าสีหน้าจอ เฟดจอเป็นสีดำ
        yield return StartCoroutine(FadeScreenToBlack(true)); // เฟดไปสีดำ 0.5 วินาที
        yield return new WaitForSeconds(0.5f); // ค้างจอดำ 0.5 วินาที

        // 6. คืนค่าสีเดิม
        ModifySceneForUltimate(false);

        // 7. เฟดจอกลับมาเป็นโปร่งใส
        yield return StartCoroutine(FadeScreenToBlack(false));
    }


    private IEnumerator ExpandBlackCircleAndFadeScreen()
    {
        if (blackCirclePrefab != null && screenFade != null)
        {
            Vector3 startPosition = _player.transform.position;
            currentBlackCircle = Instantiate(blackCirclePrefab, startPosition, Quaternion.identity);

            float elapsedTime = 0f;
            float startScale = 0.1f; // เริ่มต้นขนาดเล็ก
            float targetScale = 20.0f; // ขยายจนเต็มหน้าจอ
            float startAlpha = 0f; // เริ่มต้นโปร่งใส
            float endAlpha = 1f; // สิ้นสุดไม่โปร่งใส

            while (elapsedTime < circleDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / circleDuration;

                float scale = Mathf.Lerp(startScale, targetScale, t);
                currentBlackCircle.transform.localScale = new Vector3(scale, scale, scale);

                screenFade.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

                yield return null;
            }

            Destroy(currentBlackCircle);
        }
    }

    private IEnumerator FadeScreenToBlack(bool fadeToBlack)
    {
        if (screenFade != null)
        {
            float elapsedTime = 0f;
            float startAlpha = fadeToBlack ? 0f : 1f;
            float endAlpha = fadeToBlack ? 1f : 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / fadeDuration;
                screenFade.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }

            screenFade.alpha = endAlpha;
        }
    }

    private void ModifySceneForUltimate(bool applyChanges)
    {
        string[] tagsToRemove = { "Item", "Enemy", "Map" }; // Tags ของวัตถุที่จะลบ
        string[] tagsToModify = { "Floor", "Wall" }; // Tags ของวัตถุที่จะแก้ไข Sprite

        if (applyChanges)
        {
            // ลบวัตถุที่ไม่ใช่ Floor, Wall และ Boss
            foreach (string tag in tagsToRemove)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject obj in objects)
                {
                    removedObjects.Add(obj); // เก็บวัตถุที่ถูกลบ
                    obj.SetActive(false); // ปิดการแสดงผลแทนการลบทิ้ง
                }
            }

            // เปลี่ยน Sprite สำหรับ Floor และ Wall
            foreach (string tag in tagsToModify)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject obj in objects)
                {
                    SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
                    if (renderer != null)
                    {
                        if (!originalSprites.ContainsKey(obj))
                        {
                            originalSprites[obj] = renderer.sprite; // เก็บ Sprite เดิม
                        }

                        renderer.sprite = (tag == "Floor") ? floorSpriteDuringUltimate : wallSpriteDuringUltimate;
                    }
                }
            }
        }
        else
        {
            // คืนค่า Sprite สำหรับ Floor และ Wall
            foreach (var entry in originalSprites)
            {
                SpriteRenderer renderer = entry.Key.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.sprite = entry.Value; // คืน Sprite เดิม
                }
            }

            originalSprites.Clear();

            // คืนวัตถุที่ถูกลบ
            foreach (GameObject obj in removedObjects)
            {
                obj.SetActive(true); // เปิดการแสดงผลอีกครั้ง
            }

            removedObjects.Clear();
        }
    }

    private IEnumerator StartUltimateEffect()
    {
        if (ultimateEffectPrefab != null)
        {
            Vector3 ultimatePosition = transform.position + new Vector3(0, 0.5f, 0);
            currentUltimateEffect = Instantiate(ultimateEffectPrefab, ultimatePosition, Quaternion.identity);
            currentUltimateEffect.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            SpriteRenderer renderer = currentUltimateEffect.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = new Color(1f, 1f, 1f, 0f);
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
                renderer.color = new Color(1f, 1f, 1f, t);
            }

            yield return null;
        }

        yield return new WaitForSeconds(waitBeforeSwordEffect);

        yield return StartCoroutine(ShowSwordEffect());

        EndUltimateMove();
        _player.isUseultimateMoveNow = false;
    }

    private IEnumerator ShowSwordEffect()
    {
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");

        foreach (GameObject boss in bosses)
        {
            if (swordEffectPrefab != null)
            {
                GameObject swordEffect = Instantiate(swordEffectPrefab, boss.transform.position, Quaternion.identity);
                Destroy(swordEffect, swordEffectDuration);
            }

            if (cameraShake != null)
            {
                cameraShake.StartShake(0.2f, swordEffectDuration);
            }

            SpriteRenderer renderer = boss.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                StartCoroutine(ChangeBossColor(renderer));
            }
        }

        yield return new WaitForSeconds(swordEffectDuration);

        foreach (GameObject boss in bosses)
        {
            var bossScript = boss.GetComponent<OOPBob>();
            if (bossScript != null)
            {
                bossScript.Hit();
            }
        }
    }

    private IEnumerator ChangeBossColor(SpriteRenderer renderer)
    {
        Color originalColor = renderer.color;
        renderer.color = bossHitColor;

        yield return new WaitForSeconds(swordEffectDuration);

        renderer.color = originalColor;
    }

    private void EndUltimateMove()
    {
        if (currentUltimateEffect != null)
        {
            Destroy(currentUltimateEffect);
        }

        Debug.Log("Ultimate Move Finished!");
    }
}
