using UnityEngine;

namespace Player
{
    public class PlayerCamFollow : MonoBehaviour
    {
        public Transform followTarget;

        private void Update()
        {
            transform.position = followTarget.position;
        }
    }
}
