using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Manages a search for covers.
    /// </summary>
    public class CoverSearch
    {
        private Vector3 _position;
        private Vector3 _head;
        private CoverState _current;
        private float _tallThreshold;
        private float _lowRadius;
        private float _tallRadius;
        private float _radius;
        private float _leaveDistance;
        private float _distance;
        private float _adjacentDistance;

        private Collider[] _colliders = new Collider[16];
        private Cover[] _covers = new Cover[16];
        private int _colliderCount;

        /// <summary>
        /// Updates cover search.
        /// </summary>
		public void Update(CoverState current, Vector3 position, Vector3 head, float tallThreshold, float searchRadius, float lowRadius, float tallRadius, float capsuleRadius, float leaveDistance, float enterDistance, float adjacentDistance,LayerMask Cover)
        {
            _current = current;
			_colliderCount = Physics.OverlapSphereNonAlloc(position, searchRadius, _colliders,Cover);
            _position = position;
            _head = head;
            _tallThreshold = tallThreshold;
            _lowRadius = lowRadius;
            _tallRadius = tallRadius;
            _radius = capsuleRadius;
            _leaveDistance = leaveDistance;
            _distance = enterDistance;
            _adjacentDistance = adjacentDistance;

            for (int i = 0; i < _colliderCount; i++)
                _covers[i] = _colliders[i].GetComponent<Cover>();
        }

        /// <summary>
        /// Find a cover closest to the character.
        /// </summary>
        public Cover FindClosest()
        {
            Cover result = null;

            for (int i = 0; i < _colliderCount; i++)
            {
                var cover = _covers[i];

                if (cover != null && cover == _current.Main && doesCoverFit(cover))
                    result = cover;
            }

            for (int i = 0; i < _colliderCount; i++)
            {
                var cover = _covers[i];

                if (cover != null && cover != _current.Main && doesCoverFit(cover))
                {
                    if (result == null)
                        result = cover;
                    else if (_current.MainChangeAge >= 1)
                    {
                        _head.y = _position.y;
                        var headDistance = Vector3.Distance(_head, cover.ClosestPointTo(_head));

                        if (headDistance < 0.3f)
                            result = cover;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Find an adjacent cover to the left of the given one.
        /// </summary>
        public Cover FindLeftOf(Cover middle)
        {
            return findAdjacentTo(middle, middle.LeftCorner(_position.y), -120, 60);
        }

        /// <summary>
        /// Find an adjacent cover to the right of the given one.
        /// </summary>
        public Cover FindRightOf(Cover middle)
        {
            return findAdjacentTo(middle, middle.RightSide(_position.y), -60, 120);
        }

        /// <summary>
        /// Find an adjacent cover.
        /// </summary>
        private Cover findAdjacentTo(Cover middle, Vector3 point, float minAngle, float maxAngle)
        {
            float closestDistance = 0f;
            Cover closestCover = null;

            float angle = middle.Angle;

            for (int i = 0; i < _colliderCount; i++)
            {
                var cover = _covers[i];

                if (cover != null && cover != middle)
                {
                    var closest = cover.ClosestPointTo(point);
                    var distance = Vector3.Distance(point, closest);
                    var closestAngle = cover.Angle;
                    var deltaAngle = Mathf.DeltaAngle(angle, closestAngle);

                    if (distance <= _adjacentDistance && deltaAngle >= minAngle && deltaAngle <= maxAngle)
                        if (closestCover == null || distance < closestDistance)
                        {
                            closestCover = cover;
                            closestDistance = distance;
                        }
                }
            }

            return closestCover;
        }

        /// <summary>
        /// Check if the given cover is fitting.
        /// </summary>
        private bool doesCoverFit(Cover cover)
        {
            if (cover == null || cover.Top < _position.y + 0.5f)
                return false;

            var position = _position + cover.Forward * _radius;

            var distance = Vector3.Distance(position, cover.ClosestPointTo(position));

            var hasLeft = FindLeftOf(cover) != null;
            var hasRight = FindRightOf(cover) != null;

            var leaveRadius = cover.IsTall(_position.y, _tallThreshold) ? _tallRadius : _lowRadius;

            var isInFront = cover.IsInFront(_position, cover == _current.Main) &&
                            (cover.IsInFront(_position + cover.Right * leaveRadius, cover == _current.Main) || hasRight) &&
                            (cover.IsInFront(_position + cover.Left * leaveRadius, cover == _current.Main) || hasLeft);

            var isOld = isInFront && distance <= _leaveDistance && cover == _current.Main;
            if (isOld) return true;

            var isNew = isInFront && distance <= _distance && cover != _current.Main;
            if (isNew) return true;

            return false;
        }
    }
}