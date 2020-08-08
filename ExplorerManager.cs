using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class ExplorerManager
    {
        public GameObject menuRoot { get; private set; }
        public GameObject inputController { get; private set; }
        public GameObject moveHandle { get; private set; }
        public bool active { get; private set; }

        private GameObject currentGameObject;
        
        private PathPanel pathPanel;
        private ChildrenPanel childrenPanel;
        private ComponentsPanel componentsPanel;
        private InspectorPanel inspectorPanel;

        private Reticle reticle;
        
        public ExplorerManager()
        {
            Globals.DebugCanvas = LoadedAssets.InstantiatePostProcessed(LoadedAssets.AssetEnums.debugcanvas);
            
            menuRoot = new GameObject("DebugRoot");

            moveHandle = LoadedAssets.InstantiatePostProcessed(LoadedAssets.AssetEnums.movehandle, menuRoot.transform);
            moveHandle.AddComponent<MoveHandle>().eman = this;
            RectTransform rectT = moveHandle.GetComponent<RectTransform>();
            rectT.SetPosition(new Vector3(350, 150, 0));


            inputController = new GameObject("InputController");
            inputController.AddComponent<ExplorerInputHandler>().SetExplorerManager(this);

            menuRoot.transform.parent = Globals.DebugCanvas.transform;
            rectT = menuRoot.AddComponent<RectTransform>();
            rectT.anchorMin = new Vector2(.5f, .5f);
            rectT.anchorMax = new Vector2(.5f, .5f);
            rectT.SetLocalPosition(new Vector3(0, 0, 0));
            childrenPanel = new ChildrenPanel(menuRoot, this);
            pathPanel = new PathPanel(menuRoot, this);
            inspectorPanel = new InspectorPanel(menuRoot, this);
            componentsPanel = new ComponentsPanel(menuRoot, this);
            SetActive(false);
            reticle = new Reticle(LoadedAssets.AssetEnums.reticle, Globals.DebugCanvas);
        }

        public void SetActive(bool newActive)
        {
            active = newActive;
            menuRoot.SetActive(newActive);
        }
        
        public void SetCurrentGameObject(GameObject go)
        {
            currentGameObject = go;
            childrenPanel.SetCurrentGO(go);
            pathPanel.SetCurrentGO(go);
            componentsPanel.SetCurrentGO(go);
            inspectorPanel.ClearInspectorControls();
            reticle.SetCurrentGameObject(go);
        }
        
        public void SetCurrentComponent(Component C)
        {
            inspectorPanel.SetComponent(C);
        }

        public void UpdatePositionRelative(Vector2 positionDelta)
        {
            RectTransform rectT = menuRoot.GetComponent<RectTransform>();
            Vector3 currentPos = rectT.GetPosition();
            Vector3 newPos = new Vector3(currentPos.x + positionDelta.x, currentPos.y + positionDelta.y, currentPos.z);
            rectT.SetPosition(newPos);
        }
    }
}
