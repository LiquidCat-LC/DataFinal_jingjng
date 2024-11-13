using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGenerator : MonoBehaviour
{
    [Header("Set MapGenerator")]
        public int X;
        public int Y;

        [Header("Set Player")]
        public OOPPlayer player;
        public Vector2Int playerStartPos;

        [Header("Set Exit")]
        public OOPExit Exit;

        [Header("Set Prefab")]
        public GameObject[] floorsPrefab;
        public GameObject[] wallsPrefab;
        public GameObject[] demonWallsPrefab;
        public GameObject[] itemsPrefab;
        public GameObject[] keysPrefab;
        public GameObject[] enemiesPrefab;
        public GameObject[] bossesPrefab;
        public GameObject[] fireStormPrefab;
        public GameObject[] smallCurrencyPrefab;
        public GameObject[] bigCurrencyPrefab;

        [Header("Set Transform")]
        public Transform floorParent;
        public Transform wallParent;
        public Transform itemPotionParent;
        public Transform enemyParent;
        public Transform itemCurrencyParent;

        [Header("Set object Count")]
        public int obsatcleCount;
        public int itemPotionCount;
        public int itemKeyCount;
        public int itemFireStormCount;
        public int enemyCount;
        public int bossesCount;
        public int currencyCount;

        public int[,] mapdata;

        public OOPWall[,] walls;
        public OOPItemPotion[,] potions;
        public OOPScroll[,] fireStorms;
        public OOPItemKey[,] keys;
        public OOPEnemy[,] enemies;
        public OOPBob[,] bosses;
        public OOPCurrency[,] smallCurrencies;
        public OOPCurrency[,] bigCurrencies;

        // block types ...
        [Header("Block Types")]
        public int playerBlock = 99;
        public int empty = 0;
        public int demonWall = 1;
        public int potion = 2;
        public int bonuesPotion = 3;
        public int exit = 4;
        public int key = 5;
        public int enemy = 6;
        public int fireStorm = 7;
        public int boss = 8;
        public int smallCurrency = 9;
        public int bigCurrency = 10;

        bool BigCurrencyPlaced = false;


    // Start is called before the first frame update
    void Awake()
        {
            mapdata = new int[X, Y];
            for (int x = -1; x < X + 1; x++)
            {
                for (int y = -1; y < Y + 1; y++)
                {
                    if (x == -1 || x == X || y == -1 || y == Y)
                    {
                        int r = Random.Range(0, wallsPrefab.Length);
                        GameObject obj = Instantiate(wallsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
                        obj.transform.parent = wallParent;
                        obj.name = "Wall_" + x + ", " + y;
                    }
                    else
                    {
                        int r = Random.Range(0, floorsPrefab.Length);
                        GameObject obj = Instantiate(floorsPrefab[r], new Vector3(x, y, 1), Quaternion.identity);
                        obj.transform.parent = floorParent;
                        obj.name = "floor_" + x + ", " + y;
                    }
                }
            }

            player.mapGenerator = this;
            player.positionX = playerStartPos.x;
            player.positionY = playerStartPos.y;
            player.transform.position = new Vector3(playerStartPos.x, playerStartPos.y, -0.1f);
            mapdata[playerStartPos.x, playerStartPos.y] = playerBlock;

            mapdata[X - 1, Y - 1] = exit;
            Exit.transform.position = new Vector3(X - 1, Y - 1, 0);

            walls = new OOPWall[X, Y];
            int count = 0;
            while (count < obsatcleCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == 0)
                {
                    PlaceDemonWall(x, y);
                    count++;
                }
            }

            potions = new OOPItemPotion[X, Y];
            count = 0;
            while (count < itemPotionCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceItem(x, y);
                    count++;
                }
            }

            keys = new OOPItemKey[X, Y];
            count = 0;
            while (count < itemKeyCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceKey(x, y);
                    count++;
                }
            }

            enemies = new OOPEnemy[X, Y];
            count = 0;
            while (count < enemyCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceEnemy(x, y);
                    count++;
                }
            }

            bosses = new OOPBob[X, Y];
            count = 0;
            while (count < bossesCount)
            {
                int x = Random.Range(0, X );
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceBoss(x, y);
                    count++;
                }
            }

            fireStorms = new OOPScroll[X, Y];
            count = 0;
            while (count < itemFireStormCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceFireStorm(x, y);
                    count++;
                }
            }

            smallCurrencies = new OOPCurrency[X, Y];
            bigCurrencies = new OOPCurrency[X, Y];
    }
    private void FixedUpdate()
    {
        if (!BigCurrencyPlaced)
        {
            if (player.inventory.numberOfItem("SmallCurrency") >= 3)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);

                if (mapdata[x, y] == empty)
                {
                    PlaceBigCurrency(x, y);
                    mapdata[x, y] = bigCurrency;
                    BigCurrencyPlaced = true;
                    Debug.Log("Placed BigCurrency");
                }
            }
        }
    }

    public int GetMapData(float x, float y)
        {
            if (x >= X || x < 0 || y >= Y || y < 0) return -1;
            return mapdata[(int)x, (int)y];
        }

        public void PlaceItem(int x, int y)
        {
            int r = Random.Range(0, itemsPrefab.Length);
            GameObject obj = Instantiate(itemsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = itemPotionParent;
            mapdata[x, y] = potion;
            potions[x, y] = obj.GetComponent<OOPItemPotion>();
            potions[x, y].positionX = x;
            potions[x, y].positionY = y;
            potions[x, y].mapGenerator = this;
            obj.name = $"Item_{potions[x, y].Name} {x}, {y}";
        }

        public void PlaceKey(int x, int y)
        {
            int r = Random.Range(0, keysPrefab.Length);
            GameObject obj = Instantiate(keysPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = itemPotionParent;
            mapdata[x, y] = key;
            keys[x, y] = obj.GetComponent<OOPItemKey>();
            keys[x, y].positionX = x;
            keys[x, y].positionY = y;
            keys[x, y].mapGenerator = this;
            obj.name = $"Item_{keys[x, y].Name} {x}, {y}";
        }

        public void PlaceEnemy(int x, int y)
        {
            int r = Random.Range(0, enemiesPrefab.Length);
            GameObject obj = Instantiate(enemiesPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = enemyParent;
            mapdata[x, y] = enemy;
            enemies[x, y] = obj.GetComponent<OOPEnemy>();
            enemies[x, y].positionX = x;
            enemies[x, y].positionY = y;
            enemies[x, y].mapGenerator = this;
            obj.name = $"Enemy_{enemies[x, y].Name} {x}, {y}";
        }

        public void PlaceBoss(int x, int y)
        {
            int r = Random.Range(0, bossesPrefab.Length);
            GameObject obj = Instantiate(bossesPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = enemyParent;
            mapdata[x, y] = boss;
            bosses[x, y] = obj.GetComponent<OOPBob>();
            bosses[x, y].positionX = x;
            bosses[x, y].positionY = y;
            bosses[x, y].mapGenerator = this;
            obj.name = $"Boss_{bosses[x, y].Name} {x}, {y}";
        }


        public void PlaceDemonWall(int x, int y)
        {
            int r = Random.Range(0, demonWallsPrefab.Length);
            GameObject obj = Instantiate(demonWallsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = wallParent;
            mapdata[x, y] = demonWall;
            walls[x, y] = obj.GetComponent<OOPWall>();
            walls[x, y].positionX = x;
            walls[x, y].positionY = y;
            walls[x, y].mapGenerator = this;
            obj.name = $"DemonWall_{walls[x, y].Name} {x}, {y}";
        }

        public void PlaceFireStorm(int x, int y)
        {
            int r = Random.Range(0, fireStormPrefab.Length);
            GameObject obj = Instantiate(fireStormPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = wallParent;
            mapdata[x, y] = fireStorm;
            fireStorms[x, y] = obj.GetComponent<OOPScroll>();
            fireStorms[x, y].positionX = x;
            fireStorms[x, y].positionY = y;
            fireStorms[x, y].mapGenerator = this;
            obj.name = $"FireStorm_{fireStorms[x, y].Name} {x}, {y}";
        }

        public void PlaceSmallCurrency(int x, int y)
        {   
            GameObject obj = Instantiate(smallCurrencyPrefab[0], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = itemCurrencyParent;
            mapdata[x, y] = smallCurrency;
            smallCurrencies[x, y] = obj.GetComponent<OOPCurrency>();
            smallCurrencies[x, y].positionX = x;
            smallCurrencies[x, y].positionY = y;
            smallCurrencies[x, y].mapGenerator = this;
            obj.name = $"Item_{smallCurrencies[x, y].Name} {x}, {y}";
        }
        public void PlaceBigCurrency(int x, int y)
        {
            GameObject obj = Instantiate(bigCurrencyPrefab[0], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = itemCurrencyParent;
            mapdata[x, y] = bigCurrency;
            bigCurrencies[x, y] = obj.GetComponent<OOPCurrency>();
            bigCurrencies[x, y].positionX = x;
            bigCurrencies[x, y].positionY = y;
            bigCurrencies[x, y].mapGenerator = this;
            obj.name = $"Item_{bigCurrencies[x, y].Name} {x}, {y}";
        }

    public OOPEnemy[] GetEnemies()
        {
            List<OOPEnemy> list = new List<OOPEnemy>();
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    list.Add(enemy);
                }
            }
            return list.ToArray();
        }
        public OOPBob[] GetBoss()
        {
            List<OOPBob> list = new List<OOPBob>();
            foreach (var boss in bosses)
            {
                if (boss != null)
                {
                    list.Add(boss);
                }
            }
            return list.ToArray();
        }

        public void MoveEnemies()
        {
            List<OOPEnemy> list = new List<OOPEnemy>();
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    list.Add(enemy);
                }
            }
            foreach (var enemy in list)
            {
                enemy.RandomMove();
            }
        }

        public void MoveBoss()
        {
            List<OOPBob> list = new List<OOPBob>();
            foreach (var boss in bosses)
            {
                if (boss != null)
                {
                    list.Add(boss);
                }
            }
            foreach (var boss in list)
            {
                boss.RandomMove();
            }
        }
}