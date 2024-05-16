using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public Transform grappleTarget;
        public Camera playerCam;

        private bool _isShow;
        private Vector3 _targetWorldPosition;

        public void SetGrappleTargetPos(Vector3 worldPos)
        {
            _targetWorldPosition = worldPos;
        }

        public void SetIfTargetShow(bool isShow)
        {
            _isShow = isShow;
            if (!isShow)
            {
                grappleTarget.transform.position = new Vector3(0, 1500, 0);
            }
        }

        private void Update()
        {
            var screenPos = playerCam.WorldToScreenPoint(_targetWorldPosition);
            grappleTarget.transform.position = screenPos;
        }
    }
}
