using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SRCameraShake
{
    [System.Serializable]
    public class CameraShakeData
    {
        public string name = string.Empty;
        //预设好的振动效果
        public CameraShakePresets presetsType = CameraShakePresets.Default;
        public ShakeType mShakeType = ShakeType.EaseOut;
        public NoiseType mNoiseType = NoiseType.Sin;
        [Tooltip("控制振动的移动")]
        public Vector3 MoveExtents = new Vector3(0.1f, 0.1f, 0.1f);
        [Tooltip("控制振动的方向")]
        public Vector3 RotateExtents = new Vector3(0, 5, 5);
        [Tooltip("与振动强度相关")]
        public float Speed = 100;
        [Tooltip("持续时间,-1表示循环 ")]
        public float Duration = 1;
    }

    [System.Serializable]
    public class CameraShakeDataList : ScriptableObject
    {
        public List<CameraShakeData> cameraShakeDataList;

        public CameraShakeDataList()
        {
            cameraShakeDataList = new List<CameraShakeData>();
        }
    }
}
