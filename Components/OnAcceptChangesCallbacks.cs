using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace ObjectExplorer
{
    public class OnAcceptChangesCallBack : UnityEvent<string> { }

    public class OnAcceptChangesCallbacks : MonoBehaviour
    {
        public OnAcceptChangesCallBack callBacks = new OnAcceptChangesCallBack();
    }
}
