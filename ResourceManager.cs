using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    // Resources owned by the player
    [HideInInspector]
    public int earth = 0;
    [HideInInspector]
    public int water = 0;
    [HideInInspector]
    public int fire = 0;
    [HideInInspector]
    public int air = 0;

    // Tiles under player control
    private int forestTiles = 0;
    private int lakeTiles = 0;
    private int desertTiles = 0;
    private int mountainTiles = 0;

    // Level upgrade costs
    public int levelTwoCost = 2;
    public int levelThreeCost = 3;

    public int terraformCost;
    public int tileMaxLevel = 3;

    public TileStats selectedTileStats;
    private LevelManager levelManager;
    private TileStuff tileStuff;

    private GameObject selectedTile;

    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelManager>();
        tileStuff = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileStuff>();

        earth = 0;
        water = 0;
        fire = 0;
        air = 0;
    }

    // Collects resources once turn is ended
    public void CollectResources()
    {
        forestTiles = GetComponent<LevelManager>().forestTiles;
        lakeTiles = GetComponent<LevelManager>().lakeTiles;
        desertTiles = GetComponent<LevelManager>().desertTiles;
        mountainTiles = GetComponent<LevelManager>().mountainTiles;

        earth += forestTiles;
        water += lakeTiles;
        fire += desertTiles;
        air += mountainTiles;
    }

    private void TerraformCost()
    {
        earth -= terraformCost;
        water -= terraformCost;
        fire -= terraformCost;
        air -= terraformCost;
    }

    private void UpdateTileInfo()
    {
        GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx].level++;
        GetComponent<LevelManager>().selectedTileLevel =
        GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx].level;
        FindObjectOfType<SfxHandler>().GetComponent<SfxHandler>().PlaySystemSFX(0);
    }

    //public void UpgradeTile()
    //{
    //    selectedTileStats = GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx];

    //    if (selectedTileStats.isHealthy)
    //    {
    //        switch (selectedTileStats.tileType)
    //        {
    //            case 1:
    //                TileTypeUpgrade(earth);
    //                break;

    //            case 2:
    //                TileTypeUpgrade(water);
    //                break;

    //            case 3:
    //                TileTypeUpgrade(fire);
    //                break;

    //            case 4:
    //                TileTypeUpgrade(air);
    //                break;
    //        } 
    //    }
    //    else
    //    {
    //        Debug.Log("Can only upgrade healthy tiles");
    //    }
    //}

    //private void TileTypeUpgrade(int resource)
    //{
    //    if (resource > 1 && selectedTileStats.level < 5)
    //    {
    //        switch (selectedTileStats.level)
    //        {
    //            case 1:
    //                if (resource - levelTwoCost >= 0)
    //                {
    //                    resource -= levelTwoCost;
    //                    UpdateTileInfo();
    //                }
    //                else
    //                {
    //                    Debug.Log("Not enough resources");
    //                }
    //                break;
    //            case 2:
    //                if (resource - levelThreeCost >= 0)
    //                {
    //                    resource -= levelThreeCost;
    //                    UpdateTileInfo();
    //                }
    //                else
    //                {
    //                    Debug.Log("Not enough resources");
    //                }
    //                break;
    //            case 3:
    //                if (resource - levelFourCost >= 0)
    //                {
    //                    resource -= levelFourCost;
    //                    UpdateTileInfo();
    //                }
    //                else
    //                {
    //                    Debug.Log("Not enough resources");
    //                }
    //                break;
    //            case 4:
    //                if (resource - levelFiveCost >= 0)
    //                {
    //                    resource -= levelFiveCost;
    //                    UpdateTileInfo();
    //                }
    //                else
    //                {
    //                    Debug.Log("Not enough resources");
    //                }
    //                break;
    //        }
    //    }
    //    else if (selectedTileStats.level == 5)
    //    {
    //        Debug.Log("Tile is at max level");
    //    }
    //    else
    //    {
    //        Debug.Log("Upgrading error");
    //    }
    //}

    private void UpdateTerraformInfo(int tileType)
    {
        GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx].level = 1;
        GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx].tileType = tileType;
        GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx].health = 100;
        GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx].isHealthy = true;
        GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx].isUnhealthy = false;
        GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx].isCorrupted = false;
        GetComponent<LevelManager>().selectedTileLevel = 1;
    }

    public void IncrementTerraCost()
    {
        terraformCost++;
    }

    // Terraform to forest
    public void Forest()
    {
        selectedTile = GetComponent<LevelManager>().tiles[GetComponent<LevelManager>().tileIdx];
        selectedTileStats = GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx];
        if (selectedTileStats.tileType != 1 && !selectedTileStats.isCorrupted && earth - terraformCost >= 0
            && water - terraformCost >= 0 && fire - terraformCost >= 0 && air - terraformCost >= 0)
        {
            TerraformCost();
            tileStuff.swapingTiles(6, GetComponent<LevelManager>().tileIdx, selectedTile);
            GetComponent<LevelManager>().tiles[GetComponent<LevelManager>().tileIdx] = tileStuff.Tiles[GetComponent<LevelManager>().tileIdx];
            levelManager.DecrementTileType(GetComponent<LevelManager>().tileIdx);
            UpdateTerraformInfo(1);
            GetComponent<LevelManager>().forestTiles++;
            FindObjectOfType<SfxHandler>().GetComponent<SfxHandler>().PlayTerraformSFX(0);
        }
        else
        {
            Debug.Log("Terraforming error");
        }
    }

    // Terraform to lake
    public void Lake()
    {
        selectedTile = GetComponent<LevelManager>().tiles[GetComponent<LevelManager>().tileIdx];
        selectedTileStats = GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx];
        if (selectedTileStats.tileType != 2 && !selectedTileStats.isCorrupted && earth - terraformCost >= 0
            && water - terraformCost >= 0 && fire - terraformCost >= 0 && air - terraformCost >= 0)
        {
            TerraformCost();
            tileStuff.swapingTiles(12, GetComponent<LevelManager>().tileIdx, selectedTile);
            GetComponent<LevelManager>().tiles[GetComponent<LevelManager>().tileIdx] = tileStuff.Tiles[GetComponent<LevelManager>().tileIdx];
            levelManager.DecrementTileType(GetComponent<LevelManager>().tileIdx);
            UpdateTerraformInfo(2);
            GetComponent<LevelManager>().lakeTiles++;
            FindObjectOfType<SfxHandler>().GetComponent<SfxHandler>().PlayTerraformSFX(3);
        }
        else
        {
            Debug.Log("Terraforming error");
        }
    }

    // Terraform to desert
    public void Desert()
    {
        selectedTile = GetComponent<LevelManager>().tiles[GetComponent<LevelManager>().tileIdx];
        selectedTileStats = GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx];
        if (selectedTileStats.tileType != 3 && !selectedTileStats.isCorrupted && earth - terraformCost >= 0
            && water - terraformCost >= 0 && fire - terraformCost >= 0 && air - terraformCost >= 0)
        {
            TerraformCost();
            tileStuff.swapingTiles(0, GetComponent<LevelManager>().tileIdx, selectedTile);
            GetComponent<LevelManager>().tiles[GetComponent<LevelManager>().tileIdx] = tileStuff.Tiles[GetComponent<LevelManager>().tileIdx];
            levelManager.DecrementTileType(GetComponent<LevelManager>().tileIdx);
            UpdateTerraformInfo(3);
            GetComponent<LevelManager>().desertTiles++;
            FindObjectOfType<SfxHandler>().GetComponent<SfxHandler>().PlayTerraformSFX(1);
        }
        else
        {
            Debug.Log("Terraforming error");
        }
    }

    // Terraform to mountain
    public void Mountain()
    {
        selectedTile = GetComponent<LevelManager>().tiles[GetComponent<LevelManager>().tileIdx];
        selectedTileStats = GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx];
        if (selectedTileStats.tileType != 4 && !selectedTileStats.isCorrupted && earth - terraformCost >= 0
            && water - terraformCost >= 0 && fire - terraformCost >= 0 && air - terraformCost >= 0)
        {
            TerraformCost();
            tileStuff.swapingTiles(3, GetComponent<LevelManager>().tileIdx, selectedTile);
            GetComponent<LevelManager>().tiles[GetComponent<LevelManager>().tileIdx] = tileStuff.Tiles[GetComponent<LevelManager>().tileIdx];
            levelManager.DecrementTileType(GetComponent<LevelManager>().tileIdx);
            UpdateTerraformInfo(4);
            GetComponent<LevelManager>().mountainTiles++;
            FindObjectOfType<SfxHandler>().GetComponent<SfxHandler>().PlayTerraformSFX(2);
        }
        else
        {
            Debug.Log("Terraforming error");
        }
    }

    // Terraform to tundra
    public void Tundra()
    {
        selectedTile = GetComponent<LevelManager>().tiles[GetComponent<LevelManager>().tileIdx];
        selectedTileStats = GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx];
        if (selectedTileStats.tileType != 4 && !selectedTileStats.isCorrupted && earth - terraformCost >= 0
            && water - terraformCost >= 0 && fire - terraformCost >= 0 && air - terraformCost >= 0)
        {
            TerraformCost();
            tileStuff.swapingTiles(9, GetComponent<LevelManager>().tileIdx, selectedTile);
            GetComponent<LevelManager>().tiles[GetComponent<LevelManager>().tileIdx] = tileStuff.Tiles[GetComponent<LevelManager>().tileIdx];
            levelManager.DecrementTileType(GetComponent<LevelManager>().tileIdx);
            UpdateTerraformInfo(4);
            GetComponent<LevelManager>().mountainTiles++;
            FindObjectOfType<SfxHandler>().GetComponent<SfxHandler>().PlayTerraformSFX(4);
        }
        else
        {
            Debug.Log("Terraforming error");
        }
    }

    // Upgrade selected tile
    public void UpgradeTile()
    {
        selectedTileStats = GetComponent<LevelManager>().tileStats[GetComponent<LevelManager>().tileIdx];

        if (selectedTileStats.isHealthy)
        {
            switch (selectedTileStats.tileType)
            {
                case 1:
                    if (water > 1 && selectedTileStats.level < 3)
                    {
                        switch (selectedTileStats.level)
                        {
                            case 1:
                                if (water - levelTwoCost >= 0)
                                {
                                    water -= levelTwoCost;
                                    UpdateTileInfo();
                                }
                                else
                                {
                                    Debug.Log("Not enough resources");
                                }
                                break;
                            case 2:
                                if (water - levelThreeCost >= 0)
                                {
                                    water -= levelThreeCost;
                                    UpdateTileInfo();
                                }
                                else
                                {
                                    Debug.Log("Not enough resources");
                                }
                                break;
                        }
                    }
                    else if (selectedTileStats.level == tileMaxLevel)
                    {
                        Debug.Log("Tile is at max level");
                    }
                    else
                    {
                        Debug.Log("Upgrading error");
                    }
                    break;

                case 2:
                    if (air > 1 && selectedTileStats.level < 3)
                    {
                        switch (selectedTileStats.level)
                        {
                            case 1:
                                if (air - levelTwoCost >= 0)
                                {
                                    air -= levelTwoCost;
                                    UpdateTileInfo();
                                }
                                else
                                {
                                    Debug.Log("Not enough resources");
                                }
                                break;
                            case 2:
                                if (air - levelThreeCost >= 0)
                                {
                                    air -= levelThreeCost;
                                    UpdateTileInfo();
                                }
                                else
                                {
                                    Debug.Log("Not enough resources");
                                }
                                break;
                        }
                    }
                    else if (selectedTileStats.level == tileMaxLevel)
                    {
                        Debug.Log("Tile is at max level");
                    }
                    else
                    {
                        Debug.Log("Upgrading error");
                    }
                    break;

                case 3:
                    if (earth > 1 && selectedTileStats.level < 3)
                    {
                        switch (selectedTileStats.level)
                        {
                            case 1:
                                if (earth - levelTwoCost >= 0)
                                {
                                    earth -= levelTwoCost;
                                    UpdateTileInfo();
                                }
                                else
                                {
                                    Debug.Log("Not enough resources");
                                }
                                break;
                            case 2:
                                if (earth - levelThreeCost >= 0)
                                {
                                    earth -= levelThreeCost;
                                    UpdateTileInfo();
                                }
                                else
                                {
                                    Debug.Log("Not enough resources");
                                }
                                break;
                        }
                    }
                    else if (selectedTileStats.level == tileMaxLevel)
                    {
                        Debug.Log("Tile is at max level");
                    }
                    else
                    {
                        Debug.Log("Upgrading error");
                    }
                    break;

                case 4:
                    if (fire > 1 && selectedTileStats.level < 3)
                    {
                        switch (selectedTileStats.level)
                        {
                            case 1:
                                if (fire - levelTwoCost >= 0)
                                {
                                    fire -= levelTwoCost;
                                    UpdateTileInfo();
                                }
                                else
                                {
                                    Debug.Log("Not enough resources");
                                }
                                break;
                            case 2:
                                if (fire - levelThreeCost >= 0)
                                {
                                    fire -= levelThreeCost;
                                    UpdateTileInfo();
                                }
                                else
                                {
                                    Debug.Log("Not enough resources");
                                }
                                break;
                        }
                    }
                    else if (selectedTileStats.level == tileMaxLevel)
                    {
                        Debug.Log("Tile is at max level");
                    }
                    else
                    {
                        Debug.Log("Upgrading error");
                    }
                    break;

                case 5:
                    if (fire > 1 && selectedTileStats.level < 3)
                    {
                        switch (selectedTileStats.level)
                        {
                            case 1:
                                if (fire - levelTwoCost >= 0)
                                {
                                    fire -= levelTwoCost;
                                    UpdateTileInfo();
                                }
                                else
                                {
                                    Debug.Log("Not enough resources");
                                }
                                break;
                            case 2:
                                if (fire - levelThreeCost >= 0)
                                {
                                    fire -= levelThreeCost;
                                    UpdateTileInfo();
                                }
                                else
                                {
                                    Debug.Log("Not enough resources");
                                }
                                break;
                        }
                    }
                    else if (selectedTileStats.level == tileMaxLevel)
                    {
                        Debug.Log("Tile is at max level");
                    }
                    else
                    {
                        Debug.Log("Upgrading error");
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("Can only upgrade healthy tiles");
        }
    }

    // Just in case
    //switch (GetComponent<LevelManager>().selectedTileType)
    //{
    //    case 1:
    //        Debug.Log("Terraforming error");
    //        break;
    //    case 2:
    //        GetComponent<LevelManager>().lakeTiles--;
    //        break;
    //    case 3:
    //        GetComponent<LevelManager>().desertTiles--;
    //        break;
    //    case 4:
    //        GetComponent<LevelManager>().mountainTiles--;
    //        break;
    //}
}
