using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    [SerializeField] private Image waterImg;

    public WaterColor color;

    public void SetColor(WaterColor waterColor)
    {
        color = waterColor;
        switch(waterColor)
        {
            case WaterColor.red:
                waterImg.color = new Color32(213, 27, 27, 255);
                break;
            case WaterColor.green:
                waterImg.color= new Color32(24, 223, 39, 255);
                break;
           case WaterColor.blue:
                waterImg.color= new Color32(6, 74, 188, 255);
                break;
           case WaterColor.yellow:
                waterImg.color= new Color32(250, 231, 40, 255);
                break;
           case WaterColor.cyan:
                waterImg.color = new Color32(78, 252, 234, 255);
                //waterImg.color= Color.cyan;
                break;
            case WaterColor.violet:
                waterImg.color = new Color32(233, 59, 246, 255);
                break;
            case WaterColor.orange:
                waterImg.color = new Color32(248, 155, 24, 255);
                break;
            case WaterColor.teal:
                waterImg.color = new Color32(79, 144, 109, 255);
                break;
            case WaterColor.pink:
                waterImg.color = new Color32(246, 150, 197, 255);
                break;
            case WaterColor.gray:
                waterImg.color = new Color32(167, 175, 174, 255);
                break;
            case WaterColor.cinnamon:
                waterImg.color = new Color32(182, 118, 41, 255);
                break;
            case WaterColor.chocolate:
                waterImg.color = new Color32(140, 49, 24, 255);
                break;
            case WaterColor.jade:
                waterImg.color = new Color32(2, 217, 177, 255);
                break;
            case WaterColor.none:
                waterImg.color = Color.white;
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
    jade
}
