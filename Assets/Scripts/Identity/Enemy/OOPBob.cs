using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPBob : Character
{
    public void Start()
    {
        GetRemainEnergy();
    }

    public override void Hit()
    {
        mapGenerator.player.Attack(this);
        this.Attack(mapGenerator.player);

        CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            cameraShake.StartShake(0.5f, 0.3f);
        }
    }

    public void Attack(OOPPlayer _player)
    {
        if (mapGenerator.player.inventory.numberOfItem("Defense") > 0)
        {
            Debug.Log("Player defended with Defense Item!");
            mapGenerator.player.inventory.UseItem("Defense");

            if (_player.currentDefenseEffect != null)
            {
                Destroy(_player.currentDefenseEffect);
                _player.currentDefenseEffect = null;
                Debug.Log("Defense Effect removed.");
            }
        }

        else
        {
            if (mapGenerator.player.inventory.numberOfItem("BigCurrency") == 0)
            {
                _player.TakeDamage(_player.energy);
            }
        }
    }

    protected override void CheckDead()
    {
        base.CheckDead();

        if (energy <= 0)
        {
            // ล้างค่าทั้ง 4 ช่องใน mapdata และ bosses
            mapGenerator.bosses[positionX, positionY] = null;
            mapGenerator.bosses[positionX + 1, positionY] = null;
            mapGenerator.bosses[positionX, positionY + 1] = null;
            mapGenerator.bosses[positionX + 1, positionY + 1] = null;

            mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
            mapGenerator.mapdata[positionX + 1, positionY] = mapGenerator.empty;
            mapGenerator.mapdata[positionX, positionY + 1] = mapGenerator.empty;
            mapGenerator.mapdata[positionX + 1, positionY + 1] = mapGenerator.empty;

            // วาง Key ในตำแหน่งตรงกลางของ Boss
            int keyX = positionX;
            int keyY = positionY;
            mapGenerator.PlaceKey(keyX, keyY);
            mapGenerator.mapdata[keyX, keyY] = mapGenerator.key;

            Debug.Log($"[CheckDead] Boss at ({positionX}, {positionY}) has been removed. Key placed at ({keyX}, {keyY}).");
        }
    }


    public void TeleportBoss()
    {
        int maxAttempts = 50; // จำนวนครั้งสูงสุดที่พยายามสุ่มหาตำแหน่งว่าง
        int attempts = 0;
        int x, y;

        do
        {
            // สุ่มตำแหน่งในแผนที่
            x = Random.Range(0, mapGenerator.mapdata.GetLength(0) - 1);
            y = Random.Range(0, mapGenerator.mapdata.GetLength(1) - 1);

            attempts++;
            if (attempts >= maxAttempts)
            {
                Debug.LogError("[TeleportBoss] Cannot find empty space to teleport after multiple attempts.");
                return; // ออกจากฟังก์ชันหากพยายามเกินจำนวนครั้งที่กำหนด
            }

        } while (!mapGenerator.IsAreaEmpty(x, y)); // ทำซ้ำจนกว่าจะเจอพื้นที่ว่าง

        Debug.Log($"[TeleportBoss] Teleporting boss to ({x}, {y}) after {attempts} attempts.");

        // ล้างตำแหน่งเดิม
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
        mapGenerator.mapdata[positionX + 1, positionY] = mapGenerator.empty;
        mapGenerator.mapdata[positionX, positionY + 1] = mapGenerator.empty;
        mapGenerator.mapdata[positionX + 1, positionY + 1] = mapGenerator.empty;

        mapGenerator.bosses[positionX, positionY] = null;
        mapGenerator.bosses[positionX + 1, positionY] = null;
        mapGenerator.bosses[positionX, positionY + 1] = null;
        mapGenerator.bosses[positionX + 1, positionY + 1] = null;

        // อัปเดตตำแหน่งใหม่
        positionX = x;
        positionY = y;

        mapGenerator.mapdata[positionX, positionY] = mapGenerator.boss;
        mapGenerator.mapdata[positionX + 1, positionY] = mapGenerator.boss;
        mapGenerator.mapdata[positionX, positionY + 1] = mapGenerator.boss;
        mapGenerator.mapdata[positionX + 1, positionY + 1] = mapGenerator.boss;

        mapGenerator.bosses[positionX, positionY] = this;
        mapGenerator.bosses[positionX + 1, positionY] = this;
        mapGenerator.bosses[positionX, positionY + 1] = this;
        mapGenerator.bosses[positionX + 1, positionY + 1] = this;

        // ย้ายตำแหน่ง GameObject
        transform.position = new Vector3(positionX + 0.5f, positionY + 0.5f, 0);
    }


}
