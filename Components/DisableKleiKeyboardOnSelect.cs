using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ObjectExplorer
{
    public class DisableKleiKeyboardOnSelect : MonoBehaviour, ISelectHandler,  IDeselectHandler
    {
        
        public void OnDeselect(BaseEventData eventData)
        {
            Global.Instance.GetInputManager().GetDefaultController().ToggleKeyboard(false);
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            
            Global.Instance.GetInputManager().GetDefaultController().ToggleKeyboard(true);
        }
    }
}
