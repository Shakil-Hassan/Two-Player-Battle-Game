using System;
using Projectile;
using UnityEngine;

namespace Core
{
    public class TankController : MonoBehaviour
    {
        [SerializeField] private string tankName;
        [SerializeField] private float power;
        [SerializeField] private float angle;
        [SerializeField] private float speed = 5f;
        [SerializeField] private Transform spawnPoint;
        
        private bool _hasFiredThisTurn;
        public float Power => power;
        public float Angle => angle;

        private void Update()
        {
            if (GameManager.Instance.tanks[GameManager.Instance.currentPlayer] == gameObject)
            {
                ProcessInput();
            }
            else
            {
                _hasFiredThisTurn = false; 
            }
        }

        private void ProcessInput()
        {
            UpdateAngleAndPower();
            HandleFiring();

            float moveDirection = Input.GetAxis("Horizontal");
            transform.position += new Vector3(moveDirection * speed * Time.deltaTime, 0, 0);
        }

        private void UpdateAngleAndPower()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 diff = mousePos - (Vector2)transform.position;
            angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            power = diff.magnitude;
        }

        private void HandleFiring()
        {
            if (_hasFiredThisTurn)
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                GameManager.Instance.trajectoryLine.enabled = true;
                DrawTrajectory();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                GameManager.Instance.trajectoryLine.enabled = false;
                Fire();
                _hasFiredThisTurn = true;
            }
            else
            {
                GameManager.Instance.trajectoryLine.enabled = false;
            }
        }



        private void DrawTrajectory()
        {
            Vector2 startPoint = spawnPoint.position; 
            Vector2 endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            endPoint = new Vector2(endPoint.x, endPoint.y); 
            
            Vector2 controlPoint = (startPoint + endPoint) / 2;
            controlPoint.y += Vector2.Distance(startPoint, endPoint) / 2;

            LineRenderer trajectoryLine = GameManager.Instance.trajectoryLine;
            int trajectoryResolution = GameManager.Instance.projectilePrefab.GetComponent<ProjectileController>().trajectoryResolution;

            trajectoryLine.positionCount = trajectoryResolution;

            for (int i = 0; i < trajectoryResolution; i++)
            {
                float t = (float)i / (trajectoryResolution - 1);
                Vector2 position = (1 - t) * (1 - t) * startPoint + 2 * (1 - t) * t * controlPoint + t * t * endPoint;
                trajectoryLine.SetPosition(i, position);
            }
        }

        public void ApplyDamage(object[] data)
        {
            int damage = (int)data[0];
            int firedByPlayer = (int)data[1];
            int hitPlayer = Array.IndexOf(GameManager.Instance.tanks, gameObject);

            if (firedByPlayer != -1)
            {
                GameManager.Instance.playerHealths[hitPlayer] -= damage;
                if (GameManager.Instance.playerHealths[hitPlayer] <= 0)
                {
                    string winnerTankName = GameManager.Instance.tanks[firedByPlayer].GetComponent<TankController>().tankName;
                    SceneTransitionManager.Instance.GameOver(winnerTankName);
                }
            }
        }

        private void Fire()
        {
            Vector2 startPoint = spawnPoint.position;
            Vector2 endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPoint = new Vector2(endPoint.x, endPoint.y);
            Vector2 controlPoint = (startPoint + endPoint) / 2;
            controlPoint.y += Vector2.Distance(startPoint, endPoint) / 2;

            GameObject projectile = Instantiate(GameManager.Instance.projectilePrefab, startPoint, Quaternion.identity);
            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
            projectileController.StartMoving(startPoint, controlPoint, endPoint);
            projectileController.FiredByPlayer = Array.IndexOf(GameManager.Instance.tanks, gameObject); 
            
            projectileController.OnProjectileDestroyed += () =>
            {
                GameManager.Instance.NextTurn();
                _hasFiredThisTurn = false;
            };
        }
    }
}

