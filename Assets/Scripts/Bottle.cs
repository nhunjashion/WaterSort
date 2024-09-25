using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public Image waterBg;

    public bool isFull = false;

    public Button bottleBtn;

    private void OnEnable()
    {
        SetData(5);
        
    }

    public void SetData(int waterCount)
    {
        Color colorBG = Field.Instance.bg.sprite.texture.GetPixel(1,1);

        waterBg.color = colorBG;
        maxWater = waterCount;
        ClearData();
        waterItemCount = maxWater;

        for (int i =0; i<maxWater;i++)
        {
            var waterItem = Instantiate(waterItemPrefabs, content);
            waterItem.waterImg.gameObject.SetActive(false);

            waterItem.name = i.ToString();
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
                    listWaterItem[i].waterImg.gameObject.SetActive(true);
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

        if (!Field.Instance.bottleSelected1)
        {
            Field.Instance.bottleSelected1 = this;
            this.gameObject.transform.DOScale(1.1f, 0.1f);
        }
        else
        {
            Field.Instance.bottleSelected2 = this;
            Field.Instance.bottleSelected1.gameObject.transform.DOScale(1.0f, 0.1f);
        }
        if (Field.Instance.bottleSelected2 != null)
        {

            Field.Instance.CheckColorWater(Field.Instance.bottleSelected1, Field.Instance.bottleSelected2);

            Field.Instance.colorCount1 = 0;
        }

        else return;
    }


    bool canMove = false;
   
    



}
