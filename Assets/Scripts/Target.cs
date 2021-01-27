using System;
using UnityEngine;

namespace HotspotProject.Targets
{
    [RequireComponent(typeof(Renderer))]
    public class Target : MonoBehaviour
    {

        public Action<Target> OnItemSelected;
        public Action<Target> OnItemDeselected;

        [SerializeField] private Color SelectedColor = new Color();
        [SerializeField] private Color DefaultColor = new Color();

        [SerializeField] private GameObject TargetObject = default;

        public Hotspot TargetHotspot;

        public string TooltipValue = "";
        [HideInInspector]
        public bool IsSelected = false;

        private Renderer Renderer = null;

        private void Start()
        {
            Renderer = GetComponent<Renderer>();

            SetSelected(false);
        }

        public void SetupItem(Action<Target> onItemSelected, Action<Target> onItemDeselected)
        {
            OnItemSelected = onItemSelected;
            OnItemDeselected = onItemDeselected;
        }

        public bool GetSelected()
        {
            return IsSelected;
        }

        public void SetSelected(bool value)
        {
            IsSelected = value;

            Renderer.material.color = value ? SelectedColor : DefaultColor;
        }

        private void OnMouseDown()
        {
            /*if (FreeCameraMovement.IsCameraMoving)
            {
                return;
            }*/

            if (IsSelected)
            {
                SetSelected(false);
                OnItemDeselected?.Invoke(this);
            }
            else
            {
                SetSelected(true);
                OnItemSelected?.Invoke(this);
            }
        }

        //Оставил для тестов - чтобы в рантайме можно было перемещаться + там еще камерой еще можно управлять)
        private void Update()
        {
            if (!IsSelected || FreeCameraMovement.IsCameraMoving)
            {
                return;
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-0.05f, 0f, 0f);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(0.05f, 0f, 0f);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0f, 0f, -0.05f);
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0f, 0f, 0.05f);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.Rotate(Vector3.up, -1);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.Rotate(Vector3.up, 1);
            }
        }

    }
}