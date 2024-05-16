using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class CameraHolder : MonoBehaviour
    {
        public float sensitivityX;
        public float sensitivityY;
        
        public float rotationX;
        public float rotationY;

        public Transform followTarget;

        private void Awake()
        {
            transform.position = followTarget.position;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            UpdateRotation();
            UpdatePosition();
        }

        private void UpdateRotation()
        {
            var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            rotationY += mouseX;
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.rotation = Quaternion.Euler(rotationX, rotationY,0);
        }

        private void UpdatePosition()
        {
            var position = followTarget.position;
            var newY = Mathf.Lerp(transform.position.y, position.y, Time.deltaTime * sensitivityY); //垂直方向插值缓冲
            transform.position = new Vector3(position.x, newY, position.z);
        }
    }
}
