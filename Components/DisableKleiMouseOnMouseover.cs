using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ObjectExplorer
{
    public class DisableKleiMouseOnMouseover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            Global.Instance.GetInputManager().GetDefaultController().ToggleMouse(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Global.Instance.GetInputManager().GetDefaultController().ToggleMouse(false);
        }
    }
}
