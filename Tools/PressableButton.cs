using System;
using UnityEngine;

namespace MonkeMirror.Tools
{
    public class PressableButton : MonoBehaviour
    {
        public Action OnPress;

        private const float DebounceTime = 0.2f;
        private float touchTime;

        private GameObject uiObject;
        private GameObject showIndicator;
        private GameObject hideIndicator;

        private void Awake()
        {
            gameObject.SetLayer(UnityLayer.GorillaInteractable);

            uiObject = transform.root.Find("UI")?.gameObject;
            showIndicator = transform.root.Find("UIButton/ShowUI")?.gameObject;
            hideIndicator = transform.root.Find("UIButton/HideUI")?.gameObject;

            if (uiObject != null)
            {
                if (uiObject.activeSelf)
                {
                    if (hideIndicator != null) hideIndicator.SetActive(true);
                    if (showIndicator != null) showIndicator.SetActive(false);
                }
                else
                {
                    if (hideIndicator != null) hideIndicator.SetActive(false);
                    if (showIndicator != null) showIndicator.SetActive(true);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Time.time - touchTime < DebounceTime)
                return;

            var hand = other.GetComponent<GorillaTriggerColliderHandIndicator>();
            if (hand == null) return;

            touchTime = Time.time;

            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, hand.isLeftHand, 0.05f);
            GorillaTagger.Instance.StartVibration(
                hand.isLeftHand,
                GorillaTagger.Instance.tapHapticStrength / 2f,
                GorillaTagger.Instance.tapHapticDuration
            );

            OnPress?.Invoke();

            if (uiObject == null)
            {
                Debug.LogWarning("[MonkeMirror] UI not found");
                return;
            }

            bool newState = !uiObject.activeSelf;
            uiObject.SetActive(newState);

  
            if (newState)
            {

                if (hideIndicator != null) hideIndicator.SetActive(true);
                if (showIndicator != null) showIndicator.SetActive(false);
            }
            else
            {
                if (hideIndicator != null) hideIndicator.SetActive(false);
                if (showIndicator != null) showIndicator.SetActive(true);
            }
        }
    }
}
