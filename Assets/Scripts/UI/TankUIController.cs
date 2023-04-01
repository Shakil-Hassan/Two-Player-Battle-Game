using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TankUIController : MonoBehaviour
    {
        [SerializeField] private List<TankController> tankControllers;
        [SerializeField] private List<Slider> playerHealthValue;
        [SerializeField] private List<TMP_Text> playerPowerTexts;
        [SerializeField] private List<TMP_Text> playerAngleTexts;

        private void Update()
        {
            for (int i = 0; i < tankControllers.Count; i++)
            {
                playerHealthValue[i].value = GameManager.Instance.playerHealths[i] / 100f;
                playerPowerTexts[i].text = "Power: " + tankControllers[i].Power.ToString("0.00");
                playerAngleTexts[i].text = "Angle: " + tankControllers[i].Angle.ToString("0.00");
            }
        }
    }
}