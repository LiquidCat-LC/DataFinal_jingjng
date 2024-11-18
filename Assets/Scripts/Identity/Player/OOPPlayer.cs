using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOPPlayer : Character
{
    public Inventory inventory;
    public UltimateMove ultimateMove;
    public GameObject defenseEffectPrefab;
    public GameObject currentDefenseEffect;

    public void Start()
    {
        PrintInfo();
        GetRemainEnergy();

        if (inventory == null)
        {
            inventory = GetComponent<Inventory>();
        }

        if (inventory != null)
        {
            PrintInfo();
            GetRemainEnergy();
            inventory.AddItem("FireStorm");
        }
        else
        {
            Debug.LogError("Inventory is not assigned to OOPPlayer!");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2.left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2.right);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseFireStorm();
        }
    }

    public void ActivateDefenseEffect()
    {
        // หาก Player มี Defense Item และยังไม่มี Effect เกราะ
        if (mapGenerator.player.inventory.numberOfItem("Defense") > 0 && currentDefenseEffect == null)
        {
            // สร้าง Effect เกราะ
            currentDefenseEffect = Instantiate(defenseEffectPrefab, transform.position, Quaternion.identity);
            currentDefenseEffect.transform.SetParent(this.transform, false); // ติด Effect กับ Player
            currentDefenseEffect.transform.localPosition = Vector3.zero; // วาง Effect ตรงกลาง
            Debug.Log("Defense Effect activated.");
        }
    }

    public void Attack(OOPEnemy _enemy)
    {
        _enemy.TakeDamage(AttackPoint);
    }
    public void Attack(OOPBob _boss)
    {
        // ถ้ามี BigCurrency มากกว่าหรือเท่ากับ 1
        if (mapGenerator.player.inventory.numberOfItem("BigCurrency") >= 1)
        {
            // ฆ่า boss ด้วยพลังชีวิตของบอส
            _boss.TakeDamage(_boss.energy);
        }
    }

    protected override void CheckDead()
    {
        base.CheckDead();
        if (energy <= 0)
        {
            Debug.Log("Player is Dead");
        }
    }

    public void UseFireStorm()
    {
        // ถ้ามี BigCurrency มากกว่าหรือเท่ากับ 1
        if (inventory.numberOfItem("BigCurrency") >= 1)
        {
            // ให้กางอนาเขต
            Debug.Log("Use Ultimate");
            if (ultimateMove != null)
            {
                ultimateMove.TriggerUltimateMove();
            }
        }

        else
        {
            if (inventory.numberOfItem("FireStorm") > 0)
            {
                inventory.UseItem("FireStorm");
                inventory.UseItem("FireStorm");
                List<OOPEnemy> enemies = GetEnemiesAroundPlayer(this.gameObject.transform.position, 1);
                int count = 3;
                if (count > enemies.Count)
                {
                    count = enemies.Count;
                }
                for (int i = 0; i < count; i++)
                {
                    enemies[i].TakeDamage(10);
                }
            }

            else
            {
                Debug.Log("No FireStorm in inventory");
            }
        }

    }

    // public OOPEnemy[] SortEnemiesByRemainningEnergy1()
    // {
    //     // do selection sort of enemy's energy
    //     var enemies = mapGenerator.GetEnemies();
    //     for (int i = 0; i < enemies.Length - 1; i++)
    //     {
    //         int minIndex = i;
    //         for (int j = i + 1; j < enemies.Length; j++)
    //         {
    //             if (enemies[j].energy < enemies[minIndex].energy)
    //             {
    //                 minIndex = j;
    //             }
    //         }
    //         var temp = enemies[i];
    //         enemies[i] = enemies[minIndex];
    //         enemies[minIndex] = temp;
    //     }
    //     return enemies;
    // }

    // public OOPEnemy[] SortEnemiesByRemainningEnergy2()
    // {
    //     var enemies = mapGenerator.GetEnemies();
    //     Array.Sort(enemies, (a, b) => a.energy.CompareTo(b.energy));
    //     return enemies;
    // }

    public List<OOPEnemy> GetEnemiesAroundPlayer(Vector2 playerPosition, float range)
    {
        var enemies = mapGenerator.GetEnemies();
        List<OOPEnemy> enemiesAround = new List<OOPEnemy>();

        foreach (var enemy in enemies)
        {
            float distanceX = Mathf.Abs(enemy.positionX - playerPosition.x);
            float distanceY = Mathf.Abs(enemy.positionY - playerPosition.y);

            if (distanceX <= range && distanceY <= range)
            {
                enemiesAround.Add(enemy);
            }
        }

        return enemiesAround;
    }

}
