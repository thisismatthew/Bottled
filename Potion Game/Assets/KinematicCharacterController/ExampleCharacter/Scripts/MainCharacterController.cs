using System.Collections.Generic;
using UnityEngine;
using Obi;


namespace KinematicCharacterController.Examples
{
    [System.Serializable]
    public enum CharacterState
    {
        Default,
        Climbing,
        Dead,
    }


    public struct PlayerCharacterInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool JumpDown;
        public bool JumpUp;
        public bool CrouchDown;
        public bool CrouchUp;
        public bool SelfDestruct;
        public bool UsePotion;
        public bool Interact;
    }

    public struct AICharacterInputs
    {
        public Vector3 MoveVector;
        public Vector3 LookVector;
    }

    public enum BonusOrientationMethod
    {
        None,
        TowardsGravity,
        TowardsGroundSlopeAndGravity,
    }

    [RequireComponent(typeof(Potion))]
    public class MainCharacterController : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor;
        public Animator anim;
        public CharacterState CurrentCharacterState { get; private set; }

        [Header("Stable Movement")]
        public float DefaultStableMoveSpeed = 10f;
        public float ActualMoveSpeed = 0f;
        public float TopStableMoveSpeed = 18f;
        public float Acceleration = 0.5f;
        public float StableMovementSharpness = 15f;
        public float OrientationSharpness = 10f;

        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 15f;
        public float AirAccelerationSpeed = 15f;
        public float Drag = 0.1f;

        [Header("Jumping")]
        public bool AllowJumpingWhenSliding = false;
        public float TimeToMaxJumpApex = 0.3f;
        public float MaxJumpHeight = 4f;
        public float MinJumpHeight = 1f;
        public float HangTime = 0.3f;
        public float HangtimeGravityDampness = 0.3f;
        //public float JumpScalableForwardSpeed = 10f;
        public float JumpPreGroundingGraceTime = 0f;
        public float JumpPostGroundingGraceTime = 0f;

        [Header("Climbing")]
        public ObiRope CurrentClimbRope;
        private int _currentRopeParticleIndex;
        public int ClimbingSpeed = 1;
        private bool _isClimbing = false;
        private bool _startedClimbing = false;
        public bool JumpingFromClimbing = false;
        private float _upDownInput = 0f;
        
        

        [Header("Misc")]
        public List<Collider> IgnoredColliders = new List<Collider>();
        public BonusOrientationMethod BonusOrientationMethod = BonusOrientationMethod.None;
        public float BonusOrientationSharpness = 10f;
        public Vector3 Gravity = new Vector3(0, -30f, 0);
        public Transform MeshRoot;
        //public Transform CameraFollowPoint;
        public float CrouchedCapsuleHeight = 0.5f;
        public GameObject SmashedCharacterPrefab;
        public GameObject Body;
        public GameObject RightArm;
        public GameObject LeftArm;
        public SpringJoint SpringWeightObject;
        public Rigidbody SpringRoot;
        public ParticleSystem JumpClouds;
        public ParticleSystem RunClouds;

        [Header("Interaction")]
        public GameObject Interactable;
        public Transform GrabPosition;
        public bool NearCauldron;
        public Vector3 CauldronThrowTarget;

        [Header("Overides")]
        public Transform LookTargetOveride = null;

        private Collider[] _probedColliders = new Collider[8];
        private RaycastHit[] _probedHits = new RaycastHit[8];
        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private float _maxJumpVelocity;
        private float _minJumpVelocity;
        private bool _jumpEndRequested = false;
        private bool _jumpRequested = false;
        private bool _jumpConsumed = false;
        private bool _jumpedThisFrame = false;
        private bool _jumpApexReached = false;
        private bool _hangtimeConsumed = false;
        private float _hangtimeInitial;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump = 0f;
        private Vector3 _internalVelocityAdd = Vector3.zero;
        private bool _shouldBeCrouching = false;
        private bool _isCrouching = false;
        private Potion _potion;
        private bool _isHolding = false;

        private Vector3 lastInnerNormal = Vector3.zero;
        private Vector3 lastOuterNormal = Vector3.zero;

        //Arm wiggle
        private Renderer R_ArmRend;
        private Renderer L_ArmRend;
        private float _armPower;
        private float _armSpeed;

        private void Awake()
        {
            //get Potion Component;
            _potion = GetComponent<Potion>();
            // Handle initial state
            TransitionToState(CharacterState.Default);
            // Assign the characterController to the motor
            Motor.CharacterController = this;

            //Get Arms Renderer
            R_ArmRend = RightArm.GetComponent<Renderer>();
            L_ArmRend = LeftArm.GetComponent<Renderer>();
            _hangtimeInitial = HangTime;
        }

        private void Update()
        {
            Gravity.y = -(2 * MaxJumpHeight) / Mathf.Pow(TimeToMaxJumpApex, 2);
            _maxJumpVelocity = (TimeToMaxJumpApex * Mathf.Abs(Gravity.y));
            _minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Gravity.y) * MinJumpHeight);
            CharacterMoving();
            //lol lets not forget to remove this... or at least add it to the player input struct.
            if (Input.GetKeyDown(KeyCode.X))
            {
                anim.SetTrigger("Dance");
            }
        }

        /// <summary>
        /// Handles movement state transitions and enter/exit callbacks
        /// </summary>
        public void TransitionToState(CharacterState newState)
        {
            CharacterState tmpInitialState = CurrentCharacterState;
            OnStateExit(tmpInitialState, newState);
            CurrentCharacterState = newState;
            OnStateEnter(newState, tmpInitialState);
        }

        /// <summary>
        /// Event when entering a state
        /// </summary>
        public void OnStateEnter(CharacterState state, CharacterState fromState)
        {
            //Debug.Log("entering state: " + state.ToString());
            switch (state)
            {
                case CharacterState.Default:
                    {
                        Body.active = true;
                        SpringWeightObject.transform.position = transform.position;
                        SpringWeightObject.connectedBody = SpringRoot;
                        break;
                    }
                case CharacterState.Climbing:
                    {
                        _startedClimbing = true;
                        break;
                    }
                case CharacterState.Dead:
                    {
                        Body.active = false;
                        Instantiate(SmashedCharacterPrefab,transform.position, transform.rotation, transform.parent);
                        SpringWeightObject.connectedBody = null;
                        break;
                    }
            }
        }

        /// <summary>
        /// Event when exiting a state
        /// </summary>
        public void OnStateExit(CharacterState state, CharacterState toState)
        {
            //Debug.Log("leaving state: " + state.ToString());
            switch (state)
            {
                case CharacterState.Default:
                    {
                        break;
                    }
                case CharacterState.Climbing:
                    {
                        CurrentClimbRope = null;
                        break;
                    }
            }
        }


        

        /// <summary>
        /// This is called every frame by ExamplePlayer in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs)
        {

            // Clamp input
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

            if (LookTargetOveride == null)
                _lookInputVector = _moveInputVector.normalized;
            else
                _lookInputVector = (LookTargetOveride.position - this.transform.position).normalized;

            // Move and look inputs
            _moveInputVector = cameraPlanarRotation * moveInputVector;
/*            Debug.DrawLine(_moveInputVector, _moveInputVector * 2, Color.green);
            Debug.Log(cameraPlanarDirection);*/

            //let the character self destruct at any stage
            if (inputs.SelfDestruct)
            {
                GetComponent<PlayerDeathTrigger>().SmashCharacter();
            }

            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        anim.SetBool("Climbing", false);
                        // Jumping input
                        if (inputs.JumpDown)
                        {
                            _timeSinceJumpRequested = 0f;
                            _jumpRequested = true;
                            
                        }

                        if (inputs.JumpUp && !Motor.GroundingStatus.IsStableOnGround)
                        {
                            JumpingFromClimbing = false;
                            _jumpEndRequested = true;
                        }


                        // Crouching input
                        if (inputs.CrouchDown)
                        {
                            _shouldBeCrouching = true;

                            if (!_isCrouching)
                            {
                                _isCrouching = true;
                                Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                                MeshRoot.localScale = new Vector3(1f, 0.5f, 1f);
                            }
                        }
                        else if (inputs.CrouchUp)
                        {
                            _shouldBeCrouching = false;
                        }

                        //Use input
                        //this is pretty messy, the rigidbody gets detroyed and remade when an object is picked up/down. 
                        if (inputs.UsePotion)
                        {
                            if (Interactable == null)
                            {
                                _potion.UsePotion();
                            }
                            else
                            {
                                if (_isHolding)
                                {
                                    Interactable.AddComponent<Rigidbody>(); 
                                    Interactable.transform.parent = null;
                                    IgnoredColliders.Remove(Interactable.GetComponent<BoxCollider>());
                                    _isHolding = false;
                                    if(NearCauldron)
                                        Interactable.GetComponent<Pickupable>().ThrowToTarget(CauldronThrowTarget);
                                }
                                else
                                {

                                    _isHolding = true;
                                    Interactable.transform.parent = this.transform;
                                    Interactable.GetComponent<Pickupable>().MoveToTarget(GrabPosition.position);
                                    IgnoredColliders.Add(Interactable.GetComponent<BoxCollider>());
                                    Destroy(Interactable.GetComponent<Rigidbody>());


                                }

                            }

                        }
                        break;

                    }
                case CharacterState.Climbing:
                    {
                        anim.SetBool("Climbing", true);
                        _isClimbing = true;

                        _upDownInput = inputs.MoveAxisForward;




                        //if a jump is requested just drop the player off the ledge
                        if (inputs.CrouchDown)
                        {
                            TransitionToState(CharacterState.Default);
                            _isClimbing = false;
                            JumpingFromClimbing = true;

                        }
                        

                        if (inputs.JumpDown)
                        {
                            _timeSinceJumpRequested = 0f;
                            _jumpRequested = true;
                            JumpingFromClimbing = true;
                        }

                        if (inputs.JumpUp)
                        {
                            _isClimbing = true;
                            _jumpRequested = false;
                            JumpingFromClimbing = false;
                        }

                        break;
                    }
            }
                 
        }

        /// <summary>
        /// This is called every frame by the AI script in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref AICharacterInputs inputs)
        {
            _moveInputVector = inputs.MoveVector;
            _lookInputVector = inputs.LookVector;
        }

        private Quaternion _tmpTransientRot;


        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        if (_lookInputVector.sqrMagnitude > 0f && OrientationSharpness > 0f)
                        {
                            // Smoothly interpolate from current to target look direction
                            Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                            // Set the current rotation (which will be used by the KinematicCharacterMotor)
                            currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
                        }

                        Vector3 currentUp = (currentRotation * Vector3.up);
                        if (BonusOrientationMethod == BonusOrientationMethod.TowardsGravity)
                        {
                            // Rotate from current up to invert gravity
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                        else if (BonusOrientationMethod == BonusOrientationMethod.TowardsGroundSlopeAndGravity)
                        {
                            if (Motor.GroundingStatus.IsStableOnGround)
                            {
                                Vector3 initialCharacterBottomHemiCenter = Motor.TransientPosition + (currentUp * Motor.Capsule.radius);

                                Vector3 smoothedGroundNormal = Vector3.Slerp(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * currentRotation;

                                // Move the position to create a rotation around the bottom hemi center instead of around the pivot
                                Motor.SetTransientPosition(initialCharacterBottomHemiCenter + (currentRotation * Vector3.down * Motor.Capsule.radius));
                            }
                            else
                            {
                                Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                                currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                            }
                        }
                        else
                        {
                            Vector3 smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                        break;
                    }

                
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Ground movement
                        if (Motor.GroundingStatus.IsStableOnGround)
                        {
                            //make sure we have landed from a ledge jump
                            JumpingFromClimbing = false;
                            float currentVelocityMagnitude = currentVelocity.magnitude;


                            Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;
                            if (currentVelocityMagnitude > 0f && Motor.GroundingStatus.SnappingPrevented)
                            {
                                // Take the normal from where we're coming from
                                Vector3 groundPointToCharacter = Motor.TransientPosition - Motor.GroundingStatus.GroundPoint;
                                if (Vector3.Dot(currentVelocity, groundPointToCharacter) >= 0f)
                                {
                                    effectiveGroundNormal = Motor.GroundingStatus.OuterGroundNormal;
                                }
                                else
                                {
                                    effectiveGroundNormal = Motor.GroundingStatus.InnerGroundNormal;
                                }
                            }

                            // Reorient velocity on slope
                            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                            //update Acceleration to movespeed
                            if (currentVelocity.magnitude < DefaultStableMoveSpeed - 0.5f)
                            {
                                ActualMoveSpeed = DefaultStableMoveSpeed;
                            }
                            else
                            {
                                if (ActualMoveSpeed < TopStableMoveSpeed)
                                {
                                    ActualMoveSpeed += Acceleration;
                                }
                            }

                            // Calculate target velocity
                            Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                            Vector3 targetMovementVelocity = reorientedInput * ActualMoveSpeed;


                            // Smooth movement Velocity
                            currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-StableMovementSharpness * deltaTime));
                        }
                        // Air movement
                        else
                        {
                            // Add move input
                            if (_moveInputVector.sqrMagnitude > 0f)
                            {
                                Vector3 addedVelocity = _moveInputVector * AirAccelerationSpeed * deltaTime;

                                Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                                // Limit air velocity from inputs
                                if (currentVelocityOnInputsPlane.magnitude < MaxAirMoveSpeed)
                                {
                                    // clamp addedVel to make total vel not exceed max vel on inputs plane
                                    Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, MaxAirMoveSpeed);
                                    addedVelocity = newTotal - currentVelocityOnInputsPlane;
                                }
                                else
                                {
                                    // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                                    if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                                    {
                                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                                    }
                                }

                                // Prevent air-climbing sloped walls
                                if (Motor.GroundingStatus.FoundAnyGround)
                                {
                                    if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                                    {
                                        Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                                        addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                                    }
                                }

                                // Apply added velocity
                                currentVelocity += addedVelocity;
                            }

                            // Gravity
                            currentVelocity += Gravity * deltaTime;

                            // Drag
                            currentVelocity *= (1f / (1f + (Drag * deltaTime)));

                        }

                        // Handle jumping
                        _jumpedThisFrame = false;
                        _timeSinceJumpRequested += deltaTime;
                        Vector3 jumpDirection = Motor.CharacterUp;
                        if (_jumpRequested)
                        {
                            // See if we actually are allowed to jump
                            if (!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime))
                            {
                                // Calculate jump direction before ungrounding
                                if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                                {
                                    jumpDirection = Motor.GroundingStatus.GroundNormal;
                                }

                                // Makes the character skip ground probing/snapping on its next update. 
                                // If this line weren't here, the character would remain snapped to the ground when trying to jump. 
                                Motor.ForceUnground();

                                // Add to the return velocity and reset jump state
                                currentVelocity += (jumpDirection * _maxJumpVelocity) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                                
                                //currentVelocity += (_moveInputVector * JumpScalableForwardSpeed);
                                _jumpRequested = false;
                                _jumpConsumed = true;
                                _jumpedThisFrame = true;
                            }
                        }

                        if (_jumpEndRequested)
                        {
                           
                            //check we are traveling up
                            if (currentVelocity.y > _minJumpVelocity)
                            {
                                //TODO also reduce the forward velocity of the jump. 
                                currentVelocity += (jumpDirection * _minJumpVelocity) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                                
                            }
                            _jumpEndRequested = false;
                        }
                        
                        //we've hit the apex and we're on our way down
                        if (_jumpConsumed)
                        {
                            //Debug.Log("current vel: " + currentVelocity);
                            Vector3 down = transform.position;
                            down.y = 0;

                            if (currentVelocity.y < -0.01f & _hangtimeConsumed == false)
                            {
                                Debug.DrawLine(transform.position, down, Color.green);
                                //Debug.Log("current vel: " + currentVelocity);
                                _jumpApexReached = true;
                                //Debug.Log("Apex reached");
                            }

                            if (_jumpApexReached)
                            {
                                if (HangTime >= 0)
                                {
                                    //Debug.Log("hanging");
                                    HangTime -= Time.deltaTime;
                                    currentVelocity.y = currentVelocity.y * HangtimeGravityDampness;
                                }
                                else
                                {
                                    //Debug.Log("hanging done");
                                    _jumpApexReached = false;
                                    _hangtimeConsumed = true;
                                }
                            }

                        }
                        else
                        {
                            //Debug.Log("hanging restored");
                            _jumpApexReached = false;
                            _hangtimeConsumed = false;
                            HangTime = _hangtimeInitial;
                        }

                        // Take into account additive velocity
                        if (_internalVelocityAdd.sqrMagnitude > 0f)
                        {
                            currentVelocity += _internalVelocityAdd;
                            _internalVelocityAdd = Vector3.zero;
                        }
                        break;
                    }
                case CharacterState.Climbing:
                    {
                        
                        //if this is the first update where we are climbing set velocity to 0
                        if (_startedClimbing == true)
                        {
                            _currentRopeParticleIndex = CurrentClimbRope.FindClosestRopeParticle(transform.position);
                            
                            //all we need are the two end points of the spline 
                            currentVelocity = Vector3.zero;
                            _startedClimbing = false;
                        }

                        if (_isClimbing)
                        {
                            
                            Vector3 target = Vector3.zero;
                            Debug.Log(_upDownInput);
                            //move the target spline point along, and snap the character to it. 
                            if (_upDownInput < 0 && _currentRopeParticleIndex <= CurrentClimbRope.particleCount)
                            {
                                _currentRopeParticleIndex += ClimbingSpeed;
                                anim.SetFloat("ClimbState", 1);
                                anim.SetBool("ClimbUp", false);
                            }

                            else if (_upDownInput > 0 && _currentRopeParticleIndex >= 0)
                            {
                                _currentRopeParticleIndex -= ClimbingSpeed;
                                anim.SetFloat("ClimbState", 1);
                                anim.SetBool("ClimbUp", true);
                            }
                            else
                            {
                                anim.SetFloat("ClimbState", 0);
                            }

                            target = CurrentClimbRope.GetParticlePosition(_currentRopeParticleIndex);
                            //now lets make sure that the character is oriented and positioned right fo animations
                            //offset the snapped position by the hieght of the player to make it hang below the spline
                            target.y -= Motor.Capsule.height;
                            //now move the character backwards by its width
                            target += -(Motor.CharacterForward * (Motor.Capsule.radius*2));



                            Motor.MoveCharacter(target);

                            /*
                            //if we moved to the end of the Rope take us off
                            if (_currentRopeParticleIndex == CurrentClimbRope.particleCount || _currentRopeParticleIndex == 0)
                            {
                                TransitionToState(CharacterState.Default);
                            }*/
                        }


                        // Handle jumping from climbing
                        _timeSinceJumpRequested += deltaTime;
                        Vector3 jumpDirection = Motor.CharacterUp;
                        if (_jumpRequested)
                        {
                            TransitionToState(CharacterState.Default);
                            // Add to the return velocity and reset jump state
                            currentVelocity += (jumpDirection * _maxJumpVelocity) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                            //currentVelocity += (_moveInputVector * JumpScalableForwardSpeed);
                            _jumpRequested = false;
                            _isClimbing = false;
                        }
                        break;
                    }
                case CharacterState.Dead:
                    {
                        Debug.Log("Velocity Stopped ");
                        currentVelocity = Vector3.zero;
                        break;
                    }
            }
            anim.SetFloat("VerticalVelocity", currentVelocity.y);
            anim.SetFloat("ForwardVelocity", Mathf.Sqrt(currentVelocity.z* currentVelocity.z + currentVelocity.x * currentVelocity.x));
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        // Handle jump-related values
                        {
                            // Handle jumping pre-ground grace period
                            if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                            {
                                _jumpRequested = false;
                            }

                            if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
                            {
                                // If we're on a ground surface, reset jumping values
                                if (!_jumpedThisFrame)
                                {
                                    _jumpConsumed = false;
                                }
                                _timeSinceLastAbleToJump = 0f;
                            }
                            else
                            {
                                // Keep track of time since we were last able to jump (for grace period)
                                _timeSinceLastAbleToJump += deltaTime;
                            }
                        }

                        // Handle uncrouching
                        if (_isCrouching && !_shouldBeCrouching)
                        {
                            // Do an overlap test with the character's standing height to see if there are any obstructions
                            Motor.SetCapsuleDimensions(0.5f, 2f, 1f);
                            if (Motor.CharacterOverlap(
                                Motor.TransientPosition,
                                Motor.TransientRotation,
                                _probedColliders,
                                Motor.CollidableLayers,
                                QueryTriggerInteraction.Ignore) > 0)
                            {
                                // If obstructions, just stick to crouching dimensions
                                Motor.SetCapsuleDimensions(0.5f, CrouchedCapsuleHeight, CrouchedCapsuleHeight * 0.5f);
                            }
                            else
                            {
                                // If no obstructions, uncrouch
                                MeshRoot.localScale = new Vector3(1f, 1f, 1f);
                                _isCrouching = false;
                            }
                        }
                        break;
                    }
                case CharacterState.Climbing:
                    {
                        // Handle jumping pre-ground grace period
                        if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                        {
                            _jumpRequested = false;
                        }
                        break;
                    }
            }
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            // Handle landing and leaving ground
            if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLanded();
            }
            else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround)
            {
                OnLeaveStableGround();
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (IgnoredColliders.Count == 0)
            {
                return true;
            }

            if (IgnoredColliders.Contains(coll))
            {
                return false;
            }

            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void AddVelocity(Vector3 velocity)
        {
            switch (CurrentCharacterState)
            {
                case CharacterState.Default:
                    {
                        _internalVelocityAdd += velocity;
                        break;
                    }
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        protected void OnLanded()
        {
            anim.SetTrigger("Grounded");
            anim.SetBool("Airborne", false);
            JumpClouds.Play();
        }

        protected void OnLeaveStableGround()
        {
            anim.SetTrigger("Jump");
            anim.SetBool("Airborne", true);
            JumpClouds.Play();
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }

        private void CharacterMoving()
        {
            Vector3 vel = Camera.main.transform.forward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
            Vector3 localVel = transform.InverseTransformDirection(vel);
            if (localVel.z > 0.01f && Motor.GroundingStatus.IsStableOnGround)
            {
                anim.SetBool("Run", true);
                _armPower = Mathf.SmoothStep(0.5f, 0.85f, -0.02f);
                _armSpeed = Mathf.SmoothStep(3f, 4.5f, 0.02f);
            }
            else
            {
                anim.SetBool("Run", false);
                _armPower = Mathf.SmoothStep(0.85f, 0.5f, 0.02f);
                _armSpeed = Mathf.SmoothStep(4.5f, 3f, -0.02f);
            }

            Debug.Log("goin crazy " + ActualMoveSpeed);
            if (ActualMoveSpeed > 11f && ActualMoveSpeed < 12f)
            {
                RunClouds.Play();
            }

            R_ArmRend.material.SetFloat("_Power", _armPower);
            R_ArmRend.material.SetFloat("_WobbleSpeed", _armSpeed);
            L_ArmRend.material.SetFloat("_Power", _armPower);
            L_ArmRend.material.SetFloat("_WobbleSpeed", _armSpeed);
        }
    }
}