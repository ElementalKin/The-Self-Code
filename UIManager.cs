using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Text earthTxt;
    private Text waterTxt;
    private Text fireTxt;
    private Text airTxt;

    private Text earthNextTurn;
    private Text waterNextTurn;
    private Text fireNextTurn;
    private Text airNextTurn;

    private Text tileLevelTxt;
    private Text tileHealthTxt;
    private Text upgradeCostTxt;
    private Text terraCostTxt;
    private Text turnTxt;

    private GameObject upgradeCost;
    private GameObject terraCost;

    public Image costType;
    public Sprite fireSprite;
    public Sprite airSprite;
    public Sprite earthSprite;
    public Sprite waterSprite;

    public int selectedTileLevel;
    public int selectedTileType;
    public int turn;

    public GameObject storyPanel;
    public GameObject howToPanel;
    public GameObject howToTwo;
    public GameObject howToThree;

    public GameObject statusMenu;
    public GameObject terraformMenu;

    public Slider slider;

    public readonly GameObject selectedTile;
    private TileStuff tileStuff;
    private ResourceManager resourceManager;
    private LevelManager levelManager;

    private void Awake()
    {
        turn = 1;
        statusMenu.SetActive(true);
        terraformMenu.SetActive(true);
        storyPanel.SetActive(false);

        resourceManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResourceManager>();
        levelManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LevelManager>();

        earthTxt = GameObject.Find("EarthValue").GetComponent<Text>();
        waterTxt = GameObject.Find("WaterValue").GetComponent<Text>();
        fireTxt = GameObject.Find("FireValue").GetComponent<Text>();
        airTxt = GameObject.Find("AirValue").GetComponent<Text>();

        earthNextTurn = GameObject.Find("EarthNT").GetComponent<Text>();
        waterNextTurn = GameObject.Find("WaterNT").GetComponent<Text>();
        fireNextTurn = GameObject.Find("FireNT").GetComponent<Text>();
        airNextTurn = GameObject.Find("AirNT").GetComponent<Text>();

        tileLevelTxt = GameObject.Find("LevelValue").GetComponent<Text>();
        tileHealthTxt = GameObject.Find("HealthValue").GetComponent<Text>();
        upgradeCostTxt = GameObject.Find("UpgradeCostValue").GetComponent<Text>();
        terraCostTxt = GameObject.Find("TerraCostValue").GetComponent<Text>();
        turnTxt = GameObject.Find("TurnValue").GetComponent<Text>();

        upgradeCost = GameObject.Find("UpgradeCostText");
        terraCost = GameObject.Find("TerraCostText");
    }

    void Start()
    {
        tileStuff = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileStuff>();

        statusMenu.SetActive(false);
        terraformMenu.SetActive(false);

        storyPanel.SetActive(true);
    }

    void Update()
    {
        earthTxt.text = resourceManager.earth.ToString();
        waterTxt.text = resourceManager.water.ToString();
        fireTxt.text = resourceManager.fire.ToString();
        airTxt.text = resourceManager.air.ToString();

        earthNextTurn.text = "+" + levelManager.forestTiles.ToString();
        waterNextTurn.text = "+" + levelManager.lakeTiles.ToString();
        fireNextTurn.text = "+" + levelManager.desertTiles.ToString();
        airNextTurn.text = "+" + levelManager.mountainTiles.ToString();

        turnTxt.text = turn.ToString();

        if (statusMenu.activeSelf == true)
        {
            SetHealth(GameObject.Find("GameManager").GetComponent<LevelManager>().selectedTileHealth);
            tileLevelTxt.text = GameObject.Find("GameManager").GetComponent<LevelManager>().selectedTileLevel.ToString();
            tileHealthTxt.text = GameObject.Find("GameManager").GetComponent<LevelManager>().selectedTileHealth.ToString();
            terraCostTxt.text = GameObject.Find("GameManager").GetComponent<ResourceManager>().terraformCost.ToString();

            if (GameObject.Find("GameManager").GetComponent<LevelManager>().selectedTileHealth <=
                GameObject.Find("GameManager").GetComponent<LevelManager>().corruptedThreshold)
            {
                upgradeCost.SetActive(false);
                for (int i = 0; i < upgradeCost.transform.childCount; i++)
                {
                    upgradeCost.transform.GetChild(i).gameObject.SetActive(false);
                }
                terraCost.SetActive(false);
                for (int i = 0; i < terraCost.transform.childCount; i++)
                {
                    terraCost.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            else if (GameObject.Find("GameManager").GetComponent<LevelManager>().selectedTileLevel == 1)
            {
                upgradeCost.SetActive(true);
                for (int i = 0; i < upgradeCost.transform.childCount; i++)
                {
                    upgradeCost.transform.GetChild(i).gameObject.SetActive(true);
                }
                terraCost.SetActive(true);
                for (int i = 0; i < terraCost.transform.childCount; i++)
                {
                    terraCost.transform.GetChild(i).gameObject.SetActive(true);
                }
                upgradeCostTxt.text = GameObject.Find("GameManager").GetComponent<ResourceManager>().levelTwoCost.ToString();
            }
            else if (GameObject.Find("GameManager").GetComponent<LevelManager>().selectedTileLevel == 2)
            {
                upgradeCost.SetActive(true);
                for (int i = 0; i < upgradeCost.transform.childCount; i++)
                {
                    upgradeCost.transform.GetChild(i).gameObject.SetActive(true);
                }
                terraCost.SetActive(true);
                for (int i = 0; i < terraCost.transform.childCount; i++)
                {
                    terraCost.transform.GetChild(i).gameObject.SetActive(true);
                }
                upgradeCostTxt.text = GameObject.Find("GameManager").GetComponent<ResourceManager>().levelThreeCost.ToString();
            }
            else
            {
                upgradeCost.SetActive(false);
                for (int i = 0; i < upgradeCost.transform.childCount; i++)
                {
                    upgradeCost.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            switch (GameObject.Find("GameManager").GetComponent<LevelManager>().selectedTileType)
            {
                case 1:
                    costType.sprite = waterSprite;
                    break;
                case 2:
                    costType.sprite = airSprite;
                    break;
                case 3:
                    costType.sprite = earthSprite;
                    break;
                case 4:
                    costType.sprite = fireSprite;
                    break;
            }
        }
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void TerraformSelection()
    {
        terraformMenu.SetActive(true);
    }

    public void NextTurn()
    {
        resourceManager.CollectResources();
        levelManager.CorruptionSpread();
        tileStuff.lineOfSight();
        statusMenu.SetActive(false);
        terraformMenu.SetActive(false);
        levelManager.isTileSelected = false;
        turn++;

        if (turn % 5 == 0)
        {
            resourceManager.IncrementTerraCost();
        }
    }

    public void ExitMenu()
    {
        statusMenu.SetActive(false);
        terraformMenu.SetActive(false);
        GameObject.Find("GameManager").GetComponent<LevelManager>().isTileSelected = false;
        Destroy(GameObject.Find("GameManager").GetComponent<LevelManager>().currentHighlight);
    }

    public void ExitTerraformMenu()
    {
        terraformMenu.SetActive(false);
    }

    public void HelpButton()
    {
        storyPanel.SetActive(true);
    }

    public void StoryNext()
    {
        storyPanel.SetActive(false);
        howToPanel.SetActive(true);
    }

    public void HowToNext()
    {
        howToPanel.SetActive(false);
        howToTwo.SetActive(true);
    }

    public void HowTwoNext()
    {
        howToTwo.SetActive(false);
        howToThree.SetActive(true);
    }

    public void HowToBack()
    {
        howToPanel.SetActive(false);
        storyPanel.SetActive(true);
    }

    public void HowTwoBack()
    {
        howToTwo.SetActive(false);
        howToPanel.SetActive(true);
    }

    public void HowToThreeBack()
    {
        howToThree.SetActive(false);
        howToTwo.SetActive(true);
    }

    public void ExitHowTo()
    {
        howToPanel.SetActive(false);
        howToTwo.SetActive(false);
        howToThree.SetActive(false);
    }
}
