using UnityEngine;

/// <summary>
/// A very basic character controller for demo purposes.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class BasicCharacterController : MonoBehaviour {
	public float LookSensitivity = 5.0f;
	public float MoveSpeed = 6.0f;
	public float JumpSpeed = 8.0f;
	public float Gravity = 20.0f;

	private float m_rotationX;
	private float m_rotationY;
	private Vector3 m_movement;

	void Update() {
		UpdateMovement();
		UpdateLookAt();
	}

	private void UpdateMovement() {
		if (GetComponent<CharacterController>().isGrounded) {
			m_movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
			m_movement = MoveSpeed * transform.TransformDirection(m_movement);
			if (Input.GetButtonDown("Jump")) {
				m_movement.y = JumpSpeed;
			}
		}

		m_movement.y -= Gravity * Time.deltaTime;
		GetComponent<CharacterController>().Move(m_movement * Time.deltaTime);
	}

	private void UpdateLookAt() {
		m_rotationX += Input.GetAxis("Mouse X") * LookSensitivity;
		m_rotationY += Input.GetAxis("Mouse Y") * LookSensitivity;
		m_rotationX = ClampAngle(m_rotationX, -360.0f, 360.0f);
		m_rotationY = ClampAngle(m_rotationY, -89.0f, 89.0f);

		Quaternion xRot = Quaternion.AngleAxis(m_rotationX, Vector3.up);
		Quaternion yRot = Quaternion.AngleAxis(m_rotationY, -Vector3.right);
		transform.localRotation = xRot * yRot;
	}

	private float ClampAngle(float angle, float min, float max) {
		if (angle < -360.0f) {
			angle += 360.0f;
		}
		if (angle > 360.0f) {
			angle -= 360.0f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}