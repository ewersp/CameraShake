using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ScriptableObjectUtil
{
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        ScriptableObject asset = ScriptableObject.CreateInstance<T>();
        ProjectWindowUtil.CreateAsset(asset, "New " + typeof(T).Name + ".asset");
    }

}

public class DataCreateUtil
{
    [MenuItem("Tools/Data/Create CameraShakeDataList")]
    static void CreateCameraShakeDataList()
    {
        ScriptableObjectUtil.CreateAsset<SRCameraShake.CameraShakeDataList>();
    }
}
