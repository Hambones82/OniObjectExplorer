using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class InspectorPanel : ScrollablePanel
    {
        protected static readonly LoadedAssets.AssetEnums holderType = LoadedAssets.AssetEnums.inspectorpanel;
        protected static readonly LoadedAssets.AssetEnums labelType = LoadedAssets.AssetEnums.inspectorlabel;
        protected static readonly LoadedAssets.AssetEnums fieldType = LoadedAssets.AssetEnums.inspectorinputfield;

        private Dictionary<Type, IInspectorGenerator> inspectorGenerators;

        private IInspectorGenerator defaultInspectorGenerator;

        private IInspectorGenerator currentInspectorGenerator;

        private Component currentComponent;

        public InspectorPanel(GameObject parent, ExplorerManager eManager)
            :base(parent, eManager, LoadedAssets.AssetEnums.inspectorview)
        {
            uIObjectPool = InspectorPoolFactory.GetInspectorPool(holderType);
            inspectorGenerators = new Dictionary<Type, IInspectorGenerator>();
            defaultInspectorGenerator = new DefaultInspectorGenerator();
            currentComponent = null;
        }

        public void RegisterInspectorGenerator(Type componentType, IInspectorGenerator IG)
        {
            if(!(componentType.IsAssignableFrom(typeof(Component))))
            {
                Debug.Log("specified component type is not a component");
                return;
            }
            if(inspectorGenerators.ContainsKey(componentType))
            {
                Debug.Log("specified inspector generator is already registered");
                return;
            }
            inspectorGenerators.Add(componentType, IG);
        }

        public void ClearInspectorControls()
        {
            List<GameObject> inspectorControls = new List<GameObject>();
            foreach (GameObject contentObject in contentObjects)
            {
                Debug.Log($"number of children is {contentObject.transform.childCount}");
                inspectorControls.Add(contentObject.transform.GetChild(0).gameObject);
                inspectorControls.Add(contentObject.transform.GetChild(1).gameObject);
            }
            if(currentInspectorGenerator != null)
            {
                //this probably doesn't work...  well.. maybe it does???  no bc you set it based on new component... but want old component
                //to do the cleanup...
                currentInspectorGenerator.ClearInspectorControls(inspectorControls);
            }
            else
            {
                Debug.Log("inspector generator is null -- should only happen once"); //ok, this is supposed to happen once...
            }
            RemoveObjects();
        }

        

        public void SetComponent(Component C)
        {
            currentComponent = C;
            currentInspectorGenerator = GetInspectorGenerator(C);
            ClearInspectorControls();
            
            foreach(List<GameObject> panelContents in currentInspectorGenerator.GetComponentControls(C))
            {
                //create a new panel...
                GameObject newPanel = uIObjectPool.GetGameObject();
                contentObjects.Add(newPanel);
                foreach(GameObject go in panelContents)
                {
                    go.transform.SetParent(newPanel.transform);
                }
                newPanel.SetActive(true);
                newPanel.transform.SetParent(panelContent.transform);
                newPanel.transform.SetAsFirstSibling();
            }
        }

        private IInspectorGenerator GetInspectorGenerator(Component C)
        {
            IInspectorGenerator IG;
            if(inspectorGenerators.TryGetValue(C.GetType(), out IG))
            {
                return IG;
            }
            else
            {
                return GetDefaultInspectorGenerator();
            }
        }

        private IInspectorGenerator GetDefaultInspectorGenerator()
        {
            return defaultInspectorGenerator;
        }

        protected override void SetPosition()
        {
            panel.GetComponent<RectTransform>().SetLocalPosition(new Vector3(800, 0, 0));
        }
    }
}
