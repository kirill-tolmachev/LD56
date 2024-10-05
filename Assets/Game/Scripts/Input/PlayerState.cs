using UnityEngine;

namespace Game.Scripts
{
    public class PlayerState
    {
        public Vector3 Position { get; set; }
        public Vector3 AimPosition { get; set; }
        public float AimAngle => Vector3.Angle(AimPosition - Position, Vector3.up);

        public float ShotInterval => 1f;
    }
}