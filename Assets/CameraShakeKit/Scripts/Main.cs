using UnityEngine;

namespace SRCameraShake
{
    public class Main : MonoBehaviour
    {
        void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CameraShakeManager.Instance.Play(CameraShakePresets.Default);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CameraShakeManager.Instance.Play(CameraShakePresets.Ambient);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CameraShakeManager.Instance.Play(CameraShakePresets.Impact);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CameraShakeManager.Instance.StopAll();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Cursor.lockState = CursorLockMode.None;
                GetComponent<BasicCharacterController>().enabled = !GetComponent<BasicCharacterController>().enabled;
            }
        }
    }
}
