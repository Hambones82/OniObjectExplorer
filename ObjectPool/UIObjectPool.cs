using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class UIObjectPool
    {
        protected Stack<GameObject> availableObjects = new Stack<GameObject>();
        protected static readonly int initialNumber = 10;

        public IPoolObjectCreator poolObjectCreator;
        public IPoolObjectRecycler poolObjectRecycler;
        
        public UIObjectPool(IPoolObjectCreator objectCreator, IPoolObjectRecycler objectRecycler)
        {
            poolObjectCreator = objectCreator;
            poolObjectRecycler = objectRecycler;
            for (int i = 0; i < initialNumber; i++)
            {
                availableObjects.Push(poolObjectCreator.CreateNewPoolObject());
            }
        }

        public GameObject GetGameObject()
        {
            GameObject returnObject;
            if (availableObjects.Count > 0)
            {
                returnObject = availableObjects.Pop();
            }
            else
            {
                returnObject = poolObjectCreator.CreateNewPoolObject();
            }
            return returnObject;
        }

        public void RecycleObject(GameObject go)
        {
            poolObjectRecycler.RecycleObject(go);
            go.SetActive(false);
            go.transform.SetParent(Globals.DebugCanvas.transform);
            availableObjects.Push(go);
        }
    }
}
