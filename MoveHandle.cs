using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class MoveHandle
    {
        public GameObject moveHandle;

        public MoveHandle(GameObject target, GameObject parent)
        {
            moveHandle = LoadedAssets.InstantiatePostProcessed(LoadedAssets.AssetEnums.movehandle, parent.transform);
            moveHandle.AddComponent<MoveHandleComponent>().target = target;
        }
    }
}
