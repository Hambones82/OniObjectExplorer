using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    //maybe i do want to make this a singleton...?  otherwise i need a pointer in like... the patches classes...
    public class ExplorerManager
    {
        public GameObject menuRoot { get; private set; }
        private bool active;

        private GameObject currentGameObject;
        
        private PathPanel pathPanel;
        private ChildrenPanel childrenPanel;
        private ComponentsPanel componentsPanel;
        private InspectorPanel inspectorPanel;

        private Reticle reticle;
        
        public ExplorerManager()
        {
            menuRoot = new GameObject("DebugRoot");
            menuRoot.transform.parent = Globals.DebugCanvas.transform;
            RectTransform rectT = menuRoot.AddComponent<RectTransform>();
            rectT.anchorMin = new Vector2(.5f, .5f);
            rectT.anchorMax = new Vector2(.5f, .5f);
            rectT.SetLocalPosition(new Vector3(0, 0, 0));
            childrenPanel = new ChildrenPanel(menuRoot, this);
            pathPanel = new PathPanel(menuRoot, this);
            inspectorPanel = new InspectorPanel(menuRoot, this);
            componentsPanel = new ComponentsPanel(menuRoot, this);
            SetActive(false);
            reticle = new Reticle(LoadedAssets.AssetEnums.reticle, Globals.DebugCanvas);
            //JUtils.ObjectHierarchies.LogChildren(menuRoot.transform, true, true);
            //Debug.Log($"menuroot's parent is: {menuRoot.transform.parent.name}");
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
    }
}
