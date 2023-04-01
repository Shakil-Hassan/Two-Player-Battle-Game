using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoSingletonGeneric<GameManager>
    {
       
        public List<int> playerHealths;
        [SerializeField] private int resetHealth = 100;
        public GameObject[] tanks;
        public GameObject projectilePrefab;
        public LineRenderer trajectoryLine;

        public int currentPlayer;
        

        private void Start()
        {
            currentPlayer = 0;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 1) 
            {
                currentPlayer = 0;
                ResetPlayerHealths();
            }
        }

        public void SetReferences(GameObject[] tanks, GameObject projectilePrefab, LineRenderer trajectoryLine)
        {
            this.tanks = tanks;
            this.projectilePrefab = projectilePrefab;
            this.trajectoryLine = trajectoryLine;
        }
        public void NextTurn()
        {
            currentPlayer = (currentPlayer + 1) % tanks.Length;
        }
        
        public void ResetPlayerHealths()
        {
            for (int i = 0; i < playerHealths.Count; i++)
            {
                playerHealths[i] = resetHealth;
            }
        }
    }
}