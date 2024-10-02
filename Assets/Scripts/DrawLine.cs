using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSort
{
    public class DrawLine : MonoBehaviour
    {
        public static DrawLine Instance;
        public LineRenderer lr;

        private void OnEnable()
        {
            Instance = this;
            this.lr = GetComponent<LineRenderer>();



        }

        public void SetColor(Color color)
        {
            lr.endWidth = 0.08f;
            lr.startWidth = 0.08f;

            this.lr.startColor = color;
            this.lr.endColor = color;
        }

        public void DrawLink(Vector3[] vertexPositions)
        {

            this.lr.positionCount = vertexPositions.Length;
            this.lr.SetPositions(vertexPositions);
        }
    }
}

