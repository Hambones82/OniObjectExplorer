using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace ObjectExplorer
{
    public class ObjectExplorerPatches
    {

        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class DbInitialize
        {
            public static void Prefix()
            {
                LoadedAssets.LoadAssets();
                LoadedAssets.RegisterPostProcessor(new InputFieldDialogPostProcessor());
                LoadedAssets.RegisterPostProcessor(new UnityFieldDisablesKleiKeyboardPostProcessor());
                LoadedAssets.RegisterPostProcessor(new UnityFieldAddCallbackComponentPostProcessor());
                LoadedAssets.RegisterPostProcessor(new OnMouseoverDisableKleiMousePostProcessor());
                LoadedAssets.RegisterPostProcessor(new UnityToggleAddCallbackComponentPostProcessor());
                LoadedAssets.RegisterPostProcessor(new AddInspectorFieldComponentPostProcessor());
            }
        }

        [HarmonyPatch(typeof(ResourceCategoryScreen), "OnActivate")]
        public static class RCSOnActivate
        {
            public static ExplorerManager explorer;
            public static void Postfix()
            {
                
                explorer = new ExplorerManager();
                explorer.SetActive(true);
                explorer.SetCurrentGameObject(GameScreenManager.Instance.ssOverlayCanvas);
            }
        }
    }
}
