using Game.Scripts.Entities;
using Game.Scripts.Systems;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Util
{
    public class ProjectileBBox : MonoBehaviour
    {
        private ProjectileLifetimeSystem _projectileLifetimeSystem;
        
        [Inject]
        public void Construct(ProjectileLifetimeSystem projectileLifetimeSystem)
        {
            _projectileLifetimeSystem = projectileLifetimeSystem;
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Projectile projectile))
            {
                _projectileLifetimeSystem.Destroy(projectile);
            }
        }
    }
}