using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    public partial class PlayerController : MonoBehaviour
    {
        [Header("PlayerSettings")] 
        public float playerHeight = 2f;

        [Header("Quote")] 
        public CameraHolder cameraHolder;

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
            _rigidBody.drag = _isOnGround ? groundDrag : 0;
            
            //Input
            UpdateMovementInput();
            
            //Grapple
            if (_grapplingCdTimer > 0) _grapplingCdTimer -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            Move();
            
            if(_isGrappling)
                lineRenderer.SetPosition(0, lineStart.position);
        }
    }
}
