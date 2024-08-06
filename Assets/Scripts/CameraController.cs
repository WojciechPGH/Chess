using UnityEngine;

namespace Chess
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;
        [SerializeField]
        private float _distance = 25f;
        [SerializeField]
        private float _rotSpeed = 360.0f;
        private float x, y;

        private bool _isRotating = false;
        private bool IsRotating
        {
            get { return _isRotating; }
            set
            {
                if (_isRotating != value)
                {
                    _isRotating = value;
                    OnRotateChangeValue();
                }
            }
        }

        private void Start()
        {
            Vector3 angle = transform.eulerAngles;
            x = angle.x;
            y = angle.y;
            UpdateTransform();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButton(1))
            {
                UpdateTransform();
                IsRotating = true;
            }
            else
            {
                if (IsRotating)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    IsRotating = false;
                }
            }
        }

        private void UpdateTransform()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            x += Input.GetAxis("Mouse X") * _rotSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * _rotSpeed * Time.deltaTime;

            y = ClampAngle(y, 20, 90);

            Quaternion rot = Quaternion.Euler(y, x, 0f);
            Vector3 pos = rot * new Vector3(0f, 0f, -_distance) + _target.position;

            transform.SetPositionAndRotation(pos, rot);
        }

        private void OnRotateChangeValue()
        {
            StateMachine stateMachine = StateMachine.Instance;
            switch (IsRotating)
            {
                case true: stateMachine.AddState(new GameCameraRotateState()); break;
                case false: stateMachine.RemoveState(); break;
            }
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    }
}
