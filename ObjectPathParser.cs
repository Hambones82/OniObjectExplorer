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
            //if(contains bad chars) return null

            Debug.Log($"Attempting to parse path {path}");

            PathDescriptor pathDescriptor = FindType(path);
            if(pathDescriptor.objectType == null)
            {
                Debug.Log("parser was unable to get type");
            }
            else
            {
                Debug.Log($"Retrieved type is {pathDescriptor.objectType.ToString()}");
            }

            string[] tokens = pathDescriptor.remainder;
            Type classType = pathDescriptor.objectType;

            Debug.Log($"Searching for property {tokens[0]}");
            PropertyInfo currentProperty = classType.GetProperty(tokens[0]);
            FieldInfo currentField = classType.GetField(tokens[0]);
            object currentObject;
            if (currentProperty == null && currentField == null)
            {
                Debug.Log($"The first parameter ({tokens[0]}) specified in the path does not exist in the specified type.");
                return null;
            }
            else if(currentProperty != null)
            {
                currentObject = currentProperty.GetValue(null, null);
                Debug.Log($"Found property {tokens[0]}");
            }
            else //if(currentField != null)
            {
                currentObject = currentField.GetValue(null);
                Debug.Log($"Found field {tokens[0]}");
            }
            
            
            Debug.Log($"current object type: {currentObject.GetType()}");
            //should be instance here...
            if (currentObject == null)
            {
                Debug.Log($"Unable to get the value of the specified parameter {tokens[0]}");
                return null;
            }
            else
            {
                Debug.Log($"Retrieved object for property {tokens[0]}");
            }

            

            for (int i = 1; i < tokens.Length; i++)
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

        //returns an array of size 1, 2, or 3
        //if 0, no type was found
        //if 1, no namespace

        //nested types...
        public static PathDescriptor FindType(string typeString)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            string[] tokens = typeString.Split('.');

            if (tokens.Length < 2) return null;


            //find the entire namespace by repeatedly lengthening the type to include more initial .-separated strings each iteration
            Type classType = null;
            string tryString = tokens[0];
            int wordsTraversed = 1;
            for (wordsTraversed = 1; wordsTraversed < tokens.Length; wordsTraversed++)
            {
                bool typeFound = false;
                foreach (Assembly assem in assemblies)
                {
                    //
                    classType = assem.GetType(tryString);
                    if (classType != null)
                    {
                        Debug.Log($"Found assembly with type {tryString}");
                        Debug.Log($"Assembly is: {assem.ToString()}");
                        typeFound = true;
                        break;//found the string
                    }
                }
                if (typeFound == true)
                {
                    break;
                }
                else
                {
                    tryString += "." + tokens[wordsTraversed];
                }
            }
            //here, tryString is the namespace + type, which is what we want

            if (classType == null)
            {
                Debug.Log("Unable to find assembly with specified type");
                return null;
            }
            string[] result = new string[tokens.Length-wordsTraversed];
            Array.Copy(tokens, wordsTraversed, result, 0, result.Length);
            return new PathDescriptor(classType, result);
        }
        
        //this would be more concise with an out parameter
        public class PathDescriptor
        {
            public Type objectType;
            public string[] remainder;

            public PathDescriptor(Type objectType, string[] remainder)
            {
                this.objectType = objectType;
                this.remainder = remainder;
            }
        }
    }
}
