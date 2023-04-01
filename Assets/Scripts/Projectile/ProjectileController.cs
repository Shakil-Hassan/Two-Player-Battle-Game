using System;
using System.Collections;
using Core;
using UnityEngine;

namespace Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        public int trajectoryResolution = 100;
        public Action OnProjectileDestroyed;
        private bool _hasDestroyed;
   
        public int FiredByPlayer { get; set; } = -1;

        private LineRenderer trajectoryLine;
        private Vector2 startPoint;
        private Vector2 controlPoint;
        private Vector2 endPoint;
        private Rigidbody2D rb;
        private CircleCollider2D circleCollider;

        private void Awake()
        {
            trajectoryLine = GameManager.Instance.trajectoryLine;
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();

            circleCollider.enabled = false;
            StartCoroutine(EnableColliderAfterDelay(0.2f));
        }

        private IEnumerator EnableColliderAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            circleCollider.enabled = true;
        }

        public void StartMoving(Vector2 startPoint, Vector2 controlPoint, Vector2 endPoint)
        {
            this.startPoint = startPoint;
            this.controlPoint = controlPoint;
            this.endPoint = endPoint;

            StartCoroutine(MoveAlongCurve());
        }

        private IEnumerator MoveAlongCurve()
        {
            rb.gravityScale = 0f;

            float time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime;
                Vector2 position = (1 - time) * (1 - time) * startPoint + 2 * (1 - time) * time * controlPoint + time * time * endPoint;
                transform.position = position;
                yield return null;
            }

            rb.gravityScale = 1f;
            ApplyContinuedForce();
        }

        private void ApplyContinuedForce()
        {
            Vector2 direction = (endPoint - controlPoint).normalized;
            float force = 5f;
            float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f); 
            rb.AddForce(direction * force * randomFactor, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TankController tank = collision.GetComponent<TankController>();
            if (tank != null)
            {
                tank.SendMessage("ApplyDamage", new object[] { 10, FiredByPlayer });
            }

            Destroy(gameObject);
            
            if (!_hasDestroyed)
            {
                OnProjectileDestroyed?.Invoke();
                _hasDestroyed = true;
            }
        }

    }
}
