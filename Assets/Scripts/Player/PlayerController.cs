using System;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    public partial class PlayerController : MonoBehaviour
    {
        [Header("PlayerSettings")] 
        public float playerHeight = 2f;

        public float rotateSpeed = 5f;
        [Header("Quote")] 
        public CameraHolder cameraHolder;
        public UIManager uiManager;
        private Rigidbody _rigidBody;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _isReadyToJump = true;
            _isGrappling = true;
        }

        private void Update()
        {
            //Ground Check
            UpdateGroundCheck();
            _rigidBody.drag = _isOnGround && !_isGrapplingMoving ? groundDrag : 0;
            
            //Input
            UpdateMovementInput();
            
            //Grapple
            if (_grapplingCdTimer > 0) _grapplingCdTimer -= Time.deltaTime;
            
            //UI
            if (FindGrappleTarget(out var pos))
            {
                uiManager.SetIfTargetShow(true);
                uiManager.SetGrappleTargetPos(pos);
            }
            else
            {
                uiManager.SetIfTargetShow(false);
            }
        }

        private void FixedUpdate()
        {
            Move();
            
            //Rotate
            var camY = cameraHolder.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, camY, 0), rotateSpeed);
            
            if(_isGrappling)
                lineRenderer.SetPosition(0, lineStart.position);
        }
    }
}
