using UnityEngine;

namespace Game.Scripts.Entities
{
    public class DragDirectionArrow : MonoBehaviour
    {
        private Slot _slot;

        public Slot SelectedSlot { get; private set; }
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }
        
        public void Show(Slot slot)
        {
            _slot = slot;
            transform.position = slot.transform.position;
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            ToggleHighlight(SelectedSlot, false);
            SelectedSlot = null;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            Vector3 mousePosition = Input.mousePosition;

            float zDistance = Camera.main.WorldToScreenPoint(transform.position).z;
            mousePosition.z = zDistance;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector2 direction = (mouseWorldPosition - transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            
            bool isRight = angle is > -45 and < 45;
            bool isLeft = angle is < -135 or > 135;
            
            var oldSelectedSlot = SelectedSlot;

            if (isRight)
            {
                SelectedSlot = _slot.Right;
            }
            else if (isLeft)
            {
                SelectedSlot = _slot.Left;
            }
            else
            {
                SelectedSlot = null;
            }
            
            if (oldSelectedSlot != SelectedSlot)
            { 
                ToggleHighlight(oldSelectedSlot, false);
                ToggleHighlight(SelectedSlot, true);
            }
        }
        
        private void ToggleHighlight(Slot slot, bool toggle)
        {
            if (slot == null)
                return;
            
            slot.ToggleHighlight(toggle);
        }
    }
}