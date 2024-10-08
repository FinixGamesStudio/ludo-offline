using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{
    public class LudoBoxProperties : MonoBehaviour
    {
        public TokenPopupPositions tokenPopup;

        public int CanvasOrder;
        public Image trail;
        public int index;
        public BoxType boxType;

        public void UpdateMyColor(Color color) => trail.color = color;

        public List<LudoTokenController> allTokenInSameBox;

        public void TokenArrangement()
        {
            allTokenInSameBox.Clear();
            int myChildCount = transform.GetChild(1).childCount;

            for (int i = 0; i < myChildCount; i++)
                allTokenInSameBox.Add(transform.GetChild(1).GetChild(i).GetComponent<LudoTokenController>());

            CockieManage();
            //Transform transformObj = transform.GetChild(1);

            //if (myChildCount >= 1 && myChildCount <= 7)
            //{
            //    for (int i = 0; i < myChildCount; i++)
            //    {
            //        float xPos = -20f + i * 7.5f;
            //        float yPos = i < 4 ? 15f : -5f;

            //        transformObj.GetChild(i).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
            //        transformObj.GetChild(i).transform.GetComponent<Canvas>().sortingOrder = 300 + i;

            //        float scale = i < 4 ? 1f - (i * 0.1f) : 0.4f;
            //        transformObj.GetChild(i).transform.localScale = new Vector2(scale, scale);
            //    }
            //}
        }

        public void CockieManage()
        {
            allTokenInSameBox.Clear();
            int myChildCount = transform.GetChild(1).childCount;

            for (int i = 0; i < myChildCount; i++)
                allTokenInSameBox.Add(transform.GetChild(1).GetChild(i).GetComponent<LudoTokenController>());

            Transform transformObj = transform.GetChild(1);

            if (myChildCount == 1)
            {
                transformObj.GetChild(0).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 30f);
                transformObj.GetChild(0).transform.localScale = new Vector2(1f, 1f);
            }
            if (myChildCount == 2)
            {
                transformObj.GetChild(0).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10f, 30f);
                transformObj.GetChild(1).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(10f, 30f);
                transformObj.GetChild(0).transform.localScale = new Vector2(0.8f, 0.8f);
                transformObj.GetChild(1).transform.localScale = new Vector2(0.8f, 0.8f);
            }
            if (myChildCount == 3)
            {
                transformObj.GetChild(0).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 25f);
                transformObj.GetChild(1).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20f, 25f);
                transformObj.GetChild(2).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, 25f);
                transformObj.GetChild(0).transform.GetComponent<Canvas>().sortingOrder = 302;
                transformObj.GetChild(1).transform.GetComponent<Canvas>().sortingOrder = 303;
                transformObj.GetChild(2).transform.GetComponent<Canvas>().sortingOrder = 301;
                transformObj.GetChild(0).transform.localScale = new Vector2(0.7f, 0.7f);
                transformObj.GetChild(1).transform.localScale = new Vector2(0.7f, 0.7f);
                transformObj.GetChild(2).transform.localScale = new Vector2(0.7f, 0.7f);
            }
            if (myChildCount == 4)
            {
                transformObj.GetChild(0).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20f, 15f);
                transformObj.GetChild(1).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-7f, 15f);
                transformObj.GetChild(2).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(7f, 15f);
                transformObj.GetChild(3).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(20, 15);
                transformObj.GetChild(0).transform.GetComponent<Canvas>().sortingOrder = 301;
                transformObj.GetChild(1).transform.GetComponent<Canvas>().sortingOrder = 302;
                transformObj.GetChild(2).transform.GetComponent<Canvas>().sortingOrder = 303;
                transformObj.GetChild(3).transform.GetComponent<Canvas>().sortingOrder = 304;
                transformObj.GetChild(0).transform.localScale = new Vector2(0.6f, 0.6f);
                transformObj.GetChild(1).transform.localScale = new Vector2(0.6f, 0.6f);
                transformObj.GetChild(2).transform.localScale = new Vector2(0.6f, 0.6f);
                transformObj.GetChild(3).transform.localScale = new Vector2(0.6f, 0.6f);
            }
            if (myChildCount == 5)
            {
                transformObj.GetChild(0).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20f, 15f);
                transformObj.GetChild(1).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-7f, 15f);
                transformObj.GetChild(2).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(7f, 15f);
                transformObj.GetChild(3).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, 15f);
                transformObj.GetChild(4).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -5f);
                transformObj.GetChild(0).transform.GetComponent<Canvas>().sortingOrder = 301;
                transformObj.GetChild(1).transform.GetComponent<Canvas>().sortingOrder = 302;
                transformObj.GetChild(2).transform.GetComponent<Canvas>().sortingOrder = 303;
                transformObj.GetChild(3).transform.GetComponent<Canvas>().sortingOrder = 304;
                transformObj.GetChild(4).transform.GetComponent<Canvas>().sortingOrder = 305;
                transformObj.GetChild(0).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(1).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(2).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(3).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(4).transform.localScale = new Vector2(0.5f, 0.5f);
            }
            if (myChildCount == 6)
            {
                transformObj.GetChild(0).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20f, 15f);
                transformObj.GetChild(1).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-7f, 15f);
                transformObj.GetChild(2).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(7f, 15f);
                transformObj.GetChild(3).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, 15f);
                transformObj.GetChild(4).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10f, -5f);
                transformObj.GetChild(5).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, -5f);
                transformObj.GetChild(0).transform.GetComponent<Canvas>().sortingOrder = 301;
                transformObj.GetChild(1).transform.GetComponent<Canvas>().sortingOrder = 302;
                transformObj.GetChild(2).transform.GetComponent<Canvas>().sortingOrder = 303;
                transformObj.GetChild(3).transform.GetComponent<Canvas>().sortingOrder = 304;
                transformObj.GetChild(4).transform.GetComponent<Canvas>().sortingOrder = 305;
                transformObj.GetChild(5).transform.GetComponent<Canvas>().sortingOrder = 306;
                transformObj.GetChild(0).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(1).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(2).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(3).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(4).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(5).transform.localScale = new Vector2(0.5f, 0.5f);
            }
            if (myChildCount == 7)
            {
                transformObj.GetChild(0).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-20f, 15f);
                transformObj.GetChild(1).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-7f, 15f);
                transformObj.GetChild(2).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(7f, 15f);
                transformObj.GetChild(3).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, 15f);
                transformObj.GetChild(4).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-15f, -5f);
                transformObj.GetChild(5).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -5f);
                transformObj.GetChild(6).transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(15, -5f);
                transformObj.GetChild(0).transform.GetComponent<Canvas>().sortingOrder = 301;
                transformObj.GetChild(1).transform.GetComponent<Canvas>().sortingOrder = 302;
                transformObj.GetChild(2).transform.GetComponent<Canvas>().sortingOrder = 303;
                transformObj.GetChild(3).transform.GetComponent<Canvas>().sortingOrder = 304;
                transformObj.GetChild(4).transform.GetComponent<Canvas>().sortingOrder = 305;
                transformObj.GetChild(5).transform.GetComponent<Canvas>().sortingOrder = 306;
                transformObj.GetChild(6).transform.GetComponent<Canvas>().sortingOrder = 307;
                transformObj.GetChild(0).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(1).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(2).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(3).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(4).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(5).transform.localScale = new Vector2(0.5f, 0.5f);
                transformObj.GetChild(6).transform.localScale = new Vector2(0.5f, 0.5f);
            }
        }
    }
    public enum TokenPopupPositions
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public enum BoxType
    {
        None,
        Star,

    }
}