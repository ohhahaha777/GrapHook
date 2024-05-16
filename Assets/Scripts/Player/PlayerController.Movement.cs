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
        public float groundDrag = 5f;
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
        public float minGrappleDistance;
        public float maxGrappleDistance;
        public float grappleDelayTime;
        public float grappleCd;
        public GameObject[] grappleTargets;
        public float overshootYAxis;
        private float _grapplingCdTimer;
        private Vector3 _grappleTargetPoint;
        private bool _isGrappling;
        private bool _isGrapplingMoving;
        private void Move()
        {
            if(_isGrapplingMoving) return;
            //在地面
            if (_isOnGround)
            {
                _rigidBody.AddForce(_currentMoveDirection * (moveSpeed * 8f), ForceMode.Force);
            }
            //在空中
            else
            {
                _rigidBody.AddForce(_currentMoveDirection * (moveSpeed * airDrag), ForceMode.Force);
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

        private void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
        {
            _isGrapplingMoving = true;
            _rigidBody.velocity = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        }


        private bool FindGrappleTarget(out Vector3 targetPos)
        {
            Vector3 tempPos = Vector3.zero;
            var tempDis = float.MaxValue;
            for (int i = 0; i < grappleTargets.Length; i++)
            {
                float dot = Vector3.Dot(cameraHolder.transform.forward,  grappleTargets[i].transform.position - cameraHolder.transform.position);
                //摄像机正面
                if (dot > 0)
                {
                    var dis = Vector3.Distance(transform.position, grappleTargets[i].transform.position);
                    if (dis >= minGrappleDistance && dis <= maxGrappleDistance && dis < tempDis)
                    {
                        tempPos = grappleTargets[i].transform.position;
                        tempDis = dis;
                    }
                }
            }
            targetPos = tempPos;

            return tempDis < float.MaxValue;
        }
        
        private void StartGrappling()
        {
            if(_grapplingCdTimer > 0) return;

            if (FindGrappleTarget(out var point))
            {
                _isGrappling = true;
                _grappleTargetPoint = point;
                Invoke(nameof(ExecuteGrappling), grappleDelayTime);
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(1, point);
            }
        }
        
        private void ExecuteGrappling()
        {
            var lowestPoint = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            var grapplePointRelativeYPos = _grappleTargetPoint.y - lowestPoint.y;
            var highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;
            if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;
            
            JumpToPosition(_grappleTargetPoint, highestPointOnArc);
            Invoke(nameof(StopGrappling),2f);
        }
        
        private void StopGrappling()
        {
            _isGrappling = false;
            _isGrapplingMoving = false;
            _grapplingCdTimer = grappleCd;
            lineRenderer.enabled = false;
        }

        //计算钩锁速度向量  
        private Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
        {
            float gravity = Physics.gravity.y;
            float displacementX = endPoint.x - startPoint.x;
            float displacementY = endPoint.y - startPoint.y;
            float displacementZ = endPoint.z - startPoint.z;

            var offsetX = displacementX > 0 ? 2 : -2;
            var offsetZ = displacementZ > 0 ? 2 : -2;
            
            Vector3 displacementX0Z = new Vector3(displacementX + offsetX, 0, displacementZ + offsetZ);
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
            Vector3 velocityXZ = displacementX0Z / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
                                                    + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));
            return velocityXZ + velocityY;
        }
    }
}
