using UnityEngine;

/// <summary>
/// Contains the data for each camera shake type.
/// </summary>
public class CameraShake : MonoBehaviour {

	/// <summary>
	/// Which type of shake to use.
	/// </summary>
	public enum ShakeType { Constant, EaseIn, EaseOut, EaseInOut }

	/// <summary>
	/// Which type of noise to apply.
	/// </summary>
	public enum NoiseType { Perlin, Sin }

	/// <summary>
	/// The shake type.
	/// </summary>
	public ShakeType Shake = ShakeType.EaseOut;

	/// <summary>
	/// The noise type.
	/// </summary>
	public NoiseType Noise = NoiseType.Perlin;

	/// <summary>
	/// How much to move the camera, in local space.
	/// </summary>
	public Vector3 MoveExtents;

	/// <summary>
	/// How much to rotate the camera, in local space.
	/// </summary>
	public Vector3 RotateExtents;

	/// <summary>
	/// How fast to move the camera.
	/// </summary>
	public float Speed;

	/// <summary>
	/// The duration of the shake; -1 for looping.
	/// </summary>
	public float Duration;

	/// <summary>
	/// Internal seed for variable shake amounts.
	/// </summary>
	private Vector3 m_seed;

	/// <summary>
	/// The start time of this shake.
	/// </summary>
	private float m_startTime;

	/// <summary>
	/// Whether this shake is a loop.
	/// </summary>
	private bool m_loop;

	/// <summary>
	/// How long to transition out, in seconds.
	/// </summary>
	private const float kTransitionDuration = 1.0f;

	/// <summary>
	/// How high to value the seed range.
	/// </summary>
	private const float kSeedRange = 1000.0f;

	/// <summary>
	/// Initialize shake data.
	/// </summary>
	void Awake() {
		m_startTime = Time.time;
		m_seed = new Vector3(Random.Range(0.0f, kSeedRange), Random.Range(0.0f, kSeedRange), Random.Range(0.0f, kSeedRange));
		m_loop = Duration == -1.0f ? true : false;

		// If loop, ease in
		if (m_loop) {
			Duration = kTransitionDuration;
		}
	}

	/// <summary>
	/// Finish a shake.
	/// </summary>
	/// <param name="immediate">True to stop immediately this frame, false to ramp down.</param>
	public void Finish(bool immediate = false) {

		// Ramp down the shake effect
		if (m_loop || Duration > kTransitionDuration) {
			m_loop = false;
			Shake = ShakeType.EaseOut;
			Duration = kTransitionDuration;

			if (immediate) {
				m_startTime = Time.time - Duration;
			} else {
				m_startTime = Time.time;
			}
		}
	}

	/// <summary>
	/// Compute the shake matrix.
	/// </summary>
	/// <returns>The shake matrix.</returns>
	public Matrix4x4 ComputeMatrix() {
		Vector3 current = Speed * (Time.time * Vector3.one + m_seed);
		Vector3 adjustedMove = AdjustExtents(MoveExtents, Shake);
		Vector3 adjustedRotate = AdjustExtents(RotateExtents, Shake);

		Vector3 pos = Vector3.zero;
		if (MoveExtents != Vector3.zero) {
			pos = ApplyNoise(current, adjustedMove);
		}

		Quaternion rot = Quaternion.identity;
		if (RotateExtents != Vector3.zero) {
			rot = Quaternion.Euler(ApplyNoise(current, adjustedRotate));
		}

		return Matrix4x4.TRS(pos, rot, Vector3.one);
	}

	/// <summary>
	/// Check if the shake is done.
	/// </summary>
	/// <returns>True if done, false otherwise.</returns>
	public bool IsDone() {
		return !m_loop && GetT() >= 1.0f;
	}

	/// <summary>
	/// Compute the current time value (0-1) for the shake. 
	/// </summary>
	/// <returns>The current time value.</returns>
	private float GetT() {
		float t = Mathf.Clamp((Time.time - m_startTime) / Duration, 0.0f, 1.0f);
		return ApplyEaseOutSin(0.0f, 1.0f, t);
	}

	/// <summary>
	/// Apply basic ease out (sin).
	/// </summary>
	/// <param name="start">Start value.</param>
	/// <param name="end">End value.</param>
	/// <param name="value">Current value.</param>
	/// <returns>The interpolated value.</returns>
	private float ApplyEaseOutSin(float start, float end, float value) {
		return (end - start) * Mathf.Sin((value / 1.0f) * (Mathf.PI / 2.0f)) + start;
	}

	/// <summary>
	/// Adjust extents based on a shake type.
	/// </summary>
	/// <param name="extents">The extents to adjust.</param>
	/// <param name="shakeType">The shake type.</param>
	/// <returns>The adjusted shake extents.</returns>
	private Vector3 AdjustExtents(Vector3 extents, ShakeType shakeType) {
		switch (shakeType) {
			case ShakeType.Constant:
				return extents;
			case ShakeType.EaseIn:
				return Vector3.Slerp(Vector3.zero, extents, GetT());
			case ShakeType.EaseOut:
				return Vector3.Slerp(extents, Vector3.zero, GetT());
			case ShakeType.EaseInOut:
				return GetT() < 0.5f ? AdjustExtents(extents, ShakeType.EaseIn) : AdjustExtents(extents, ShakeType.EaseOut);
		}
		return extents;
	}

	/// <summary>
	/// Apply noise to a given target.
	/// </summary>
	/// <param name="target">The current target.</param>
	/// <param name="amplitude">The noise amplitude.</param>
	/// <returns>The target value with noise applied.</returns>
	private Vector3 ApplyNoise(Vector3 target, Vector3 amplitude) {
		switch (Noise) {
			case NoiseType.Sin: {
				float x = amplitude.x * Mathf.Sin(target.x);
				float y = amplitude.y * Mathf.Sin(target.y);
				float z = amplitude.z * Mathf.Sin(target.z);
				return new Vector3(x, y, z);
			}
			case NoiseType.Perlin: {
				float x = amplitude.x * 2.0f * (Mathf.PerlinNoise(target.x, target.x) - 0.5f);
				float y = amplitude.y * 2.0f * (Mathf.PerlinNoise(target.y, target.y) - 0.5f);
				float z = amplitude.z * 2.0f * (Mathf.PerlinNoise(target.z, target.z) - 0.5f);
				return new Vector3(x, y, z);
			}
		}
		return target;
	}
}
