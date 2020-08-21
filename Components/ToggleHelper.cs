using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class ToggleCallback : UnityEvent<bool> { }

    public class ToggleHelper : MonoBehaviour
    {
        public ToggleCallback callBacks = new ToggleCallback();
        public Toggle toggleElement;

        public void SetIsOn(bool b)
        {
            toggleElement.onValueChanged.RemoveAllListeners();
            toggleElement.isOn = b;
            toggleElement.onValueChanged.AddListener(callBacks.Invoke);
        }
    }
}