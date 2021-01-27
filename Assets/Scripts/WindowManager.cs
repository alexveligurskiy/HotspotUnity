using System.Collections;
using System.Collections.Generic;
using HotspotProject.Targets;
using UnityEngine;
using UnityEngine.UI;

namespace HotspotProject
{
    public class WindowManager : MonoBehaviour
    {
        public static WindowManager Instance;
        
        [SerializeField] private Camera MainCamera = default;

        [SerializeField] private RectTransform CanvasRect = default;
        [SerializeField] private RectTransform ContentObjectRect = default;
        [SerializeField] private RectTransform TextContentRect = default;
        [SerializeField] private Text MainText = default;

        [SerializeField] private RectTransform TopAnchorRect = default;
        [SerializeField] private RectTransform BottomAnchorRect = default;

        private Target CurrentSelectedTarget = null;

        private float TopBottomPadding => 50f;
        private float LeftRightPadding => ((Screen.width - ContentObjectRect.rect.width * CanvasScaler) / 2f) + TopBottomPadding * 2f;
        private float TextContentHeight => TextContentRect.rect.height;
        private float CanvasScaler => CanvasRect.localScale.x;

        private bool IsCurrentTop = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                Debug.LogError("[TargetsManager] Try to create multiple instances of TargetsManager, the object was destroyed!");
            }
        }

        private void Start()
        {
            TargetsManager.Instance.OnItemSelected += SetupWindowOnSelected;
            TargetsManager.Instance.OnItemDeselected += SetupWindowOnDeselected;

            ActivateAnchor(true);
        }
        private void OnDestroy()
        {
            TargetsManager.Instance.OnItemSelected -= SetupWindowOnSelected;
            TargetsManager.Instance.OnItemDeselected -= SetupWindowOnDeselected;
        }

        private void Update()
        {
            if (CurrentSelectedTarget != null)
            {
                if (!CurrentSelectedTarget.TargetHotspot.IsVisible)
                {
                    ContentObjectRect.gameObject.SetActive(false);
                    return;
                }

                if (!ContentObjectRect.gameObject.activeSelf)
                {
                    ContentObjectRect.gameObject.SetActive(true);
                }
                ChangeAnchorPosition(CurrentSelectedTarget.TargetHotspot.Transform);
            }
        }

        /// <summary>
        /// Устанавливаем Анкера
        /// </summary>
        /// <param name="target"></param>
        private void ActivateAnchor(bool isTopAnchor)
        {
            IsCurrentTop = isTopAnchor;
            TopAnchorRect.gameObject.SetActive(false);
            BottomAnchorRect.gameObject.SetActive(false);

            if (isTopAnchor)
            {
                TopAnchorRect.gameObject.SetActive(true);
            }
            else
            {
                BottomAnchorRect.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Устанавливаем Тултип при нажатии на обьект
        /// </summary>
        /// <param name="target"></param>
        public void SetupWindowOnSelected(Target target)
        {
            CurrentSelectedTarget = target;
            MainText.text = target.TooltipValue;
            ContentObjectRect.gameObject.SetActive(true);
        }

        /// <summary>
        /// Сбрасываем Тултип при нажатии на обьект
        /// </summary>
        /// <param name="target"></param>
        private void SetupWindowOnDeselected(Target target)
        {
            CurrentSelectedTarget = null;
            MainText.text = "";
            ContentObjectRect.gameObject.SetActive(false);
        }

        /// <summary>
        /// Меняем позици анкеров (верхнего и нижнего) в зависимости от того в какой край упираемся + сразу выставляем позицию центральному елементу с текстом
        /// Понимаю, что вариант может не самый лучший, вообще это все можно сделать по другому, но на тот момент, придумать что то более правильное просто не успел )))
        /// </summary>
        /// <param name="TargetHotspotTransform"></param>
        private void ChangeAnchorPosition(Transform TargetHotspotTransform)
        {
            Vector3 hotspotUIPosition = MainCamera.WorldToScreenPoint(TargetHotspotTransform.position);

            float TextContentXPos = ContentObjectRect.position.x;
            float TextContentYPos = IsCurrentTop ? GetYPosTop(hotspotUIPosition.y) : GetYPosBottom(hotspotUIPosition.y);

            if (IsCurrentTop)
            {
                if ((TextContentHeight * CanvasScaler <= hotspotUIPosition.y + TopBottomPadding) && hotspotUIPosition.y < Screen.height / 2f)
                {
                    ActivateAnchor(false);
                    return;
                }

                float ancXPos = (hotspotUIPosition.x <= LeftRightPadding || hotspotUIPosition.x >= Screen.width - LeftRightPadding)
                    ? TopAnchorRect.position.x
                    : hotspotUIPosition.x;
                TopAnchorRect.position = new Vector3(ancXPos, hotspotUIPosition.y, 0);
            }
            else
            {
                if ((Screen.height - TextContentHeight * CanvasScaler >= hotspotUIPosition.y + TopBottomPadding) && hotspotUIPosition.y > Screen.height / 2f)
                {
                    ActivateAnchor(true);
                    return;
                }

                float ancXPos = (hotspotUIPosition.x <= LeftRightPadding || hotspotUIPosition.x >= Screen.width - LeftRightPadding)
                    ? BottomAnchorRect.position.x
                    : hotspotUIPosition.x;
                BottomAnchorRect.position = new Vector3(ancXPos, hotspotUIPosition.y, 0);
            }

            ContentObjectRect.position = new Vector3(TextContentXPos, TextContentYPos, 0);
        }

        private float GetYPosTop(float hotspotY)
        {
            return (hotspotY - (TextContentHeight * CanvasScaler / 2 + TopAnchorRect.rect.height * CanvasScaler)) + 5f;
        }

        private float GetYPosBottom(float hotspotY)
        {
            return (hotspotY + (TextContentHeight * CanvasScaler / 2 + BottomAnchorRect.rect.height * CanvasScaler)) - 5f;
        }
    }
}
