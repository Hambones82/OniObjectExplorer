using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public static class ButtonPoolFactory 
    {
        public static UIObjectPool GetPathButtonPool(LoadedAssets.AssetEnums buttonType)
        {
            UIObjectPool returnPool = new UIObjectPool(new ButtonCreator(buttonType), new ButtonRecycler());
            return returnPool;
        }
        
    }
}
