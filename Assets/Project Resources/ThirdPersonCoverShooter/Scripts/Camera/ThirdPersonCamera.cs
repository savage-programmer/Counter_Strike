using UnityEngine;
using CnControls;

namespace ThirdPersonCover
{
    /// <summary>
    /// Manages the camera object by setting an appropriate orientation depending on the target object's state.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ThirdPersonCamera : MonoBehaviour
    {
		public LayerMask HitLayer;
        /// <summary>
        /// Target object with CharacterMotor attached.
        /// </summary>
        [Tooltip("Target object with CharacterMotor attached.")]
        public GameObject Target;

        /// <summary>
        /// Multiplier for horizontal camera rotation.
        /// </summary>
        [Tooltip("Multiplier for horizontal camera rotation.")]
        [Range(0, 10)]
        public float HorizontalRotateSpeed = 0.9f;

        /// <summary>
        /// Multiplier for vertical camera rotation.
        /// </summary>
        [Tooltip("Multiplier for vertical camera rotation.")]
        [Range(0, 10)]
        public float VerticalRotateSpeed = 1.0f;

        /// <summary>
        /// Handle visiblity of crosshair.
        /// </summary>
        [Tooltip("Handle visiblity of crosshair.")]
        public bool IsCrosshairEnabled = true;

        /// <summary>
        /// Crosshair texture to be displayed in the middle of screen.
        /// </summary>
        [Tooltip("Crosshair texture to be displayed in the middle of screen.")]
        public Texture Crosshair;

        /// <summary>
        /// Size of the crosshair as a fraction of screen height.
        /// </summary>
        [Tooltip("Size of the crosshair as a fraction of screen height.")]
        [Range(0, 1)]
        public float CrosshairSize = 0.05f;

        /// <summary>
        /// Is camera responding to mouse movement when the mouse cursor is unlocked.
        /// </summary>
        [Tooltip("Is camera responding to mouse movement when the mouse cursor is unlocked.")]
        public bool RotateWhenUnlocked = false;

        /// <summary>
        /// Camera settings for all gameplay situations.
        /// </summary>
        [Tooltip("Camera settings for all gameplay situations.")]
        public CameraStates States = CameraStates.GetDefault();

        /// <summary>
        /// Horizontal orientation of the camera in degrees.
        /// </summary>
        [HideInInspector]
        public float Horizontal;

        /// <summary>
        /// Vertical orientation of the camera in degrees.
        /// </summary>
        [HideInInspector]
        public float Vertical;

        private Vector3 _pivot;
        private Vector3 _offset;
        private Vector3 _orientation;
        private float _crosshairAlpha;

        private Vector3 _motorPosition;
        private Quaternion _motorRotation;
        private float _motorPivotSpeed = 1;
        private bool _wasInCover;

        private float _lastTargetDistance;

        // Raycast cache.
        private RaycastHit[] _hits = new RaycastHit[64];

		bool GotEnemy;
        private void Awake()
        {

          
            _offset = States.Default.Offset;
        }
        private void Start()
        {
            SelectPlayer();
        }

        /// <summary>
        /// Draws the crosshair.
        /// </summary>
        private void OnGUI()
        {
			if (Time.timeScale <= 0)
				return;
            if (Crosshair == null || !IsCrosshairEnabled)
                return;

            var size = Screen.height * CrosshairSize;
            var point = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);

            if (Target != null)
            {
                var motor = Target.GetComponent<CharacterMotor>();

                if (motor != null)
                {
                    var target = motor.GunOrigin + motor.GunTargetDirection * _lastTargetDistance;
                    var current = motor.GunOrigin + motor.GunDirection * _lastTargetDistance;

                    var p1 = GetComponent<Camera>().WorldToScreenPoint(target);
                    var p2 = GetComponent<Camera>().WorldToScreenPoint(current);

                    point.x += (p2.x - p1.x) * motor.RecoilIntensity;
                    point.y -= (p2.y - p1.y) * motor.RecoilIntensity;
                }
            }

            var previous = GUI.color;
//            GUI.color = new Color(1, 1, 1, _crosshairAlpha);
			if (GotEnemy) 
			{
//				print ("color");
				GUI.color = Color.red;
			}
			else
				GUI.color = previous;
            
			GUI.DrawTexture(new Rect(point.x - size * 0.5f, point.y - size * 0.5f, size, size), Crosshair);
//			GUI.color = previous;
        }

