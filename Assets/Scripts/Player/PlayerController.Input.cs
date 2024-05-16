using UnityEngine;

namespace Player
{
    public partial class PlayerController
    {
        [Header("Input")] 
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode grappleKey = KeyCode.Mouse1;

        private void UpdateMovementInput()
        {
            //Move
            var rot = Quaternion.Euler(0, cameraHolder.rotationY,0);
            var dir = rot * Vector3.forward * Input.GetAxis("Vertical") + rot * Vector3.right * Input.GetAxis("Horizontal");
            SetMovementInput(dir);
            
            //Jump
            if (Input.GetKey(jumpKey) && _isReadyToJump && _isOnGround)
            {
                Jump();
                Invoke(nameof(ResetJump), jumpCd);
            }
        }
    }
}
