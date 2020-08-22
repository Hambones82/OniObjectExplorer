using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

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

        private static readonly List<Type> basicTypes = new List<Type>()
        {
            typeof(int),
            typeof(string),
            typeof(float),
            typeof(bool)
        };

        private static readonly List<Type> nonBasicTypes = new List<Type>()
        {
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector4),
            typeof(Color),
            typeof(Text),
            typeof(LocText)
        };

        public static bool IsEnum(Type type)
        {
            return (type.IsEnum && !type.IsDefined(typeof(FlagsAttribute), false));
        }

        public static bool IsPrimitiveType(Type type)
        {
            return (basicTypes.Contains(type) || IsEnum(type));
        }

        public static bool IsNonBasicType(Type type)
        {
            return nonBasicTypes.Contains(type);
        }

        public static bool IsPermittedType(Type type)
        {
            return IsNonBasicType(type) || IsPrimitiveType(type) || IsEnum(type);
        }

        public static bool IsPermittedBasicField(FieldInfo field)
        {
            return (!field.IsStatic && !field.IsLiteral && !field.IsInitOnly && IsPrimitiveType(field.FieldType));
        }

        public static bool IsPermittedBasicProperty(PropertyInfo property)
        {
            return (property.GetIndexParameters().Length == 0) && IsPrimitiveType(property.PropertyType);
        }
    }
}
