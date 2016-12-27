using UnityEngine;

public class Main : MonoBehaviour {

	void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			CameraShakeManager.Instance.Play("Camera Shakes/Default");
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			CameraShakeManager.Instance.Play("Camera Shakes/Ambient");
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			CameraShakeManager.Instance.Play("Camera Shakes/Impact");
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			CameraShakeManager.Instance.StopAll();
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			GetComponent<BasicCharacterController>().enabled = !GetComponent<BasicCharacterController>().enabled;
		}
	}
}
