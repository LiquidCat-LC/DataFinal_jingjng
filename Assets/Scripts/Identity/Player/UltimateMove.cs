using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateMove : MonoBehaviour
{
    public OOPPlayer _player;
    public GameObject ultimateEffectPrefab; // Prefab �ͧ Ultimate Effect
    public GameObject swordEffectPrefab; // Prefab �Ϳ࿡��ѹ�Һ
    public CameraShake cameraShake; // ��ҧ�ԧ CameraShake

    public float fadeDuration = 1.0f; // ��������㹡��࿴ 
    public float moveDuration = 1.0f; // ��������㹡������͹ Ultimate Effect
    public float waitBeforeSwordEffect = 3.0f; // ���ҡ�͹�ʴ��ѹ�Һ
    public float swordEffectDuration = 5.0f; // ���������ʴ��Ϳ࿡��ѹ�Һ
    public Color bossHitColor = Color.red; // ��ᴧ�ͧ��������ⴹ�ѹ

    private GameObject currentUltimateEffect; // �����ҧ�ԧ�ͧ Ultimate Effect
    private GameObject currentSwordEffect; // �����ҧ�ԧ�ͧ Sword Effect
    private List<SpriteRenderer> mapRenderers = new List<SpriteRenderer>(); // ��¡�� SpriteRenderer �Ἱ���

    private void Start()
    {
        CacheMapObjectsByTags(); // ����������ѵ���Ἱ����� Tags
    }

    public void TriggerUltimateMove()
    {
        StartCoroutine(StartUltimateMove());
    }

    private IEnumerator StartUltimateMove()
    {

        if (ultimateEffectPrefab != null)
        {
            // ��駵��˹� Ultimate Effect
            Vector3 ultimatePosition = transform.position + new Vector3(0, 0.5f, 0);
            currentUltimateEffect = Instantiate(ultimateEffectPrefab, ultimatePosition, Quaternion.identity);
            currentUltimateEffect.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            SpriteRenderer renderer = currentUltimateEffect.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = new Color(1f, 1f, 1f, 0f); // ������������
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
                renderer.color = new Color(1f, 1f, 1f, t); // ���� � ࿴���
            }

            yield return null;
        }

        // �� 3 �Թҷա�͹������ѹ�Һ
        yield return new WaitForSeconds(waitBeforeSwordEffect);

        // �ʴ� Sword Effect ���������ѧ�ҡ�ѹ��
        yield return StartCoroutine(ShowSwordEffect());

        // �� Ultimate Move
        EndUltimateMove();
        _player.isUseultimateMoveNow = false;
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
        // ���Һ�ʷء���㹩ҡ (�� Tag "Boss")
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");

        foreach (GameObject boss in bosses)
        {
            // ���ҧ Sword Effect �����˹觢ͧ���
            if (swordEffectPrefab != null)
            {
                GameObject swordEffect = Instantiate(swordEffectPrefab, boss.transform.position, Quaternion.identity);
                Destroy(swordEffect, swordEffectDuration); // ź Sword Effect ��ѧ�ҡ�������
            }

            // ��蹡��ͧ������պ��
            if (cameraShake != null)
            {
                cameraShake.StartShake(0.2f, swordEffectDuration);
            }

            // ����¹�պ������ᴧ���ͺ觺͡��ҡ��ѧⴹ����
            SpriteRenderer renderer = boss.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                StartCoroutine(ChangeBossColor(renderer));
            }
        }

        // �ͨ����� Sword Effect �Ш�
        yield return new WaitForSeconds(swordEffectDuration);

        // ���պ�ʷء�����ѧ Sword Effect ��
        foreach (GameObject boss in bosses)
        {
            var bossScript = boss.GetComponent<OOPBob>();
            if (bossScript != null)
            {
                bossScript.Hit(); // ��ʨ����Ѻ�������ѧ Sword Effect
            }
        }
    }

    // �ѧ��ѹ����¹�պ����ᴧ���Ǥ���
    private IEnumerator ChangeBossColor(SpriteRenderer renderer)
    {
        Color originalColor = renderer.color;
        renderer.color = bossHitColor; // ����¹����ᴧ

        yield return new WaitForSeconds(swordEffectDuration); // �ͨ����� Sword Effect �Ш�

        renderer.color = originalColor; // �׹����������
    }

    private void CacheMapObjectsByTags()
    {
        // ���� GameObjects ��� Tags
        string[] tagsToCache = { "Item", "Map", "Enemy" };

        foreach (string tag in tagsToCache)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    mapRenderers.Add(renderer); // ���� SpriteRenderer ŧ���¡��
                }
            }
        }
    }
}
