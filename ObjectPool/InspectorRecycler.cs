using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ObjectExplorer
{
    public class InspectorRecycler : IPoolObjectRecycler
    {
        //recycling the whole object -- yes
        public GameObject RecycleObject(GameObject go)
        {
            //go.GetComponentInChildren<InputFieldCallbacks>().callBacks.RemoveAllListeners();
            go.SetActive(false);
            return go;
        }
    }
}
