using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WaterSort
{
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
            Field.Instance.currentLevel = level;

            GameSceneManager.Instance.popupSelectLevel.SetActive(false);
            GameSceneManager.Instance.popupMenu.SetActive(false);

            Field.Instance.LoadLevelData();
            Field.Instance.LoadListColor();
            Field.Instance.LoadData();
        }
    }
}

