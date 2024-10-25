using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class PlayerMove : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float precision = 0.01f;
        [SerializeField] private LayerMask nodeLayer;
        [SerializeField] private LayerMask obstacleLayer;

        [Header("Debug Settings")]
        [SerializeField] private bool showDebugInfo = false;

        private Vector3 targetPosition;
        private Vector3 startPosition;
        private bool isMoving = false;
        private float journeyLength;
        private float startTime;
        public int StepCount;
        public Transform StarPos;
        public bool canMove = false;
        public bool isDone = true;

        void Start()
        {
            transform.position = StarPos.position;
        }

        private void Update()
        {
            HandleInput();
            if (isMoving)
            {
                PreciseMove();
                isDone = false;
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        canMove = true;
                        Debug.Log("Player clicked, canMove set to true");
                        Debug.Log(IsMoving());
                    }
                    else if (canMove && ((1 << hit.collider.gameObject.layer) & nodeLayer) != 0)
                    {
                        Debug.Log("adad");
                        Transform nodeTransform = hit.collider.transform;
                        InitializeMovement(nodeTransform.position);
                        StepCount += 1;
                        canMove = false;
                    }
                }
            }
        }

        private void InitializeMovement(Vector3 newTarget)
        {
            startPosition = transform.position;
            targetPosition = new Vector3(newTarget.x, newTarget.y, transform.position.z);
            journeyLength = Vector3.Distance(startPosition, targetPosition);
            startTime = Time.time;
            isMoving = true;

            if (showDebugInfo)
            {
                Debug.Log($"Starting movement from {startPosition} to {targetPosition}. Distance: {journeyLength}");
            }
        }

        private void PreciseMove()
        {
            Vector3 newPosition = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            transform.position = newPosition;

            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTarget <= precision)
            {
                StartCoroutine(ResetCanMove());

                Debug.Log("can move" + canMove);
                FinalizeMovement();
            }

            UpdateRotation();

            if (showDebugInfo)
            {
                Debug.DrawLine(transform.position, targetPosition, Color.red);
                Debug.Log($"Distance to target: {distanceToTarget}");
            }
        }

        private void UpdateRotation()
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        private void FinalizeMovement()
        {
            transform.position = targetPosition;
            isMoving = false;
            isDone = true;
            if (showDebugInfo)
            {
                float totalTime = Time.time - startTime;
                Debug.Log($"Movement completed. Total time: {totalTime:F3}s");
            }
        }

        private IEnumerator ResetCanMove()
        {


            yield return new WaitForSeconds(0.5f); // Thời gian chờ trước khi reset canMove
            canMove = false;

            Debug.Log("canMove reset to false");
        }

        // Public methods để kiểm soát movement từ bên ngoài
        public void StopMovement()
        {
            isMoving = false;
        }

        public bool IsMoving()
        {
            return isMoving;
        }

        public Vector3 GetTargetPosition()
        {
            return targetPosition;
        }

        public float GetRemainingDistance()
        {
            if (!isMoving) return 0f;
            return Vector3.Distance(transform.position, targetPosition);
        }

        private bool IsValidDestination(Vector3 position)
        {
            return !float.IsNaN(position.x) && !float.IsNaN(position.y) && !float.IsInfinity(position.x) &&
                   !float.IsInfinity(position.y);
        }

        private void OnDrawGizmos()
        {
            if (showDebugInfo && isMoving)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(startPosition, targetPosition);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(targetPosition, precision);
            }
        }
    }
}


