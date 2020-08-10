using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public class InternalRefs : MonoBehaviour
    {
        public Dictionary<string, GameObject> RefsList = new Dictionary<string, GameObject>();
        public GameObject this[string lookup]
        {
            get
            {
                GameObject retVal;
                if(!RefsList.TryGetValue(lookup, out retVal))
                {
                    retVal = null;
                }
                return retVal;
            }
            set
            {
                RefsList[lookup] = value;
            } 
               
        }
    }
}
