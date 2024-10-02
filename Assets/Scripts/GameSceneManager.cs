using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaterSort
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {

        public bool selectBottle1;
        public int indexBottle;
        public Bottle bottleSelected;
        public Bottle bottleSelected2;

        public GameObject popupWin;
        public GameObject popupLose;
        public GameObject popupSelectLevel;
        public GameObject popupMenu;

        public Button SettingBtn;


        public void OnClickReset()
        {
            Field.Instance.LoadLevelData();
            Field.Instance.LoadListColor();
            Field.Instance.LoadData();
        }

        public void OnClickSelectLevel()
        {
            popupSelectLevel.SetActive(true);
            Field.Instance.ClearData();
        }

        public void OnClickNextLevel()
        {
            popupWin.SetActive(false);

            Field.Instance.currentLevel++;

            Field.Instance.LoadLevelData();
            Field.Instance.LoadListColor();
            Field.Instance.LoadData();
        }


        public void OnCLickTryAgian()
        {
            popupLose.SetActive(false);

            Field.Instance.LoadLevelData();
            Field.Instance.LoadListColor();
            Field.Instance.LoadData();
        }


        public void OnClickQuit()
        {
            Application.Quit();
        }


        public void OnClickSetting()
        {
            StartCoroutine(SettingBtnAnim());

            if (popupMenu.activeSelf) popupMenu.SetActive(false);
            else popupMenu.SetActive(true);
        }


        public IEnumerator SettingBtnAnim()
        {
            SettingBtn.interactable = false;
            Debug.Log("Z : " + SettingBtn.gameObject.transform.localRotation.z);
            if(SettingBtn.gameObject.transform.localRotation.z == 1)
                SettingBtn.gameObject.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
            else
                SettingBtn.gameObject.transform.DORotate(new Vector3(0, 0, 180), 0.5f);

            yield return new WaitForSeconds(.6f);

            SettingBtn.interactable = true;
        }
    }
}

