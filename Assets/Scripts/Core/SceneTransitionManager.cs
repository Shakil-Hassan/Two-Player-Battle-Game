using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneTransitionManager : MonoSingletonGeneric<SceneTransitionManager>
    {
        
        [HideInInspector]
        public string WinnerName { get; private set; }
        
        public void StartGame()
        {
            StartCoroutine(LoadSceneAsync(1)); 
        }

        public void GameOver(string winnerName)
        {
            WinnerName = winnerName;
            StartCoroutine(LoadSceneAsync(2)); 
        }

        public IEnumerator LoadSceneAsync(int buildIndex)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}