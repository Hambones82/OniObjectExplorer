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
        public GameObject explorerRoot { get; private set; }
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

        private ExplorerInputHandler inputHandler;


        public ExplorerManager()
        {
            Globals.DebugCanvas = LoadedAssets.InstantiatePostProcessed(LoadedAssets.AssetEnums.debugcanvas);
            
            menuRoot = new GameObject("DebugRoot");
            explorerRoot = new GameObject("ExplorerRoot");
            

            moveHandle = new MoveHandle(explorerRoot, explorerRoot);
            moveHandle.moveHandle.GetComponent<RectTransform>().SetPosition(new Vector3(-100, 180,0));

            inputController = new GameObject("InputController");
            inputHandler = inputController.AddComponent<ExplorerInputHandler>();
            inputHandler.SetExplorerManager(this);

            explorerRoot.transform.parent = Globals.DebugCanvas.transform;
            RectTransform explRect = explorerRoot.AddComponent<RectTransform>();
            explRect.anchorMin = new Vector2(.5f, .5f);
            explRect.anchorMax = new Vector2(.5f, .5f);
            explRect.SetLocalPosition(new Vector3(0, 0, 0));
            menuRoot.transform.parent = explorerRoot.transform;
            RectTransform rectT = menuRoot.AddComponent<RectTransform>();
            rectT.anchorMin = new Vector2(.5f, .5f);
            rectT.anchorMax = new Vector2(.5f, .5f);
            rectT.SetLocalPosition(new Vector3(0, 0, 0));
            childrenPanel = new ChildrenPanel(menuRoot, this);
            pathPanel = new PathPanel(menuRoot, this);
            inspectorPanel = new InspectorPanel(menuRoot, this);
            componentsPanel = new ComponentsPanel(menuRoot, this);
            reticle = new Reticle(LoadedAssets.AssetEnums.reticle, Globals.DebugCanvas);

            SetActive(false);
            
            dialogManager = new DialogManager();

            buttonMenu = new MainButtonMenu(menuRoot);
            buttonMenu.SetPosition(new Vector2(-100, 165));

            
            buttonMenu.AddButton("Browse by Object Name", () => dialogManager.ActivateDialog((string s) => 
            {
                GameObject browseTo = GameObjectPathParser.GetGOFromPath(s);
                if(browseTo != null)
                {
                    SetCurrentGameObject(browseTo);
                    dialogManager.DeactivateDialog();
                }
            }, STRINGS.DIALOGS.BROWSEBYPATH.TITLE, STRINGS.DIALOGS.BROWSEBYPATH.MESSAGE));
            buttonMenu.AddButton("Inspect", () =>
            {
                inputHandler.StartInspecting(); //ugly...??  have a separate class handle this -- shouldn't be too hard.
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
            explorerRoot.SetActive(newActive);
            if(newActive == false)
            {
                reticle.DeActivate();
                if(inputHandler != null)
                {
                    inputHandler.EndInspecting();
                }
            }
            if(newActive == true)
            {
                reticle.SetCurrentGameObject(currentGameObject);
            }
        }
        
        public void SetCurrentGameObject(GameObject go)
        {
            if (go == null) return;
            currentGameObject = go;
            childrenPanel.SetCurrentGO(go);
            pathPanel.SetCurrentGO(go);
            componentsPanel.SetCurrentGO(go);
            inspectorPanel.RemoveObjects();
            reticle.SetCurrentGameObject(go);
            currentComponent = null;
        }
        
        public void SetCurrentComponent(Component C)
        {
            if (C == null) return;
            currentComponent = C;
            inspectorPanel.SetComponent(C);
        }

        public void ShowGameObjectList(List<GameObject> goList)
        {
            childrenPanel.ShowGOList(goList);
            currentGameObject = null;
            pathPanel.RemoveObjects();
            componentsPanel.RemoveObjects();
            inspectorPanel.RemoveObjects();
            reticle.DeActivate();
        }
    }
}
