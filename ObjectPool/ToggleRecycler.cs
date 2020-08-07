﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class ToggleRecycler : IPoolObjectRecycler
    {
        public GameObject RecycleObject(GameObject go)
        {
            go.GetComponentInChildren<Toggle>().onValueChanged.RemoveAllListeners();
            go.transform.Find("Background/Checkmark").GetComponent<Image>().color =
                        TUNING.CONTROLS.EDITABLE.textColor;
            go.GetComponent<Toggle>().interactable = true;
            go.SetActive(false);
            return go;
        }
    }
}
