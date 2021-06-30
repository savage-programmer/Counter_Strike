using UnityEngine;
using CnControls;

namespace ThirdPersonCover
{
    /// <summary>
    /// Takes player input and translates that to commands to CharacterMotor.
    /// </summary>
    [RequireComponent(typeof(CharacterMotor))]
    public class PlayerController : MonoBehaviour
    {
        private bool _wasSpaceDown;
        private Vector3 _lastDirection;
		bool Crouch;
		bool aim;
		void Start()
		{
			var motor = GetComponent<CharacterMotor>();
			if (motor == null) return;
			motor.InputWeapon (3);
			Crouch = false;
			aim = false;
		}
        private void Update()
        {
			#if UNITY_EDITOR
			var isSpaceDown = Input.GetKey(KeyCode.Space);
			#else
			var isSpaceDown = CnInputManager.GetButton("Jump");
			#endif
            if (_wasSpaceDown)
            {
                if (!isSpaceDown)
                    _wasSpaceDown = false;
                else
                    isSpaceDown = false;
            }
            else
                _wasSpaceDown = isSpaceDown;

            var motor = GetComponent<CharacterMotor>();
            if (motor == null) return;

            if (isSpaceDown) motor.InputSpace();

			#if UNITY_EDITOR
			if (Input.GetKey(KeyCode.C)) motor.InputCrouch();

			if (Input.GetKey(KeyCode.Alpha1)) motor.InputWeapon(0);
			if (Input.GetKey(KeyCode.Alpha2)) motor.InputWeapon(1);
			if (Input.GetKey(KeyCode.Alpha3)) motor.InputWeapon(2);
			if (Input.GetKey(KeyCode.Alpha4)) motor.InputWeapon(3);

            if (Input.GetKey(KeyCode.R)) motor.InputReload();

			if (Input.GetMouseButton(0)) motor.InputFire();
			if (Input.GetMouseButton(1)) motor.InputZoom();
			#else

			if(CnInputManager.GetButtonDown("C"))
			{
				Crouch = !Crouch;
			}
//			else if(CnInputManager.GetButtonUp("C"))
//			{
//				Crouch = false;
//			}
			if(Crouch)
				motor.InputCrouch();

			if (CnInputManager.GetButton("0")) motor.InputWeapon(0);
			if (CnInputManager.GetButton("1")) motor.InputWeapon(1);
			if (CnInputManager.GetButton("2")) motor.InputWeapon(2);
			if (CnInputManager.GetButton("3")) motor.InputWeapon(3);

			if (CnInputManager.GetButton("R")) motor.InputReload();

			if (CnInputManager.GetButton("F")) motor.InputFire();
			if (CnInputManager.GetButtonDown("Z"))
			{
				aim = !aim;
			}
//			else if (CnInputManager.GetButtonUp("Z"))
//			{
//				aim = false;
//			}0
			if (aim) motor.InputZoom();
			#endif
            CharacterMovement movement = new CharacterMovement();
			Vector3 direction = Vector3.zero;
#if UNITY_EDITOR
            var isForward = Input.GetKey(KeyCode.W);
            var isBackward = Input.GetKey(KeyCode.S);
            var isRight = Input.GetKey(KeyCode.D);
            var isLeft = Input.GetKey(KeyCode.A);

           

            if (isForward)
                direction.z += 1;
            if (isBackward)
                direction.z -= 1;
            if (isRight)
                direction.x += 1;
            if (isLeft)
                direction.x -= 1;

            if (isForward && isBackward) direction.z = _lastDirection.z;
            if (isRight && isLeft) direction.x = _lastDirection.x;

            _lastDirection = direction;
#else
			direction = new Vector3 (CnInputManager.GetAxis("H"),0f,CnInputManager.GetAxis("V"));
#endif
            if (direction.magnitude > float.Epsilon)
                movement.Direction = Quaternion.Euler(0, motor.LookAngle, 0) * direction.normalized;

            if (motor.IsZoomed)
				movement.Magnitude =1.0f;// 0.5f;
            else
            {
                if (motor.Gun != null)
                {
					if (CnInputManager.GetButton ("Sprint"))
						movement.Magnitude = 1.0f;// 0.5f;
                    else
                        movement.Magnitude = 1.0f;
                }
                else
                {
					if (CnInputManager.GetButton("Sprint"))
                        movement.Magnitude = 1.0f;
                    else
						movement.Magnitude =1.0f; // 0.5f;
                }
            }

            motor.InputMovement(movement);
        }

    }
}