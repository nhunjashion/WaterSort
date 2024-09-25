using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
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
    public Image bg;

    [Header("BOTTLE SELECT")]

    public Bottle bottleSelected1;
    public Bottle bottleSelected2;

    public WaterColor colorSelect1;
    public WaterColor colorSelect2;

    public int colorCount1 = 0;
    public int blankSlotColor = 0;

    public int waterIndex1 = 0;
    public int waterIndex2 = 0;


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


    private void OnEnable()
    {
        Instance = this;
        LoadListLevelData();
    }

    private void Start()
    {
       

        LoadLevelData();


        LoadListColor();

        LoadData();

        

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

    public void LoadLevelData()
    {
        currentLevel = UserDataManager.Instance.Level;

        levelTxt.text = "Level: " + currentLevel.ToString();
        colorAmount = listLevels[currentLevel].colorAmount;
        bottleAmount = listLevels[currentLevel].bottleAmount;
        levelTarget = colorAmount;

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

        listLevelWaterColor.Clear();
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


    bool canMove = false;

    public int indexColorSelect1;
    public void CheckColorWater()
    {

        if (bottleSelected1 == bottleSelected2) return;
        else
        {
            blankSlotColor = bottleSelected2.listWaterItem.Count - bottleSelected2.listwaterItemActive.Count;

            if (bottleSelected1.listwaterItemActive.Count == 0) colorSelect1 = WaterColor.none;
            else colorSelect1 = bottleSelected1.listwaterItemActive[0].color;

            if (bottleSelected2.listwaterItemActive.Count != 0)
            {
                colorSelect2 = bottleSelected2.listwaterItemActive[0].color;
            }
            else colorSelect2 = colorSelect1;


            for (int i = 0; i < bottleSelected1.listwaterItemActive.Count; i++)
            {
                if (bottleSelected1.listwaterItemActive[i].color == colorSelect1)
                {
                    colorCount1++;
                 
                }
                else break;
            }

            for(int i = 0; i< bottleSelected1.listWaterItem.Count; i++)
            {
                if (bottleSelected1.listWaterItem[i].color == colorSelect1)
                {
                    indexColorSelect1 = i;
                }
                else if (bottleSelected1.listWaterItem[i].color == WaterColor.none) continue;
                else break;
            }


            if (colorSelect1 == colorSelect2 && blankSlotColor > 0)
            {
                canMove = true;
            }
            else canMove = false;


            if (canMove)
            {
                MoveWaterItem();
            }
            else ResetBottle();


            Debug.Log("CAN MOVE: " + canMove);

        }
    }

    public void MoveWaterItem()
    {
        WaterColor coloChange = colorSelect1;

        if (blankSlotColor > 0)
        {
            StartCoroutine(PourAnim());
            
            if(colorCount1 <= blankSlotColor)
            {
                //tang o mau cho bottle 2
                int index = blankSlotColor - 1;
                for (int i = colorCount1-1; i >= 0; i--)
                {
                    //bottleSelected2.listwaterItemActive.Insert(0, bottleSelected2.listwaterItemActive[i]);
                    bottleSelected2.listWaterItem[index].SetColor(coloChange);
                    bottleSelected2.listWaterItem[index].waterImg.gameObject.SetActive(true);
                    index--;
                }



                index = 0;
                //giam o mau cho bottle 1
                for (int i = 0; i <=indexColorSelect1; i++)
                {

                        bottleSelected1.listWaterItem[i].waterImg.gameObject.SetActive(false);
                        bottleSelected1.listWaterItem[i].color = WaterColor.none;
                        //bottleSelected1.listwaterItemActive.RemoveAt(index);
                        index++;

                }
            }

            if(colorCount1 > blankSlotColor)
            {
                int index = blankSlotColor - 1;
                for (int i = blankSlotColor - 1; i >= 0; i--)
                {
                   // bottleSelected2.listwaterItemActive.Insert(0, bottleSelected2.listwaterItemActive[i]);
                    bottleSelected2.listWaterItem[index].SetColor(coloChange);
                    bottleSelected2.listWaterItem[index].waterImg.gameObject.SetActive(true);
                    index--;
                }

                index = 0;
                //indexColorSelect1 = blankSlotColor-1;
                for (int i =0; i <= indexColorSelect1; i--)
                {
                    if (index < blankSlotColor)
                    {
                        bottleSelected1.listWaterItem[i].waterImg.gameObject.SetActive(false);
                        bottleSelected1.listWaterItem[i].color = WaterColor.none;
                        index++;    
                    }
                    else break;
                }
            }
        }
        else return;





        bottleSelected1.listwaterItemActive.Clear();
        bottleSelected2.listwaterItemActive.Clear();
        for (int i = 0; i < bottleSelected1.listWaterItem.Count(); i++)
        {
            if (bottleSelected1.listWaterItem[i].waterImg.gameObject.activeSelf)
            {
                bottleSelected1.listwaterItemActive.Add(bottleSelected1.listWaterItem[i]);
            }
        }
        for (int i = 0; i < bottleSelected2.listWaterItem.Count(); i++)
        {
            if (bottleSelected2.listWaterItem[i].waterImg.gameObject.activeSelf)
            {
                bottleSelected2.listwaterItemActive.Add(bottleSelected2.listWaterItem[i]);
            }
        }

        bottleSelected2.CheckCompleteBottle();
        //ResetBottle();

    }




    IEnumerator PourAnim()
    {
        Bottle bottle1 = bottleSelected1;
        Bottle bottle2 = bottleSelected2;
        bottleSelected1.bottleBtn.interactable = false;
        Vector3 pos = bottle1.transform.localPosition;

        bottle1.bottleBtn.interactable = false;
        Vector3 pos2 = bottle2.transform.localPosition;

        bottle1.gameObject.transform.DOLocalMove(new Vector3(pos2.x, pos2.y + 50f), 0.3f);
        bottle1.gameObject.transform.DORotate(new Vector3(0, 0, 90), 0.3f);

        yield return new WaitForSeconds(0.7f);

        bottle1.bottleBtn.interactable = true;
        bottle2.bottleBtn.interactable = true;
        bottle1.gameObject.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        bottle1.gameObject.transform.DOLocalMove(pos, 0.2f);


        ResetBottle();
       //Field.Instance.CheckWater();
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

    public void ResetBottle()
    {
        bottleSelected1 = null;
        bottleSelected2 = null;
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
