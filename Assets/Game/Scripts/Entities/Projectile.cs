using System;
using UnityEngine;

namespace Game.Scripts.Entities
{
    public class Projectile : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            // if (other.gameObject.CompareTag("Player"))
        }
    }
}