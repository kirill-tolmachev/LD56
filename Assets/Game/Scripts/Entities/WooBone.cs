using UnityEngine;

namespace Game.Scripts.Entities
{
    public class WooBone : MonoBehaviour
    {
        private Woo _woo;

        private void Awake()
        {
            _woo = GetComponentInParent<Woo>();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            float pressure = CalculatePressure(collision);

            if (pressure >= _woo.PressureThreshold)
            {
                _woo.Explode();
            }
        }

        private float CalculatePressure(Collision2D collision)
        {
            float totalNormalImpulse = 0f;

            foreach (ContactPoint2D contact in collision.contacts)
            {
                totalNormalImpulse += contact.normalImpulse;
            }

            return totalNormalImpulse;
        }
    }
}