        private void LateUpdate()
        {
			
			if (Time.timeScale <= 0)
				return;
			
            if (Target == null)
                return;
			#if UNITY_EDITOR
			if (Cursor.lockState == CursorLockMode.Locked || RotateWhenUnlocked)
            {
				Horizontal = Mathf.LerpAngle(Horizontal, Horizontal + Input.GetAxis("Mouse X") * HorizontalRotateSpeed * Time.timeScale, 1.0f);
				Vertical = Mathf.LerpAngle(Vertical, Vertical - Input.GetAxis("Mouse Y") * VerticalRotateSpeed * Time.timeScale, 1.0f);
            }
			#else 
			if (RotateWhenUnlocked)//Cursor.lockState == CursorLockMode.Locked || RotateWhenUnlocked)
			{
			Horizontal = Mathf.LerpAngle(Horizontal, Horizontal + CnInputManager.GetAxis("MX") * HorizontalRotateSpeed * Time.timeScale, 1.0f);
			Vertical = Mathf.LerpAngle(Vertical, Vertical - CnInputManager.GetAxis("MY") * VerticalRotateSpeed * Time.timeScale, 1.0f);
			}
			#endif
            Vertical = Mathf.Clamp(Vertical, -45, 70);

            var motor = Target.GetComponent<CharacterMotor>();

            if (motor != null)
            {
                var newPosition = motor.GetPivotPosition();
                var newRotation = motor.GetPivotRotation();

                _motorPivotSpeed = Mathf.Lerp(_motorPivotSpeed, 1, Time.deltaTime * 5);

                if (motor.IsInCover != _wasInCover)
                    _motorPivotSpeed = 0;

                _motorPosition = Vector3.Lerp(_motorPosition, newPosition, Mathf.Lerp(Time.deltaTime * 5, 1, _motorPivotSpeed));
                _motorRotation = Quaternion.Slerp(_motorRotation, newRotation, Mathf.Lerp(Time.deltaTime * 3, 1, _motorPivotSpeed));

                var rotation = Quaternion.Euler(Vertical, Horizontal, 0);
                var transformPivot = _motorRotation * _pivot + _motorPosition;
                var cameraPosition = transformPivot + rotation * _offset;
                var cameraTarget = _motorPosition + _pivot + rotation * Vector3.forward * 100;

                var forward = Quaternion.Euler(_orientation) * (cameraTarget - cameraPosition);
                cameraTarget = cameraPosition + forward;

                transform.position = cameraPosition;
                transform.LookAt(cameraTarget);

                var lookPosition = transform.position + transform.forward * 100;
                motor.SetLookTarget(lookPosition);

                var minDistance = Vector3.Distance(cameraPosition, motor.Top);
                var hitDistance = Vector3.Distance(cameraPosition, lookPosition);
                var closestHit = lookPosition;
				RaycastHit[] hits_2 = new RaycastHit[1];

				int o = 0;
				if (Physics.Raycast (transform.position, transform.forward, out hits_2 [0], 40, HitLayer)) 
				{
//					print ("hit "+hits_2 [0].transform.gameObject.name);
					if(hits_2 [0].transform.CompareTag("Enemy"))
						GotEnemy = true;
					else
						GotEnemy = false;
					o = 1;
				}
				else
					GotEnemy = false;
				for (int i = 0; i < 0; i++)
                {
					var hit = hits_2[i];

                    if (hit.collider.gameObject != motor.gameObject && !hit.collider.isTrigger)
                    {
                        var cameraDist = Vector3.Distance(cameraPosition, hit.point);

                        if (cameraDist > minDistance && cameraDist < hitDistance)
                        {
                            hitDistance = cameraDist;
                            closestHit = hit.point + (motor.IsAimingPrecisely ? transform.forward : Vector3.zero);
                        }
                    }
                }

                _lastTargetDistance = Vector3.Distance(transform.position, closestHit);
                motor.SetFireTarget(closestHit);
                motor.FireFrom(transform.position);

                float alphaTarget = 1;
                CameraState state;

                if (!motor.IsAlive)
                    state = States.Dead;
                else if (motor.IsZoomed)
                {
                    if (motor.IsCornerAiming)
                    {
                        if (motor.IsInTallCover)
                            state = motor.IsStandingLeftInCover ? States.LeftTallCornerZoom : States.RightTallCornerZoom;
                        else
                            state = motor.IsStandingLeftInCover ? States.LeftLowCornerZoom : States.RightLowCornerZoom;
                    }
                    else if (motor.IsInCover)
                    {
                        if (!motor.IsInTallCover && motor.IsAimingBackFromCover)
                            state = States.LowCoverBackZoom;
                        else
                            state = States.LowCoverZoom;
                    }
                    else if (motor.IsCrouching)
                        state = States.CrouchZoom;
                    else
                        state = States.Zoom;
                }
                else if (motor.IsClimbing)
                    state = States.Climb;
                else if (motor.CanPeekLeftCorner || motor.CanPeekRightCorner)
                    state = motor.IsStandingLeftInCover ? States.LeftCorner : States.RightCorner;
                else if (motor.IsInCover)
                {
                    if (motor.IsInTallCover)
                    {
                        if (motor.IsAimingBackFromCover)
                            state = States.TallCoverBack;
                        else
                        {
                            if (!motor.CanPeekLeftCorner &&
                                !motor.CanPeekRightCorner &&
                                !motor.CanWallAim)
                                alphaTarget = 0;

                            if (motor.IsStandingLeftInCover)
                                state = States.TallCoverLeft;
                            else
                                state = States.TallCoverRight;
                        }
                    }
                    else
                        state = States.LowCover;
                }
                else if (motor.IsCrouching)
                    state = States.Crouch;
                else if (motor.IsIntendingToAim)
                    state = States.Aim;
                else
                {
                    alphaTarget = 0;
                    state = States.Default;
                }

                var camera = GetComponent<Camera>();

                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, state.FOV, Time.deltaTime * 6);

                _crosshairAlpha = Mathf.Lerp(_crosshairAlpha, alphaTarget, Time.deltaTime * 6);

                _pivot = Vector3.Lerp(_pivot, state.Pivot, Time.deltaTime * 6);
                _offset = Vector3.Lerp(_offset, state.Offset, Time.deltaTime * 6);
                _orientation = Vector3.Lerp(_orientation, state.Orientation, Time.deltaTime * 6);

                _wasInCover = motor.IsInCover;
            }
            else
                _crosshairAlpha = 0;
        }

        public void SelectPlayer()
        {
            int temp = GlobalScripts.CurrPlayer;
         //   temp = 3;
            Target = MissionController.instance.Players[temp].gameObject;
        }
    }

    
}