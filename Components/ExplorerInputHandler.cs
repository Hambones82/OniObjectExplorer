using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class ExplorerInputHandler : MonoBehaviour
    {
        private ExplorerManager explorerManager;

        public void Update()
        {
            if(Input.GetKeyDown(TUNING.INPUT.KEYS.debugEnable))
            {
                explorerManager.SetActive(!explorerManager.active);
            }
        }

        public void SetExplorerManager(ExplorerManager explorer)
        {
            explorerManager = explorer;
        }
    }
}
