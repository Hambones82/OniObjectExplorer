using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ObjectExplorer
{
    public class MoveHandle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool isClicked { get; private set; }
        private Vector2 lastPosition;
        private Vector2 positionDelta;

        public ExplorerManager eman;

        public void OnPointerDown(PointerEventData eventData)
        {
            isClicked = true;
            lastPosition = eventData.pressPosition;
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isClicked = false;
        }

        public void Update()
        {
            if(isClicked == true)
            {
                positionDelta = new Vector2(Input.mousePosition.x - lastPosition.x, Input.mousePosition.y - lastPosition.y);
                eman.UpdatePositionRelative(positionDelta);
                lastPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }
    }
}
