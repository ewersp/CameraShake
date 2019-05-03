using UnityEngine;

namespace SRCameraShake
{
    public enum ShakeType
    {
        Constant,//默认
        EaseIn, //淡入
        EaseOut,//淡出
        EaseInOut// 淡入淡出 
    }

    public enum NoiseType
    {
        Perlin,//柏林噪波
        Sin // sin波形噪波 
    }

    public enum CameraShakePresets
    {
        Default,
        Ambient,//测试循环振动 
        Impact,
    }

    public class CameraShake : MonoBehaviour
    {
        public ShakeType mShakeType = ShakeType.EaseOut;
        public NoiseType mNoiseType = NoiseType.Perlin;
        //本地移动和旋转
        public Vector3 MoveExtents;
        public Vector3 RotateExtents;
        public float Speed;
        //持续时间 -1表示循环 
        public float Duration;

        Vector3 seed;
        float startTime;
        bool isLoop;
        // 过渡时间 
        const float kTransitionDuration = 1.0f;
        const float kSeedRange = 1000.0f;

        public void UpdateData(CameraShakeData data)
        {
            if (data != null)
            {
                mShakeType = data.mShakeType;
                mNoiseType = data.mNoiseType;
                MoveExtents = data.MoveExtents;
                RotateExtents = data.RotateExtents;
                Speed = data.Speed;
                Duration = data.Duration;
            }
        }

        //void Start()
        //{
        //    OnInit();
        //}

        public void OnInit()
        {
            startTime = Time.time;
            seed = new Vector3(Random.Range(0.0f, kSeedRange), Random.Range(0.0f, kSeedRange), Random.Range(0.0f, kSeedRange));
            isLoop = (Duration == -1.0f);
            // If loop, ease in
            if (isLoop)
            {
                Duration = kTransitionDuration;
            }
        }

        /// <summary>
        /// 完成一次振动 
        /// </summary>
        /// <param name="immediate">True表示立即停止此帧，false表示斜坡式停止</param>
        public void OnFinish(bool immediate = false)
        {
            if (isLoop || Duration > kTransitionDuration)
            {
                isLoop = false;
                mShakeType = ShakeType.EaseOut;
                Duration = kTransitionDuration;

                if (immediate)
                {
                    startTime = Time.time - Duration;
                }
                else
                {
                    startTime = Time.time;
                }
            }
        }

        public Matrix4x4 ComputeMatrix()
        {
            Vector3 current = Speed * (Time.time * Vector3.one + seed);
            Vector3 adjustedMove = AdjustExtents(MoveExtents, mShakeType);
            Vector3 adjustedRotate = AdjustExtents(RotateExtents, mShakeType);

            Vector3 pos = Vector3.zero;
            if (MoveExtents != Vector3.zero)
            {
                pos = ApplyNoise(current, adjustedMove);
            }

            Quaternion rot = Quaternion.identity;
            if (RotateExtents != Vector3.zero)
            {
                rot = Quaternion.Euler(ApplyNoise(current, adjustedRotate));
            }

            return Matrix4x4.TRS(pos, rot, Vector3.one);
        }

        public bool IsDone()
        {
            Debug.Log("GetTime():" + GetTime());
            Debug.Log("isLoop:" + isLoop);
            return !isLoop && GetTime() >= 1.0f;
        }

        /// <summary>
        /// 获取当前时间值（0-1）
        /// </summary>
        /// <returns>The current time value.</returns>
        float GetTime()
        {
            float t = Mathf.Clamp((Time.time - startTime) / Duration, 0.0f, 1.0f);
            return ApplyEaseOutSin(0.0f, 1.0f, t);
        }

        float ApplyEaseOutSin(float start, float end, float value)
        {
            return (end - start) * Mathf.Sin((value / 1.0f) * (Mathf.PI / 2.0f)) + start;
        }

        /// <summary>
        /// Adjust extents based on a shake type.
        /// </summary>
        /// <param name="extents">The extents to adjust.</param>
        /// <param name="shakeType">The shake type.</param>
        /// <returns>The adjusted shake extents.</returns>
        Vector3 AdjustExtents(Vector3 extents, ShakeType shakeType)
        {
            switch (shakeType)
            {
                case ShakeType.Constant:
                    return extents;
                case ShakeType.EaseIn:
                    return Vector3.Slerp(Vector3.zero, extents, GetTime());
                case ShakeType.EaseOut:
                    return Vector3.Slerp(extents, Vector3.zero, GetTime());
                case ShakeType.EaseInOut:
                    return GetTime() < 0.5f ? AdjustExtents(extents, ShakeType.EaseIn) : AdjustExtents(extents, ShakeType.EaseOut);
            }
            return extents;
        }

        Vector3 ApplyNoise(Vector3 target, Vector3 amplitude)
        {
            switch (mNoiseType)
            {
                case NoiseType.Sin:
                    {
                        float x = amplitude.x * Mathf.Sin(target.x);
                        float y = amplitude.y * Mathf.Sin(target.y);
                        float z = amplitude.z * Mathf.Sin(target.z);
                        return new Vector3(x, y, z);
                    }
                case NoiseType.Perlin:
                    {
                        float x = amplitude.x * 2.0f * (Mathf.PerlinNoise(target.x, target.x) - 0.5f);
                        float y = amplitude.y * 2.0f * (Mathf.PerlinNoise(target.y, target.y) - 0.5f);
                        float z = amplitude.z * 2.0f * (Mathf.PerlinNoise(target.z, target.z) - 0.5f);
                        return new Vector3(x, y, z);
                    }
            }
            return target;
        }
    }
}
