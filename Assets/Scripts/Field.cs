using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    public static Field Instance;
    Action _onClose;


    [SerializeField] private Bottle bottle;
    [SerializeField] private Transform gameField;


    public List<Bottle> listBottle = new();

    public List<WaterColor> listColor = new();
    public int colorCount ;

    public int levelTarget;

    public int levelProcess = 0;


    
    public List<LevelDataSO> listLevels = new List<LevelDataSO>();
    public LevelDataSO[] dataLevel;

    public int colorAmount;
    public int bottleAmount;
    public int currentLevel;

    public WaterColor waterColor;
    public List<WaterColor> listLevelWaterColor = new();

    public bool isUpdate = false;
    public TextMeshProUGUI levelTxt;

    private void Start()
    {
        Instance = this;
        LoadListLevelData();
    }


    public void UpdateData()
    {
        
        LoadLevel();
        isUpdate = true;
    }


    public void SetTextLevel(string text)
    {
        levelTxt.text= "Level: " + text;
    }


    public void LoadListLevelData()
    {
        dataLevel = Resources.LoadAll<LevelDataSO>("ScriptableObjects");
        listLevels.OrderByDescending(t => int.Parse(t.level.ToString()) ).ToList();
        foreach(LevelDataSO item in dataLevel)
        {
            listLevels.Add(item);
            
        }
    }

    public void LoadLevel()
    {
        currentLevel = UserDataManager.Instance.Level;
        SetTextLevel(currentLevel.ToString());
        LoadLevelData();
    }

    public void LoadLevelData()
    {
        colorAmount = listLevels[currentLevel].colorAmount;
        bottleAmount = listLevels[currentLevel].bottleAmount;
        levelTarget = colorAmount;
        listLevelWaterColor.Clear();
        for(int i = 0; i < colorAmount; i++)
        {
            waterColor = (WaterColor)Random.Range(1, 13);
            while(listLevelWaterColor.Contains(waterColor))
            {
                waterColor = (WaterColor)Random.Range(1, 13);
            }
            listLevelWaterColor.Add(waterColor);
        }
    }



    public void LoadListColor()
    {
        listColor.Clear();
        int index = 0;
        
        int colorCheckCount = 1;
        colorCount = colorAmount * 5;


        for (int i = 0; i < colorCount; i++)
        {

            WaterColor colorCheck = listLevelWaterColor[index];

            if (colorCheckCount <5)
            {
                colorCheckCount++;
                listColor.Add(colorCheck);
            } 
            else 
            {
                colorCheckCount = 1;
                colorCheck = listLevelWaterColor[index++];
                listColor.Add(colorCheck);
            }               
        }
    }

    public void LoadData()
    {
        ClearData();

        for (int i =0; i<bottleAmount;i++)
        {
            var bottleItem = Instantiate(bottle,gameField);
            bottleItem.index = i;
            listBottle.Add(bottleItem);
            //listFirstBottle.Add(bottleItem,bottleItem.transform);
        }
    }


    public void ClearData()
    {
        listBottle.Clear();
        for (int i = 0; i < gameField.childCount; i++)
        {
            Destroy(gameField.GetChild(i).gameObject);
        }
    }

    
    public static void Show()
    {
        Instance.LoadListColor();
        Instance.levelProcess = 0;
        Instance.LoadData();
    }



    public static void Test()
    {
        Instance.LoadListColor();
    }


    public void CheckWin()
    {
        if(levelProcess == levelTarget)
        {
            Debug.Log("====CONGRATULATION====");
            GameSceneManager.Instance.popupWin.gameObject.SetActive(true);
        }
    }
    public bool isLose = false;
    public bool isEnd = false;
    public Bottle itemCheck;
    public void CheckWater()
    {
        if (CheckEmptyBottle()) return;
        else
        {
            try
            {
                int index = 0;
                for (int j = 0; j < listBottle.Count; j++)
                {
                    itemCheck = listBottle[index];
                    if (listBottle[j].listwaterItemActive[0].color != itemCheck.listwaterItemActive[0].color)
                    {
                        isLose = true;
                    }
                    else if(listBottle[j].listwaterItemActive[0].color == itemCheck.listwaterItemActive[0].color)
                    {
                        if (listBottle[j].listwaterItemActive.Count == 5 || itemCheck.listwaterItemActive.Count == 5)
                        {
                            isLose = true;
                        }
                        else
                        {
                            isLose = false;
                            break;
                        }

                    }
                    else
                    {
                        isLose = false;
                        break;
                    }
                    index++;
                }

                Debug.Log("isLose: " + isLose);
            }
            catch(Exception ex)
            {
                Debug.Log("error: " + ex);
            }

            
        }



        if(isLose)
        {
            GameSceneManager.Instance.popupLose.gameObject.SetActive(true);
        }
    }



    public bool CheckEmptyBottle()
    {
        bool haveBottleEmpty = true;
        foreach (var obj in listBottle)
        {
            if (obj.listwaterItemActive.Count == 0)
            {
                haveBottleEmpty = true;
            }
            else haveBottleEmpty = false;
        }

        return haveBottleEmpty;
    }
}
