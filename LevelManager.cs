using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{
    public GameObject[] tiles;
    public TileStats[] tileStats;
    private TileValues tileMap;
    private TileStuff tileStuff;

    [HideInInspector]
    public GameObject selectedTile;
    public GameObject highlight;
    public GameObject currentHighlight;

    // Selected tile info
    public bool isTileSelected = false;
    public int selectedTileLevel;
    public int selectedTileType;
    public int selectedTileHealth;

    // Tile index and total
    public int tileIdx;
    private int tileCount;

    // Tile health state
    public int healthyTiles;
    public int unhealthyTiles;
    public int corruptedTiles;

    // Health levels
    public int unhealthyStartVal;
    public int healthyThreshold;
    public int corruptedThreshold;

    // Tile types
    public int forestTiles = 0;
    public int lakeTiles = 0;
    public int desertTiles = 0;
    public int mountainTiles = 0;

    // UI elements
    private UIManager UImanager;
    public GameObject upgradeButton;
    public GameObject terraformButton;
    public GameObject winScreen;
    public GameObject loseScreen;

    // Corruption spread variables
    private GameObject[] tileNeighbors = new GameObject[6];
    private bool corruptedNeighbor;
    private bool healthyNeighbor;
    private int highestLevelNeighbor;
    private int healForce = 0;
    public int healForceOne = 10;
    public int healForceTwo = 20;
    public int healForceThree = 30;
    public int corruptionForce = 15;

    private void Awake()
    {
        UImanager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
    }

    void Start()
    {
        tileStuff = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileStuff>();
        InitializeTileStats();
    }

    void Update()
    {
        TileSelection();
    }

    // Sets health values and corrupted state for each tile
    private void InitializeTileStats()
    {
        tiles = GameObject.Find("TileManager").GetComponent<TileStuff>().Tiles;
        tileCount = tiles.Length;
        tileStats = new TileStats[tileCount];

        for (int i = 0; i < tileStats.Length; i++)
        {
            //tileStats[i].level = 1;

            if (tiles[i].name.Contains("_Healthy_"))
            {
                tileStats[i].level = 1;
                tileStats[i].health = 100;
                tileStats[i].corruption = 0;

                tileStats[i].isHealthy = true;
                tileStats[i].isUnhealthy = false;
                tileStats[i].isCorrupted = false;

                healthyTiles++;
            }
            else if (tiles[i].name.Contains("_Unhealthy_"))
            {
                tileStats[i].level = 0;
                tileStats[i].health = unhealthyStartVal;
                tileStats[i].corruption = 0;

                tileStats[i].isHealthy = false;
                tileStats[i].isUnhealthy = true;
                tileStats[i].isCorrupted = false;

                unhealthyTiles++;
            }
            else if (tiles[i].name.Contains("_Corrupted_"))
            {
                tileStats[i].level = 0;
                tileStats[i].health = 0;
                tileStats[i].corruption = 0;

                tileStats[i].isHealthy = false;
                tileStats[i].isUnhealthy = false;
                tileStats[i].isCorrupted = true;

                corruptedTiles++;
            }

            if (tiles[i].tag == "Forest")
            {
                tileStats[i].tileType = 1;
                if (tileStats[i].isHealthy)
                {
                    forestTiles++;
                }
            }
            else if (tiles[i].tag == "Lake")
            {
                tileStats[i].tileType = 2;
                if (tileStats[i].isHealthy)
                {
                    lakeTiles++;
                }
            }
            else if (tiles[i].tag == "Desert")
            {
                tileStats[i].tileType = 3;
                if (tileStats[i].isHealthy)
                {
                    desertTiles++;
                }
            }
            else if (tiles[i].tag == "Mountain")
            {
                tileStats[i].tileType = 4;
                if (tileStats[i].isHealthy)
                {
                    mountainTiles++;
                }
            }
            else if (tiles[i].tag == "Tundra")
            {
                tileStats[i].tileType = 5;
                if (tileStats[i].isHealthy)
                {
                    mountainTiles++;
                }
            }
            else if (tiles[i].tag == "Mom")
            {
                tileStats[i].tileType = 1;
                forestTiles++;
            }
        }
    }

    // Selecting a tile with left-mouse click
    private void TileSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    selectedTile = hit.collider.gameObject;
                    if (selectedTile.tag != "Mom" && selectedTile.tag != "FOW")
                    {
                        Destroy(currentHighlight);
                        currentHighlight = Instantiate(highlight, new Vector3(selectedTile.transform.position.x, selectedTile.transform.position.y + 0.25f, selectedTile.transform.position.z), selectedTile.transform.rotation);
                        for (int i = 0; i < tiles.Length; i++)
                        {
                            if (selectedTile == tiles[i])
                            {
                                selectedTileLevel = tileStats[i].level;
                                selectedTileType = tileStats[i].tileType;
                                selectedTileHealth = tileStats[i].health;
                                tileIdx = i;
                                UImanager.statusMenu.SetActive(true);
                                if (tileStats[i].isCorrupted)
                                {
                                    upgradeButton.SetActive(false);
                                    terraformButton.SetActive(false);
                                }
                                else if(tileStats[i].isUnhealthy)
                                {
                                    upgradeButton.SetActive(false);
                                }
                                else
                                {
                                    upgradeButton.SetActive(true);
                                    terraformButton.SetActive(true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Reduces healthy count of specified tile type, called when tile changes from healthy to unhealthy 
    public void DecrementTileType(int i)
    {
        if (tileStats[i].isHealthy)
        {
            switch (tileStats[i].tileType)
            {
                case 1:
                    forestTiles--;
                    break;
                case 2:
                    lakeTiles--;
                    break;
                case 3:
                    desertTiles--;
                    break;
                case 4:
                    mountainTiles--;
                    break;
            }
        }
    }

    // Corruption spread, called once every turn
    public void CorruptionSpread()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            corruptedNeighbor = false;
            healthyNeighbor = false;

            tileNeighbors[0] = tiles[i].GetComponent<TileValues>().GetNeighbor(0);
            tileNeighbors[1] = tiles[i].GetComponent<TileValues>().GetNeighbor((HexDirection)1);
            tileNeighbors[2] = tiles[i].GetComponent<TileValues>().GetNeighbor((HexDirection)2);
            tileNeighbors[3] = tiles[i].GetComponent<TileValues>().GetNeighbor((HexDirection)3);
            tileNeighbors[4] = tiles[i].GetComponent<TileValues>().GetNeighbor((HexDirection)4);
            tileNeighbors[5] = tiles[i].GetComponent<TileValues>().GetNeighbor((HexDirection)5);

            for (int j = 0; j < tiles.Length; j++)
            {
                for (int k = 0; k < tileNeighbors.Length; k++)
                {
                    if (tiles[j] == tileNeighbors[k] && tileStats[j].isCorrupted)
                    {
                        corruptedNeighbor = true;
                    }
                }
            }

            if (corruptedNeighbor)
            {
                if (!tileStats[i].isHealthy)
                {
                    if (tileStats[i].health - corruptionForce >= 0)
                    {
                        tileStats[i].health -= corruptionForce;
                    }
                    else if (tileStats[i].health - corruptionForce < 0)
                    {
                        tileStats[i].health = 0;
                    }
                    if (tileStats[i].health < corruptedThreshold && tileStats[i].isUnhealthy)
                    {
                        switch (tileStats[i].tileType)
                        {
                            case 1:
                                tileStuff.swapingTiles(8, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                break;
                            case 2:
                                tileStuff.swapingTiles(14, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                break;
                            case 3:
                                tileStuff.swapingTiles(2, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                break;
                            case 4:
                                tileStuff.swapingTiles(5, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                break;
                            case 5:
                                tileStuff.swapingTiles(11, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                break;
                        }
                    }
                }
                else if (tileStats[i].isHealthy && tileStats[i].level == 1)
                {
                    if (tileStats[i].health - (corruptionForce - 10) >= 0)
                    {
                        tileStats[i].health -= (corruptionForce - 10);
                    }
                    else if (tileStats[i].health - (corruptionForce - 10) < 0)
                    {
                        tileStats[i].health = 0;
                    }
                    if (tileStats[i].health < healthyThreshold)
                    {
                        switch (tileStats[i].tileType)
                        {
                            case 1:
                                tileStuff.swapingTiles(7, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                forestTiles--;
                                break;
                            case 2:
                                tileStuff.swapingTiles(13, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                lakeTiles--;
                                break;
                            case 3:
                                tileStuff.swapingTiles(1, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                desertTiles--;
                                break;
                            case 4:
                                tileStuff.swapingTiles(4, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                mountainTiles--;
                                break;
                            case 5:
                                tileStuff.swapingTiles(10, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                mountainTiles--;
                                break;
                        }
                    }
                }
                else if (tileStats[i].isHealthy && tileStats[i].level == 2)
                {
                    if (tileStats[i].health - (corruptionForce - 20) <= 100)
                    {
                        tileStats[i].health -= (corruptionForce - 20);
                    }
                    else if (tileStats[i].health - corruptionForce - 20 > 100)
                    {
                        tileStats[i].health = 100;
                    }
                }
                else if (tileStats[i].isHealthy && tileStats[i].level == 3)
                {
                    if (tileStats[i].health - (corruptionForce - 30) <= 100)
                    {
                        tileStats[i].health -= (corruptionForce - 30);
                    }
                    else if (tileStats[i].health - (corruptionForce - 30) > 100)
                    {
                        tileStats[i].health = 100;
                    }
                }
            }                                  

            if (!tileStats[i].isHealthy)
            {
                highestLevelNeighbor = 0;

                for (int j = 0; j < tiles.Length; j++)
                {
                    for (int k = 0; k < tileNeighbors.Length; k++)
                    {
                        if (tiles[j] == tileNeighbors[k] && tileStats[j].isHealthy)
                        {
                            healthyNeighbor = true;
                            if (tileStats[j].level > highestLevelNeighbor)
                            {
                                highestLevelNeighbor = tileStats[j].level;
                            }
                        }
                    }
                }

                switch (highestLevelNeighbor)
                {
                    case 1:
                        healForce = healForceOne;
                        break;
                    case 2:
                        healForce = healForceTwo;
                        break;
                    case 3:
                        healForce = healForceThree;
                        break;
                }

                if (healthyNeighbor)
                {
                    if (tileStats[i].health + healForce <= 100)
                    {
                        tileStats[i].health += healForce;
                    }
                    else if (tileStats[i].health + healForce > 100)
                    {
                        tileStats[i].health = 100;
                    }

                    if (tileStats[i].health >= corruptedThreshold && tileStats[i].isCorrupted)
                    {
                        switch (tileStats[i].tileType)
                        {
                            case 1:
                                tileStuff.swapingTiles(7, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                break;
                            case 2:
                                tileStuff.swapingTiles(13, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                break;
                            case 3:
                                tileStuff.swapingTiles(1, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                break;
                            case 4:
                                tileStuff.swapingTiles(4, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                break;
                            case 5:
                                tileStuff.swapingTiles(10, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                break;
                        }
                    }
                    else if (tileStats[i].health >= healthyThreshold && tileStats[i].isUnhealthy)
                    {
                        switch (tileStats[i].tileType)
                        {
                            case 1:
                                tileStuff.swapingTiles(6, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                forestTiles++;
                                break;
                            case 2:
                                tileStuff.swapingTiles(12, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                lakeTiles++;
                                break;
                            case 3:
                                tileStuff.swapingTiles(0, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                desertTiles++;
                                break;
                            case 4:
                                tileStuff.swapingTiles(3, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                mountainTiles++;
                                break;
                            case 5:
                                tileStuff.swapingTiles(9, i, tiles[i]);
                                tiles[i] = tileStuff.Tiles[i];
                                mountainTiles++;
                                break;
                        }
                    }
                }
            }
        }

        CheckTileState();
    }

    // Update tile state based on health
    private void CheckTileState()
    {
        healthyTiles = 0;
        unhealthyTiles = 0;
        corruptedTiles = 0;

        for (int i = 0; i < tileStats.Length; i++)
        {
            if (tileStats[i].health < corruptedThreshold)
            {
                tileStats[i].isCorrupted = true;
                tileStats[i].isHealthy = false;
                tileStats[i].isUnhealthy = false;
                tileStats[i].level = 0;
                corruptedTiles++;
            }
            else if (tileStats[i].health >= corruptedThreshold && tileStats[i].health < healthyThreshold)
            {
                tileStats[i].isCorrupted = false;
                tileStats[i].isHealthy = false;
                tileStats[i].isUnhealthy = true;
                tileStats[i].level = 0;
                unhealthyTiles++;
            }
            else if (tileStats[i].health >= healthyThreshold)
            {
                if (!tileStats[i].isHealthy)
                {
                    tileStats[i].level = 1;
                }
                tileStats[i].isCorrupted = false;
                tileStats[i].isHealthy = true;
                tileStats[i].isUnhealthy = false;
                healthyTiles++;
            }
        }

        if (corruptedTiles == 0)
        {
            WinCon();
        }
        else if (tileStats[210].isCorrupted)
        {
            LoseCon();
        }
    }

    private void WinCon()
    {
        winScreen.SetActive(true);
        Debug.Log("You Win");
    }

    private void LoseCon()
    {
        loseScreen.SetActive(true);
        Debug.Log("You Lose");
    }
}