using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OOPEnemy : Character
{
    public void Start()
    {
        GetRemainEnergy();
    }

    public override void Hit()
    {
        mapGenerator.player.Attack(this);
        this.Attack(mapGenerator.player);
    }

    public void Attack(OOPPlayer _player)
    {
        _player.TakeDamage(AttackPoint);
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

    public void RandomMove()
        {
            int toX = positionX;
            int toY = positionY;
            int random = Random.Range(0, 4);
            switch (random)
            {
                case 0:
                    // up
                    toY += 1;
                    break;
                case 1:
                    // down 
                    toY -= 1;
                    break;
                case 2:
                    // left
                    toX -= 1;
                    break;
                case 3:
                    // right
                    toX += 1;
                    break;
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
            }
        }
}
