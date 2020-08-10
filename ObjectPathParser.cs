using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;

namespace ObjectExplorer
{
    public static class GameObjectPathParser
    {
        public static GameObject GetGOFromPath(string path)
        {
            Debug.Log($"Attempting to parse path {path}");
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            //if(contains bad chars) return null
            
            string[] tokens = path.Split('.');
            if (tokens.Length < 2) return null;
            Type classType = null;
            foreach (Assembly assem in assemblies)
            {
                classType = assem.GetType(tokens[0]);
                if (classType != null)
                {
                    Debug.Log($"Found assembly with type {tokens[0]}");
                    Debug.Log($"Assembly is: {assem.ToString()}");
                    break;
                }
            }
            if (classType == null)
            {
                Debug.Log("Unable to find assembly with specified type");
                return null;
            }

            Debug.Log($"Searching for property {tokens[1]}");
            PropertyInfo currentProperty = classType.GetProperty(tokens[1]);

            if (currentProperty == null)
            {
                Debug.Log($"The first parameter ({tokens[1]}) specified in the path does not exist in the specified type.");
                return null;
            }
            else
            {
                Debug.Log($"Found property {tokens[1]}");
            }
            
            object currentObject = currentProperty.GetValue(null, null);
            Debug.Log($"current object type: {currentObject.GetType()}");
            //should be instance here...
            if (currentObject == null)
            {
                Debug.Log($"Unable to get the value of the specified parameter {tokens[1]}");
                return null;
            }
            else
            {
                Debug.Log($"Retrieved object for property {tokens[1]}");
            }

            /*
            foreach(PropertyInfo p in currentObject.GetType().GetProperties())
            {
                Debug.Log($"Instance has property: {p.Name}");
            }

            foreach (FieldInfo f in currentObject.GetType().GetFields())
            {
                Debug.Log($"Instance has field: {f.Name}");
            }
            */
            FieldInfo currentField;

            for (int i = 2; i < tokens.Length; i++)
            {
                Debug.Log($"Searching for property {tokens[i]}");
                currentProperty = currentObject.GetType().GetProperty(tokens[i]);
                currentField = currentObject.GetType().GetField(tokens[i]);
                if (currentProperty == null && currentField == null)
                {
                    Debug.Log($"Property {tokens[i]} does not exist in current object, returning.");
                    return null;
                }

                if(currentProperty != null)
                {
                    currentObject = currentProperty.GetValue(currentObject, null);
                }
                else if(currentField != null)
                {
                    currentObject = currentField.GetValue(currentObject);
                }
                if (currentObject == null)
                {
                    Debug.Log($"Unable to get value for property {tokens[i]} in current object, returning.");
                    return null;
                }
                
            }
            if(currentObject.GetType() == typeof(GameObject))
            {
                Debug.Log("Found a gameobject matching the specified path");
                return (GameObject)currentObject;
            }
                
            else
            {
                Debug.Log("Unable to find a gameobject matching the specified path");
                return null;
            }
        }
    }
}
