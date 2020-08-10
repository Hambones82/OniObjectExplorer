﻿using System;
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
        public bool active { get; private set; }

        private GameObject currentGameObject;
        
        private PathPanel pathPanel;
        private ChildrenPanel childrenPanel;
        private ComponentsPanel componentsPanel;
        private InspectorPanel inspectorPanel;
        private MoveHandle moveHandle;
        private DialogManager dialogManager;
        private MainButtonMenu buttonMenu;

        private Reticle reticle;
        
        public ExplorerManager()
        {
            Globals.DebugCanvas = LoadedAssets.InstantiatePostProcessed(LoadedAssets.AssetEnums.debugcanvas);
            
            menuRoot = new GameObject("DebugRoot");

            moveHandle = new MoveHandle(menuRoot, menuRoot);
            moveHandle.moveHandle.GetComponent<RectTransform>().SetPosition(new Vector3(350, 180,0));

            inputController = new GameObject("InputController");
            inputController.AddComponent<ExplorerInputHandler>().SetExplorerManager(this);

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

            dialogManager = new DialogManager();

            buttonMenu = new MainButtonMenu(menuRoot);
            buttonMenu.SetPosition(new Vector2(350, 165));

            Debug.Log($"Assembly qualified name of this: {typeof(ExplorerManager).AssemblyQualifiedName}");
            Debug.Log($"Assembly qualified name of global: {typeof(Global).AssemblyQualifiedName}");

            //should be in the button menu itself? -- i think we should integrate this more into the menu..
            buttonMenu.AddButton("Open dialog", () => dialogManager.ActivateDialog((string s) => 
            {
                Debug.Log("anon func is running");
                GameObject browseTo = GameObjectPathParser.GetGOFromPath(s);
                if(browseTo != null)
                {
                    SetCurrentGameObject(browseTo);
                    dialogManager.DeactivateDialog();
                }
            }, STRINGS.DIALOGS.BROWSEBYPATH.TITLE, STRINGS.DIALOGS.BROWSEBYPATH.MESSAGE));

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
