using UnityEngine;
using UnityEngine.AI;

namespace ThirdPersonCover
{
    /// <summary>
    /// Makes decisions based on the gameplay situation and gives input to CharacterMotor.
    /// </summary>
    [RequireComponent(typeof(CharacterMotor))]
    public class AIController : MonoBehaviour
    {
		public int weapon;
		public bool Look;
		public bool Fire;
		public NavMeshAgent agent;
		 Vector3 target,target2,target3,target4;
		public GameObject targetobj,back;
		bool walk=false,turn=true;
		public float pathdictance;

        /// <summary>
        /// Time in seconds for AI to react to changes in the world.
        /// </summary>
        [Tooltip("Time in seconds for AI to react to changes in the world.")]
        [Range(0, 10)]
        public float ReactionTime = 0.3f;

        /// <summary>
        /// Aiming precision. Lower values translate to better aiming.
        /// </summary>
        [Tooltip("Aiming precision. Lower values translate to better aiming.")]
        [Range(0, 10)]
        public float TargetRadius = 1.0f;

        /// <summary>
        /// Distance for AI to notice visible threat or a teammate being attacked.
        /// </summary>
        [Tooltip("Distance for AI to notice visible threat or a teammate being attacked.")]
        [Range(0, 1000)]
        public float SightDistance = 20;

        /// <summary>
        /// Distance for AI to notice threat from any direction. Sneaking characters are ignored.
        /// </summary>
        [Tooltip("Distance for AI to notice threat from any direction. Sneaking characters are ignored.")]
        [Range(0, 1000)]
        public float NoticeDistance = 6;

        /// <summary>
        /// Distance in any direction for AI to notice a teammate being hurt.
        /// </summary>
        [Tooltip("Distance in any direction for AI to notice a teammate being hurt.")]
        [Range(0, 1000)]
        public float FriendHurtDistance = 12;

        /// <summary>
        /// Distance to keep away from the threat.
        /// </summary>
        [Tooltip("Distance to keep away from the threat.")]
        [Range(0, 1000)]
        public float MinStandDistance = 7;

        /// <summary>
        /// Distance to maintain when attacking an enemy.
        /// </summary>
        [Tooltip("Distance to maintain when attacking an enemy.")]
        [Range(0, 1000)]
        public float MaxStandDistance = 10;

        /// <summary>
        /// Field of sight to notice changes in the world.
        /// </summary>
        [Tooltip("Field of sight to notice changes in the world.")]
        [Range(0, 360)]
        public float FieldOfView = 160;

        private bool _isAware = false;
        private Vector3 _target;
        private Vector3 _currentTarget;

        private float _targetDelay = 0.0f;

        private GameObject _enemy;

        private float _reaction = 0.0f;
        private bool _isGettingAware;

        private RaycastHit[] _hits = new RaycastHit[64];

        /// <summary>
        /// React to attacks and notify other AI about it.
        /// </summary>
		/// 
		/// 
		/// 
		void Awake()

		{
			if (targetobj) {

				target3=new Vector3(targetobj.gameObject.transform.position.x,targetobj.gameObject.transform.position.y,targetobj.gameObject.transform.position.z);
				target4 = new Vector3 (-1f, 0f, 0f);
				target = new Vector3 (1f, 0f, 0f);
				//target2=new Vector3(targetobj.gameObject.transform.position.x*-1,targetobj.gameObject.transform.position.y*-1,targetobj.gameObject.transform.position.z*-1);
				target2=new Vector3(back.gameObject.transform.position.x,back.gameObject.transform.position.y,back.gameObject.transform.position.z);
			//target2 = new Vector3 (0f, 0f, 0f);
				walking ();
				Debug.Log ("walk on start");
			}
		}




