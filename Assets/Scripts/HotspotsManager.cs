using System.Collections;
using System.Collections.Generic;
using HotspotProject.Targets;
using UnityEngine;

namespace HotspotProject
{
    public class HotspotsManager : MonoBehaviour
    {
        [SerializeField] private Camera MainCamera = default;
        [SerializeField] private GameObject HotspotPrefab = default;
        [SerializeField] private Transform Content = default;

        public static HotspotsManager Instance;

        private List<RectTransform> HotspotsRects = new List<RectTransform>();

        private List<Target> Targets = new List<Target>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                Debug.LogError("[TargetsManager] Try to create multiple instances of HotspotsManager, the object was destroyed!");
            }
        }

        private void Start()
        {
            Targets = TargetsManager.Instance.GetTargets();

            if (Targets.Count > 0)
            {
                for(int a = 0;a < Targets.Count;a++)
                {
                    var target = Targets[a];
                    var ObjRect = Instantiate(HotspotPrefab, Content, true).GetComponent<RectTransform>();
                    HotspotsRects.Add(ObjRect);
                    SetHotspotPosition(ObjRect, target.TargetHotspot.transform);
                }
            }
        }

        private void Update()
        {
            if (HotspotsRects.Count > 0 && HotspotsRects.Count == Targets.Count)
            {
                for (int a = 0; a < Targets.Count; a++)
                {
                    var rect = HotspotsRects[a];
                    var targetHotspotTransform = Targets[a].TargetHotspot.transform;
                    SetHotspotPosition(rect, targetHotspotTransform);
                }
            }
        }


        public void SetHotspotPosition(RectTransform hotspotTransform, Transform objectTransform)
        {
            hotspotTransform.position = MainCamera.WorldToScreenPoint(objectTransform.position);
        }
    }
}