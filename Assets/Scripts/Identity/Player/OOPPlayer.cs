using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OOPPlayer : Character
{
    public Inventory inventory;
    public UltimateMove ultimateMove;
    public GameObject defenseEffectPrefab;
    public GameObject currentDefenseEffect;
    public bool isUseultimateMoveNow;

    protected override void Start()
    {
        isUseultimateMoveNow = false;
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
            //inventory.AddItem("FireStorm");
        }
        else
        {
            Debug.LogError("Inventory is not assigned to OOPPlayer!");
        }
    }

    public void Update()
    {
        if (isUseultimateMoveNow == false)
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

        UIManager.Instance.UpdateEnergyUI();
    }

    public void ActivateDefenseEffect()
    {
        if (
            mapGenerator.player.inventory.numberOfItem("Defense") > 0
            && currentDefenseEffect == null
        )
        {
            currentDefenseEffect = Instantiate(
                defenseEffectPrefab,
                transform.position,
                Quaternion.identity
            );
            currentDefenseEffect.transform.SetParent(this.transform, false);
            currentDefenseEffect.transform.localPosition = Vector3.zero;
            Debug.Log("Defense Effect activated.");
        }
    }

    public void Attack(OOPEnemy _enemy)
    {
        _enemy.TakeDamage(AttackPoint);
    }

    public void Attack(OOPBob _boss)
    {
        if (mapGenerator.player.inventory.numberOfItem("BigCurrency") >= 1)
        {
            _boss.TakeDamage(_boss.energy);
        }
    }

    protected override void CheckDead()
    {
        base.CheckDead();
        if (energy <= 0)
        {
            Debug.Log("Player is Dead");
            UIManager.Instance.losepanel.SetActive(true);
        }
    }

    public void UseFireStorm()
    {
        if (inventory.numberOfItem("BigCurrency") >= 1)
        {
            if (ultimateMove != null && !isUseultimateMoveNow)
            {
                Debug.Log("Use Ultimate");
                isUseultimateMoveNow = true;
                ultimateMove.TriggerUltimateMove();
            }
        }
        else
        {
            if (inventory.numberOfItem("FireStorm") > 0)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.shootSound);
                inventory.UseItem("FireStorm");
                //inventory.UseItem("FireStorm");
                List<OOPEnemy> enemies = GetEnemiesAroundPlayer(
                    this.gameObject.transform.position,
                    1
                );

                int count = 3;
                if (count > enemies.Count)
                {
                    count = enemies.Count;
                }
                for (int i = 0; i < count; i++)
                {
                    enemies[i].TakeDamage(10);
                }
                Debug.Log("FireStorm count: " + inventory.numberOfItem("FireStorm"));
            }
            else
            {
                Debug.Log("No FireStorm in inventory");
            }
        }
    }

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
