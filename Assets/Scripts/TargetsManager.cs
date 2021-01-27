using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotspotProject.Targets
{
    public class TargetsManager : MonoBehaviour
    {
        public Action<Target> OnItemSelected;
        public Action<Target> OnItemDeselected;
        [SerializeField] private Transform TargetsContent = default;
        [SerializeField] private List<Target> TargetsList = new List<Target>();

        public static TargetsManager Instance;

        private Target CurrentSelectedtarget = null;

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
            SetupTargets();
        }

        public List<Target> GetTargets()
        {
            return TargetsList;
        }

        /// <summary>
        /// Тут конечно вот этого всего быть не должно, сюда по хорошему конфиг с данными, пул написать, но это не на сегодня задача
        /// Ну или если у нас AR, то точки у нас по сути будет свои и половину этого нам не надо вообще
        /// </summary>
        private void SetupTargets()
        {
            if (TargetsList.Count > 0)
            {
                foreach (var target in TargetsList)
                {
                    target.SetupItem(OnTargetSelected, OnTargetDeselected);
                }
            }
        }

        private void OnTargetSelected(Target target)
        {
            //Дабы по списку не бежать держим текущий и выключаем его
            if (CurrentSelectedtarget != null)
            {
                CurrentSelectedtarget.SetSelected(false);
            }

            CurrentSelectedtarget = target;

            OnItemSelected?.Invoke(target);
        }

        private void OnTargetDeselected(Target target)
        {
            CurrentSelectedtarget = null;
            OnItemDeselected?.Invoke(target);
        }
    }
}