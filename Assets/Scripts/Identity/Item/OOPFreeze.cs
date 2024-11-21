using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPFreeze : Identity
{
    public override void Hit()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.itemPickupSound);
        // ���� Enemy ������㹩ҡ
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            var enemyScript = enemy.GetComponent<OOPEnemy>();
            if (enemyScript != null)
            {
                // ���� Enemy ��ǹ��
                enemyScript.ApplyFreezeEffect();
            }
        }

        // ź Freeze Item �ҡ Map
        mapGenerator.frozen[positionX, positionY] = null;
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;

        // ����µ�� Freeze Item
        Destroy(gameObject);
    }
}
