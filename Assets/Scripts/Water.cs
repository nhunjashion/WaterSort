using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    public Image waterImg;

    public WaterColor color;

    public void SetColor(WaterColor waterColor)
    {
        color = waterColor;
        switch(waterColor)
        {
            case WaterColor.red:
                waterImg.color = new Color32(147, 31, 26, 255);
                break;
            case WaterColor.green:
                waterImg.color= new Color32(50, 205, 50, 255);
                break;
           case WaterColor.blue:
                waterImg.color= new Color32(45, 34, 145, 255);
                break;
           case WaterColor.yellow:
                waterImg.color= new Color32(255, 215, 0, 255);
                break;
           case WaterColor.cyan:
                waterImg.color = new Color32(20, 232, 183, 255);
                //waterImg.color= Color.cyan;
                break;
            case WaterColor.violet:
                waterImg.color = new Color32(199, 21, 133, 255);
                break;
            case WaterColor.orange:
                waterImg.color = new Color32(234, 140, 69, 255);
                break;
            case WaterColor.teal:
                waterImg.color = new Color32(0, 81, 84, 255);
                break;
            case WaterColor.pink:
                waterImg.color = new Color32(188, 76, 100, 255);
                break;
            case WaterColor.gray:
                waterImg.color = new Color32(101, 101, 103, 255);
                break;
            case WaterColor.cinnamon:
                waterImg.color = new Color32(210, 105, 30, 255);
                break;
            case WaterColor.chocolate:
                waterImg.color = new Color32(100, 26, 26, 255);
                break;
            case WaterColor.jade:
                waterImg.color = new Color32(0, 168, 107, 255);
                break;
            case WaterColor.navajo:
                waterImg.color = new Color32(225, 222, 173, 255);
                break;
            case WaterColor.crimson:
                waterImg.color = new Color32(202, 23, 8, 255);
                break;
            case WaterColor.plum:
                waterImg.color = new Color32(221, 160, 221, 255);
                break;
            case WaterColor.sky:
                waterImg.color = new Color32(71, 136, 195, 255);
                break;
            case WaterColor.none:
                waterImg.color = new Color32(255, 255, 255, 0);
                break;
        }
    }
}

public enum WaterColor
{
    none,
    red,
    green,
    blue,
    yellow,
    cyan,
    violet,
    orange,
    teal,
    pink,
    gray,
    cinnamon,
    chocolate,
    jade,
    navajo,
    crimson,
    plum,
    sky
}
