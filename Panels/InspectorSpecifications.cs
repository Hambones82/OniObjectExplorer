using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ObjectExplorer
{
    public static class InspectorSpecifications
    {
        public static Dictionary<Type, string[]> ComponentSpecifications = new Dictionary<Type, string[]>()
        {
            {typeof(RectTransform), new string[] {"anchoredPosition", "anchorMax", "anchorMin", "offsetMax",
                                                  "offsetMin", "pivot", "sizeDelta"} }
        };

        public static Dictionary<Type, string[]> MemberSpecifications = new Dictionary<Type, string[]>()
        {
            {typeof(Vector2),  new string[]{ "x", "y" } },
            {typeof(Vector3),  new string[]{ "x", "y", "z" } },
            {typeof(Vector4), new string[] { "x", "y", "z", "w"} },
            {typeof(Color), new string[] {"r", "g", "b", "a"} }
        };
    }
}
