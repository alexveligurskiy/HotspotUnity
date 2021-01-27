using UnityEngine;

namespace HotspotProject
{
    public class FreeCameraMovement : MonoBehaviour
    {
        public static FreeCameraMovement Instance;

        public static bool IsCameraMoving;

        public float movementSpeed = 10f;
        public float freeLookSensitivity = 3f;

        private bool looking = false;

        //Вызывается на кнопке - обычно так не делаю, потому что тяжело следить за тем что от куда вызывается, но тут на это времени не было
        public static void SetCameraIsMoving(bool value)
        {
            IsCameraMoving = value;
            Debug.Log($"[FreeCameraMovement] Set camera moving - {IsCameraMoving}");
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                Debug.LogError("[FreeCameraMovement] Try to create multiple instances of FreeCameraMovement, the object was destroyed!");
            }
        }

        private void Update()
        {
            if (!IsCameraMoving)
            {
                return;
            }

            var movementSpeed = this.movementSpeed;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position = transform.position + (-transform.right * movementSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position = transform.position + (transform.right * movementSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position = transform.position + (-transform.forward * movementSpeed * Time.deltaTime);
            }

            if (looking)
            {
                float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
                float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
                transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                StartLooking();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                StopLooking();
            }
        }

        private void OnDisable()
        {
            StopLooking();
        }

        private void StartLooking()
        {
            looking = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void StopLooking()
        {
            looking = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}