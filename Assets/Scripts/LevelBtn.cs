using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelBtn : MonoBehaviour
{
    public TextMeshProUGUI levelTxt;
    public int level;

    public void SetLevelText(string text)
    {
        levelTxt.text = text;
        level = int.Parse(text);
    }

    public void OnClick()
    {
        UserDataManager.Instance.Level = level;
        GameSceneManager.Instance.popupSelectLevel.gameObject.SetActive(false);
    }
}
