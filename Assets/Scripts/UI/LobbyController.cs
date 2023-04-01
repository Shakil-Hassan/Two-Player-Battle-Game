
using Core;
using UnityEngine;

namespace UI
{
    public class LobbyController : MonoBehaviour
    {
        public void PlayGame()
        { 
            SceneTransitionManager.Instance.StartGame();
        }
    }
}