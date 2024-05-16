using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    public partial class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed;

        private Vector3 _currentMoveDirection;

        [Header("Ground")] 
        public float groundDrag = 0.5f;
        public LayerMask groundLayer;
        private bool _isOnGround;

        [Header("Jump")] 
        public float jumpForce;
        public float jumpCd;
        public float airDrag;
        private bool _isReadyToJump;

        [Header("Grapple")] 
        public LineRenderer lineRenderer;
        public Transform lineStart;
        public float maxGrappleDistance;
        public float grappleDelayTime;
        public float grappleCd;
        private float _grapplingCdTimer;
        private Vector3 _grappleTargetPoint;
        private bool _isGrappling;
        private void Move()
        {
            //On Ground
            if (_isOnGround)
            {
                _rigidBody.MovePosition(_rigidBody.position +
                                        _currentMoveDirection * (moveSpeed * Time.fixedDeltaTime));
            }
            //In Air
            else
            {
                _rigidBody.AddForce(_currentMoveDirection.normalized * (moveSpeed * airDrag), ForceMode.Force);
            }
            
            //Grapple
            if (Input.GetKeyDown(grappleKey))
            {
                StartGrappling();
            }
        }
        
        private void UpdateGroundCheck()
        {
            _isOnGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + 0.1f, groundLayer);
        }
        private void SetMovementInput(Vector3 input)
        {
            _currentMoveDirection = input.normalized;
        }

        private void Jump()
        {
            _isReadyToJump = false;

            var velocity = _rigidBody.velocity;
            velocity = new Vector3(velocity.x, 0, velocity.z);
            _rigidBody.velocity = velocity;

            _rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            _isReadyToJump = true;
        }

        public bool CheckGrappleTarget()
        {
            return true;
        }

        private void StartGrappling()
        {
            if(_grapplingCdTimer > 0) return;
            _isGrappling = true;
            if (Physics.Raycast(lineStart.position,lineStart.forward, out var hit, maxGrappleDistance))
            {
                _grappleTargetPoint = hit.point;
                Invoke(nameof(ExcuteGrappling), grappleDelayTime);
            }
            else
            {
                _grappleTargetPoint = lineStart.position + lineStart.forward * maxGrappleDistance;
                Invoke(nameof(StopGrappling), grappleDelayTime);
            }

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, _grappleTargetPoint);
        }
        
        private void ExcuteGrappling()
        {
            
        }
        
        private void StopGrappling()
        {
            _isGrappling = false;
            _grapplingCdTimer = grappleCd;
            lineRenderer.enabled = false;
        }
    }
}
