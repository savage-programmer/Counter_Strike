using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Component attached to weapons. Fires bullets when prompted so by CharacterMotor.
    /// </summary>
    public class Gun : MonoBehaviour
    {
		public LayerMask RayLayer;
        /// <summary>
        /// Point of creation for bullets in world space.
        /// </summary>
        public Vector3 Origin { get { return _lastAimOrigin; } }

        /// <summary>
        /// Direction to target changed by recoil.
        /// </summary>
        public Vector3 Direction
        {
            get { return getRecoiledDirectionFrom(Origin); }
        }

        /// <summary>
        /// Perfect vector from aim origin to the set target.
        /// </summary>
        public Vector3 TargetDirection
        {
            get { return getTargetDirectionFrom(Origin); }
        }

        /// <summary>
        /// Current intensity of recoil affecting the gun. In range of 0 to 1.
        /// </summary>
        public float RecoilIntensity
        {
            get { return _recoilIntensity; }
        }

        /// <summary>
        /// Offset of the right hand in world space.
        /// </summary>
        public Vector3 RecoilShift
        {
            get { return _recoil; }
        }

        /// <summary>
        /// Returns true if currently clip is empty.
        /// </summary>
        public bool IsClipEmpty
        {
            get { return Clip <= 0; }
        }

        /// <summary>
        /// Rate of fire in bullets per second.
        /// </summary>
        [Tooltip("Rate of fire in bullets per second.")]
        [Range(0, 1000)]
        public float Rate = 7;

        /// <summary>
        /// Maximum distance of a bullet hit. Objects further than this value are ignored.
        /// </summary>
        [Tooltip("Maximum distance of a bullet hit. Objects further than this value are ignored.")]
        public float Distance = 50;

        /// <summary>
        /// Damage dealt by a single bullet.
        /// </summary>
        [Tooltip("Damage dealt by a single bullet.")]
        [Range(0, 1000)]
        public float Damage = 10;

        /// <summary>
        /// Size of a clip. Clip is set to this value on reload.
        /// </summary>
        [Tooltip("Size of a clip. Clip is set to this value on reload.")]
        [Range(0, 1000)]
        public int ClipSize = 10;

        /// <summary>
        /// Current number of bullets in a clip.
        /// </summary>
        [Tooltip("Current number of bullets in a clip.")]
        [Range(0, 1000)]
        public int Clip = 10;

        /// <summary>
        /// Link to the object that controls the aiming direction.
        /// </summary>
        [Tooltip("Link to the object that controls the aiming direction.")]
        public GameObject Aim;

        /// <summary>
        /// Settings that manage gun's recoil behaviour.
        /// </summary>
        [Tooltip("Settings that manage gun's recoil behaviour.")]
        public RecoilSettings RecoilSettings = RecoilSettings.Default();

        /// <summary>
        /// Link to the object that controls the position of character's left hand relative to the weapon.
        /// </summary>
        [Tooltip("Link to the object that controls the position of character's left hand relative to the weapon.")]
        public Transform LeftHandDefault;

        /// <summary>
        /// Links to objects that overwrite the value in LeftHand based on the gameplay situation.
        /// </summary>
        [Tooltip("Links to objects that overwrite the value in LeftHand based on the gameplay situation.")]
        public HandOverwrite LeftHandOverwrite;

        /// <summary>
        /// Owning object with a CharacterMotor component.
        /// </summary>
        [HideInInspector]
        public GameObject Character;

        /// <summary>
        /// Target position in world space set by the character.
        /// </summary>
        [HideInInspector]
        public Vector3 Target;

        private float _recoilIntensity;

        private bool _isUsingCustomRaycastOrigin;
        private Vector3 _customRaycastOrigin;

        private float _fireWait = 0;
        private bool _isFiring;
        private bool _isAllowed;
        private bool _wasAllowedAndFiring;

        private Vector3 _lastAimOrigin;

        private Vector3 _intendedForward;

        private Vector3 _recoil;

        private List<RecoilImpulse> _recoilImpulses = new List<RecoilImpulse>();

        private RaycastHit[] _hits = new RaycastHit[16];

        private void OnValidate()
        {
            Distance = Mathf.Max(0, Distance);
        }

        /// <summary>
        /// Calculates direction to Target from given origin and adjusts it by recoil.
        /// </summary>
        private Vector3 getRecoiledDirectionFrom(Vector3 origin)
        {
            var rotation = Quaternion.FromToRotation(_intendedForward, transform.forward);
            var oldVector = getTargetDirectionFrom(origin);
            var newVector = rotation * getTargetDirectionFrom(origin);
            newVector.x = oldVector.x;
            newVector.z = oldVector.z;
            return newVector.normalized;
        }

        /// <summary>
        /// Calculates perfect vector from given origin to Target.
        /// </summary>
        private Vector3 getTargetDirectionFrom(Vector3 origin)
        {
            var value = Target - origin;

            if (value.magnitude > float.Epsilon)
                value.Normalize();

            return value;
        }

        /// <summary>
        /// Clears recoil impulses so that gun is stable when brought back to use again.
        /// </summary>
        private void OnDisable()
        {
            _recoilImpulses.Clear();
        }

        /// <summary>
        /// Sets Clip to be ClipSize.
        /// </summary>
        public void Reload()
        {
            Clip = ClipSize;
        }

        /// <summary>
        /// Sets the fire mode on. It stays on until CancelFire() is called.
        /// Gun fires only when both fire mode is on and the gun is allowed to fire.
        /// </summary>
        public void Fire()
        {
            _isFiring = true;
        }

        /// <summary>
        /// Sets the fire mode off.
        /// </summary>
        public void CancelFire()
        {
            _isFiring = false;
        }

        /// <summary>
        /// Sets whether the gun is allowed to fire. Manipulated when changing weapons or a reload animation is playing.
        /// </summary>
        /// <param name="value"></param>
        public void Allow(bool value)
        {
            _isAllowed = value;
        }

        /// <summary>
        /// Origin to cast bullets from.
        /// </summary>
        private Vector3 raycastOrigin
        {
            get { return _isUsingCustomRaycastOrigin ? _customRaycastOrigin : Origin; }
        }

        /// <summary>
        /// Sets the position from which bullets are spawned. The game usually sets it as the camera position.
        /// </summary>
        public void SetFireFrom(Vector3 point)
        {
            _isUsingCustomRaycastOrigin = true;
            _customRaycastOrigin = point;
        }

        /// <summary>
        /// Stop using the firing origin set in SetFireFrom() and default to firing from the Aim object.
        /// </summary>
        public void StopFiringFromCustom()
        {
            _isUsingCustomRaycastOrigin = false;
        }

        /// <summary>
        /// Set the aim origin when manipulated by IK.
        /// </summary>
        public void UpdateAimOrigin()
        {
            _lastAimOrigin = Aim == null ? transform.position : Aim.transform.position;
        }

        /// <summary>
        /// Sets the forward vector of the gun before moving it by IK. The difference after manipulation is used to calculate recoil direction.
        /// </summary>
        public void UpdateIntendedRotation()
        {
            _intendedForward = transform.forward;
        }

        private void LateUpdate()
        {
            // Notify character if the trigger is pressed. Used to make faces.
            {
                var isAllowedAndFiring = _isFiring && _isAllowed;

                if (Character != null)
                {
                    if (isAllowedAndFiring && !_wasAllowedAndFiring) Character.SendMessage("OnStartGunFire", SendMessageOptions.DontRequireReceiver);
                    if (!isAllowedAndFiring && _wasAllowedAndFiring) Character.SendMessage("OnStopGunFire", SendMessageOptions.DontRequireReceiver);
                }

                _wasAllowedAndFiring = isAllowedAndFiring;
            }

            // Update recoil.
            {
                // Starts from 1 when firing a bullet and decays to 0.
                _recoilIntensity -= Time.deltaTime * 10;

                // Decay the recoil.

                if (RecoilSettings.DecayTime <= float.Epsilon)
                {
                    // If DecayTime is zero or less clear _recoil immediately.

                    _recoil = Vector3.zero;
                }
                else
                {
                    var recoilLeft = _recoil.magnitude;

                    if (recoilLeft > float.Epsilon)
                    {
                        var value = _recoil / recoilLeft;
                        var decrease = RecoilSettings.Strength * Time.deltaTime / RecoilSettings.DecayTime;

                        if (decrease >= recoilLeft)
                            _recoil = Vector3.zero;
                        else
                            _recoil -= value * decrease;
                    }
                }

                // Sum all currently acting recoil impulses and add it to the recoil shift.

                if (RecoilSettings.AttackTime <= float.Epsilon)
                {
                    // If AttackTime is zero or leso sum up all impulses immediately.

                    foreach (var recoil in _recoilImpulses)
                        _recoil += recoil.Direction * RecoilSettings.Strength;

                    _recoilImpulses.Clear();
                }
                else
                    for (int index = _recoilImpulses.Count - 1; index >= 0; index--)
                    {
                        var recoil = _recoilImpulses[index];
                        recoil.Progress += Time.deltaTime / RecoilSettings.AttackTime;

                        if (recoil.Progress >= 1)
                            _recoilImpulses.RemoveAt(index);
                        else
                        {
                            _recoilImpulses[index] = recoil;
                            _recoil += recoil.Direction * RecoilSettings.Strength * Mathf.Clamp01(Time.deltaTime / RecoilSettings.AttackTime);
                        }
                    }

                if (_recoil.magnitude > RecoilSettings.Limit)
                    _recoil = _recoil.normalized * RecoilSettings.Limit;
            }

            _fireWait -= Time.deltaTime;

            // Check if the trigger is pressed.
            if (_isFiring && _isAllowed)
            {
                // Time in seconds between bullets.
                var fireDelay = 1.0f / Rate;

                var delay = 0f;

                if (_fireWait < 0.5f * fireDelay && fireDelay < RecoilSettings.DecayTime + RecoilSettings.AttackTime)
                    _recoilIntensity += Time.deltaTime * 20;

                // Fire all bullets in this frame.
                while (_fireWait < 0)
                {
                    if (!IsClipEmpty)
                    {
                        SendMessage("OnFire", delay, SendMessageOptions.DontRequireReceiver);

                        _recoilImpulses.Add(new RecoilImpulse(Vector3.Lerp(-Direction, Vector3.up, RecoilSettings.UpForce)));

                        raycast();

                        Clip--;
                    }

                    delay += fireDelay;
                    _fireWait += fireDelay;
                    _isFiring = false;
                }
            }

            // Clamp recoil intensity to 0 and 1.
            _recoilIntensity = Mathf.Clamp01(_recoilIntensity);

            // Clamp fire delay timer.
            if (_fireWait < 0) _fireWait = 0;
        }

        /// <summary>
        /// Cast a single bullet using raycasting.
        /// </summary>
        private void raycast()
        {
            RaycastHit closestHit = new RaycastHit();
			RaycastHit hit_1 = new RaycastHit();
            float closestDistance = Distance * 10;

            var minDistance = 0f;

            if (_isUsingCustomRaycastOrigin)
                minDistance = Vector3.Distance(Origin, raycastOrigin);

//			for (int i = 0; i < Physics.RaycastNonAlloc(raycastOrigin, getRecoiledDirectionFrom(raycastOrigin), _hits, Distance); i++)
			RaycastHit[] hits_2 = new RaycastHit[1];
			if (Physics.Raycast (raycastOrigin, getRecoiledDirectionFrom (raycastOrigin), out hits_2 [0], Distance, RayLayer)) 
			{
				_hits = new RaycastHit[hits_2.Length];
				_hits = hits_2;
				for (int i = 0; i < 1; i++) {
					var hit = _hits [i];

					if (hit.collider.gameObject != Character && hit.distance < closestDistance && !hit.collider.isTrigger && hit.distance > minDistance) {
						closestHit = hit;
						closestDistance = hit.distance;
					}
				}
			}

            if (closestHit.collider != null)
            {
                closestHit.collider.SendMessage("OnHit",
                                                new Hit(closestHit.point, -Direction, Damage, Character),
                                                SendMessageOptions.DontRequireReceiver);

                Debug.DrawLine(raycastOrigin, closestHit.point, Color.yellow, 0.5f);
            }
            else
                Debug.DrawRay(raycastOrigin, Distance * Direction, Color.red, 0.5f);
        }
    }
}