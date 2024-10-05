using UnityEngine;

namespace Game.Scripts.Entities
{
    public class Conveyor : MonoBehaviour
    {
        public Slot[] Slots;

        private void Start()
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i].Initialize(this, i == 0 ? null : Slots[i - 1], i == Slots.Length - 1 ? null : Slots[i + 1]);
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                foreach (var slot in Slots)
                {
                    slot.BBox.enabled = false;
                
                    if (slot.Woo != null)
                    {
                        slot.Woo.PlayJump();
                    }
                }
            }
            
        }
    }
}