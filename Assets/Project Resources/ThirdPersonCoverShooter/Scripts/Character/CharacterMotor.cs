using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Manages the character, it’s movement, appearance and use of weapons.
    /// </summary>
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMotor : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// Is character currently climbing.
        /// </summary>
        public bool IsClimbing
        {
            get { return _isClimbing; }
        }

        /// <summary>
        /// Is the character currently in cover.
        /// </summary>
        public bool IsInCover
        {
            get { return _cover.In && !_isAimingBackInTallCover; }
        }

        /// <summary>
        /// Is the character currently crouching.
        /// </summary>
        public bool IsCrouching
        {
            get { return _isCrouching; }
        }

        /// <summary>
        /// Degrees in world space of direction the character is intended to face.
        /// </summary>
        public float LookAngle
        {
            get { return _horizontalLookAngle; }
        }

        /// <summary>
        /// Is the character currently facing left in a cover.
        /// </summary>
        public bool IsStandingLeftInCover
        {
            get { return _cover.In && _cover.IsStandingLeft; }
        }

        /// <summary>
        /// Is the character currently in tall cover.
        /// </summary>
        public bool IsInTallCover
        {
            get { return _cover.In && _cover.IsTall; }
        }

        /// <summary>
        /// Is the character or camera currently facing away from the cover.
        /// </summary>
        public bool IsAimingBackFromCover
        {
            get
            {
                return _cover.In &&
                       (_isAimingBackInTallCover ||
                        !_cover.IsFront(_horizontalLookAngle, (_wasAimingBackFromCover ? 1f : -1f) +
                            (_cover.IsStandingLeft ? CoverSettings.Angles.LowerAim.Left : CoverSettings.Angles.LowerAim.Right)));
            }
        }

        /// <summary>
        /// Is currently aiming from a cover.
        /// </summary>
        public bool IsCornerAiming
        {
            get
            {
                if (!_isInCornerAimState || _isLeavingCornerAimBecauseAngle)
                    return false;

                if (!_isFalling && !_isClimbing && IsGunReady && (!_gun.IsClipEmpty || !_cover.In))
                {
                    if (_coverAim.IsAiming || _isAimingBackInTallCover)
                        return true;
                    else if (!_cover.In && _currentWeapon > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Gun component of the currently used weapon.
        /// </summary>
        public Gun Gun
        {
            get { return _gun; }
        }

        /// <summary>
        /// Is the gun not being changed or reloaded.
        /// </summary>
        public bool IsGunReady
        {
            get
            {
                return _currentWeapon > 0 && _gun != null && !_isChangingWeapon && !_isReloading;
            }
        }

        /// <summary>
        /// Is the character currently using zoom.
        /// </summary>
        public bool IsZoomed
        {
            get { return _coverAim.IsZoomed && !_isClimbing && _isGrounded && (!_cover.IsTall || _coverAim.IsAiming) && IsGunReady; }
        }

        /// <summary>
        /// Is the character currently in cover and standing near the left corner.
        /// </summary>
        public bool IsNearLeftCorner
        {
            get
            {
                if (_isInCornerAimState && _lastWalkedCoverDirection < 0)
                    return true;

                return _cover.In &&
                       _cover.HasLeftCorner &&
                       _cover.IsByLeftCorner(transform.position, CoverSettings.CornerAimTriggerDistance);
            }
        }

        /// <summary>
        /// Is the character currently in cover and standing near the right corner.
        /// </summary>
        public bool IsNearRightCorner
        {
            get
            {
                if (_isInCornerAimState && _lastWalkedCoverDirection > 0)
                    return true;

                return _cover.In &&
                       _cover.HasRightCorner &&
                       _cover.IsByRightCorner(transform.position, CoverSettings.CornerAimTriggerDistance);
            }
        }

        /// <summary>
        /// Is the character currently in a state where it can aim from the left corner.
        /// </summary>
        public bool CanPeekLeftCorner
        {
            get
            {
                if (_isInCornerAimState && _lastWalkedCoverDirection < 0)
                    return true;

                return !_wasIntendingToFire &&
                       IsNearLeftCorner &&
                       !_isAimingBackInTallCover &&
                       _cover.IsLeft(_horizontalLookAngle, CoverSettings.Angles.LeftCorner, _wasAbleToPeekCorner) &&
                       _cover.IsFrontField(_horizontalLookAngle, CoverSettings.Angles.Front) &&
                       _lastWalkedCoverDirection < 0;
            }
        }

        /// <summary>
        /// Is the character currently in a state where it can aim from the right corner.
        /// </summary>
        public bool CanPeekRightCorner
        {
            get
            {
                if (_isInCornerAimState && _lastWalkedCoverDirection > 0)
                    return true;

                return !_wasIntendingToFire &&
                       IsNearRightCorner &&
                       !_isAimingBackInTallCover &&
                       _cover.IsRight(_horizontalLookAngle, CoverSettings.Angles.RightCorner, _wasAbleToPeekCorner) &&
                       _cover.IsFrontField(_horizontalLookAngle, CoverSettings.Angles.Front) &&
                       _lastWalkedCoverDirection > 0;
            }
        }

        /// <summary>
        /// Is the camera looking away enough from the cover axis for the character to aim at a wall.
        /// </summary>
        public bool CanWallAim
        {
            get
            {
                return !_cover.IsFront(_horizontalLookAngle, CoverSettings.Angles.TallWallAim);
            }
        }

        /// <summary>
        /// Position of the currently held gun where bullets would appear. 
        /// </summary>
        public Vector3 GunOrigin
        {
            get { return _gun == null ? transform.position : _gun.Origin; }
        }

        /// <summary>
        /// Direction of the gun affected by recoil.
        /// </summary>
        public Vector3 GunDirection
        {
            get { return _gun == null ? transform.forward : _gun.Direction; }
        }

        /// <summary>
        /// Direction of the gun going straight towards the set target.
        /// </summary>
        public Vector3 GunTargetDirection
        {
            get { return _gun == null ? transform.forward : _gun.TargetDirection; }
        }

        /// <summary>
        /// Position of the top of the capsule.
        /// </summary>
        public Vector3 Top
        {
            get { return transform.position + Vector3.up * _defaultCapsuleHeight; }
        }

        /// <summary>
        /// Current intensity of recoil affecting a gun. In range of 0 to 1.
        /// </summary>
        public float RecoilIntensity
        {
            get { return _gun == null ? 0 : _gun.RecoilIntensity; }
        }

        /// <summary>
        /// Is aiming or intending to aim.
        /// </summary>
        public bool IsIntendingToAim
        {
            get
            {
                if (!_isFalling && !_isClimbing && _gun != null &&
                    (!_cover.In || _isLeavingCornerAimBecauseAngle || !_isInCornerAimState || (!_isLeavingCornerAim && _normalizedCornerAim > 0.2f)))
                {
                    if (_coverAim.IsAiming || _isAimingBackInTallCover)
                        return true;
                    else if (!_cover.In && _currentWeapon > 0)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Is currently aiming.
        /// </summary>
        public bool IsAiming
        {
            get { return IsGunReady && (!_gun.IsClipEmpty || !_cover.In) && IsIntendingToAim; }
        }

        /// <summary>
        /// Was there any intended movement in cover.
        /// </summary>
        public bool IsWalkingInCover
        {
            get { return _wasMovingInCover; }
        }

        #endregion

		#region My Feilds
		public bool IsAI;

		#endregion My Feilds

        #region Public fields

        /// <summary>
        /// Controls wheter the character is in a state of death. Dead characters have no collisions and ignore any input.
        /// </summary>
        [Tooltip("Controls wheter the character is in a state of death.")]
        public bool IsAlive = true;

        /// <summary>
        /// Distance below feet to check for ground.
        /// </summary>
        [Tooltip("Distance below feet to check for ground.")]
        [Range(0, 1)]
        public float GroundThreshold = 0.3f;

        /// <summary>
        /// Minimal height to trigger state of falling. It’s ignored when jumping over gaps.
        /// </summary>
        [Tooltip("Minimal height to trigger state of falling. It’s ignored when jumping over gaps.")]
        [Range(0, 10)]
        public float FallThreshold = 2.0f;

        /// <summary>
        /// How quickly the character model is orientated towards the running direction.
        /// </summary>
        [Tooltip("How quickly the character model is orientated towards the running direction.")]
        [Range(0, 50)]
        public float RunningRotationSpeed = 5.0f;

        /// <summary>
        /// Movement to obstacles closer than this is ignored. 
        /// It is mainly used to prevent character running into walls.
        /// Not used when in cover.
        /// </summary>
        [Tooltip("Movement to obstacles closer than this is ignored. Not used when in cover.")]
        [Range(0, 2)]
        public float ObstacleDistance = 0.3f;

        /// <summary>
        /// Gravity force applied to this character.
        /// </summary>
        [Tooltip("Gravity force applied to this character.")]
        public float Gravity = 10;

        /// <summary>
        /// Extra rotation applied to the character when standing still. 
        /// It is used in addition to rotation from animations.
        /// </summary>
        [Tooltip("Extra rotation applied to the character when standing still. ")]
        [Range(0, 10)]
        public float ExtraTurnSpeed = 1f;

        /// <summary>
        /// Controls whether weapons are aimed at a point in a world (on) or at an infinite distance (off). 
        /// You want this turned off for player characters so that guns are stable and at the crosshair.
        /// </summary>
        [Tooltip("Controls whether weapons are aimed at a point in a world (on) or at an infinite distance (off). ")]
        public bool IsAimingPrecisely = false;

        /// <summary>
        /// Descriptions of currently held weapons.
        /// </summary>
        [Tooltip("Descriptions of currently held weapons.")]
        public WeaponDescription[] Weapons;

        /// <summary>
        /// IK settings for the character.
        /// </summary>
        [Tooltip("IK settings for the character.")]
        public IKSettings IK;

        /// <summary>
        /// Settings for cover behaviour.
        /// </summary>
        [Tooltip("Settings for cover behaviour.")]
        public CoverSettings CoverSettings = CoverSettings.Default();

        /// <summary>
        /// Settings for climbing.
        /// </summary>
        [Tooltip("Settings for climbing.")]
        public ClimbSettings ClimbSettings = ClimbSettings.Default();

        /// <summary>
        /// Settings for jumping.
        /// </summary>
        [Tooltip("Settings for jumping.")]
        public JumpSettings JumpSettings = JumpSettings.Default();

        #endregion

        #region Private fields

        private CoverState _cover;

        private bool _isGrounded = true;
        private bool _wasGrounded;
        private bool _isFalling;

        private Vector3 _lookTarget;
        private Vector3 _fireTarget;
        private Vector3 _currentFireTarget;

        private float _horizontalLookAngle;
        private float _verticalLookAngle;
        private float _lookAngleDiff;

		public int _currentWeapon = 0;
        private bool _wasMovingInCover;

        private CoverAimState _coverAim;

        private float _leftHandIntensity = 0;
        private float _armAimIntensity = 0;
        private float _headAimIntensity = 0;
        private float _aimPivotIntensity = 0;

        private bool _isChangingWeapon = false;
        private Gun _gun;

        private Cover _ignoreCoverUntilWalkedInAgain;

        private float _movementInput = 0;

        private int _lastWalkedCoverDirection = 0;

        private float _coverTime = 0;

        private bool _useGravity = true;
        private bool _isClimbing = false;
        private bool _hasBeganClimbing = false;
        private bool _isClimbingAVault = false;
        private float _climbHeight = 0;
        private float _climbAngle = 0;
        private float _climbTime = 0;
        private float _ignoreFallTimer = 0;
        private float _normalizedClimbTime = 0;

        private bool _isReloading = false;
        private bool _hasBeganReloading = false;
        private float _reloadTime = 0;
        private float _normalizedReloadTime = 0;

        private bool _isAimingBackInTallCover = false;

        private bool _wasIntendingToFire = false;

        private bool _isJumping = false;
        private bool _wantsToJump = false;
        private float _nextJumpTimer = 0;
        private float _jumpLegTimer = 0;
        private float _jumpTimer = 0;

        private float _defaultCapsuleHeight = 2.0f;
        private float _defaultCapsuleCenter = 1.0f;

        private float _leftMoveIntensity = 1;
        private float _rightMoveIntensity = 1;
        private float _backMoveIntensity = 1;
        private float _frontMoveIntensity = 1;

        private IK _aimSolver = new IK();
        private CoverSearch _coverSearch = new CoverSearch();

        private Vector3 _localMovement = new Vector3(0, 0, 0);

        private bool _isCrouching = false;
        private bool _wantsToCrouch = false;
        private CharacterMovement _inputMovement;
        private CharacterMovement _currentMovement;
        private bool _isSpaceDown;
        private bool _wantsToZoom;
        private bool _wantsToFire;

        private Quaternion _lastHit = Quaternion.identity;
        private float _lastHitStrength;

        private int _lastFoot;

        private float _coverDelay;

        private bool _isInCornerAimState;
        private bool _isLeavingCornerAim;
        private bool _isLeavingCornerAimBecauseAngle;
        private float _normalizedCornerAim;
        private bool _ignoreCornerUntilFreshlyAble;
        private bool _isAnimatingCornerAim;
        private bool _isCornerAiming;
        private Vector3 _cornerAimStart;

        private Vector3 _positionToSnap;
        private Vector3 _positionToSnapStart;
        private float _positionSnapTimer;
        private float _positionSnapTimerStart;

        private bool _wasAbleToPeekCorner;

        private bool _wasAimingBackFromCover;

        private float _directionChangeDelay;

        private float _tallCoverTimer = 0;

        private float _oppositeTallBackAimTimer = 0;

        private bool _isExitingLeftCorner = false;
        private bool _isExitingRightCorner = false;
        private float _cornerExitTimer = 0f;
        private float _cornerExitForwardAngle;

        private bool _wasAlive = true;
        private bool _wasZoomed;

        private Transform _lastAimTransform;

        private Collider[] _colliderCache = new Collider[16];
        private RaycastHit[] _raycastHits = new RaycastHit[16];

        private WeaponAnimationStates[] _weaponStates = WeaponAnimationStates.Default();

        #endregion

        #region Public methods

        /// <summary>
        /// Sets the position for the character to look and aim at.
        /// </summary>
        public void SetLookTarget(Vector3 target)
        {
            if (_gun != null)
            {
                var vector = target - _gun.transform.position;
                var distance = vector.magnitude;

                if (distance < 2 && distance > 0.01f)
                    _lookTarget = _gun.transform.position + vector.normalized * 2;
                else
                    _lookTarget = target;
            }
            else
                _lookTarget = target;

            calculateTurn();
        }

        /// <summary>
        /// Sets the position to raycast bullets from.
        /// </summary>
        /// <param name="position"></param>
        public void FireFrom(Vector3 position)
        {
            if (_gun != null)
                _gun.SetFireFrom(position);
        }

        /// <summary>
        /// Sets the position to fire bullets at.
        /// </summary>
        /// <param name="target"></param>
        public void SetFireTarget(Vector3 target)
        {
            _fireTarget = target;
        }

        #endregion

        #region Events
		public LayerMask None;
		public void Sinkit()
		{
			this.gameObject.layer = None;
		}
        /// <summary>
        /// Sets IsAlive to false upon the character death.
        /// </summary>
        public void OnDead()
        {
			if (!IsAlive)
				return;
            IsAlive = false;
			if (GetComponent<AIController> ()) 
			{
				ObjectiveHandler.instance.EnemyKilled ();
				GetComponent<bl_MiniMapItem> ().HideItem ();
				Invoke ("Sinkit",3f);
				Destroy (gameObject,5f);
			}
			else if(GetComponent<PlayerController>())
				GameManager_GJ11.instance.OnFailAfter(3);
        }

		public void ReSpawn()
		{
			GetComponent<CharacterHealth> ().Health = GetComponent<CharacterHealth> ().MaxHealth;
			IsAlive = true;
		}

        /// <summary>
        /// Affects the character spine by a bullet hit.
        /// </summary>
        public void OnHit(Hit hit)
        {
            if (IK.HitBone == null)
                return;

            var forwardDot = Vector3.Dot(-IK.HitBone.transform.forward, hit.Normal);
            var rightDot = Vector3.Dot(IK.HitBone.transform.right, hit.Normal);

            _lastHit = Quaternion.Euler(forwardDot * 40, 0, rightDot * 40);
            _lastHitStrength = 1.0f;
        }

        /// <summary>
        /// Rotation of the pivot for the camera.
        /// </summary>
        public Quaternion GetPivotRotation()
        {
            Quaternion body;

            if (_cover.In && !_isAimingBackInTallCover && !IsZoomed)
                body = Quaternion.AngleAxis(_cover.ForwardAngle + (_cover.IsFront(_horizontalLookAngle) ? 0 : 180), Vector3.up);
            else
                body = transform.rotation;

            return Quaternion.Lerp(body, body * Quaternion.Euler(0, _horizontalLookAngle - transform.eulerAngles.y, 0), _aimPivotIntensity);
        }

        /// <summary>
        /// Position of the pivot for the camera.
        /// </summary>
        public Vector3 GetPivotPosition()
        {
            if (_cover.In && !_isAimingBackInTallCover && !IsZoomed)
            {
                float margin;

                if (_cover.IsStandingLeft)
                {
                    if (_cover.HasLeftAdjacent)
                        margin = CoverSettings.CornerAimTriggerDistance;
                    else
                        margin = -CoverSettings.PivotSideMargin;
                }
                else
                {
                    if (_cover.HasRightAdjacent)
                        margin = CoverSettings.CornerAimTriggerDistance;
                    else
                        margin = -CoverSettings.PivotSideMargin;
                }

                if (_cover.Width > -margin * 2 + float.Epsilon)
                {
                    var left = _cover.LeftSide(margin) - _cover.ForwardDirection * CoverSettings.EnterDistance;
                    var right = _cover.RightSide(margin) - _cover.ForwardDirection * CoverSettings.EnterDistance;

                    left.y = transform.position.y;
                    right.y = transform.position.y;

                    return Util.FindClosestToPath(left, right, transform.position);
                }
                else
                {
                    var pos = _cover.Main.transform.position - _cover.ForwardDirection * CoverSettings.EnterDistance;
                    pos.y = transform.position.y;
                    return pos;
                }
            }
            else
                return transform.position;
        }

        #endregion

        #region Input

        /// <summary>
        /// Inputs a command to either jump or take cover if it is not automatic.
        /// </summary>
        public void InputSpace()
        {
            _isSpaceDown = true;
        }

        /// <summary>
        /// Sets the character movement for the next update.
        /// </summary>
        /// <param name="movement"></param>
        public void InputMovement(CharacterMovement movement)
        {
            _inputMovement = movement;
        }

        /// <summary>
        /// Sets the character crouching state for the next update.
        /// </summary>
        public void InputCrouch()
        {
            _wantsToCrouch = true;
        }

        /// <summary>
        /// Sets the character zoom state for the next update.
        /// </summary>
        public void InputZoom()
        {
            _wantsToZoom = true;
        }

        /// <summary>
        /// Sets the character state of firing for the next update.
        /// </summary>
        public void InputFire()
        {
            _wantsToFire = true;
        }

        /// <summary>
        /// Attempts to start reloading a gun.
        /// </summary>
        public void InputReload()
        {
            startReload();
        }

        /// <summary>
        /// Sets the currently held weapon.
        /// </summary>
        /// <param name="id">Index of the weapon based on 1. Value of 0 hides weapons.</param>
        public void InputWeapon(int id)
        {
            if (_currentWeapon != id && _isGrounded)
            {
                _isChangingWeapon = true;
                _currentWeapon = id;
            }
        }

        #endregion

        #region Behaviour

        private void Awake()
        {
            var capsule = GetComponent<CapsuleCollider>();
            _defaultCapsuleHeight = capsule.height;
            _defaultCapsuleCenter = capsule.center.y;

            SetLookTarget(transform.position + transform.forward * 1000);
            SetFireTarget(transform.position + transform.forward * 1000);
		
		
			// my custom
			IsAI = GetComponent<AIController>() != null ;
        }

        private void LateUpdate()
        {
			if (GameManager_GJ11.instance.State == GameStates.Paused)
				return;
            if (IsAlive)
            {
                _isCrouching = _wantsToCrouch;

                {
                    var distance = Vector3.Distance(_fireTarget, Top);
                    var dir = Vector3.Lerp((_currentFireTarget - Top).normalized, (_fireTarget - Top).normalized, Time.deltaTime * 16);

                    _currentFireTarget = Top + dir * distance;
                }

                calculateTurn();

                if (_useGravity && IsAlive && !_isClimbing)
                    GetComponent<Rigidbody>().velocity -= new Vector3(0, Gravity, 0) * Time.deltaTime;

                updateHeadAimIntennsity();
                updateArmAimIntennsity();
                updateLeftHandIntensity();
                updateAimPivotIntensity();
                updateGuns();
                updateReload();

                if (_isClimbing)
                    updateClimb();
                else if (_isInCornerAimState)
                    updateCornerAim();
                else
                    updateCommon();

                updateIK();
            }
            else
            {
                _isCrouching = false;

                GetComponent<Rigidbody>().velocity = Vector3.zero;
                updateGround();
            }

            updateCapsule();
            updateAnimator();

            _isSpaceDown = false;
            _inputMovement = new CharacterMovement();
            _wantsToCrouch = false;
            _wantsToFire = false;
            _wantsToZoom = false;
            _wasAbleToPeekCorner = !_isClimbing && (CanPeekLeftCorner || CanPeekRightCorner);
            _wasAimingBackFromCover = IsAimingBackFromCover;

            {
                var isZoomed = IsZoomed;
                if (isZoomed && !_wasZoomed) SendMessage("OnZoom", SendMessageOptions.DontRequireReceiver);
                if (!isZoomed && _wasZoomed) SendMessage("OnUnzoom", SendMessageOptions.DontRequireReceiver);
                _wasZoomed = isZoomed;

                var isAlive = IsAlive;
                if (isAlive && !_wasAlive) SendMessage("OnAlive", SendMessageOptions.DontRequireReceiver);
                if (!isAlive && _wasAlive) SendMessage("OnDead", SendMessageOptions.DontRequireReceiver);
                _wasAlive = isAlive;
            }
        }

        #endregion

        #region Private methods

        private void snapToPosition(Vector3 value, float time)
        {
            _positionToSnap = value;
            _positionToSnapStart = transform.position;
            _positionSnapTimer = time;
            _positionSnapTimerStart = time;
        }

        private void calculateTurn()
        {
            var diff = _lookTarget - transform.position;

            if (diff.magnitude > 0)
            {
                diff.Normalize();
                _horizontalLookAngle = Mathf.Atan2(diff.x, diff.z) * 180f / Mathf.PI;
                _verticalLookAngle = Mathf.Asin(diff.y) * 180f / Mathf.PI;
            }
        }

        private void updateAimingBackInTallCover()
        {
			
            if (_cover.In && _cover.IsTall && _currentWeapon > 0)
            {
                if ((IsNearLeftCorner && _cover.IsLeft(_horizontalLookAngle)) ||
                    (IsNearRightCorner && _cover.IsRight(_horizontalLookAngle)))
                {
                    if (_isAimingBackInTallCover)
                        _isAimingBackInTallCover = !_cover.IsFront(_horizontalLookAngle, CoverSettings.Angles.CornerBackAimLeave);
                    else
                        _isAimingBackInTallCover = !_cover.IsFrontField(_horizontalLookAngle, CoverSettings.Angles.Front);
                }
                else
                {
                    if ((_cover.IsStandingLeft && _cover.IsLeft(_horizontalLookAngle)) ||
                        (_cover.IsStandingRight && _cover.IsRight(_horizontalLookAngle)))
                    {
                        _isAimingBackInTallCover = !_cover.IsFront(_horizontalLookAngle, CoverSettings.Angles.TallBack.Face);
                    }
                    else
                    {
                        _isAimingBackInTallCover = !_cover.IsFront(_horizontalLookAngle, CoverSettings.Angles.TallBack.Opposite);

                        if (_isAimingBackInTallCover)
                            _oppositeTallBackAimTimer = CoverSettings.Angles.TallBack.OppositeSustainTime;
                    }
                }
            }
            else
                _isAimingBackInTallCover = false;

            if (_oppositeTallBackAimTimer > 0)
            {
                _oppositeTallBackAimTimer -= Time.deltaTime;
                _isAimingBackInTallCover = true;
            }
        }

        private void updateCapsule()
        {
            var capsule = GetComponent<CapsuleCollider>();

            if (IsAlive)
            {
                capsule.enabled = true;

                var targetHeight = _defaultCapsuleHeight;

                if (_isClimbing && _normalizedClimbTime < 0.5f)
                    targetHeight = ClimbSettings.CapsuleHeight;
                else if (_isJumping && _jumpTimer < JumpSettings.HeightDuration)
                    targetHeight = JumpSettings.CapsuleHeight;
                else if (_cover.In && !_cover.IsTall && (!IsAiming || IsCornerAiming))
                    targetHeight = Mathf.Lerp(CoverSettings.LowCapsuleHeight, targetHeight, _cover.IsTall ? 1.0f : 0.0f);

                capsule.height = Mathf.Lerp(capsule.height, targetHeight, Time.deltaTime * 10);
                capsule.center = new Vector3(capsule.center.x, _defaultCapsuleCenter - (_defaultCapsuleHeight - capsule.height) * 0.5f, capsule.center.z);
            }
            else
                capsule.enabled = false;
        }

        private void updateArmAimIntennsity()
        {
            float targetIntensity = 0f;

            if (_isFalling || (_isClimbing && _normalizedClimbTime < 0.6f) || _isReloading)
                targetIntensity = 0;
            else if (!_cover.In)
            {
                if (_currentWeapon > 0 && !_isChangingWeapon)
                    targetIntensity = 1;
            }
            else if (IsAiming)
                targetIntensity = 1;

            if (_isClimbing || _ignoreFallTimer > 0)
                _armAimIntensity = Mathf.Lerp(_armAimIntensity, targetIntensity, Mathf.Clamp01(Time.deltaTime * 6));
            else if (_tallCoverTimer > 0 && targetIntensity > _armAimIntensity)
                _armAimIntensity = Mathf.Lerp(_armAimIntensity, targetIntensity, Mathf.Clamp01(Time.deltaTime * 5));
            else
                _armAimIntensity = Mathf.Lerp(_armAimIntensity, targetIntensity, Mathf.Clamp01(Time.deltaTime * 15));
        }

        private void updateLeftHandIntensity()
        {
            float targetIntensity = 0f;

            if (IsGunReady && !_isClimbing && !_isFalling)
                targetIntensity = 1f;

            _leftHandIntensity = Mathf.Lerp(_leftHandIntensity, targetIntensity, Mathf.Clamp01(Time.deltaTime * 15));
        }

        private void updateHeadAimIntennsity()
        {
            float targetIntensity = 0f;

            if (_isFalling || (_isClimbing && _normalizedClimbTime < 0.6f) || _isReloading)
                targetIntensity = 0;
            else if (!_cover.In)
            {
                if (_currentWeapon > 0)
                    targetIntensity = 1;
            }
            else if (IsAiming)
                targetIntensity = 1;

            if (targetIntensity > _headAimIntensity)
                _headAimIntensity = Mathf.Lerp(_headAimIntensity, targetIntensity, Time.deltaTime * 2);
            else
                _headAimIntensity = Mathf.Lerp(_headAimIntensity, targetIntensity, Time.deltaTime * 15);
        }

        private void updateAimPivotIntensity()
        {
            float targetIntensity = 0f;

            if (_isFalling || _isClimbing)
                targetIntensity = 0;
            else if (!_cover.In)
            {
                if (_currentWeapon > 0)
                    targetIntensity = 1;
            }
            else if (_coverAim.IsAiming)
                targetIntensity = 1;

            if (_isClimbing || _ignoreFallTimer > 0)
                _aimPivotIntensity = Mathf.Lerp(_aimPivotIntensity, targetIntensity, Mathf.Clamp01(Time.deltaTime * 4));
            else
                _aimPivotIntensity = Mathf.Lerp(_aimPivotIntensity, targetIntensity, Mathf.Clamp01(Time.deltaTime * 8));
        }

        private void updateReload()
        {
			
            if (_isReloading)
            {
                _reloadTime += Time.deltaTime;

                var animator = GetComponent<Animator>();
                var info = animator.GetCurrentAnimatorStateInfo(1);

                var isInState = false;

                foreach (var weapon in Weapons)
                    if (info.IsName(_weaponStates[(int)weapon.Type].Reload))
                        isInState = true;

                if (isInState)
                    _hasBeganReloading = true;
                else if (_hasBeganReloading)
                    _isReloading = false;

                if (_hasBeganReloading)
                {
                    if (info.normalizedTime >= 0.2f && _gun != null)
                        _gun.Reload();

                    if (info.normalizedTime > _normalizedReloadTime)
                        _normalizedReloadTime = info.normalizedTime;

                    if (_normalizedReloadTime > 0.8f)
                        _isReloading = false;
                }

                if (_reloadTime > 10)
                    _isReloading = false;
            }
            else
                _reloadTime = 0;
        }

        private void OnAnimatorMove()
        {
			if (GameManager_GJ11.instance.State == GameStates.Paused) 
			{
//				print ("paused");
				return;
			}

//			print ("running");

            var body = GetComponent<Rigidbody>();

            if (_positionSnapTimer > float.Epsilon)
            {
                body.velocity = Vector3.zero;
                transform.position = Vector3.Lerp(_positionToSnapStart, _positionToSnap, 1.0f - _positionSnapTimer / _positionSnapTimerStart);
				_positionSnapTimer -= Time.unscaledDeltaTime;
            }
            else
            {
                var animator = GetComponent<Animator>();

				if (Time.timeScale <= 0)
					return;
				var animatorMovement = animator.deltaPosition / Time.unscaledDeltaTime;
                var animatorSpeed = animatorMovement.magnitude;

                if (!IsAlive)
                {
                }
                else if (_isClimbing)
                {
                    if (_normalizedClimbTime >= ClimbSettings.FallTime)
                        animatorMovement.y -= Gravity * (_normalizedClimbTime - ClimbSettings.FallTime) / (1.0f - ClimbSettings.FallTime);

                    body.velocity = animatorMovement;

                    var turnIntensity = Mathf.Clamp01(_normalizedClimbTime - 0.6f) * (1.0f - 0.6f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, Mathf.LerpAngle(_climbAngle, _horizontalLookAngle, turnIntensity), 0), Mathf.Clamp01(Time.deltaTime * 20));
                }
                else if (_isInCornerAimState)
                {
                    transform.Rotate(0, animator.deltaRotation.eulerAngles.y, 0);
                    body.velocity = animatorMovement;

                    if (_isLeavingCornerAim && _normalizedCornerAim < 0.4f)
                        snapToPosition(_cornerAimStart, 0.1f);
                }
                else if (!_isGrounded || _isJumping || _wantsToJump)
                {
					var turn = _lookAngleDiff * Time.unscaledDeltaTime * RunningRotationSpeed;
                    transform.Rotate(0, turn, 0);
                }
                else
                {
                    float minThreshold = _currentMovement.IsMoving ? -1.0f : 0.25f;
                    float maxThreshold = 1.0f;

                    float manualSpeed;
                    float manualInfluence;

					float rootMovement = animator.deltaPosition.magnitude / Time.unscaledDeltaTime;

                    if (rootMovement >= minThreshold || IsAiming)
                    {
                        manualSpeed = _cover.In ? CoverSettings.RotationSpeed : RunningRotationSpeed;
                        manualInfluence = Mathf.Clamp01((rootMovement - minThreshold) / (maxThreshold - minThreshold));
                    }
                    else if (_cover.In)
                    {
                        manualSpeed = CoverSettings.RotationSpeed;
                        manualInfluence = 1.0f;
                    }
                    else
                    {
                        manualSpeed = 0.0f;
                        manualInfluence = 0.0f;
                    }

                    var turn = Mathf.LerpAngle(animator.deltaRotation.eulerAngles.y,
							_lookAngleDiff * Time.unscaledDeltaTime * manualSpeed,
                                               manualInfluence);

                   	transform.Rotate(0, turn, 0);

                    animatorMovement.y = body.velocity.y;

                    if (_cover.In)
                    {
                        if (animatorSpeed > float.Epsilon)
                            body.velocity = _currentMovement.Direction * Vector3.Dot(_currentMovement.Direction, animatorMovement / animatorSpeed) * animatorSpeed;
                        else
                            body.velocity = new Vector3(0, body.velocity.y, 0);
                    }
                    else
                        body.velocity = animatorMovement;
                }
            }
        }

        private bool wantsToAim
        {
            get { return _coverAim.IsZoomed || _wantsToFire; }
        }

        private void updateCornerAim()
        {
            if (!_isInCornerAimState)
                return;

            _currentMovement = new CharacterMovement();

            var animator = GetComponent<Animator>();

            if (!_isLeavingCornerAimBecauseAngle &&
                !_isLeavingCornerAim &&
                _isCornerAiming)
                if (_inputMovement.IsMoving ||
                    !_cover.IsFrontField(_horizontalLookAngle, CoverSettings.Angles.Front))
                {
                    _isInCornerAimState = false;
                    _ignoreCornerUntilFreshlyAble = true;
                    animator.SetBool("ExitCornerAim", true);
                    updateCover(true);
                    return;
                }

            updateAimingBackInTallCover();

            if (_isAimingBackInTallCover)
            {
                _isInCornerAimState = false;
                return;
            }

            var info = animator.GetCurrentAnimatorStateInfo(0);

            var to = Animator.StringToHash("To Corner Aim");
            var aim = Animator.StringToHash("Corner Aim");
            var from = Animator.StringToHash("From Corner Aim");

            var setToTrue = true;

            _isCornerAiming = false;

            if (info.shortNameHash == to)
                _normalizedCornerAim = info.normalizedTime;
            else if (info.shortNameHash == aim)
            {
                _normalizedCornerAim = 1.0f;
                _isCornerAiming = true;
            }
            else if (info.shortNameHash == from)
            {
                _normalizedCornerAim = 1.0f - info.normalizedTime;
                _isLeavingCornerAim = true;
            }
            else
            {
                if (_isAnimatingCornerAim)
                {
                    if (wantsToAim && (CanPeekLeftCorner || CanPeekRightCorner) && canFire && !_isLeavingCornerAimBecauseAngle)
                    {
                        startCornerAim();
                        return;
                    }
                    else
                        _isInCornerAimState = false;
                }

                setToTrue = false;
                _normalizedCornerAim = 0;
            }

            if (!_isLeavingCornerAimBecauseAngle)
                if ((_cover.IsStandingLeft && !_cover.IsLeft(_horizontalLookAngle, CoverSettings.Angles.LeftCorner, true)) ||
                    (_cover.IsStandingRight && !_cover.IsRight(_horizontalLookAngle, CoverSettings.Angles.RightCorner, true)))
                    _isLeavingCornerAimBecauseAngle = true;

            if (_isLeavingCornerAimBecauseAngle && _cover.IsFrontField(_horizontalLookAngle, CoverSettings.Angles.Front))
                _coverAim.ImmediateLeave();

            if (setToTrue)
                _isAnimatingCornerAim = true;

            updateAim();

            if (!wantsToAim || !canFire)
            {
                if (_isCornerAiming)
                    _coverAim.Leave();
            }
            else if (!_isLeavingCornerAim && !_isLeavingCornerAimBecauseAngle)
            {
                if (_wantsToFire)
                    fire();
            }

            updateFireIntention();
        }

        private bool canFire
        {
            get
            {
                return !_isReloading && !_isChangingWeapon && IsGunReady && !_gun.IsClipEmpty;
            }
        }

        private void startCornerAim()
        {
            _isInCornerAimState = true;
            _isAnimatingCornerAim = false;
            _isLeavingCornerAim = false;
            _isLeavingCornerAimBecauseAngle = false;
            _normalizedCornerAim = 0;
            _lastWalkedCoverDirection = _cover.Direction;
            _cornerAimStart = transform.position;
            _coverAim.CoverAim(_horizontalLookAngle);
        }

        private void updateClimb()
        {
            if (_isClimbing)
            {
                _climbTime += Time.deltaTime;

                var animator = GetComponent<Animator>();

                var info = animator.GetCurrentAnimatorStateInfo(0);
                var vault = Animator.StringToHash("Vault");
                var climb = Animator.StringToHash("Climb");

                if (info.shortNameHash == climb ||
                    info.shortNameHash == vault)
                {
                    var time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

                    if (time > _normalizedClimbTime)
                        _normalizedClimbTime = time;

                    _hasBeganClimbing = true;
                }
                else if (_hasBeganClimbing)
                    _isClimbing = false;

                if (_climbTime > 5)
                    _isClimbing = false;

                _isGrounded = true;
                _isFalling = false;
                _isJumping = false;
                _nextJumpTimer = 0;
                _ignoreFallTimer = 0.2f;
            }
            else
                _climbTime = 0;

            updateLookAngleDiff();
            updateGround();
        }

        private void startReload()
        {
            if (_isReloading || _gun == null)
                return;

            _isReloading = true;
            _hasBeganReloading = false;
            _reloadTime = 0;
            _normalizedReloadTime = 0;

            _gun.SendMessage("OnReloadStart", SendMessageOptions.DontRequireReceiver);
        }

        private bool startClimb(Cover cover)
        {
            if (!_isClimbing)
            {
                _climbHeight = cover.Top - transform.position.y;

                if (_climbHeight <= 1.1f)
                {
                    _normalizedClimbTime = 0;
                    _isClimbing = true;
                    _isClimbingAVault = cover.IsVault;
                    _hasBeganClimbing = false;
                    _climbTime = 0;
                    _climbAngle = cover.Angle;
                    _ignoreCoverUntilWalkedInAgain = _cover.Main;
                    _cover.Clear();

                    return true;
                }
            }

            return false;
        }

        private bool canClimb()
        {
            var left = _cover.LeftSide(0);
            var right = _cover.RightSide(0);
            var delta = Util.FindDeltaPath(left, right, transform.position);

            return delta >= 0 && delta <= 1 && _cover.IsFrontField(_horizontalLookAngle, CoverSettings.Angles.Front);
        }

        private void updateCover(bool isForcedToMaintain)
        {
            var wasNearLeftCorner = false;
            var wasNearRightCorner = false;

            if (_cover.In)
            {
                wasNearLeftCorner = IsNearLeftCorner;
                wasNearRightCorner = IsNearRightCorner;
                _cornerExitForwardAngle = _cover.ForwardAngle;

                _coverTime += Time.deltaTime;
                _tallCoverTimer = _cover.IsTall ? 1.0f : 0.0f;

                _isExitingLeftCorner = false;
                _isExitingRightCorner = false;
            }
            else
            {
                _coverTime = 0;

                if (_tallCoverTimer > 0)
                    _tallCoverTimer -= Time.deltaTime;

                if (_cornerExitTimer > 0)
                    _cornerExitTimer -= Time.deltaTime;
                else
                {
                    _isExitingLeftCorner = false;
                    _isExitingRightCorner = false;
                }
            }

            var searchRadius = CoverSettings.EnterDistance + CoverSettings.CornerAimTriggerDistance;

            var capsule = GetComponent<CapsuleCollider>();
            var head = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).position;

            _coverSearch.Update(_cover,
                                transform.position,
                                head,
                                CoverSettings.TallThreshold,
                                searchRadius,
                                CoverSettings.LowSideRadius,
                                CoverSettings.TallSideRadius,
                                capsule.radius,
                                CoverSettings.LeaveDistance,
                                CoverSettings.EnterDistance,
                                CoverSettings.CornerAimTriggerDistance,
								CoverSettings.Cover);

            if (_isGrounded && (isForcedToMaintain || _inputMovement.IsMoving || !IsAiming))
                _cover.Maintain(_coverSearch, transform.position, CoverSettings.TallThreshold);

            if (_cover.In)
            {
                _coverDelay = 0;

                if (canClimb() && Input.GetKey(KeyCode.W) && _isSpaceDown)
                {
                    _isSpaceDown = false;
                    startClimb(_cover.Main);
                }
                else if (_isSpaceDown)
                {
                    _isSpaceDown = false;
                    leaveCover();
                }
            }
            else if (!_isClimbing && _isGrounded)
            {
                var isNewCover = _cover.Take(_coverSearch, transform.position, CoverSettings.TallThreshold);

                if (isNewCover)
                {
                    if (!_cover.IsFrontField(_horizontalLookAngle, CoverSettings.Angles.Front))
                    {
                        if (_coverDelay < CoverSettings.BackDelay)
                        {
                            _cover.Clear();
                            _coverDelay += Time.deltaTime;
                        }
                        else
                            _coverDelay = 0;
                    }
                    else
                        _coverDelay = 0;

                    if (_cover.IsLeft(_horizontalLookAngle))
                        _cover.StandLeft();
                    else
                        _cover.StandRight();

                    _lastWalkedCoverDirection = _cover.Direction;
                    instantCoverAnimatorUpdate();
                }
                else
                    _coverDelay = 0;

                if (_cover.IsTall && !_cover.IsFront(_horizontalLookAngle))
                    _cover.Clear();
                else if (_cover.In && canClimb() && Input.GetKey(KeyCode.W) && _isSpaceDown)
                {
                    _isSpaceDown = false;
                    startClimb(_cover.Main);
                }
                else if (_cover.In)
                {
                    if (_isSpaceDown)
                    {
                        _isSpaceDown = false;
                        _ignoreCoverUntilWalkedInAgain = null;
                    }
                    else if (_cover.Main == _ignoreCoverUntilWalkedInAgain || !CoverSettings.IsAuto)
                        _cover.Clear();
                }
                else
                    _ignoreCoverUntilWalkedInAgain = null;
            }

            _cover.Update();
            updateAimingBackInTallCover();

            if (!_cover.In)
            {
                if (wasNearLeftCorner)
                {
                    _isExitingLeftCorner = true;
                    _cornerExitTimer = 0.3f;
                }
                else if (wasNearRightCorner)
                {
                    _isExitingRightCorner = true;
                    _cornerExitTimer = 0.3f;
                }

                _wasMovingInCover = false;
                _isAimingBackInTallCover = false;
            }
        }

        private void updateGuns()
        {
            var weaponToShow = 0;

            for (int i = 0; i < Weapons.Length; i++)
                if (_gun == Weapons[i].Gun)
                    weaponToShow = i + 1;

            var animator = GetComponent<Animator>();
            var state = animator.GetCurrentAnimatorStateInfo(1);
            var next = animator.GetNextAnimatorStateInfo(1);

            if (_isChangingWeapon)
            {
                if (_currentWeapon == 0)
                {
                    if (state.IsName("None") || state.IsName("Unarmed Cover") || next.IsName("None") || next.IsName("Unarmed Cover"))
                    {
                        _isChangingWeapon = false;
                        weaponToShow = 0;
                    }
                }
                else
                {
                    for (int i = 0; i < Weapons.Length; i++)
                        if (_currentWeapon == i + 1)
                        {
                            if (state.IsName("None") || next.IsName("None"))
                                weaponToShow = 0;
                            else
                            {
                                var isTransitional = false;

                                foreach (var name in _weaponStates[(int)Weapons[i].Type].Transitional)
                                    if (state.IsName(name) || next.IsName(name))
                                    {
                                        weaponToShow = i + 1;
                                        isTransitional = true;
                                        break;
                                    }

                                if (!isTransitional)
                                {
                                    foreach (var name in _weaponStates[(int)Weapons[i].Type].Common)
                                        if (state.IsName(name) || next.IsName(name))
                                        {
                                            weaponToShow = i + 1;
                                            _isChangingWeapon = false;
                                            break;
                                        }
                                }
                            }
                        }
                }
            }

            if (!_isChangingWeapon)
            {
                var previousGun = _gun;

                if (_currentWeapon > 0 && _currentWeapon <= Weapons.Length)
                    _gun = Weapons[_currentWeapon - 1].Gun;
                else
                    _gun = null;

                if (previousGun != _gun)
                {
                    if (previousGun != null) previousGun.CancelFire();
                    if (_gun != null) _gun.CancelFire();
                }
            }


            for (int i = 0; i < Weapons.Length; i++)
            {
                var show = weaponToShow == i + 1;
                var weapon = Weapons[i];

                if (weapon.Item != null && weapon.Item.activeSelf != show) weapon.Item.SetActive(show);
                if (weapon.Holster != null && weapon.Holster.activeSelf != !show) weapon.Holster.SetActive(!show);
            }

            if (_gun != null)
            {
                if (!_wantsToFire && (!_cover.In || _coverAim.Step == AimStep.None))
                    _gun.CancelFire();

                _gun.Target = _currentFireTarget;
                _gun.Character = gameObject;
                _gun.Allow(IsGunReady && !_isFalling && (!_cover.In || _coverAim.Step == AimStep.Aiming));
            }
        }

        private void updateAim()
        {
            _coverAim.Update();

            if (!_isClimbing && _wantsToZoom && IsGunReady)
                _coverAim.IsZoomed = true;
            else
                _coverAim.IsZoomed = false;
        }

        private void updateFire()
        {
            if (_isClimbing)
                return;

            bool isAbleToPeek;

            updateAimingBackInTallCover();

            if (_isAimingBackInTallCover || _isReloading || _gun == null || _gun.IsClipEmpty)
                isAbleToPeek = false;
            else if (_cover.IsFrontField(_horizontalLookAngle, CoverSettings.Angles.Front))
            {
                if ((CanPeekLeftCorner || CanPeekRightCorner))
                {
                    if (_cover.IsTall && !_inputMovement.IsMoving)
                        isAbleToPeek = wantsToAim && !_wasIntendingToFire;
                    else
                        isAbleToPeek = true;
                }
                else
                    isAbleToPeek = false;
            }
            else
                isAbleToPeek = false;

            if (_isAimingBackInTallCover)
            {
                isAbleToPeek = false;

                if (_cover.IsLeft(_horizontalLookAngle))
                    _cover.StandLeft();
                else
                    _cover.StandRight();

                _lastWalkedCoverDirection = _cover.Direction;
            }

            if (!isAbleToPeek)
                _ignoreCornerUntilFreshlyAble = false;

            if ((wantsToAim || _isAimingBackInTallCover) && IsGunReady)
            {
                var canFire = true;

                if (_gun != null && _gun.IsClipEmpty)
                {
                    startReload();
                    canFire = false;
                }

                if (_cover.In && _coverTime < 0.5f)
                    canFire = false;

                if (canFire && _cover.IsTall && !_isAimingBackInTallCover && !isAbleToPeek)
                    if ((!IsNearLeftCorner && !IsNearRightCorner) ||
                        (IsNearLeftCorner && !_cover.IsLeft(_horizontalLookAngle) ||
                        (IsNearRightCorner && !_cover.IsRight(_horizontalLookAngle))))
                        canFire = CanWallAim;

                if (_ignoreCornerUntilFreshlyAble && isAbleToPeek)
                    canFire = false;

                if (canFire && isAbleToPeek && !_wasIntendingToFire)
                {
                    startCornerAim();

                    if (_wantsToFire)
                        fire();
                }
                else
                {
                    if (canFire)
                    {
                        if (_wantsToFire)
                            fire();
                        else
                            _coverAim.CoverAim(_horizontalLookAngle);
                    }
                    else
                        _coverAim.Leave();
                }
            }
            else
                _coverAim.Leave();

            updateFireIntention();
        }

        private void updateFireIntention()
        {
            _wasIntendingToFire = wantsToAim && _gun != null && !_gun.IsClipEmpty && !_isReloading;
        }

        private void fire()
        {
            _coverAim.Angle = _horizontalLookAngle;

            if (_cover.In)
                _coverAim.CoverAim(_horizontalLookAngle);
            else
                _coverAim.FreeAim(_horizontalLookAngle);

            if (_gun != null)
                if (_gun != null)
                    _gun.Fire();
        }

        private bool isFree(Vector3 direction)
        {
            var capsule = GetComponent<CapsuleCollider>();

//            var count = Physics.RaycastNonAlloc(transform.position + new Vector3(0, capsule.height * 0.3f, 0),
//                                                direction,
//                                                _raycastHits,
//                                                capsule.radius + (_cover.In ? float.Epsilon : ObstacleDistance));
			RaycastHit hit = new RaycastHit();
			var count = 0;
			if (Physics.Raycast (transform.position + new Vector3 (0, capsule.height * 0.3f, 0),
				  	direction,
					out hit,
				   	capsule.radius + (_cover.In ? float.Epsilon : ObstacleDistance),
					CoverSettings.Cover)) 
			{
				count = 1;
			}

            for (int i = 0; i < count; i++)
				if (hit.collider.gameObject != gameObject)
                    return false;

            return true;
        }

        private void updateCommon()
        {
            updateCover(false);
            if (_isClimbing) return;

            updateAim();
            updateFire();
            if (_isInCornerAimState) return;

			// my customization ----------
//			if (IsAI)
//				UpdateAIWalk ();
//			else
            	updateWalk();
            //---------------------------
			updateLookAngleDiff();
            updateVertical();
        }

		// my customization ----------
		private void UpdateAIWalk()
		{
			
		}
		//---------------------------

        private void updateWalk()
        {
            Vector3 movement;

            if (_directionChangeDelay > float.Epsilon)
                _directionChangeDelay -= Time.deltaTime;

            _currentMovement = _inputMovement;

            if (_currentMovement.Direction.sqrMagnitude > 0.1f)
            {
                var overallIntensity = 1.0f;

                if (_cover.In)
                {
                    _currentMovement.Magnitude = 1.0f;

                    var intendedWalkAngle = Mathf.Atan2(_currentMovement.Direction.x, _currentMovement.Direction.z) * Mathf.Rad2Deg;

                    if (!_isAimingBackInTallCover)
                    {
                        var angle = intendedWalkAngle - _cover.ForwardAngle;

                        var angle0 = angle - angle % 90;
                        var angle1 = angle0 + 90;
                        var angle2 = angle0 - 90;

                        if (Mathf.Abs(angle - angle0) < Mathf.Abs(angle - angle1))
                        {
                            if (Mathf.Abs(angle - angle0) < Mathf.Abs(angle - angle2))
                                angle = angle0;
                            else
                                angle = angle2;
                        }
                        else
                        {
                            if (Mathf.Abs(angle - angle1) < Mathf.Abs(angle - angle2))
                                angle = angle1;
                            else
                                angle = angle2;
                        }

                        _currentMovement.Direction = Quaternion.Euler(0, Mathf.DeltaAngle(intendedWalkAngle, angle + _cover.ForwardAngle), 0) * _currentMovement.Direction;
                    }

                    if (isFree(_currentMovement.Direction) && !IsAiming)
                    {
                        if (_cover.IsLeft(intendedWalkAngle, 0) &&
                            (_cover.IsTall || !_cover.IsRight(_horizontalLookAngle, 90 - CoverSettings.Angles.LowWalkFaceChange)) &&
                            _cover.MainChangeAge >= 0.5f)
                        {
                            if (_cover.IsStandingRight)
                                _directionChangeDelay = CoverSettings.DirectionChangeDelay;

                            _cover.StandLeft();
                        }
                        else if (_cover.IsRight(intendedWalkAngle, 0) &&
                                 (_cover.IsTall || !_cover.IsLeft(_horizontalLookAngle, 90 - CoverSettings.Angles.LowWalkFaceChange)) &&
                                 _cover.MainChangeAge >= 0.5f)
                        {
                            if (_cover.IsStandingLeft)
                                _directionChangeDelay = CoverSettings.DirectionChangeDelay;

                            _cover.StandRight();
                        }
                    }

                    _lastWalkedCoverDirection = _cover.Direction;

                    if (_directionChangeDelay > float.Epsilon)
                        overallIntensity = 0.0f;
                }

                var local = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * _currentMovement.Direction;

                _leftMoveIntensity = Mathf.Lerp(_leftMoveIntensity, isFree(-transform.right) ? 1.0f : 0.0f, Time.deltaTime * 4);
                _rightMoveIntensity = Mathf.Lerp(_rightMoveIntensity, isFree(transform.right) ? 1.0f : 0.0f, Time.deltaTime * 4);
                _backMoveIntensity = Mathf.Lerp(_backMoveIntensity, isFree(-transform.forward) ? 1.0f : 0.0f, Time.deltaTime * 4);
                _frontMoveIntensity = Mathf.Lerp(_frontMoveIntensity, isFree(transform.forward) ? 1.0f : 0.0f, Time.deltaTime * 4);

                if (local.x < -float.Epsilon) local.x *= _leftMoveIntensity;
                if (local.x > float.Epsilon) local.x *= _rightMoveIntensity;
                if (local.z < -float.Epsilon) local.z *= _backMoveIntensity;
                if (local.z > float.Epsilon) local.z *= _frontMoveIntensity;

                _currentMovement.Direction = Quaternion.Euler(0, transform.eulerAngles.y, 0) * local;
                movement = local * _currentMovement.Magnitude * overallIntensity;
            }
            else
                movement = Vector3.zero;

            _localMovement = Vector3.Lerp(_localMovement, movement, Time.deltaTime * 8);
            _movementInput = Mathf.Clamp(movement.magnitude * 2, 0, 1);
        }

        private void leaveCover()
        {
            if (_cover.In)
            {
                _ignoreCoverUntilWalkedInAgain = _cover.Main;
                _cover.Clear();
            }
        }

        private float deltaAngleToTurnTo(float target)
        {
            var angle = Mathf.DeltaAngle(transform.eulerAngles.y, target);

            if (Mathf.Abs(angle) <= 90)
                return angle;

            if (_isExitingLeftCorner)
            {
                if (angle < 0)
                    angle = 360 + angle;
            }
            else if (_isExitingRightCorner)
            {
                if (angle > 0)
                    angle = -360 + angle;
            }
            else if (_cover.In && !_cover.IsTall && !IsAiming)
            {
                var angleToCover = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, _cover.ForwardAngle));
                var halfwayToCover = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y + angle * 0.5f, _cover.ForwardAngle));

                if (angleToCover < 100 && halfwayToCover < angleToCover)
                {
                    if (angle > 0)
                        angle = -360 + _lookAngleDiff;
                    else
                        angle = 360 + _lookAngleDiff;
                }
            }

            return angle;
        }

        private void updateLookAngleDiff()
        {
            if (_cover.In && !IsAiming)
            {
                _lookAngleDiff = deltaAngleToTurnTo(_cover.FaceAngle);
                _lastWalkedCoverDirection = _cover.Direction;
            }
            else
            {
                _lookAngleDiff = deltaAngleToTurnTo(_horizontalLookAngle);

                if (_cover.In)
                {
                    if (_cover.IsTall)
                    {
                        if (_cover.IsLeft(_horizontalLookAngle))
                            _cover.StandLeft();
                        else
                            _cover.StandRight();
                    }
                    else
                    {
                        if (_cover.IsStandingLeft && _cover.IsRight(_horizontalLookAngle, CoverSettings.Angles.LowAimFaceChange.Left))
                            _cover.StandRight();
                        else if (_cover.IsStandingRight && _cover.IsLeft(_horizontalLookAngle, CoverSettings.Angles.LowAimFaceChange.Right))
                            _cover.StandLeft();
                    }
                }
            }
        }

        private void updateVertical()
        {
            if (_jumpTimer < 999) _jumpTimer += Time.deltaTime;
            if (_ignoreFallTimer > 0) _ignoreFallTimer -= Time.deltaTime;

            updateGround();

            if (_isGrounded)
            {
                if (_nextJumpTimer > -float.Epsilon) _nextJumpTimer -= Time.deltaTime;

                if (!_cover.In && !_isJumping && _nextJumpTimer < float.Epsilon && _isSpaceDown)
                {
                    _wantsToJump = true;
                    _isSpaceDown = false;
                    SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);
                }
            }
            else if (GetComponent<Rigidbody>().velocity.y < -5)
                _isJumping = false;

            if (_isGrounded)
            {
                if (_wantsToJump)
                {
                    _isJumping = true;
                    _jumpTimer = 0;

                    var body = GetComponent<Rigidbody>();

                    var v = transform.rotation * _localMovement * JumpSettings.Speed;
                    v.y = JumpSettings.Strength;
                    body.velocity = v;
                }
                else if (_isJumping)
                    _isJumping = false;
            }
            else
                _wantsToJump = false;

            if (_ignoreFallTimer <= float.Epsilon)
            {
                if (!_isFalling)
                {
                    if (GetComponent<Rigidbody>().velocity.y < -6 &&
                        !findGround(FallThreshold))
                        _isFalling = true;
                }
                else
                {
                    if (_isGrounded)
                        _isFalling = false;
                }
            }
            else
                _isFalling = false;

			if (_isFalling && false)
            {
                Vector3 edge;
                if (findEdge(out edge, 0.1f))
                {
                    var offset = transform.position - edge;
                    offset.y = 0;
                    var distance = offset.magnitude;

                    if (distance > 0.01f)
                    {
                        offset /= distance;
                        transform.position += offset * Mathf.Clamp(Time.deltaTime * 3, 0, distance);
                    }
                }
            }
        }

        private void updateGround()
        {
            var body = GetComponent<Rigidbody>();

            if (_ignoreFallTimer < float.Epsilon)
            {
                if (_cover.In)
                    _isGrounded = findGround(GroundThreshold + 1.0f);
                else
                    _isGrounded = findGround(GroundThreshold);
            }
            else
                _isGrounded = true;

            if (_isGrounded && !_wasGrounded && IsAlive)
            {
                SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
                _nextJumpTimer = 0.2f;
            }

            _wasGrounded = _isGrounded;
        }

        private void updateAnimator()
        {
            var animator = GetComponent<Animator>();

            if (IsAlive)
            {
                var state = animator.GetCurrentAnimatorStateInfo(0);

                float runCycle = Mathf.Repeat(state.normalizedTime, 1);
                float jumpLeg = (runCycle < 0.5f ? 1 : -1) * _movementInput;
                if (_isGrounded)
                {
                    if (_jumpLegTimer > 0)
                        _jumpLegTimer -= Time.deltaTime;
                    else
                        animator.SetFloat("JumpLeg", jumpLeg);
                }
                else
                    _jumpLegTimer = 0.5f;

                if (IsAlive &&
                    (state.IsName("Walk Armed") || state.IsName("Walk Unarmed")))
                {
                    if (runCycle > 0.6f)
                    {
                        if (_lastFoot != 1)
                        {
                            _lastFoot = 1;
                            SendMessage("OnFootstep", SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    else if (runCycle > 0.1f)
                    {
                        if (_lastFoot != 0)
                        {
                            _lastFoot = 0;
                            SendMessage("OnFootstep", SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
                else
                    _lastFoot = -1;

                animator.SetBool("IsDead", false);
                animator.SetFloat("Rotation", _lookAngleDiff, 0.2f, Time.deltaTime);
                animator.SetFloat("MovementX", _localMovement.x);
                animator.SetFloat("MovementZ", _localMovement.z);
                animator.SetFloat("MovementInput", _movementInput);
                animator.SetBool("IsFalling", _isFalling && !_isJumping);
                animator.SetBool("IsGrounded", _isGrounded);
                animator.SetInteger("WeaponType", _currentWeapon == 0 ? 0 : (1 + (int)Weapons[_currentWeapon - 1].Type));
                animator.SetBool("IsAiming", IsAiming);
                animator.SetBool("IsCornerAiming", IsCornerAiming);

                if (!_ignoreCornerUntilFreshlyAble)
                    animator.SetBool("ExitCornerAim", false);

                animator.SetBool("IsInCover", _cover.In && (_isInCornerAimState || (!_isAimingBackInTallCover && !IsAiming)));

                if (_cover.In)
                {
                    animator.SetFloat("CoverDirection", _cover.Direction, 0.2f, Time.deltaTime);
                    animator.SetFloat("CoverHeight", _cover.IsTall ? 1.0f : 0.0f, 0.1f, Time.deltaTime);
                }

                animator.SetBool("IsClimbing", _isClimbing && !_isClimbingAVault);
                animator.SetBool("IsVault", _isClimbing && _isClimbingAVault);
                animator.SetBool("IsJumping", _isJumping);
                animator.SetFloat("VerticalVelocity", GetComponent<Rigidbody>().velocity.y);
                animator.SetBool("IsCrouching", _isCrouching);
                animator.SetBool("IsReloading", _isReloading);
                animator.SetFloat("LowAim", (!_cover.IsTall && IsAimingBackFromCover) ? 1.0f : 0.0f, 0.1f, Time.deltaTime);

                if (_verticalLookAngle < 0f)
                    animator.SetFloat("LookHeight", Mathf.Clamp(_verticalLookAngle / 55f, -1, 1));
                else
                    animator.SetFloat("LookHeight", Mathf.Clamp(_verticalLookAngle / 40f, -1, 1));
            }
            else
                animator.SetBool("IsDead", true);
        }

        private void instantCoverAnimatorUpdate()
        {
            var animator = GetComponent<Animator>();

            animator.SetFloat("CoverDirection", _cover.Direction);
            animator.SetFloat("CoverHeight", _cover.IsTall ? 1.0f : 0.0f);
        }

        private void updateIK()
        {
            if (!IsAlive)
                return;

            transform.Rotate(0, _lookAngleDiff * ExtraTurnSpeed * Time.deltaTime, 0);

            if (_lastHitStrength > float.Epsilon)
            {
                if (IK.HitBone != null)
                    IK.HitBone.localRotation *= Quaternion.Lerp(Quaternion.identity, _lastHit, _lastHitStrength);

                _lastHitStrength -= Time.deltaTime * 5.0f;
            }

            if (_gun != null)
                _lastAimTransform = _gun.transform.Find("Aim");

            if (_lastAimTransform != null && IK.AimBones.Length > 0)
            {
                _aimSolver.Target = _lastAimTransform;
                _aimSolver.Bones = IK.AimBones;
                _aimSolver.Aim(IsAimingPrecisely ? _currentFireTarget : _lookTarget, _armAimIntensity, IK.Iterations);
            }

            if (_gun != null)
                _gun.UpdateIntendedRotation();

            if (_gun != null && IK.RightHand != null && _gun.RecoilShift.magnitude > 0.01f)
            {
                _aimSolver.Target = IK.RightHand;
                _aimSolver.Bones = IK.RecoilBones;
                _aimSolver.Move(IK.RightHand.position + _gun.RecoilShift, 1.0f, IK.Iterations);
            }

            if (_gun != null && IK.LeftHand != null)
            {
                Transform hand = null;

                if (IsAiming)
                    hand = _gun.LeftHandOverwrite.Aim;
                else if (_cover.In)
                {
                    if (_cover.IsTall)
                    {
                        if (_cover.IsStandingLeft)
                            hand = _gun.LeftHandOverwrite.TallCoverLeft;
                        else
                            hand = _gun.LeftHandOverwrite.TallCoverRight;
                    }
                    else
                    {
                        if (_cover.IsStandingLeft)
                            hand = _gun.LeftHandOverwrite.LowCoverLeft;
                        else
                            hand = _gun.LeftHandOverwrite.LowCoverRight;
                    }
                }

                if (hand == null)
                    hand = _gun.LeftHandDefault;

                if (hand != null)
                {
                    _aimSolver.Target = IK.LeftHand;
                    _aimSolver.Bones = IK.LeftArmBones;
                    _aimSolver.Move(hand.position, _leftHandIntensity, IK.Iterations);
                }
            }

            if (IK.Sight != null && IK.SightBones.Length > 0)
            {
                _aimSolver.Target = IK.Sight;
                _aimSolver.Bones = IK.SightBones;
                _aimSolver.Aim(IsAimingPrecisely ? _currentFireTarget : _lookTarget, _headAimIntensity, IK.Iterations);
            }

            if (_gun != null)
                _gun.UpdateAimOrigin();
        }
		public LayerMask Ground;
        private bool findGround(float threshold)
        {
			RaycastHit hit_ = new RaycastHit ();
			int count = 0;
			if (Physics.Raycast (transform.position + (Vector3.up * (0.1f)), Vector3.down, out hit_, threshold, Ground)) 
			{
				count = 1;
			}
			for (int i = 0; i <count ; i++)
            {
				var hit = hit_;

                if (!hit.collider.isTrigger)
                    if (hit.collider.gameObject != gameObject)
                        return true;
            }

            return false;
        }

        private float getGoundHeight()
        {
			RaycastHit hit_ = new RaycastHit ();
			int count = 0;
			if (Physics.Raycast (transform.position + (Vector3.up * (0.1f)), Vector3.down, out hit_, Ground)) 
			{
				count = 1;
			}
			for (int i = 0; i < count; i++)
            {
				var hit = hit_;

                if (hit.collider.gameObject != gameObject)
                    return hit.point.y;
            }

            return 0;
        }

        private bool findEdge(out Vector3 position, float threshold)
        {
            var capsule = GetComponent<CapsuleCollider>();
            var bottom = transform.TransformPoint(capsule.center - new Vector3(0, capsule.height * 0.5f + capsule.radius, 0));
            var count = Physics.OverlapSphereNonAlloc(bottom, capsule.radius + threshold, _colliderCache);

            for (int i = 0; i < count; i++)
                if (_colliderCache[i].gameObject != gameObject)
                {
                    position = _colliderCache[i].ClosestPointOnBounds(bottom);
                    return true;
                }

            position = Vector3.zero;
            return false;
        }

        #endregion
    }
}