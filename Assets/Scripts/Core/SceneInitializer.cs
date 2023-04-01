using UnityEngine;

namespace Core
{
    public class SceneInitializer : MonoBehaviour
    {
        [SerializeField] private GameObject[] tanks;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private LineRenderer trajectoryLine;

        private void Start()
        {
            GameManager.Instance.SetReferences(tanks, projectilePrefab, trajectoryLine);
        }
    }
}