using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OOPEnemy : Character
{
    protected override void Start()
    {
        base.Start();
        GetRemainEnergy();
    }

    public override void Hit()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.hitSound);
        mapGenerator.player.Attack(this);
        this.Attack(mapGenerator.player);

        CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            cameraShake.StartShake(0.1f, 0.3f);
        }
    }

    public void Attack(OOPPlayer _player)
    {
        if (mapGenerator.player.inventory.numberOfItem("Defense") > 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.hitArmorSound);
            Debug.Log("Player defended with Defense Item!");
            mapGenerator.player.inventory.UseItem("Defense");
            UIManager.Instance.Updatedefense(false);

            if (_player.currentDefenseEffect != null)
            {
                Destroy(_player.currentDefenseEffect);
                _player.currentDefenseEffect = null;
                Debug.Log("Defense Effect removed.");
            }
        }

        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.hitSound);
            _player.TakeDamage(AttackPoint);
        }
    }

    protected override void CheckDead()
    {
        base.CheckDead();
        if (energy <= 0)
        {
            mapGenerator.enemies[positionX, positionY] = null;
            mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;

            int randomResult = Random.Range(0, 2);

            if (randomResult == 0)
            {
                mapGenerator.PlaceFireStorm(positionX, positionY);
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.fireStorm;
                Debug.Log("Placed FireStorm at position.");
            }
            else
            {
                mapGenerator.PlaceItem(positionX, positionY);
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.potion;
                Debug.Log("Placed Item at position.");
            }

            int x, y;
            do
            {
                x = Random.Range(0, mapGenerator.X);
                y = Random.Range(0, mapGenerator.Y);
            }
            while (mapGenerator.mapdata[x, y] != mapGenerator.empty);

            mapGenerator.PlaceSmallCurrency(x, y);
            mapGenerator.mapdata[x, y] = mapGenerator.smallCurrency;
            Debug.Log($"Placed smallCurrency at {x},{y} position.");
        }
    }

    public GameObject freezeEffectPrefab; // Prefab ����Ѻ Freeze Effect
    public GameObject currentFreezeEffect; // �����ҧ�ԧ����Ѻ Freeze Effect
    private bool isFrozen = false; // ʶҹ�����

    public void ApplyFreezeEffect()
    {
        // ���ҧ Freeze Effect �ҡ�ѧ�����
        if (freezeEffectPrefab != null && currentFreezeEffect == null)
        {
            currentFreezeEffect = Instantiate(freezeEffectPrefab, transform.position, Quaternion.identity);

            // �Դ Freeze Effect �Ѻ Enemy
            currentFreezeEffect.transform.SetParent(this.transform, false);
            currentFreezeEffect.transform.localPosition = Vector3.zero; // �ҧ�ç��ҧ�ͧ Enemy
        }

        isFrozen = true; // ���ʶҹ�����
        Debug.Log($"Freeze Effect applied to Enemy at ({positionX}, {positionY}).");
    }

    public void RandomMove()
    {
        if (isFrozen)
        {
            // �����á: Enemy �١�����������Թ
            isFrozen = false; // ������ʶҹ���������Ѻ���駶Ѵ�
            Debug.Log($"Enemy at ({positionX}, {positionY}) is frozen and cannot move.");

            return; // ��ش��÷ӧҹ�����������칹��
        }

        // ź Freeze Effect �ҡ��
        if (currentFreezeEffect != null)
        {
            Destroy(currentFreezeEffect);
            currentFreezeEffect = null;
            Debug.Log($"Freeze Effect removed from Enemy at ({positionX}, {positionY}).");
        }

        // �������͹��軡�Ԣͧ Enemy
        int toX = positionX;
        int toY = positionY;
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0: toY += 1; break; // up
            case 1: toY -= 1; break; // down
            case 2: toX -= 1; break; // left
            case 3: toX += 1; break; // right
        }

        if (!HasPlacement(toX, toY))
        {
            mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
            mapGenerator.enemies[positionX, positionY] = null;
            positionX = toX;
            positionY = toY;
            mapGenerator.mapdata[positionX, positionY] = mapGenerator.enemy;
            mapGenerator.enemies[positionX, positionY] = this;
            transform.position = new Vector3(positionX, positionY, 0);

            Debug.Log($"Enemy at ({positionX}, {positionY}) moved to a new position.");
        }
    }




}
