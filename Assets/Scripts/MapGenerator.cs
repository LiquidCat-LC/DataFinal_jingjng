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

    [Header("Set WallPrefab")]
    public GameObject[] cornerWallPrefab;
    public GameObject[] topWallPrefab;
    public GameObject[] bottomWallPrefab;
    public GameObject[] leftWallPrefab;
    public GameObject[] rightWallPrefab;

    [Header("Set Prefab")]
    public GameObject[] floorsPrefab;
    public GameObject[] demonWallsPrefab;
    public GameObject[] itemsPrefab;
    public GameObject[] keysPrefab;
    public GameObject[] enemiesPrefab;
    public GameObject[] bossesPrefab;
    public GameObject[] fireStormPrefab;
    public GameObject[] smallCurrencyPrefab;
    public GameObject[] bigCurrencyPrefab;
    public GameObject[] freezePrefab;
    public GameObject[] defensePrefab;

    [Header("Set Transform")]
    public Transform floorParent;
    public Transform wallParent;
    public Transform itemPotionParent;
    public Transform enemyParent;
    public Transform itemCurrencyParent;
    public Transform itemPlayerParent;

    [Header("Set object Count")]
    public int obsatcleCount;
    public int itemPotionCount;
    public int itemKeyCount;
    public int itemFireStormCount;
    public int enemyCount;
    public int bossesCount;
    public int currencyCount;
    public int freezeCount;
    public int defenseCount;

    public int[,] mapdata;

    public OOPWall[,] walls;
    public OOPItemPotion[,] potions;
    public OOPScroll[,] fireStorms;
    public OOPItemKey[,] keys;
    public OOPEnemy[,] enemies;
    public OOPBob[,] bosses;
    public OOPCurrency[,] smallCurrencies;
    public OOPCurrency[,] bigCurrencies;
    public OOPFreeze[,] frozen;
    public OOPDefense[,] defended;

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
    public int freeze = 11;
    public int defense = 12;

    public bool freezeEnemiesForNextTurn = false;

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
                    string positionType = WallPositionType(x, y, X, Y);
                    GameObject obj = InstantiateWall(positionType, x, y);

                    if (obj != null)
                    {
                        obj.transform.parent = wallParent;
                        obj.name = "Wall_" + x + ", " + y;
                    }
                }
                else
                {
                    int r = Random.Range(0, floorsPrefab.Length);
                    GameObject obj = Instantiate(
                        floorsPrefab[r],
                        new Vector3(x, y, 1),
                        Quaternion.identity
                    );
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
            int x = Random.Range(0, X);
            int y = Random.Range(0, Y);
            if (mapdata[x, y] == empty)
            {
                PlaceBoss();
                //PlaceBoss(x, y);
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

        frozen = new OOPFreeze[X, Y];
        count = 0;
        while (count < freezeCount)
        {
            int x = Random.Range(0, X);
            int y = Random.Range(0, Y);
            if (mapdata[x, y] == empty)
            {
                PlaceFreeze(x, y);
                count++;
            }
        }

        defended = new OOPDefense[X, Y];
        count = 0;
        while (count < defenseCount)
        {
            int x = Random.Range(0, X);
            int y = Random.Range(0, Y);
            if (mapdata[x, y] == empty)
            {
                PlaceDefense(x, y);
                count++;
            }
        }
    }

    public int GetMapData(float x, float y)
    {
        if (x >= X || x < 0 || y >= Y || y < 0)
            return -1;
        return mapdata[(int)x, (int)y];
    }

    private string WallPositionType(int x, int y, int maxX, int maxY)
    {
        if (
            (x == -1 && y == -1)
            || (x == -1 && y == maxY)
            || (x == maxX && y == -1)
            || (x == maxX && y == maxY)
        )
        {
            return "Corner";
        }
        else if (y == maxY)
        {
            return "Top";
        }
        else if (y == -1)
        {
            return "Bottom";
        }
        else if (x == -1)
        {
            return "Left";
        }
        else if (x == maxX)
        {
            return "Right";
        }

        return "None";
    }

    private GameObject InstantiateWall(string positionType, int x, int y)
    {
        GameObject obj = null;

        switch (positionType)
        {
            case "Corner":
                if (x == -1 && y == -1) // bottom left
                {
                    obj = Instantiate(cornerWallPrefab[0], new Vector3(x, y, 0), Quaternion.identity);
                }
                else if (x == -1 && y == Y) // top left
                {
                    obj = Instantiate(cornerWallPrefab[1], new Vector3(x, y, 0), Quaternion.identity);
                }
                else if (x == X && y == -1) // bottom right
                {
                    obj = Instantiate(cornerWallPrefab[2], new Vector3(x, y, 0), Quaternion.identity);
                }
                else if (x == X && y == Y) // top right
                {
                    obj = Instantiate(cornerWallPrefab[3], new Vector3(x, y, 0), Quaternion.identity);
                }
                break;

            case "Top":
                int topIndex = Random.Range(0, topWallPrefab.Length);
                obj = Instantiate(
                    topWallPrefab[topIndex],
                    new Vector3(x, y, 0),
                    Quaternion.identity
                );
                break;

            case "Bottom":
                int bottomIndex = Random.Range(0, bottomWallPrefab.Length);
                obj = Instantiate(
                    bottomWallPrefab[bottomIndex],
                    new Vector3(x, y, 0),
                    Quaternion.identity
                );
                break;

            case "Left":
                int leftIndex = Random.Range(0, leftWallPrefab.Length);
                obj = Instantiate(
                    leftWallPrefab[leftIndex],
                    new Vector3(x, y, 0),
                    Quaternion.identity
                );
                break;

            case "Right":
                int rightIndex = Random.Range(0, rightWallPrefab.Length);
                obj = Instantiate(
                    rightWallPrefab[rightIndex],
                    new Vector3(x, y, 0),
                    Quaternion.identity
                );
                break;

            default:
                break;
        }

        return obj;
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

    public void PlaceBoss()
    {
        int maxAttempts = 100;
        int attempts = 0;
        int x,
            y;

        do
        {
            x = Random.Range(0, mapdata.GetLength(0) - 1);
            y = Random.Range(0, mapdata.GetLength(1) - 1);

            attempts++;
            if (attempts >= maxAttempts)
            {
                Debug.LogError(
                    "[PlaceBoss] Cannot find empty space to place boss after multiple attempts."
                );
                return;
            }
        } while (!IsAreaEmpty(x, y));

        int r = Random.Range(0, bossesPrefab.Length);
        GameObject obj = Instantiate(
            bossesPrefab[r],
            new Vector3(x + 0.5f, y + 0.5f, 0),
            Quaternion.identity
        );
        obj.transform.parent = enemyParent;

        mapdata[x, y] = boss;
        mapdata[x + 1, y] = boss;
        mapdata[x, y + 1] = boss;
        mapdata[x + 1, y + 1] = boss;

        OOPBob bossInstance = obj.GetComponent<OOPBob>();
        bosses[x, y] = bossInstance;
        bosses[x + 1, y] = bossInstance;
        bosses[x, y + 1] = bossInstance;
        bosses[x + 1, y + 1] = bossInstance;

        bossInstance.positionX = x;
        bossInstance.positionY = y;
        bossInstance.mapGenerator = this;

        obj.name = $"Boss_{bossInstance.Name} ({x},{y})";
        Debug.Log($"[PlaceBoss] Successfully placed boss at ({x}, {y}) after {attempts} attempts.");
    }

    public void PlaceDemonWall(int x, int y)
    {
        int r = Random.Range(0, demonWallsPrefab.Length);
        GameObject obj = Instantiate(
            demonWallsPrefab[r],
            new Vector3(x, y, 0),
            Quaternion.identity
        );
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
        obj.transform.parent = itemPotionParent;
        mapdata[x, y] = fireStorm;
        fireStorms[x, y] = obj.GetComponent<OOPScroll>();
        fireStorms[x, y].positionX = x;
        fireStorms[x, y].positionY = y;
        fireStorms[x, y].mapGenerator = this;
        obj.name = $"FireStorm_{fireStorms[x, y].Name} {x}, {y}";
    }

    public void PlaceSmallCurrency(int x, int y)
    {
        GameObject obj = Instantiate(
            smallCurrencyPrefab[0],
            new Vector3(x, y, 0),
            Quaternion.identity
        );
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
        GameObject obj = Instantiate(
            bigCurrencyPrefab[0],
            new Vector3(x, y, 0),
            Quaternion.identity
        );
        obj.transform.parent = itemCurrencyParent;
        mapdata[x, y] = bigCurrency;
        bigCurrencies[x, y] = obj.GetComponent<OOPCurrency>();
        bigCurrencies[x, y].positionX = x;
        bigCurrencies[x, y].positionY = y;
        bigCurrencies[x, y].mapGenerator = this;
        obj.name = $"Item_{bigCurrencies[x, y].Name} {x}, {y}";
    }

    public void PlaceFreeze(int x, int y)
    {
        GameObject obj = Instantiate(freezePrefab[0], new Vector3(x, y, 0), Quaternion.identity);
        obj.transform.parent = itemPlayerParent;
        mapdata[x, y] = freeze;
        frozen[x, y] = obj.GetComponent<OOPFreeze>();
        frozen[x, y].positionX = x;
        frozen[x, y].positionY = y;
        frozen[x, y].mapGenerator = this;
        obj.name = $"Item_{frozen[x, y].Name} {x}, {y}";
    }

    public void PlaceDefense(int x, int y)
    {
        GameObject obj = Instantiate(defensePrefab[0], new Vector3(x, y, 0), Quaternion.identity);
        obj.transform.parent = itemPlayerParent;
        mapdata[x, y] = defense;
        defended[x, y] = obj.GetComponent<OOPDefense>();
        defended[x, y].positionX = x;
        defended[x, y].positionY = y;
        defended[x, y].mapGenerator = this;
        obj.name = $"Item_{defended[x, y].Name} {x}, {y}";
    }

    public bool IsAreaEmpty(int x, int y)
    {
        if (x < 0 || y < 0 || x + 1 >= mapdata.GetLength(0) || y + 1 >= mapdata.GetLength(1))
        {
            Debug.Log($"[IsAreaEmpty] Out of bounds: ({x}, {y})");
            return false;
        }

        return mapdata[x, y] == empty
            && mapdata[x + 1, y] == empty
            && mapdata[x, y + 1] == empty
            && mapdata[x + 1, y + 1] == empty;
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
            boss.TeleportBoss();
        }
    }
}
