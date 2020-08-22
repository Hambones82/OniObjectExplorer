using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectExplorer
{
    public class Reticle
    {
        //private GameObject reticleImagePrefab;
        private LoadedAssets.AssetEnums reticlePrefabType;

        //rotates clockwise from bottom left to top right, to match rectransform.getworldcorners, though it doesn't actually matter

        private GameObject[] corners;

        public Reticle(LoadedAssets.AssetEnums rType, GameObject parent)
        {
            //reticleImagePrefab = prefab;
            reticlePrefabType = rType;
            corners = new GameObject[4];
            for(int i = 0; i < 4; i++)
            {
                corners[i] = LoadedAssets.InstantiatePostProcessed(reticlePrefabType, parent.transform);
                corners[i].SetActive(false);
            }
        }

        public void SetCurrentGameObject(GameObject GO)
        {
            Vector3[] fourCorners = new Vector3[4];
            RectTransform rT = GO.GetComponent<RectTransform>();
            //Transform t = GO.GetComponent<Transform>();
            if (rT != null)
            {
                rT.GetWorldCorners(fourCorners);
                SetCorners(fourCorners);
            }
            /*else if (t != null)
            {
                SetOnePoint(t.position);
            }*/
            else
            {
                DeActivate();
            }
        }
        
        public void SetCorners(Vector3[] fourcorners)
        {
            for(int i = 0; i < 4; i++)
            {
                corners[i].GetComponent<RectTransform>().SetPosition(fourcorners[i]);
                corners[i].SetActive(true);
            }
        }

        public void SetOnePoint(Vector3 point)
        {
            corners[0].GetComponent<RectTransform>().SetPosition(point);
            corners[0].SetActive(true);
            for(int i = 1; i < 4; i++)
            {
                corners[i].SetActive(false);
            }
        }

        public void DeActivate()
        {
            for (int i = 0; i < 4; i++)
            {
                corners[i].SetActive(false);
            }
        }
    }
}
