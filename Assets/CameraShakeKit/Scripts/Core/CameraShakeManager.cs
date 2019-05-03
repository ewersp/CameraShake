using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace SRCameraShake
{

    public class CameraShakeManager : MonoBehaviour
    {
        [HideInInspector]
        public static CameraShakeManager Instance = null;
        public CameraShakeDataList mCameraShakeDataList;
        public string prefabPath = "Prefabs/CameraShake";
        //为public只是方便测试查看 
        public List<CameraShake> activeShakeList = new List<CameraShake>();

        Camera mCamera;
        SRPool recyclePool;

        //单例的写法建议都在Awake赋值，确保是最先初始化的
        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            mCamera = GetComponent<Camera>();
            recyclePool = new SRPool(mCamera.transform, prefabPath, 20);
            if (mCameraShakeDataList == null)
            {
                mCameraShakeDataList = Resources.Load("Datas/CameraShakeDataList") as CameraShakeDataList;
            }
        }

        //Unity建议大多数相机逻辑在更新后期运行，以确保相机在此帧中最新。
        void LateUpdate()
        {
            Matrix4x4 shakeMatrix = Matrix4x4.identity;

            if (activeShakeList.Count == 0) return;

            for (int i = 0; i < activeShakeList.Count; i++)
            {
                if (activeShakeList[i] != null)
                {
                    shakeMatrix *= activeShakeList[i].ComputeMatrix();

                    // If done, remove
                    if (activeShakeList[i].IsDone())
                    {
                        recyclePool.ReturnObject(activeShakeList[i].gameObject);
                        activeShakeList.Remove(activeShakeList[i]);
                        mCamera.ResetWorldToCameraMatrix();
                    }
                }
                
            }

            // 相机始终向下看负z轴
            shakeMatrix *= Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1));

            // Update camera matrix
            if (activeShakeList.Count > 0)
            {
                mCamera.worldToCameraMatrix = shakeMatrix * transform.worldToLocalMatrix;
            }

        }

        /// <summary>
        /// 开启振动
        /// </summary>
        /// <param name="csPreset">预设值</param>
        /// <returns></returns>
        public CameraShake Play(CameraShakePresets csPreset)
        {
            var caObj = recyclePool.GetObject();
            CameraShake cameraShake = caObj.GetComponent<CameraShake>();
            if (cameraShake == null)
            {
                caObj.AddComponent<CameraShake>();
                cameraShake = caObj.GetComponent<CameraShake>();
            }
           
            if(cameraShake != null)
            {
                var dataList = mCameraShakeDataList.cameraShakeDataList;
                CameraShakeData data = null;
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].presetsType == csPreset)
                    {
                        data = dataList[i];
                        break;
                    }
                }
                if (data != null)
                {
                    cameraShake.UpdateData(data);
                    cameraShake.OnInit();
                    activeShakeList.Add(cameraShake);
                }
            }
            return cameraShake;
        }

        /// <summary>
        /// 开启振动
        /// </summary>
        /// <param name="csPreset">预设值</param>
        /// <returns></returns>
        public CameraShake Play(CameraShakeData data)
        {
            var caObj = recyclePool.GetObject();
            CameraShake cameraShake = caObj.GetComponent<CameraShake>();
            if (cameraShake == null)
            {
                caObj.AddComponent<CameraShake>();
                cameraShake = caObj.GetComponent<CameraShake>();
            }

            if (cameraShake != null)
            {
                if (data != null)
                {
                    cameraShake.UpdateData(data);
                    activeShakeList.Add(cameraShake);
                }
            }
            return cameraShake;
        }

        /// <summary>
        /// 停止振动 
        /// </summary>
        /// <param name="shake"></param>
        /// <param name="immediate"></param>
        public void Stop(CameraShake shake, bool immediate = false)
        {
            if (shake == null) return;
            shake.OnFinish(immediate);
        }

        public void StopAll(bool immediate = false)
        {
            for (int i = 0; i < activeShakeList.Count; i++)
            {
                Stop(activeShakeList[i], immediate);
            }
        }
    }
}