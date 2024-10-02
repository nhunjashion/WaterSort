using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WaterSort
{
    public class SelectLevel : MonoBehaviour
    {
        public LevelBtn leveBtnPrefabs;
        [SerializeField] Transform content;

        public List<LevelBtn> listBtn = new();


        private void OnEnable()
        {
            ClearData();
            for(int i=0; i< Field.Instance.listLevels.Count;i++)
            {
                var levelBtn = Instantiate(leveBtnPrefabs, content);
                levelBtn.SetLevelText(Field.Instance.listLevels[i].level.ToString());

                listBtn.Add(levelBtn);
            }
        }

        public void ClearData()
        {
            listBtn.Clear();
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }
    }
}