        public void OnHit()
        {
			if (_isAware)
				return;
            _isAware = true;

			foreach (var ai in FindObjectsOfType<AIController>()) 
			{
				if (ai != this || Vector3.Distance(ai.transform.position,this.transform.position)<ai.FriendHurtDistance)
				{
					ai.SendMessage ("OnFriendHit", gameObject, SendMessageOptions.DontRequireReceiver);
				}
			}
        }
		CharacterMotor enemyMotor ;
        /// <summary>
        /// React to attacks on other AI.
        /// </summary>
        public void OnFriendHit(GameObject friend)
        {
            var vector = friend.transform.position - transform.position;

			if (vector.magnitude < FriendHurtDistance)
			{
				_isAware = true;
				return;
			}
//			else if (vector.magnitude < SightDistance && !_isAware)
//            {
//                var angle = Mathf.Abs(Mathf.DeltaAngle(0, Mathf.Acos(Vector3.Dot(vector.normalized, transform.forward)) * Mathf.Rad2Deg));
//
//                if (angle <= FieldOfView * 0.5f)
//                    if (isInSight(friend))
//                        _isAware = true;
//            }
        }

        private void Update()
        {
			if (GameManager_GJ11.instance.State == GameStates.Paused)
				return;
			
            var wasAware = _isAware;

            var motor = GetComponent<CharacterMotor>();
            if (motor == null) return;
            if (!motor.IsAlive) return;

			if (_enemy == null || !_enemy.activeInHierarchy)
                foreach (var obj in FindObjectsOfType<PlayerController>())
                {
                    _enemy = obj.gameObject;
                    break;
                }

            if (_enemy != null)
            {
				if (enemyMotor == null) 
				{
					enemyMotor = _enemy.GetComponent<CharacterMotor>();
				}
				// enemy dont have required component
				if (enemyMotor == null)
					return;
				
                if (!enemyMotor.IsAlive) return;

                  var vector = _enemy.transform.position - motor.transform.position;
 
                // enemy(player) is in notice distance and not crouching => will discover even is behind
                if (vector.magnitude < NoticeDistance && !isCrouching(_enemy))
                    _isAware = true;
				
				else if (vector.magnitude < SightDistance && !_isAware) // enemy is in sight range and this is not attacking => will check for range
                {
                    var angle = Mathf.Abs(Mathf.DeltaAngle(0, Mathf.Acos(Vector3.Dot(vector.normalized, transform.forward)) * Mathf.Rad2Deg));

                    if (angle <= FieldOfView * 0.5f) // enemy is sight angle or is in front => will check if its hiding 
                        if (isInSight(_enemy)) // enemy is not hiding behind
                            _isAware = true;
                }

                if (_isGettingAware) // just found the enemy => will wait for given reaction time
                {
                    if (_reaction < float.Epsilon)
                    {
                        _isGettingAware = false;
                        _isAware = true;
                    }
                    else
                        _reaction -= Time.deltaTime;
                }
                else if (_isAware && !wasAware) // just found the enemy was not in sight before
                {
                    _reaction = ReactionTime;
                    _isGettingAware = true;
                    _isAware = false;
                }

				if (_isAware) {
					var perfectTarget = _enemy.transform.position + new Vector3 (0, 1, 0);

					if (!wasAware) {
						_currentTarget = _target;
						_targetDelay = 0;
					}

					if (_targetDelay < float.Epsilon) {
						_target = perfectTarget;
						_targetDelay = ReactionTime;
					} else
						_targetDelay -= Time.deltaTime;

					if (Vector3.Distance (_target, _currentTarget) > 0.5f) {
						// lerp to look at the target => when lerp completes start fire
						_currentTarget = Vector3.Lerp (_currentTarget, _target, Time.deltaTime * 20);
					}

//					print ("name: " + name + " dist: " + Vector3.Distance (_target, _currentTarget));

					if (Fire) {
						if (Vector3.Distance (_target, _currentTarget) < 0.5f)
							motor.InputFire ();
					}

					if (Look) {
						motor.SetLookTarget (_currentTarget);
						motor.SetFireTarget (_currentTarget);//+ new Vector3 (Random.Range (-1, 1) * TargetRadius, Random.Range (-1, 1) * TargetRadius, Random.Range (-1, 1) * TargetRadius));
					}

					var direction = _currentTarget - transform.position;
					direction.y = 0;
					direction.Normalize ();

					if (vector.magnitude < MinStandDistance * 0.7f)
						motor.InputMovement (new CharacterMovement (-direction, 1.0f));
					else if (vector.magnitude < MinStandDistance)
						motor.InputMovement (new CharacterMovement (-direction, 0.5f));
					else if (vector.magnitude >= MaxStandDistance) {
						if (vector.magnitude < MaxStandDistance * 1.3f)
							motor.InputMovement (new CharacterMovement (direction, 0.5f));
						else
							motor.InputMovement (new CharacterMovement (direction, 1.0f));
					}

					motor.InputWeapon (weapon);
				} else {
					
				
					if (targetobj) {
					//	agent.SetDestination (enemyMotor.transform.position);

						if (walk) {
							//motor.SetFireTarget (target2);
						
							
							motor.InputMovement (new CharacterMovement (target, 0.5f));
							motor.SetLookTarget (target2);
						//	Debug.Log ("walk");
						}


						if (turn) {
							
							//	}
						
							//motor.SetFireTarget (target3);

							motor.InputMovement (new CharacterMovement (target4,0.5f));
							motor.SetLookTarget (target3);
						//	Debug.Log ("turn");

						}

					}
					motor.InputWeapon (0);
				}

            }
        }

