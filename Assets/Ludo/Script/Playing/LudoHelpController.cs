using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Ludo
{
    public class LudoHelpController : MonoBehaviour
    {
        public Transform root;
        public void OpenHelpScreen()
        {
            OnButtonClicked("PointsSystem");
            root.localScale = Vector3.zero;
            root.DOScale(Vector3.one, 0.5f);
            gameObject.SetActive(true);
        }

        public void CloseHelpScren()
        {
            root.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        public Sprite blueButton;
        public Sprite greenButton;

        public List<GameObject> allSmallHelpPopUp;
        public List<Image> allHeaderImage;

        public void OnButtonClicked(string buttonName)
        {
            foreach (var go in allHeaderImage)
                go.sprite = blueButton;

            foreach (var item in allSmallHelpPopUp)
                item.SetActive(false);

            switch (buttonName)
            {
                case "PointsSystem":
                    allSmallHelpPopUp[0].SetActive(true);
                    allHeaderImage[0].sprite = greenButton;
                    break;
                case "ExtraTime":
                    allSmallHelpPopUp[1].SetActive(true);
                    allHeaderImage[1].sprite = greenButton;
                    break;
                case "Extraturns":
                    allSmallHelpPopUp[2].SetActive(true);
                    allHeaderImage[2].sprite = greenButton;
                    break;
                case "Safezones":
                    allSmallHelpPopUp[3].SetActive(true);
                    allHeaderImage[3].sprite = greenButton;
                    break;
                default:
                    break;
            }
        }
    }
}
