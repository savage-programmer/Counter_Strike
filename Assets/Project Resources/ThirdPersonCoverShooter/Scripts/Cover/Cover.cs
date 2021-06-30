using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Identifies an object as a cover.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class Cover : MonoBehaviour
    {
        /// <summary>
        /// Y coordinate of the cover top in world space.
        /// </summary>
        public float Top
        {
            get
            {
                if (_collider == null)
                    _collider = GetComponent<BoxCollider>();

                return _collider.bounds.max.y;
            }
        }

        /// <summary>
        /// Direction pointing towards the wall.
        /// </summary>
        public Vector3 Forward
        {
            get { return transform.forward; }
        }

        /// <summary>
        /// Direction pointing right from a character along the wall.
        /// </summary>
        public Vector3 Right
        {
            get { return transform.right; }
        }

        /// <summary>
        /// Direction pointing left from a character along the wall.
        /// </summary>
        public Vector3 Left
        {
            get { return -transform.right; }
        }

        /// <summary>
        /// Orientation of the cover in degrees in world space.
        /// </summary>
        public float Angle
        {
            get { return transform.eulerAngles.y; }
        }

        /// <summary>
        /// Width of the cover.
        /// </summary>
        public float Width
        {
            get
            {
                if (_collider == null)
                    _collider = GetComponent<BoxCollider>();

                return _collider.size.x * transform.localScale.x;
            }
        }

        /// <summary>
        /// Determines if the character will climb on a platform or vault over a cover.
        /// </summary>
        [Tooltip("Determines if the character will climb on a platform or vault over a cover.")]
        public bool IsVault = false;

        /// <summary>
        /// Can the character use the left corner of the cover.
        /// </summary>
        [Tooltip("Can the character use the left corner of the cover.")]
        public bool OpenLeft = true;

        /// <summary>
        /// Can the character use the rgiht corner of the cover.
        /// </summary>
        [Tooltip("Can the character use the rgiht corner of the cover.")]
        public bool OpenRight = true;

        private BoxCollider _collider;

        /// <summary>
        /// Return whether the cover is considered for an observer with given y coordinate.
        /// </summary>
        public bool IsTall(float observer, float threshold)
        {
            return (Top - observer) > threshold;
        }

        /// <summary>
        /// Returns true if the given position is in front of the cover.
        /// </summary>
        public bool IsInFront(Vector3 observer)
        {
            var closest = ClosestPointTo(observer);
            var vector = (closest - observer).normalized;
            var dot = Vector3.Dot(vector, Forward);

            return dot >= 1.0f;
        }

        /// <summary>
        /// Returns true if the given position is in front of the cover.
        /// </summary>
        /// <param name="isOld">Old positions use lesser thresholds.</param>
        public bool IsInFront(Vector3 observer, bool isOld)
        {
            var closest = ClosestPointTo(observer);
            var vector = (closest - observer).normalized;
            var dot = Vector3.Dot(vector, Forward);

            if (isOld)
                return dot >= 0.85f;
            else
                return dot >= 0.95f;
        }

        /// <summary>
        /// Returns the position of the left corner with the given height coordinate.
        /// </summary>
        public Vector3 LeftCorner(float height, float offset = 0)
        {
            return getSide(Left, height, offset);
        }

        /// <summary>
        /// Returns the position of the right corner with the given height coordinate.
        /// </summary>
        public Vector3 RightSide(float height, float offset = 0)
        {
            return getSide(Right, height, offset);
        }

        /// <summary>
        /// Returns true if the given position is within the given distance of the left corner.
        /// </summary>
        public bool IsByLeftCorner(Vector3 position, float distance)
        {
            return Vector3.Distance(LeftCorner(position.y), position) <= distance;
        }

        /// <summary>
        /// Returns true if the given position is within the given distance of the right corner.
        /// </summary>
        public bool IsByRightCorner(Vector3 position, float distance)
        {
            return Vector3.Distance(RightSide(position.y), position) <= distance;
        }

        /// <summary>
        /// Returns a position on a cover closest to the given point.
        /// </summary>
        public Vector3 ClosestPointTo(Vector3 point)
        {
            if (_collider == null)
                _collider = GetComponent<BoxCollider>();

            var local = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * (point - transform.position);

            var hw = _collider.size.x * 0.5f * transform.localScale.x;
            var hh = _collider.size.z * 0.5f * transform.localScale.z;

            local = Util.FindClosestToPath(new Vector3(-hw, local.y, -hh),
                                           new Vector3(hw, local.y, -hh),
                                           local);

            var result = Quaternion.Euler(0, transform.eulerAngles.y, 0) * local + transform.position;

            return result;
        }

        private Vector3 getSide(Vector3 vector, float height, float offset)
        {
            var point = ClosestPointTo(transform.position + vector * 999) + vector * offset;
            point.y = height;

            return point;
        }
    }
}