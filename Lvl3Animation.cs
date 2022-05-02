using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl3Animation : MonoBehaviour
{
    public GameObject deer;
    public GameObject armadillo;
    public GameObject dolphin;
    public GameObject polarbear;

    TileStuff tileStuff;
    LevelManager levelManager;

    private void Start()
    {
        tileStuff = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileStuff>();
        levelManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelManager>();
    }
    /// <summary>
    /// Current work around so that I wouldent mess with jesse's code.
    /// it works by looping through all of the Tiles array and checking their level and if they already have an animal or not, lvl 3 + animal = nothing, lvl 3 + no animal = add aniaml, not lvl 3 + has animal = remove animal.
    /// </summary>
    public void tileUpdate()
    {
        for (int i = 0; i < tileStuff.Tiles.Length; i++)
        {
            if (levelManager.tileStats[i].level == 3 && !checkIfAnimal(i))
            {
                PlaceAnimals(tileStuff.Tiles[i]);
            }
            else if (checkIfAnimal(i) && levelManager.tileStats[i].level != 3)
            {
                for (int e = 0; e < tileStuff.Tiles[i].transform.childCount; e++)
                {
                    if (tileStuff.Tiles[i].transform.GetChild(e).gameObject.tag == "Animal")
                    {
                        Destroy(tileStuff.Tiles[i].transform.GetChild(e).gameObject);
                    }
                }
            }
        }
    }
    /// <summary>
    /// used for removing animals from the tile.
    /// </summary>
    /// <param name="kyle">the gameobject the animal will be removed from.</param>
    public void removeAniaml(GameObject kyle)
    {
        for (int e = 0; e < kyle.transform.childCount; e++)
        {
            if (kyle.transform.GetChild(e).gameObject.tag == "Animal")
            {
                Destroy(kyle.transform.GetChild(e).gameObject);
            }
        }
    }
    /// <summary>
    /// A little bool that checks if there is an aniaml on a tile or not.
    /// </summary>
    /// <param name="i">the tile in Tiles array to check if it has an animal.</param>
    /// <returns></returns>
    public bool checkIfAnimal(int i)
    {
        for (int e = 0; e < tileStuff.Tiles[i].transform.childCount; e++)
        {
            if (tileStuff.Tiles[i].transform.GetChild(e).gameObject.tag == "Animal")
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// places the animals on the tiles.
    /// </summary>
    /// <param name="tile">the tile the animal will be placed on.</param>
    public void PlaceAnimals(GameObject tile)
    {
        GameObject tmp;
        switch (tile.GetComponent<TileValues>().tileType)
        {
            case 0:
                tmp = Instantiate(armadillo, tile.transform.position, tile.transform.rotation);
                tmp.transform.parent = tile.transform;
                break;
            //case 3:
            //    tmp = Instantiate(goat?, tile.transform.position, tile.transform.rotation);
            //    tmp.transform.parent = tile.transform;
            //    tmp.transform.position += new Vector3(0, 1.007f, 0);
                //break;
            case 6:
                tmp = Instantiate(deer, tile.transform.position, tile.transform.rotation);
                tmp.transform.parent = tile.transform;
                break;
            //case 9:
            //    tmp = Instantiate(polarbear, tile.transform.position, tile.transform.rotation);
            //    tmp.transform.parent = tile.transform;
            //    tmp.transform.position += new Vector3(0, 1.007f, 0);
            //    break;
            case 12:
                tmp = Instantiate(dolphin, tile.transform.position, tile.transform.rotation);
                tmp.transform.parent = tile.transform;
                tmp.transform.position += new Vector3(0,.55f,0);
                break;
        }

    }
}
