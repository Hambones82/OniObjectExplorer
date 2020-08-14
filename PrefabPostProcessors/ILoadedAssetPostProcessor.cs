using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public interface ILoadedAssetPostProcessor
    {
        bool CanProcess(GameObject go);
        GameObject AssetPostProcess(GameObject go, object parameters = null);
    }
}
