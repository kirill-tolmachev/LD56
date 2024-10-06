using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Entities
{
    public class Environment : MonoBehaviour
    {
        public GameObject LeftConveyor;
        public GameObject RightConveyor;
        public GameObject Capsule;
        
        public void ToggleLeftConveyor(bool value)
        {
            LeftConveyor.SetActive(value);
        }

        public void ToggleRightConveyor(bool value)
        {
            RightConveyor.SetActive(value);
        }
        
        public void ToggleCapsule(bool value)
        {
            Capsule.SetActive(value);
        }
        
        public UniTask ToggleRightConveyorAsync(bool value)
        {
            return ToggleObjectAsync(RightConveyor, value);
        }
        
        public UniTask ToggleLeftConveyorAsync(bool value)
        {
            return ToggleObjectAsync(LeftConveyor, value);
        }

        public UniTask ToggleCapsuleAsync(bool value)
        {
            return ToggleObjectAsync(Capsule, value);
        }
        
        public async UniTask ToggleObjectAsync(GameObject conveyor, bool value)
        {
            var startScale = value ? Vector3.zero : Vector3.one;
            var targetScale = value ? Vector3.one : Vector3.zero;
            
            if (value)
                conveyor.SetActive(true);
            
            await conveyor.transform.DOScale(targetScale, 0.2f).From(startScale).SetEase(Ease.InOutSine).SetAutoKill(true);
            
            if (!value)
                conveyor.SetActive(false);
        }
    }
}