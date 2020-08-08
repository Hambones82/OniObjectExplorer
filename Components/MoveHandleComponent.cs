using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ObjectExplorer
{
    public class MoveHandleComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool isClicked = false;
        private Vector2 lastPosition;
        private Vector2 positionDelta;

        public GameObject target;

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
                UpdateTargetPosition(positionDelta);
                lastPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }

        private void UpdateTargetPosition(Vector2 positionDelta)
        {
            RectTransform rectT = target.GetComponent<RectTransform>();
            Vector3 currentPos = rectT.GetPosition();
            Vector3 newPos = new Vector3(currentPos.x + positionDelta.x, currentPos.y + positionDelta.y, currentPos.z);
            rectT.SetPosition(newPos);
        }
    }
}
