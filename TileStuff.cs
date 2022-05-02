using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStuff : MonoBehaviour
{
    public int width;
    public int height;
    public int map;
    /// <summary>
    /// A premade map.
    /// </summary>
    public int[] map1 = {    8, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                             0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                              0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                             0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                              0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                             0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                              0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                             0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                             0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                             0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                             0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                             0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                              3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                             2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                             0, 1, 2, 3, 4, 5, 6, 7, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    /// <summary>
    /// Array that stores all the tiles in the map.
    /// </summary>
    public GameObject[] Tiles;
    public GameObject baseprefab;
    public GameObject[] desert;
    public GameObject[] desertUnhealthy;
    public GameObject[] desertCorrupted;
    public GameObject[] mountain;
    public GameObject[] mountainUnhealthy;
    public GameObject[] mountainCurrupted;
    public GameObject[] forest;
    public GameObject[] forestUnhealthy;
    public GameObject[] forestCorrupted;
    public GameObject[] tundra;
    public GameObject[] tundraUnhealthy;
    public GameObject[] tundraCorrupted;
    public GameObject[] ocean;
    public GameObject[] oceanUnhealthy;
    public GameObject[] oceanCorrupted;
    public GameObject[] motherTree;
    public GameObject[] corrutpedMotherTree;
    /// <summary>
    /// Fog of War
    /// </summary>
    public GameObject FOW;

    Lvl3Animation anim;
    public const float outerRadius = .999f;

    public const float innerRadius = outerRadius * 0.866025404f;


    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Lvl3Animation>();
        mapCreation(map);
        lineOfSight();
    }
    /// <summary>
    /// used to spawn a map.
    /// </summary>
    public void arrangeTiles()
    {
        Tiles = new GameObject[width * height];
        int i = 0;
        TileValues tmp;
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {

                GameObject temp = Instantiate(baseprefab, new Vector3((x + z * 0.5f - z / 2) * (innerRadius * 2f), 0, z * (outerRadius * 1.5f)), new Quaternion());
                temp.transform.Rotate(0, 30, 0);
                temp.GetComponent<TileValues>().coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
                Tiles[i] = temp;
                tmp = Tiles[i].GetComponent<TileValues>();
                tmp.arrayPostion = i;
                if (x > 0)
                {
                    tmp.SetNeighbor(HexDirection.W, Tiles[i - 1]);
                }
                if (z > 0)
                {
                    if ((z & 1) == 0)
                    {
                        tmp.SetNeighbor(HexDirection.SE, Tiles[i - width]);
                        if (x > 0)
                        {
                            tmp.SetNeighbor(HexDirection.SW, Tiles[i - width - 1]);
                        }
                    }
                    else
                    {
                        tmp.SetNeighbor(HexDirection.SW, Tiles[i - width]);
                        if (x < width - 1)
                        {
                            tmp.SetNeighbor(HexDirection.SE, Tiles[i - width + 1]);
                        }
                    }
                }
                i++;
            }
        }
    }

    /// <summary>
    /// used to choose what premade map to spawn.
    /// </summary>
    /// <param name="a">map to use.</param>
    public void mapCreation(int a)
    {
        switch (a)
        {
            case 0:
                arrangeTiles();
                break;
            case 1:
                width = 20;
                height = 20;
                arrangeTiles();
                for (int z = 0, i = 0; z < height; z++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (i < Tiles.Length)
                        {
                            Tiles[i].GetComponent<TileValues>().tileType = map1[i];
                            Tiles[i].GetComponent<TileValues>().FOW = true;
                            swapingTiles(map1[i], i, Tiles[i]);
                            addFog(Tiles[i]);
                            i++;
                        }
                    }
                }
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// void to add fog.
    /// </summary>
    /// <param name="tile">tile to add fog of war too.</param>
    public void addFog(GameObject tile)
    {
        GameObject tmp;
        tmp = Instantiate(FOW,tile.transform.position + new Vector3(0,1.7f,0), tile.transform.rotation);
        tile.GetComponent<TileValues>().FOW = true;
        tmp.transform.parent = tile.transform;
    }

    /// <summary>
    /// void to remove the fog.
    /// </summary>
    /// <param name="tile">tile to remove fog from.</param>
    public void removeFog(GameObject tile)
    {
        for (int i = 0; i < tile.transform.childCount; i++)
        {
            if (tile.transform.GetChild(i).gameObject.tag == "FOW")
            {
                Destroy(tile.transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// used for looping through healthy tiles and unfogging tiles two away from them. 
    /// </summary>
    public void lineOfSight()
    {
        for(int i = 0; i < Tiles.Length; i++)
        {
            TileValues tile = Tiles[i].GetComponent<TileValues>();
            TileValues tile1;
            TileValues tile2;
            GameObject neighboreTile;
            GameObject neighboreTile1;
            switch (tile.tileType)
            {
                case 0:
                    tile.FOW = false;
                    removeFog(Tiles[i]);
                    for (int x = 0; x <= 5; x++)
                    {
                        if (!tile.GetNeighbor((HexDirection)x).Equals(null))
                        {
                            neighboreTile = tile.GetNeighbor((HexDirection)x);
                            tile1 = neighboreTile.GetComponent<TileValues>();
                            tile1.FOW = false;
                            removeFog(neighboreTile);
                            for (int y = 0; y <= 5; y++)
                            {
                                if (!tile1.GetNeighbor((HexDirection)y).Equals(null))
                                {
                                    neighboreTile1 = tile1.GetNeighbor((HexDirection)y);
                                    tile2 = neighboreTile1.GetComponent<TileValues>();
                                    tile2.FOW = false;
                                    removeFog(neighboreTile1);
                                }
                            }
                        }
                    }
                    break;
                case 3:
                    tile.FOW = false;
                    removeFog(Tiles[i]);
                    for (int x = 0; x <= 5; x++)
                    {
                        if (!tile.GetNeighbor((HexDirection)x).Equals(null))
                        {
                            neighboreTile = tile.GetNeighbor((HexDirection)x);
                            tile1 = neighboreTile.GetComponent<TileValues>();
                            tile1.FOW = false;
                            removeFog(neighboreTile);
                            for (int y = 0; y <= 5; y++)
                            {
                                if (!tile1.GetNeighbor((HexDirection)y).Equals(null))
                                {
                                    neighboreTile1 = tile1.GetNeighbor((HexDirection)y);
                                    tile2 = neighboreTile1.GetComponent<TileValues>();
                                    tile2.FOW = false;
                                    removeFog(neighboreTile1);
                                }
                            }

                        }
                    }
                    break;
                case 6:
                    tile.FOW = false;
                    removeFog(Tiles[i]);
                    for (int x = 0; x <= 5; x++)
                    {
                        if (!tile.GetNeighbor((HexDirection)x).Equals(null))
                        {
                            neighboreTile = tile.GetNeighbor((HexDirection)x);
                            tile1 = neighboreTile.GetComponent<TileValues>();
                            tile1.FOW = false;
                            removeFog(neighboreTile);
                            for (int y = 0; y <= 5; y++)
                            {
                                if (!tile1.GetNeighbor((HexDirection)y).Equals(null))
                                {
                                    neighboreTile1 = tile1.GetNeighbor((HexDirection)y);
                                    tile2 = neighboreTile1.GetComponent<TileValues>();
                                    tile2.FOW = false;
                                    removeFog(neighboreTile1);
                                }
                            }

                        }
                    }
                    break;
                case 9:
                    tile.FOW = false;
                    removeFog(Tiles[i]);
                    for (int x = 0; x <= 5; x++)
                    {
                        if (!tile.GetNeighbor((HexDirection)x).Equals(null))
                        {
                            neighboreTile = tile.GetNeighbor((HexDirection)x);
                            tile1 = neighboreTile.GetComponent<TileValues>();
                            tile1.FOW = false;
                            removeFog(neighboreTile);
                            for (int y = 0; y <= 5; y++)
                            {
                                if (!tile1.GetNeighbor((HexDirection)y).Equals(null))
                                {
                                    neighboreTile1 = tile1.GetNeighbor((HexDirection)y);
                                    tile2 = neighboreTile1.GetComponent<TileValues>();
                                    tile2.FOW = false;
                                    removeFog(neighboreTile1);
                                }
                            }

                        }
                    }
                    break;
                case 12:
                    tile.FOW = false;
                    removeFog(Tiles[i]);
                    for (int x = 0; x <= 5; x++)
                    {
                        if (tile.GetNeighbor((HexDirection)x)??null)
                        {
                            neighboreTile = tile.GetNeighbor((HexDirection)x);
                            tile1 = neighboreTile.GetComponent<TileValues>();
                            tile1.FOW = false;
                            removeFog(neighboreTile);
                            for (int y = 0; y <= 5; y++)
                            {
                                if (tile1.GetNeighbor((HexDirection)y)??null)
                                {
                                    neighboreTile1 = tile1.GetNeighbor((HexDirection)y);
                                    tile2 = neighboreTile1.GetComponent<TileValues>();
                                    tile2.FOW = false;
                                    removeFog(neighboreTile1);
                                }
                            }

                        }
                    }
                    break;
                default:
                    break;
            }
        }

    }


    /// <summary>
    /// This void is used to set the neighbores for the swaping tiles.
    /// </summary>
    /// <param name="temp">Main tile that neighbores are being set for.</param>
    /// <param name="i">Position in the array</param>
    public void neighboreSetting(TileValues temp, int i)
    {
        if (temp.coordinates.X > 0)
        {
            temp.SetNeighbor(HexDirection.W, Tiles[i - 1]);
        }
        if (temp.coordinates.X + 1 < width)
        {
            temp.SetNeighbor(HexDirection.E, Tiles[i + 1]);
        }
        if (temp.coordinates.Z > 0)
        {
            if ((temp.coordinates.Z & 1) == 0)
            {
                temp.SetNeighbor(HexDirection.SE, Tiles[i - width]);
                if (temp.coordinates.X > 0)
                {
                    temp.SetNeighbor(HexDirection.SW, Tiles[i - width - 1]);
                }
            }
            else
            {
                temp.SetNeighbor(HexDirection.SW, Tiles[i - width]);
                if (temp.coordinates.X < width - 1)
                {
                    temp.SetNeighbor(HexDirection.SE, Tiles[i - width + 1]);
                }
            }
        }
        if (temp.coordinates.Z + 1 < height)
        {
            if ((temp.coordinates.Z & 1) == 0)
            {
                temp.SetNeighbor(HexDirection.NW, Tiles[i + width]);
                if (temp.coordinates.X + 1 % width > 0)
                {
                    temp.SetNeighbor(HexDirection.NE, Tiles[i + width - 1]);
                }
            }
            else
            {
                temp.SetNeighbor(HexDirection.NE, Tiles[i + width]);
                if (temp.coordinates.X > 0)
                {
                    temp.SetNeighbor(HexDirection.NW, Tiles[i + width + 1]);
                }
            }
        }
    }

    /// <summary>
    /// SwapingTiles is used to replace tiles and their values.
    /// </summary>
    /// <param name="a">What tile you want to replace the current tile with. 0 desert, 1 desert unhealthy, 2 desert corrupted, 3 mountain, 4 mountain unhealthy, 5 mountain corrupted, 6 forest, 7 forest unhealthy, 8 forest corrupted, 9 tundra, 10 tundrea unhealthy, 
    /// 11 tundra corrupted, 12 ocean, 13 ocean unhealthy, 14 ocean corrupted, 15 Mother tree, 16 Mother tree corrupted.</param>
    /// <param name="i">Position that the tile is in the Tiles array.</param>
    /// <param name="r">The tile you want to replace.</param>
    public void swapingTiles(int a, int i, GameObject r)
    {
        GameObject tmp;
        TileValues temp;
        int t;
        switch (a)
        {
            case 0:
                t = Random.Range(0, desert.Length);
                tmp = Instantiate(desert[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 0;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 1:
                t = Random.Range(0, desertUnhealthy.Length);
                tmp = Instantiate(desertUnhealthy[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 1;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 2:
                t = Random.Range(0, desertCorrupted.Length);
                tmp = Instantiate(desertCorrupted[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 2;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 3:
                t = Random.Range(0, mountain.Length);
                tmp = Instantiate(mountain[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 3;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 4:
                t = Random.Range(0, mountainUnhealthy.Length);
                tmp = Instantiate(mountainUnhealthy[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 4;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 5:

                t = Random.Range(0, mountainCurrupted.Length);
                tmp = Instantiate(mountainCurrupted[t], r.transform.position , r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 5;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 6:
                t = Random.Range(0, forest.Length);
                tmp = Instantiate(forest[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 6;
                temp.arrayPostion = i;
                if (r.transform.childCount == 2)
                {
                    r.transform.GetChild(1).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 7:
                t = Random.Range(0, forestUnhealthy.Length);
                tmp = Instantiate(forestUnhealthy[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 7;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 8:
                t = Random.Range(0, forestCorrupted.Length);
                tmp = Instantiate(forestCorrupted[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 8;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 9:
                t = Random.Range(0, tundra.Length);
                tmp = Instantiate(tundra[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 9;
                temp.arrayPostion = i;
                if (r.transform.childCount == 2)
                {
                    r.transform.GetChild(1).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 10:
                t = Random.Range(0, tundraUnhealthy.Length);
                tmp = Instantiate(tundraUnhealthy[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 10;
                temp.arrayPostion = i;
                if (r.transform.childCount == 2)
                {
                    r.transform.GetChild(1).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 11:
                t = Random.Range(0, tundraCorrupted.Length);
                tmp = Instantiate(tundraCorrupted[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 11;
                temp.arrayPostion = i;
                if (r.transform.childCount == 2)
                {
                    r.transform.GetChild(1).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 12:
                t = Random.Range(0, ocean.Length);
                tmp = Instantiate(ocean[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 12;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 13:
                t = Random.Range(0, oceanUnhealthy.Length);
                tmp = Instantiate(oceanUnhealthy[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 13;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 14:
                t = Random.Range(0, oceanCorrupted.Length);
                tmp = Instantiate(oceanCorrupted[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 14;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 15:
                t = Random.Range(0, motherTree.Length);
                tmp = Instantiate(motherTree[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 14;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            case 16:
                t = Random.Range(0, corrutpedMotherTree.Length);
                tmp = Instantiate(corrutpedMotherTree[t], r.transform.position, r.transform.rotation);
                temp = tmp.GetComponent<TileValues>();
                temp.coordinates = r.GetComponent<TileValues>().coordinates;
                temp.tileType = 14;
                temp.arrayPostion = i;
                if (r.transform.childCount == 1)
                {
                    r.transform.GetChild(0).transform.parent = temp.transform;
                }
                neighboreSetting(temp, i);
                Tiles[i] = tmp;
                Destroy(r);
                break;
            default:
                break;
        }
    }
}

