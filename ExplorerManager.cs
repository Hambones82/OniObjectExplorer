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
        public bool active { get; private set; }

        private GameObject currentGameObject;
        private Component currentComponent;
        
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
            ExplorerInputHandler inputHandler = inputController.AddComponent<ExplorerInputHandler>();
            inputHandler.SetExplorerManager(this);

            menuRoot.transform.parent = Globals.DebugCanvas.transform;
            RectTransform rectT = menuRoot.AddComponent<RectTransform>();
            rectT.anchorMin = new Vector2(.5f, .5f);
            rectT.anchorMax = new Vector2(.5f, .5f);
            rectT.SetLocalPosition(new Vector3(-350, -200, 0));
            childrenPanel = new ChildrenPanel(menuRoot, this);
            pathPanel = new PathPanel(menuRoot, this);
            inspectorPanel = new InspectorPanel(menuRoot, this);
            componentsPanel = new ComponentsPanel(menuRoot, this);
            SetActive(false);
            reticle = new Reticle(LoadedAssets.AssetEnums.reticle, Globals.DebugCanvas);

            dialogManager = new DialogManager();

            buttonMenu = new MainButtonMenu(menuRoot);
            buttonMenu.SetPosition(new Vector2(350, 165));

            //should be in the button menu itself? -- i think we should integrate this more into the menu..
            buttonMenu.AddButton("Open dialog", () => dialogManager.ActivateDialog((string s) => 
            {
                GameObject browseTo = GameObjectPathParser.GetGOFromPath(s);
                if(browseTo != null)
                {
                    SetCurrentGameObject(browseTo);
                    dialogManager.DeactivateDialog();
                }
            }, STRINGS.DIALOGS.BROWSEBYPATH.TITLE, STRINGS.DIALOGS.BROWSEBYPATH.MESSAGE));
            buttonMenu.AddButton("Inspect with RMB", () =>
            {
                inputHandler.inspecting = true; //ugly...??  have a separate class handle this -- shouldn't be too hard.
                //Global.Instance.GetInputManager().GetDefaultController().ToggleMouse(true);
            });
            buttonMenu.AddButton("New UI", () =>
            {
                GameObject gameObject = new GameObject("New UI");
                gameObject.AddComponent<RectTransform>();
                gameObject.transform.SetParent(Globals.DebugCanvas.transform);
                SetCurrentGameObject(gameObject);
            });
            buttonMenu.AddButton("Add Component", () =>
            {
                dialogManager.ActivateDialog((string s) =>
                {
                    Type type = GameObjectPathParser.FindTypeAndRemainder(s)?.objectType;
                    if(type == null)
                    {
                        Debug.Log("Add component dialog: invalid type entered");
                        return;
                    }
                    else
                    {
                        currentGameObject.AddComponent(type);
                        dialogManager.DeactivateDialog();
                        SetCurrentGameObject(currentGameObject);
                    }
                });
            });
            buttonMenu.AddButton("Add Child", () =>
            {
                GameObject child = new GameObject("child");
                child.AddComponent<RectTransform>();
                child.transform.SetParent(currentGameObject.transform);
                SetCurrentGameObject(child);
            });

        }

        public void SetActive(bool newActive)
        {
            active = newActive;
            menuRoot.SetActive(newActive);
        }
        
        public void SetCurrentGameObject(GameObject go)
        {
            if (go == null) return;
            currentGameObject = go;
            childrenPanel.SetCurrentGO(go);
            pathPanel.SetCurrentGO(go);
            componentsPanel.SetCurrentGO(go);
            inspectorPanel.ClearInspectorControls();
            reticle.SetCurrentGameObject(go);
            currentComponent = null;
        }
        
        public void SetCurrentComponent(Component C)
        {
            if (C == null) return;
            currentComponent = C;
            inspectorPanel.SetComponent(C);
        }

        //fix.  possibly: just refresh all values of the controls, instead of deleting them all, cleaning them up, and re-instating...
        //i might need to modify the controls to be actual components...  
        public void Refresh()
        {
            //Component cachedCurrentComponent = currentComponent;
            //SetCurrentGameObject(currentGameObject);
            //SetCurrentComponent(cachedCurrentComponent);
        }

        public void ShowGameObjectList(List<GameObject> goList)
        {
            childrenPanel.ShowGOList(goList);
            currentGameObject = null;
            pathPanel.RemoveObjects();
            componentsPanel.RemoveObjects();
            inspectorPanel.ClearInspectorControls();
            reticle.DeActivate();
        }
    }
}
