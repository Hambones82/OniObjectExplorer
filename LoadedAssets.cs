using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace ObjectExplorer
{
    public static class LoadedAssets
    {
        
        public enum AssetEnums
        {
            objectbutton,
            scrollview,
            pathbutton,
            pathview,
            reticle,
            componentbutton,
            componentview,
            inspectorpanel,
            inspectorlabel,
            inspectorinputfield,
            inspectorview,
            inspectortoggle,
            inspectordropdown,
            debugcanvas,
            movehandle,
            inputfielddialog,
            menubutton,
            menubuttonpanel
        }

        private static Dictionary<AssetEnums, AssetInFileDescriptor> assetDescriptors = new Dictionary<AssetEnums, AssetInFileDescriptor>()
        {
            [AssetEnums.objectbutton] = new AssetInFileDescriptor("objectbutton", "ObjectButton"),
            [AssetEnums.scrollview] = new AssetInFileDescriptor("scrollview", "Scroll View"),
            [AssetEnums.pathbutton] = new AssetInFileDescriptor("pathbutton", "PathButton"),
            [AssetEnums.pathview] = new AssetInFileDescriptor("pathview", "PathView"),
            [AssetEnums.reticle] = new AssetInFileDescriptor("reticle", "Reticle"),
            [AssetEnums.componentbutton] = new AssetInFileDescriptor("componentsbutton", "ComponentsButton"),
            [AssetEnums.componentview] = new AssetInFileDescriptor("componentsview", "ComponentsView"),
            [AssetEnums.inspectorpanel] = new AssetInFileDescriptor("inspectorcontrolpanel", "InspectorControlPanel"),
            [AssetEnums.inspectorlabel] = new AssetInFileDescriptor("inspectorlabel", "Inspector Label"),
            [AssetEnums.inspectorinputfield] = new AssetInFileDescriptor("inspectorinputfield", "InspectorInputField"),
            [AssetEnums.inspectorview] = new AssetInFileDescriptor("inspectorview", "InspectorView"),
            [AssetEnums.inspectortoggle] = new AssetInFileDescriptor("inspectortoggle", "InspectorToggle"),
            [AssetEnums.inspectordropdown] = new AssetInFileDescriptor("inspectordropdown", "InspectorDropdown"),
            [AssetEnums.debugcanvas] = new AssetInFileDescriptor("debugcanvas", "DebugCanvas"),
            [AssetEnums.movehandle] = new AssetInFileDescriptor("movehandle", "MoveHandle"),
            [AssetEnums.inputfielddialog] = new AssetInFileDescriptor("inputfielddialog", "InputFieldDialog"),
            [AssetEnums.menubutton] = new AssetInFileDescriptor("menubutton", "MenuButton"),
            [AssetEnums.menubuttonpanel] = new AssetInFileDescriptor("menubuttonpanel", "MenuButtonPanel")
        };

        private static Dictionary<AssetEnums, GameObject> loadedPrefabs = new Dictionary<AssetEnums, GameObject>();

        private static Dictionary<AssetEnums, List<ILoadedAssetPostProcessor>> cachedPostProcessors = new Dictionary<AssetEnums, List<ILoadedAssetPostProcessor>>();

        private static List<ILoadedAssetPostProcessor> AssetPostProcessors = new List<ILoadedAssetPostProcessor>();

        
        public static void RegisterPostProcessor(ILoadedAssetPostProcessor postProcessor)
        {
            AssetPostProcessors.Add(postProcessor);
            
            foreach(KeyValuePair<AssetEnums, GameObject> kvp in loadedPrefabs)
            {
                if(postProcessor.CanProcess(kvp.Value))
                {
                    List<ILoadedAssetPostProcessor> ppList;
                    if(!cachedPostProcessors.TryGetValue(kvp.Key, out ppList))
                    {
                        cachedPostProcessors[kvp.Key] = ppList = new List<ILoadedAssetPostProcessor>();
                    }
                    ppList.Add(postProcessor);
                }
            }
        }

        public static void LoadAssets()
        {
            Debug.Log("Loading Assets");
            foreach (KeyValuePair<AssetEnums, AssetInFileDescriptor> kvp in assetDescriptors)
            {
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets", kvp.Value.fileName);
                AssetBundle assetBundle = AssetBundle.LoadFromFile(path);

                if (assetBundle == null)
                {
                    Debug.Log($"Failed to load AssetBundle from path {path}");
                    continue;
                }

                GameObject loadedAsset = assetBundle.LoadAsset<GameObject>(kvp.Value.assetName);
                if(loadedAsset == null)
                {
                    Debug.Log("the loaded asset is null");
                    continue;
                }
                loadedPrefabs.Add(kvp.Key, loadedAsset);

                PrefabTypeTag typeTag = loadedAsset.AddComponent<PrefabTypeTag>();
                typeTag.prefabType = kvp.Key;
            }
        }
        
        public static GameObject InstantiatePostProcessed(AssetEnums assetType, Transform parent = null, object parameters = null)
        {
            GameObject returnVal = UnityEngine.Object.Instantiate(loadedPrefabs[assetType], parent);


            List<ILoadedAssetPostProcessor> ilppList;
            if(cachedPostProcessors.TryGetValue(assetType, out ilppList))
            {
                foreach (ILoadedAssetPostProcessor ilpp in ilppList)
                {
                    ilpp.AssetPostProcess(returnVal, parameters);
                }
            }
            //returnVal.transform.SetParent(parent);
            return returnVal;
        }
        
        public static GameObject GetLoadedPrefab(AssetEnums assetEnum)
        {
            GameObject go;
            if (loadedPrefabs.ContainsKey(assetEnum))
                go = loadedPrefabs[assetEnum];
            else
            {
                Debug.Log($"Can't find asset for {assetEnum.ToString()}");
                return null;
            }
            return go;
        }

        private struct AssetInFileDescriptor
        {
            public string fileName;
            public string assetName;

            public AssetInFileDescriptor(string fn, string an)
            {
                fileName = fn;
                assetName = an;
            }
        }
    }
}
