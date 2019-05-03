using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SRCameraShake
{
    public class SRPool
    {
        public string poolName = "SRPool";
        public Stack<GameObject> objects;
        public int maxPoolAmount = 10;
        //那些控件在返回池中不会被销毁的
        public List<string> ignoreCompList = new List<string>() { "Transform", "MeshFilter", "MeshRenderer"};

        GameObject parentObj;
        string mPrefabPath;

        public SRPool(Transform holder, string prefabPath, int maxPoolAmount, string poolName = "")
        {
            objects = new Stack<GameObject>();
            if (!poolName.Equals(string.Empty))
            {
                this.poolName = poolName;
            }
            this.maxPoolAmount = maxPoolAmount;
            this.mPrefabPath = prefabPath;
            OnInit(holder, prefabPath);
        }

        public void OnInit(Transform holder, string prefabPath)
        {
            parentObj = new GameObject(poolName);
            parentObj.transform.SetParent(holder);
            parentObj.transform.localPosition = Vector3.zero;
            parentObj.transform.localRotation = Quaternion.identity;

            for (int i = 0; i < maxPoolAmount; i++)
            {
                GameObject obj = LoadGameObject(prefabPath);
                obj.transform.SetParent(parentObj.transform);
                obj.SetActive(false);
                objects.Push(obj);
            }
        }

        public GameObject GetObject()
        {
            GameObject obj = null;

            if (objects.Count > 0)
            {
                obj = objects.Pop();
                obj.SetActive(true);
            }
            else
            {
                obj = LoadGameObject(mPrefabPath);
                obj.transform.SetParent(parentObj.transform);
            }
            return obj;
        }

        public void ReturnObject(GameObject obj)
        {
            if (obj != null)
            {
                obj.transform.SetParent(parentObj.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;

                // 销毁控件 
                Component[] components = obj.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    string name = components[i].GetType().Name;
                    if (!ignoreCompList.Contains(name))
                    {
                        Object.Destroy(components[i]);
                    }
                }

                obj.SetActive(false);
                objects.Push(obj);
            }
        }

        GameObject LoadGameObject(string aPathGameObjectName)
        {
            GameObject tObject = Resources.Load(aPathGameObjectName, typeof(GameObject)) as GameObject;
            if (tObject != null)
            {
                GameObject tGameObject = GameObject.Instantiate(tObject);
                return tGameObject;
            }
            return tObject;
        }

    }
}
