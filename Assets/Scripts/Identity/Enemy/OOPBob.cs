using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPBob : Character
{
    protected override void Start()
    {
        base.Start();
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
            SoundManager.Instance.PlaySound(SoundManager.Instance.hitArmorSound);
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
                SoundManager.Instance.PlaySound(SoundManager.Instance.hitSound);
                _player.TakeDamage(_player.energy);
            }
        }
    }

    protected override void CheckDead()
    {
        base.CheckDead();

        if (energy <= 0)
        {
            // ��ҧ��ҷ�� 4 ��ͧ� mapdata ��� bosses
            mapGenerator.bosses[positionX, positionY] = null;
            mapGenerator.bosses[positionX + 1, positionY] = null;
            mapGenerator.bosses[positionX, positionY + 1] = null;
            mapGenerator.bosses[positionX + 1, positionY + 1] = null;

            mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
            mapGenerator.mapdata[positionX + 1, positionY] = mapGenerator.empty;
            mapGenerator.mapdata[positionX, positionY + 1] = mapGenerator.empty;
            mapGenerator.mapdata[positionX + 1, positionY + 1] = mapGenerator.empty;

            // �ҧ Key 㹵��˹觵ç��ҧ�ͧ Boss
            int keyX = positionX;
            int keyY = positionY;
            mapGenerator.PlaceKey(keyX, keyY);
            mapGenerator.mapdata[keyX, keyY] = mapGenerator.key;

            Debug.Log($"[CheckDead] Boss at ({positionX}, {positionY}) has been removed. Key placed at ({keyX}, {keyY}).");
        }
    }


    public void TeleportBoss()
    {
        int maxAttempts = 50; // �ӹǹ�����٧�ش�������������ҵ��˹���ҧ
        int attempts = 0;
        int x, y;

        do
        {
            // �������˹��Ἱ���
            x = Random.Range(0, mapGenerator.mapdata.GetLength(0) - 1);
            y = Random.Range(0, mapGenerator.mapdata.GetLength(1) - 1);

            attempts++;
            if (attempts >= maxAttempts)
            {
                Debug.LogError("[TeleportBoss] Cannot find empty space to teleport after multiple attempts.");
                return; // �͡�ҡ�ѧ��ѹ�ҡ�������Թ�ӹǹ���駷���˹�
            }

        } while (!mapGenerator.IsAreaEmpty(x, y)); // �ӫ�Ө����Ҩ��;�鹷����ҧ

        Debug.Log($"[TeleportBoss] Teleporting boss to ({x}, {y}) after {attempts} attempts.");

        // ��ҧ���˹����
        mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
        mapGenerator.mapdata[positionX + 1, positionY] = mapGenerator.empty;
        mapGenerator.mapdata[positionX, positionY + 1] = mapGenerator.empty;
        mapGenerator.mapdata[positionX + 1, positionY + 1] = mapGenerator.empty;

        mapGenerator.bosses[positionX, positionY] = null;
        mapGenerator.bosses[positionX + 1, positionY] = null;
        mapGenerator.bosses[positionX, positionY + 1] = null;
        mapGenerator.bosses[positionX + 1, positionY + 1] = null;

        // �ѻവ���˹�����
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

        // ���µ��˹� GameObject
        transform.position = new Vector3(positionX + 0.5f, positionY + 0.5f, 0);
    }


}
