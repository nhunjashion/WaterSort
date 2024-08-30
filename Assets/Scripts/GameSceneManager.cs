using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : Singleton<GameSceneManager>
{

    public bool selectBottle1;
    public int indexBottle;
    public Bottle bottleSelected;
    public Bottle bottleSelected2;

    public GameObject popupWin;
    public GameObject popupLose;
    public GameObject popupSelectLevel;
    
    private void OnEnable()
    {
        LoadUserData();
    }
    public void OnClickReset()
    {
        Field.Instance.UpdateData();
    }

    public void OnClickCreateMap()
    {
        Field.Show();
        //Field.Test();
    }

    public void OnClickSelectLevel()
    {
        popupSelectLevel.SetActive(true);
        Field.Instance.ClearData();
    }

    public void OnClickBackground()
    {
        popupWin.SetActive(false);
        UserDataManager.Instance.Level++;
        Field.Instance.UpdateData();
        Field.Instance.ClearData();
    }


    public void OnClickBg()
    {
        popupLose.SetActive(false);
        Field.Instance.UpdateData();
        Field.Instance.ClearData();
    }


    public void LoadUserData()
    {
        int level = PlayerPrefs.GetInt("Level", 0);
        Debug.Log("Level: " + level);
        //UserDataManager.Instance.Level = level;
    }
}
