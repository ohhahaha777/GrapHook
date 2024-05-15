using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 3f;
        void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * (Time.deltaTime * moveSpeed));
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * (Time.deltaTime * moveSpeed));
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * (Time.deltaTime * moveSpeed));
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * (Time.deltaTime * moveSpeed));
            }
        }
    }
}
