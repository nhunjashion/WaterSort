using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Bottle : MonoBehaviour
{
    [SerializeField] private Water waterItemPrefabs;
    [SerializeField] private Transform content;
    public int maxWater ;

    public int waterItemCount; 
    public List<Water> listWaterItem = new();
    public List<Water> listwaterItemActive = new();

    public int index;

    public Bottle bottleSelected1;
    public Bottle bottleSelected2;

    public WaterColor colorSelect1 ;
    public WaterColor colorSelect2 ;

    public int colorCount1 = 0;
    public int colorCount2 = 0;

    public int waterIndex1 = 0;
    public int waterIndex2 = 0;

    public Button bottleBtn;

    private void OnEnable()
    {
        SetData(5);
        
    }

    public void SetData(int waterCount)
    {
        maxWater = waterCount;
        ClearData();
        waterItemCount = maxWater;

        for (int i =0; i<maxWater;i++)
        {
            var waterItem = Instantiate(waterItemPrefabs, content);
            waterItem.gameObject.SetActive(false);

            listWaterItem.Add(waterItem);


        }
        int colorAmount = Field.Instance.colorCount;
        if(colorAmount >= waterItemCount)
        {        
            Field.Instance.colorCount -= waterItemCount;
            SetColor();

        }
        else
        {
            waterItemCount = colorAmount;
            Field.Instance.colorCount -= colorAmount;
            SetColor();
        }
    }


    public void SetColor()
    {
        try
        {
            for (int i = 0; i < maxWater; i++)
            {
                int colorIndex = Random.Range(0, Field.Instance.listColor.Count - 1);
                if (Field.Instance.listColor.Count > 0)
                {       
                    WaterColor color = Field.Instance.listColor[colorIndex];
                    listWaterItem[i].SetColor(color);
                    listWaterItem[i].gameObject.SetActive(true);
                    Field.Instance.listColor.Remove(color);


                    listwaterItemActive.Add(listWaterItem[i]);
                }
                else return;
            }
        }
        catch(Exception ex)
        {
            Debug.Log("ERROR: " + ex.Message);
            GameSceneManager.Instance.OnClickCreateMap();
        }

    }
    public void ClearData()
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }


    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }

    public void OnClikBottle()
    {
        Debug.Log("Click on bottle" + index);

        if (!GameSceneManager.Instance.selectBottle1)
        {
            GameSceneManager.Instance.selectBottle1 = true;
            GameSceneManager.Instance.indexBottle = index;
            GameSceneManager.Instance.bottleSelected = this;
            this.gameObject.transform.DOScale(1.1f, 0.1f);
        }
        else
        {
            GameSceneManager.Instance.bottleSelected2 = this;
            GameSceneManager.Instance.selectBottle1 = false;
            GameSceneManager.Instance.bottleSelected.gameObject.transform.DOScale(1.0f, 0.1f);
        }
        if (GameSceneManager.Instance.bottleSelected2 != null)
        {

            CheckColorWater();

            GameSceneManager.Instance.bottleSelected = null;
            GameSceneManager.Instance.bottleSelected2 = null;

            colorCount1 = 0;
        }

        else return;
    }


    bool canMove = false;
    public void CheckColorWater()
    {

        Bottle bottleSelected1 = GameSceneManager.Instance.bottleSelected;
        Bottle bottleSelected2 = GameSceneManager.Instance.bottleSelected2;

        if (bottleSelected1 == bottleSelected2) return;
        else
        {
            
            colorCount2 = bottleSelected2.listWaterItem.Count - bottleSelected2.listwaterItemActive.Count;

            if(bottleSelected1.listwaterItemActive.Count==0) colorSelect1=WaterColor.none;
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


            if (colorSelect1 == colorSelect2 || colorCount2 == bottleSelected2.listWaterItem.Count)
            {
                canMove = true;
            }
            else canMove = false;


            if(canMove)
            {
                MoveWaterItem();
            }


            Debug.Log("CAN MOVE: " + canMove);

        }
    }

    public void MoveWaterItem()
    {            


        Bottle bottleSelected1 = GameSceneManager.Instance.bottleSelected;
        Bottle bottleSelected2 = GameSceneManager.Instance.bottleSelected2;
        WaterColor coloChange = colorSelect1;

        if (colorCount2 > 0)
        {
            StartCoroutine(PourAnim());
           
            int value = colorCount2 >= colorCount1 ? colorCount2 - colorCount1 : colorCount2;

            Debug.Log((int)value);
            if(colorCount2==1)
            {
                bottleSelected2.listWaterItem[0].SetColor(coloChange);
                bottleSelected2.listwaterItemActive.Insert(0, bottleSelected2.listWaterItem[0]);
                bottleSelected2.listWaterItem[0].gameObject.SetActive(true);
            }
            else
            {
                for (int i = colorCount2-1 ; i >= value; i --)
                {
                    bottleSelected2.listWaterItem[i].SetColor(coloChange);
                    bottleSelected2.listwaterItemActive.Insert(0, bottleSelected2.listWaterItem[i]);
                    bottleSelected2.listWaterItem[i].gameObject.SetActive(true);
                }
            }


            if(colorCount1 <= colorCount2)
            {
                for(int i = 0; i<colorCount1; i++)
                {
                    for(int j=0; j < bottleSelected1.listWaterItem.Count; j++)
                    {
                        if(bottleSelected1.listWaterItem[j].gameObject.activeSelf)
                        {
                            bottleSelected1.listWaterItem[j].gameObject.SetActive(false);
                            bottleSelected1.listwaterItemActive.Remove(bottleSelected1.listWaterItem[j]);

                            
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < colorCount2; i++)
                {
                    for (int j = 0; j < bottleSelected1.listWaterItem.Count; j++)
                    {
                        if (bottleSelected1.listWaterItem[j].gameObject.activeSelf)
                        {
                            bottleSelected1.listWaterItem[j].gameObject.SetActive(false);
                            bottleSelected1.listwaterItemActive.Remove(bottleSelected1.listWaterItem[j]);


                            break;
                        }
                    }
                }
            }

        }
        else return;

    }

    public void CheckCompleteBottle()
    {
        int colorSimilarCount = 0;
        if (this.listwaterItemActive.Count == maxWater)
        {
            WaterColor colorCheck = listwaterItemActive[0].color;

            foreach(Water item in listwaterItemActive)
            {
                if(item.color == colorCheck)
                {
                    colorSimilarCount++;
                }
            }
        }


        if(colorSimilarCount == maxWater)
        {
            bottleBtn.interactable = false;
            Field.Instance.levelProcess++;
            Field.Instance.CheckWin();
        }
    }

    IEnumerator PourAnim()
    {
        Bottle bottleSelected1 = GameSceneManager.Instance.bottleSelected;
        bottleSelected1.bottleBtn.interactable = false;
        Vector3 pos = bottleSelected1.transform.localPosition;
        Bottle bottleSelected2 = GameSceneManager.Instance.bottleSelected2;
        bottleSelected2.bottleBtn.interactable = false;
        Vector3 pos2 = bottleSelected2.transform.localPosition;

        bottleSelected1.gameObject.transform.DOLocalMove(new Vector3(pos2.x,pos2.y + 50f), 0.3f);
        bottleSelected1.gameObject.transform.DORotate(new Vector3(0, 0, 90), 0.3f);

        yield return new WaitForSeconds(0.7f);

        bottleSelected1.bottleBtn.interactable = true;
        bottleSelected2.bottleBtn.interactable = true;
        bottleSelected1.gameObject.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
        bottleSelected1.gameObject.transform.DOLocalMove(pos,0.2f);


        CheckCompleteBottle();
        Field.Instance.CheckWater();
    }
}
