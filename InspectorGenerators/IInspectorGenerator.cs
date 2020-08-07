using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//this isn't really set up that great... the functions should probably return a game object based on the component
namespace ObjectExplorer
{
    public interface IInspectorGenerator
    {
        IEnumerable<List<GameObject>> GetComponentControls(Component c);
        void RefreshInspectors();//this is definitely not necessary
        void ClearInspectorControls(List<GameObject> controlsToClear);
    }
}
