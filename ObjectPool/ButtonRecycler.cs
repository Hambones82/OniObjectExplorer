using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class ButtonRecycler : IPoolObjectRecycler
    {
        public GameObject RecycleObject(GameObject go)
        {
            go.GetComponent<Button>().onClick.RemoveAllListeners();
            go.SetActive(false);
            return go;
        }
    }
}
