using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPFreeze : Identity
{
    public override void Hit()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.itemPickupSound);
        // ค้นหา Enemy ทั้งหมดในฉาก
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            var enemyScript = enemy.GetComponent<OOPEnemy>();
            if (enemyScript != null)
            {
                // แช่แข็ง Enemy ตัวนี้
                enemyScript.ApplyFreezeEffect();
            }
        }

        // ลบ Freeze Item จาก Map
        mapGenerator.frozen[positionX, positionY] = null;
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;

        // ทำลายตัว Freeze Item
        Destroy(gameObject);
    }
}
