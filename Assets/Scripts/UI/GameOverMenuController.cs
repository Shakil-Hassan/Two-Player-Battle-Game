using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameOverMenuController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text winnerName;
        
        private void Start()
        {
            winnerName.text = "Winner: " + SceneTransitionManager.Instance.WinnerName;
        }
        
        public void Replay()
        {
            SceneTransitionManager.Instance.StartGame();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}