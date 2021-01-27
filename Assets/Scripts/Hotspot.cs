using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotspotProject.Targets
{
    public class Hotspot : MonoBehaviour
    {
        public Transform Transform;
        
        public bool IsVisible => IsVisibleForCamera && IsVisibleForRenderer;

        private bool IsVisibleForCamera = false;
        private bool IsVisibleForRenderer = true;

        /*Renderer m_Renderer;
        void Start()
        {
            m_Renderer = GetComponent<Renderer>();
        }

        //эта штука была на подстраховку, но как оказалось, она работает в том же духе, как и OnBecameVisible
        void Update()
        {
            IsVisibleForRenderer = m_Renderer.isVisible;
        }*/

        //Первый раз такой штукой пользовался, но чет она как то криво работает -
        //при уходе обьекта в правую и нижнюю часть экрана, то не отрабатывает в таком случае или отрабатывает не верно, ну или я моргаю и что то где то упустил)
        //может там что то с настройками камеры или может канваса - за выделенное время не успел до конца выяснить в чем причина этому)
        private void OnBecameInvisible()
        {
            //Debug.Log("[Hotspot] OnBecameInvisible");
            IsVisibleForCamera = false;
        }

        private void OnBecameVisible()
        {
            //Debug.Log("[Hotspot] OnBecameVisible");
            IsVisibleForCamera = true;
        }
    }
}