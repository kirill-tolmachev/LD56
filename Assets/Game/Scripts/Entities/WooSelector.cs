using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Entities
{
    public class WooSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        private Woo _woo;
        
        private void Awake()
        {
            _woo = GetComponentInParent<Woo>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Woo clicked");
            _woo.OnPointerClick(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Woo entered");
            _woo.ToggleHighlight(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Woo exited");
            _woo.ToggleHighlight(false);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            _woo.ToggleHighlight(true);
        }
    }
}