using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Attach CameraShakeManager to your primary Camera GameObject.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraShakeManager : MonoBehaviour {

	/// <summary>
	/// Internal list of active camera shake components.
	/// </summary>
	private List<CameraShake> m_activeShakes = new List<CameraShake>();

	/// <summary>
	/// Singleton reference.
	/// </summary>
	private static CameraShakeManager m_instance;
	public static CameraShakeManager Instance {
		get { return m_instance; }
	}

	/// <summary>
	/// Convenience getter for the camera.
	/// </summary>
	private Camera Camera {
		get { return GetComponent<Camera>(); }
	}

	/// <summary>
	/// Initialize singleton.
	/// </summary>
	void Awake() {
		m_instance = this;
	}

	/// <summary>
	/// Unity recommends most camera logic run in late update, to ensure the camera is most up to date this frame.
	/// </summary>
	void LateUpdate() {
		Matrix4x4 shakeMatrix = Matrix4x4.identity;

		// For each active shake
		foreach (var shake in m_activeShakes.Reverse<CameraShake>()) {

			// Concatenate its shake matrix
			shakeMatrix *= shake.ComputeMatrix();

			// If done, remove
			if (shake.IsDone()) {
				m_activeShakes.Remove(shake);
				Destroy(shake.gameObject);
				Camera.ResetWorldToCameraMatrix();
			}
		}

		// Camera always looks down the negative z-axis
		shakeMatrix *= Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1, 1, -1));

		// Update camera matrix
		if (m_activeShakes.Count > 0) {
			Camera.worldToCameraMatrix = shakeMatrix * transform.worldToLocalMatrix;
		}
	}

	/// <summary>
	/// Start a camera shake.
	/// </summary>
	/// <param name="name">The resource name of the shake to play.</param>
	/// <returns>A reference to the camera shake object.</returns>
	public CameraShake Play(string name) {
		var cs = Instantiate(Resources.Load<GameObject>(name), transform) as GameObject;
		if (cs != null) {
			m_activeShakes.Add(cs.GetComponent<CameraShake>());
		}
		return cs.GetComponent<CameraShake>();
	}

	/// <summary>
	/// Stop a camera shake.
	/// </summary>
	/// <param name="shake">The camera shake to stop.</param>
	/// <param name="immediate">True to stop immediately this frame, false to ramp down.</param>
	public void Stop(CameraShake shake, bool immediate = false) {
		if (shake == null) return;

		shake.Finish(immediate);
	}

	/// <summary>
	/// Stop all active camera shakes.
	/// </summary>
	/// <param name="immediate">True to stop immediately this frame, false to ramp down.</param>
	public void StopAll(bool immediate = false) {
		foreach (var shake in m_activeShakes) {
			Stop(shake, immediate);
		}
	}
}
