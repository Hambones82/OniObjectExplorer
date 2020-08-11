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
                if(explorerManager.active == false)
                {
                    //REFACTOR - KLEI-RELATED STUFF IN NON-KLEI COMPONENT
                    Global.Instance.GetInputManager().GetDefaultController().ToggleKeyboard(false);
                    Global.Instance.GetInputManager().GetDefaultController().ToggleMouse(false);
                }
            }
        }

        public void SetExplorerManager(ExplorerManager explorer)
        {
            explorerManager = explorer;
        }
    }
}
