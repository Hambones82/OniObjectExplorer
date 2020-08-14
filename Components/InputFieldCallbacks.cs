using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace ObjectExplorer
{
    public class InputFieldCallBack : UnityEvent<string> { }

    public class InputFieldCallbacks : MonoBehaviour
    {
        public InputFieldCallBack callBacks = new InputFieldCallBack();
    }
}
