using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ObjectExplorer
{
    public class DropdownCallback : UnityEvent<int> { }

    public class DropdownHelper : MonoBehaviour
    {
        public DropdownCallback callBacks = new DropdownCallback();
        public Dropdown dropdown;

        public void SetValue(int value)
        {
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.value=value;
            dropdown.onValueChanged.AddListener(callBacks.Invoke);
        }
    }
}
