using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ObjectExplorer
{
    public class ExplorerInputHandler : MonoBehaviour
    {
        public bool inspecting = false;

        private ExplorerManager explorerManager;

        public void Update()
        {
            if(Input.GetKeyDown(TUNING.INPUT.KEYS.debugEnable))
            {
                explorerManager.SetActive(!explorerManager.active);
                if(explorerManager.active == false)
                {
                    //REFACTOR - KLEI-RELATED STUFF IN NON-KLEI COMPONENT
                    //ALSO, SHOULD HAVE A CENTRAL THING FOR ALL THESE TOGGLES
                    Global.Instance.GetInputManager().GetDefaultController().ToggleKeyboard(false);
                    Global.Instance.GetInputManager().GetDefaultController().ToggleMouse(false);
                    //end all input fields...  all?  ....  not sure...it would have to be all, but... 
                    //need to make sure onendedit functions appropriately...
                    //well first off, if you're typing keys in, you can't press '`', so... that's not really a problem
                    //but for all the input fields... we do need a better 
                }
            }
            if (Input.GetMouseButtonDown(1) && inspecting == true)
            {
                Debug.Log("Showing raycast results:...");
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                UnityEngine.EventSystems.EventSystem.current.RaycastAll(new PointerEventData(UnityEngine.EventSystems.EventSystem.current) { position = mousePosition}, raycastResults);
                List<GameObject> goList = new List<GameObject>();
                foreach (RaycastResult r in raycastResults)
                {
                    goList.Add(r.gameObject);
                    explorerManager.ShowGameObjectList(goList);
                    //Debug.Log(r.gameObject.name);
                }
                inspecting = false;
                //StartCoroutine(ExecuteAfterTime)
                //Global.Instance.GetInputManager().GetDefaultController().ToggleMouse(false);
            }
        }

        public void SetExplorerManager(ExplorerManager explorer)
        {
            explorerManager = explorer;
        }

        
    }
}
