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

namespace WaterSort
{
    public class Field : MonoBehaviour
    {
        public static Field Instance;
        Action _onClose;


        [SerializeField] private Bottle bottle;
        [SerializeField] private Transform gameField;
        public Image bg;
        public GridLayoutGroup field;
        public GameObject line;

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
        public int currentLevel = 0;

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
            foreach(LevelDataSO item in dataLevel)
            {
                listLevels.Add(item);
            
            }
            listLevels = listLevels.OrderBy(t => t.level).ToList();
        }


        public void LoadLevelData()
        {
            field.enabled = !field.enabled;
            EffectManager.instance.HideEffect(false);

            levelTxt.text = "Level: " + currentLevel.ToString();
            colorAmount = listLevels[currentLevel].colorAmount;
            bottleAmount = listLevels[currentLevel].bottleAmount;
            levelTarget = colorAmount;
            levelProcess = 0;


            isLose = false;
            isWin = false;
            if (bottleAmount < 8) field.constraintCount = 1;
            else if (bottleAmount <= 14) field.constraintCount = 2;
            else if (bottleAmount <= 21) field.constraintCount = 3;

            for(int i = 0; i < colorAmount; i++)
            {
                waterColor = (WaterColor)Random.Range(1, 17);
                while(listLevelWaterColor.Contains(waterColor))
                {
                    waterColor = (WaterColor)Random.Range(1, 17);
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
            }

            StartCoroutine(EnabledGrid());
        }

        public IEnumerator EnabledGrid()
        {
            yield return new WaitForEndOfFrame();
            field.enabled = !field.enabled;
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
        public void CheckColorWater(Bottle start, Bottle end)
        {
            canMove = CheckWaterSimilar(start, end);

            if (canMove) MoveWaterItem();
            else ResetBottle();
        }


        public void Freeze(bool isFreeze)
        {
            foreach(var item in listBottle)
            {
                if (!item.isFull)
                    item.bottleBtn.interactable = isFreeze;
                else continue;
            }
        }

        Transform posEnd = null;
        public bool CheckWaterSimilar(Bottle start, Bottle end)
        {
            colorCount1 = 0;
            indexColorSelect1 = 0;
            posEnd = null;
            bool canPour = false;
            if (start == end) return false;
            else
            {
                blankSlotColor = end.listWaterItem.Count - end.listwaterItemActive.Count;

                if (start.listwaterItemActive.Count == 0) colorSelect1 = WaterColor.none;
                else colorSelect1 = start.listwaterItemActive[0].color;

                if (end.listwaterItemActive.Count != 0)
                {
                    colorSelect2 = end.listwaterItemActive[0].color;
                }
                else colorSelect2 = colorSelect1;


                for (int i = 0; i < start.listwaterItemActive.Count; i++)
                {
                    if (start.listwaterItemActive[i].color == colorSelect1)
                    {
                        colorCount1++;

                    }
                    else break;
                }


                int value = 0;
                for (int i = 0; i < start.listWaterItem.Count; i++)
                {
                    if (start.listWaterItem[i].color == WaterColor.none) continue;
                    if (start.listWaterItem[i].color == colorSelect1)
                    {
                        if (value >= blankSlotColor) break;
                        if(value < blankSlotColor)
                        {
                            indexColorSelect1 = i;
                            value++;
                        }
                    }
                    else break;
                }


            
                if (colorSelect1 == colorSelect2 && blankSlotColor > 0)
                {
                    posEnd = end.listWaterItem[blankSlotColor - 1].transform;
                    canPour = true;
                }
                else canPour = false;
            }


            return canPour;
        }

        public void MoveWaterItem()
        {
            Freeze(false);
            WaterColor colorChange = colorSelect1;

            if (blankSlotColor > 0)
            {
                StartCoroutine(PourAnim(CheckWinLose));
            
                if(colorCount1 <= blankSlotColor)
                {

                    int index = blankSlotColor - colorCount1;
                    bottleSelected2.ChangeColor(bottleSelected2, bottleSelected2.listWaterItem[blankSlotColor - 1], colorChange, 1, index);

                    bottleSelected1.ChangeColor(bottleSelected1,bottleSelected1.listWaterItem[0], WaterColor.none, 0, indexColorSelect1);


                }

                if(colorCount1 > blankSlotColor)
                {
                    bottleSelected2.ChangeColor(bottleSelected2, bottleSelected2.listWaterItem[blankSlotColor - 1], colorChange, 1, 0);

                    int v = colorCount1 - blankSlotColor;
                    int x = indexColorSelect1 - v;
                    bottleSelected1.ChangeColor(bottleSelected1, bottleSelected1.listWaterItem[0], WaterColor.none, 0, indexColorSelect1);
                }

                StartCoroutine(OnMoveComplete());
            }
            else return;
        }


        public void CheckCompleteBottle(Bottle bottleCheck)
        {
            int colorSimilarCount = 0;
            if (bottleCheck.listwaterItemActive.Count == 5)
            {
                WaterColor colorCheck = bottleCheck.listwaterItemActive[0].color;

                foreach (Water item in bottleCheck.listwaterItemActive)
                {
                    if (item.color == colorCheck)
                    {
                        colorSimilarCount++;
                    }
                }
            }


            if (colorSimilarCount == 5)
            {
                bottleCheck.bottleBtn.interactable = false;
                bottleCheck.isFull = true;
                EffectManager.instance.PlayEffect(bottleCheck.listWaterItem[0].transform);
                levelProcess++;
                CheckWin();
            }
        }

        IEnumerator OnMoveComplete()
        {
            yield return new WaitForSeconds(.6f);

            bottleSelected1.listwaterItemActive.Clear();
            bottleSelected2.listwaterItemActive.Clear();
            for (int i = 0; i < bottleSelected1.listWaterItem.Count(); i++)
            {
                if (bottleSelected1.listWaterItem[i].color == WaterColor.none) continue;
                else
                    bottleSelected1.listwaterItemActive.Add(bottleSelected1.listWaterItem[i]);

            }
            for (int i = 0; i < bottleSelected2.listWaterItem.Count(); i++)
            {
                if (bottleSelected2.listWaterItem[i].color == WaterColor.none) continue;
                else bottleSelected2.listwaterItemActive.Add(bottleSelected2.listWaterItem[i]);
            }

            CheckCompleteBottle(bottleSelected2);
            CheckWater();

            ResetBottle();
            Freeze(true);
        }


        IEnumerator PourAnim(Action<bool> callback)
        {
        
            Bottle bottle1 = bottleSelected1;
            Bottle bottle2 = bottleSelected2;

            Vector3 pos = bottle1.transform.localPosition;
            Vector3 pos2 = bottle2.transform.localPosition;
            Vector3 pos3 = bottle2.gameObject.transform.position;

            bottle1.transform.SetAsLastSibling();
            bottle1.gameObject.transform.DOLocalMove(new Vector3(pos2.x, pos2.y + 50f), 0.2f);
            bottle1.gameObject.transform.DORotate(new Vector3(0, 0, 80), 0.3f);
            bottle1.water.transform.DOLocalRotate(new Vector3(0, 0, -60), 0.3f);
            bottle1.water.transform.DOScale(0.8f, 0.3f);

            Vector3[] waterPos = new Vector3[2];
            waterPos[0] = new Vector3(pos3.x, pos3.y + .34f, 90);
            waterPos[1] = posEnd.position;

            Color color = bottle1.ColorSelect(bottle1.listWaterItem[indexColorSelect1]);
            line.SetActive(true);
            DrawLine.Instance.SetColor(color);
            DrawLine.Instance.DrawLink(waterPos);

            yield return new WaitForSeconds(0.7f);
            line.SetActive(false);
            bottle1.gameObject.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
            bottle1.water.transform.DORotate(Vector3.zero,0.2f);
            bottle1.water.transform.DOScale(1f,0.2f);
            bottle1.gameObject.transform.DOLocalMove(pos, 0.2f);

            yield return new WaitForSeconds(1f);
            callback(true);
        }


    

        public void CheckWinLose(bool isEndAnim)
        {

            if(isWin && isEndAnim)
            {
                GameSceneManager.Instance.popupWin.SetActive(true);
            }

            if(isLose && isEndAnim)
            {
                GameSceneManager.Instance.popupLose.SetActive(true);
            }
        
        }


        public bool isWin;
        public void CheckWin()
        {
            if(levelProcess == levelTarget)
            {
                isWin = true;
            }
        }


        public bool isLose = false;
        public bool isEnd = false;
        public Bottle itemCheck;
        public void CheckWater()
        {
            bool canPlay = true;

            for(int i = 0; i< listBottle.Count; i++)
            {
                for(int j = 0; j < listBottle.Count; j++)
                {
                    canPlay = CheckWaterSimilar(listBottle[i], listBottle[j]);

                    if (canPlay)
                    {
                        if (colorCount1 > blankSlotColor)
                        {
                            canPlay = false;
                            continue;
                        }

                        else
                        {
                            canPlay = true;
                            break;
                        }
                    }
                    else continue;
                }
                if (canPlay) break;
                else continue;
            }

            if (!canPlay)
            {
                isLose = true;
            
            }
        }

        public void ResetBottle()
        {
            bottleSelected1 = null;
            bottleSelected2 = null;
        }

    }
}

