using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ObjectExplorer
{

    public static class InspectorPoolFactory
    {
        public static UIObjectPool GetInspectorPool(LoadedAssets.AssetEnums holderType)
        {
            UIObjectPool returnPool = new UIObjectPool(new InspectorCreator(holderType), new InspectorRecycler());
            return returnPool;
        }
        
    }
}
