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
        public GameObject RecycleObject(GameObject go)
        {
            go.SetActive(false);
            return go;
        }
    }
}