        /// <summary>
        /// Helper to get the height of a character.
        /// </summary>
        private float characterHeight(GameObject obj)
        {
            var capsule = obj.GetComponent<CapsuleCollider>();

            if (capsule != null)
                return capsule.height * 0.8f;
            else
                return 1.0f;
        }

        /// <summary>
        /// Helper to get position of the capsule top of a character.
        /// </summary>
        private Vector3 characterTop(GameObject obj)
        {
            return obj.transform.position + Vector3.up * characterHeight(obj);
        }

        /// <summary>
        /// Helper to query crouching state of a character.
        /// </summary>
        private bool isCrouching(GameObject obj)
        {
            var m = obj.GetComponent<CharacterMotor>();

            if (m != null)
                return m.IsCrouching;
            else
                return false;
        }

        /// <summary>
        /// Returns true if a given object is in sight.
        /// </summary>
        private bool isInSight(GameObject enemy)
        {
            var motorTop = characterTop(gameObject);
            var enemyTop = characterTop(enemy);

            var enemyDistance = 99999f;
            var obstacleDistance = 999999f;

            for (int i = 0; i < Physics.RaycastNonAlloc(motorTop, (enemyTop - motorTop).normalized, _hits); i++)
            {
                var hit = _hits[i];
                var dist = Vector3.Distance(hit.point, motorTop);

                if (hit.collider.gameObject == enemy)
                    enemyDistance = dist;
                else if (!hit.collider.isTrigger && hit.collider.gameObject != gameObject)
                {
                    if (dist < obstacleDistance)
                        obstacleDistance = dist;
                }
            }

            return enemyDistance < obstacleDistance;
        }

		public void walking()
		{
			if (!_isAware && turn) {
				walk = true;
				turn = false;
				Debug.Log ("turn");
				target3=new Vector3(targetobj.gameObject.transform.position.x,targetobj.gameObject.transform.position.y,targetobj.gameObject.transform.position.z);
				Invoke ("turning", pathdictance);
			}
		}

		public void turning()
		{
			if (!_isAware && walk) {
				walk = false;
				turn = true;
				target2=new Vector3(back.gameObject.transform.position.x,back.gameObject.transform.position.y,back.gameObject.transform.position.z);
				Debug.Log ("walk");
				Invoke ("walking", pathdictance);
			}
		}




    }

}