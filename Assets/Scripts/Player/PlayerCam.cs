using UnityEngine;

namespace Player
{
    public class PlayerCam : MonoBehaviour
    {
        public float sensitivityX;
        public float sensitivityY;
        
        private float _rotationX;
        private float _rotationY;

        public Transform followPos;
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            _rotationX -= mouseY;
            _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);
            _rotationY += mouseX;
        
            transform.rotation = Quaternion.Euler(_rotationX, _rotationY,0);
            transform.position = followPos.position;
        }
    }
}
