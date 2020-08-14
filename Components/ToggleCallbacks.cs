using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ObjectExplorer
{
    public class ToggleCallback : UnityEvent<bool> { }

    public class ToggleCallbacks : MonoBehaviour
    {
        public ToggleCallback callBacks = new ToggleCallback();
    }
}