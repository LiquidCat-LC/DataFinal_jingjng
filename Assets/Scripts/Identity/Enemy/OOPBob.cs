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
        }

        public void Attack(OOPPlayer _player)
        {
            if (mapGenerator.player.inventory.numberOfItem("BigCurrency") == 0)
            {
                _player.TakeDamage(_player.energy);
            }
        }

        protected override void CheckDead()
        {
            base.CheckDead();
            if (energy <= 0)
            {
                mapGenerator.bosses[positionX, positionY] = null;
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.empty;
                //mapGenerator.PlaceSmallCurrency(positionX, positionY);
                //mapGenerator.mapdata[positionX, positionY] = mapGenerator.smallCurrency;
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
                mapGenerator.bosses[positionX, positionY] = null;
                positionX = toX;
                positionY = toY;
                mapGenerator.mapdata[positionX, positionY] = mapGenerator.boss;
                mapGenerator.bosses[positionX, positionY] = this;
                transform.position = new Vector3(positionX, positionY, 0);
            }
        }
}
