using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace SmallScaleInc.TopDownPixelCharactersPack1
{
    public class AnimationController : MonoBehaviour
    {
        private Animator animator;
        public string currentDirection = "isEast"; // Default direction
        public bool isCurrentlyRunning; //for debugging purposes
        public bool isCrouching = false;

        public float rollTime = 0.5f; //the time it takes to peform a roll before swtiching back to default animation.

        private PlayerInput controls;

        private void Awake()
        {
            controls = new PlayerInput();

            controls.Player.RightTrigger.canceled += ctx => OnRightTrigger(ctx);
        }


        void Start()
        {
            animator = GetComponent<Animator>();
            animator.SetBool("isEast", true); //Sets the default direction to east.
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isCrouchRunning", false);
            animator.SetBool("isCrouchIdling", false);
        }

        void Update()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            HandleMovement();
            HandleAttackAttack();

            //Other input actions:
            if(Input.GetKeyDown(KeyCode.C))
            {

                if(isCrouching == false)
                {
                    TriggerCrouchIdleAnimation();
                    isCrouching = true;
                }
                else
                {
                    isCrouching = false;
                    // Reset the crouch idle parameters after a delay or at the end of the animation
                    ResetCrouchIdleParameters();
                }
            }
            //else if (Input.GetKey(KeyCode.Alpha1))
            //{
            //    TriggerSpecialAbility1Animation();
            //}
            //else if (Input.GetKey(KeyCode.Alpha2))
            //{
            //    TriggerSpecialAbility2Animation();
            //}
            //else if (Input.GetKey(KeyCode.Alpha3))
            //{
            //    TriggerCastSpellAnimation();
            //}
            //else if (Input.GetKey(KeyCode.Alpha4))
            //{
            //    TriggerKickAnimation();
            //}
            else if(Input.GetKey(KeyCode.Alpha5))
            {
                TriggerPummelAnimation();
            }
            else if(Input.GetKey(KeyCode.Alpha6))
            {
                TriggerAttackSpinAnimation();
            }
            else if(Input.GetKey(KeyCode.LeftShift) && isCurrentlyRunning)
            {
                TriggerFlipAnimation();
            }
            else if(Input.GetKey(KeyCode.LeftControl) && isCurrentlyRunning)
            {
                TriggerRollAnimation();
            }
            else if(Input.GetKey(KeyCode.LeftAlt) && isCurrentlyRunning)
            {
                TriggerSlideAnimation();
            }

        }

        #region New Inputs


        private void OnMove(InputValue inputValue)
        {
            Vector2 input = inputValue.Get<Vector2>();

            isRunning = input.y > 0;
            isRunningBackwards = input.y < 0;
            isStrafingLeft = input.x < 0;
            isStrafingRight = input.x > 0;

            //// Capture movement input states
            //isRunning = Input.GetKey(KeyCode.W);
            //isRunningBackwards = Input.GetKey(KeyCode.S);
            //isStrafingLeft = Input.GetKey(KeyCode.A);
            //isStrafingRight = Input.GetKey(KeyCode.D);
        }

        private void OnButtonSouth(InputValue inputValue)
        {
            TriggerSpecialAbility1Animation();

        }

        private void OnButtonEast(InputValue inputValue)
        {
            TriggerSpecialAbility2Animation();
        }

        private void OnButtonWest(InputValue inputValue)
        {
            TriggerCastSpellAnimation();
        }

        private void OnButtonNorth(InputValue inputValue)
        {
            TriggerKickAnimation();
        }

        private void OnLeftTrigger(InputValue inputValue)
        {
            if(isCurrentlyRunning)
                TriggerFlipAnimation();
        }

        private void OnRightTrigger(InputValue inputValue)
        {
            //if(inputValue.isPressed)
            //{
            //    Debug.Log(" <<< ATTACK ATTACK >>> ");
            //    bool isRunning = isCurrentlyRunning;
            //    // Determine the current direction and trigger the appropriate attack attack or attack run attack
            //    if(animator.GetBool("isNorth"))
            //        TriggerAttack(isRunning, "North");
            //    else if(animator.GetBool("isSouth"))
            //        TriggerAttack(isRunning, "South");
            //    else if(animator.GetBool("isEast"))
            //        TriggerAttack(isRunning, "East");
            //    else if(animator.GetBool("isWest"))
            //        TriggerAttack(isRunning, "West");
            //    else if(animator.GetBool("isNorthEast"))
            //        TriggerAttack(isRunning, "NorthEast");
            //    else if(animator.GetBool("isNorthWest"))
            //        TriggerAttack(isRunning, "NorthWest");
            //    else if(animator.GetBool("isSouthEast"))
            //        TriggerAttack(isRunning, "SouthEast");
            //    else if(animator.GetBool("isSouthWest"))
            //        TriggerAttack(isRunning, "SouthWest");
            //}
            //else
            //{
            //    Debug.Log(" >>> RESTORING ATTACK DIRECTION <<<");
            //    // Reset attack attack parameters and return to idle state
            //    ResetAttackAttackParameters();
            //}
        }

        private void OnRightTrigger(InputAction.CallbackContext context)
        {
            float triggerValue = context.ReadValue<float>();

            Debug.Log($"---> triggerValue: {triggerValue}");

            if(triggerValue > 0)
                return;

            // Reset attack attack parameters and return to idle state
            ResetAttackAttackParameters();
            // No need to explicitly set the idle state here since it should naturally follow from resetting the attack parameters
            // and the movement handling logic already sets the appropriate idle direction based on the last known direction.

        }


        #endregion

        void UpdateDirection(string newDirection)
        {
            // Reset all directional booleans to false
            SetDirectionBools(false, false, false, false, false, false, false, false);

            // Set the new direction to true
            animator.SetBool(newDirection, true);

            // Update the current direction
            currentDirection = newDirection;

            //Reset the parameters to restart animations from new directions. 
            ResetAttackAttackParameters();

        }
        public bool isRunning;
        public bool isRunningBackwards;
        public bool isStrafingLeft;
        public bool isStrafingRight;

        Vector2 directionToMouse = Vector2.zero;
        private void OnLook(InputValue inputValue)
        {
            directionToMouse = inputValue.Get<Vector2>();
        }
        void HandleMovement()
        {
            // Calculate direction based on mouse position
            //Vector3 mouseScreenPosition = Input.mousePosition;

            var gp = Gamepad.current;

            if(controls.devices != null)
                foreach(var device in controls.devices)
                {
                    if(device is Mouse)
                    {
                        Vector3 mouseScreenPosition = controls.Player.Look.ReadValue<Vector2>();
                        mouseScreenPosition.z = Camera.main.transform.position.z - transform.position.z;
                        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

                        Vector3 directionToMouse = mouseWorldPosition - transform.position;
                    }
                }

            directionToMouse.Normalize(); // Normalize the direction vector
            //Debug.Log("DIRECTION TO MOUSE = " + directionToMouse);

            // Determine the closest cardinal or intercardinal direction
            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            if(angle < 0) angle += 360;

            string newDirection = DetermineDirectionFromAngle(angle);
            UpdateDirection(newDirection);
            string movementDirection = newDirection.Substring(2); // Remove "is" from the direction name

            //// Capture movement input states
            //isRunning = Input.GetKey(KeyCode.W);
            //isRunningBackwards = Input.GetKey(KeyCode.S);
            //isStrafingLeft = Input.GetKey(KeyCode.A);
            //isStrafingRight = Input.GetKey(KeyCode.D);

            // Set general movement boolean
            isCurrentlyRunning = isRunning || isRunningBackwards || isStrafingLeft || isStrafingRight;

            // Reset all directional movement parameters
            ResetAllMovementBools();

            // Update animator with movement conditions
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isRunningBackwards", isRunningBackwards);
            animator.SetBool("isStrafingLeft", isStrafingLeft);
            animator.SetBool("isStrafingRight", isStrafingRight);
            animator.SetBool("isCrouchRunning", isRunning);

            // Set specific movement animations
            if(isCrouching)
            {
                SetMovementAnimation(isRunning, "CrouchRun", movementDirection);
            }
            else
            {
                SetMovementAnimation(isRunning, "Move", movementDirection);
                SetMovementAnimation(isRunningBackwards, "RunBackwards", movementDirection);
                SetMovementAnimation(isStrafingLeft, "StrafeLeft", movementDirection);
                SetMovementAnimation(isStrafingRight, "StrafeRight", movementDirection);
                SetMovementAnimation(isRunningBackwards, "Move", movementDirection);
                SetMovementAnimation(isStrafingLeft, "Move", movementDirection);
                SetMovementAnimation(isStrafingRight, "Move", movementDirection);
            }
        }

        void SetMovementAnimation(bool isActive, string baseKey, string direction)
        {
            if(isActive)
            {
                string animationKey = $"{baseKey}{direction}";
                animator.SetBool(animationKey, true);
            }
        }

        void ResetAllMovementBools()
        {
            string[] directions = new string[] { "North", "South", "East", "West", "NorthEast", "NorthWest", "SouthEast", "SouthWest" };
            foreach(string baseKey in new string[] { "Move", "RunBackwards", "StrafeLeft", "StrafeRight" })
            {
                foreach(string direction in directions)
                {
                    animator.SetBool($"{baseKey}{direction}", false);
                }
            }

            animator.SetBool("CrouchRunNorth", false);
            animator.SetBool("CrouchRunSouth", false);
            animator.SetBool("CrouchRunEast", false);
            animator.SetBool("CrouchRunWest", false);
            animator.SetBool("CrouchRunNorthEast", false);
            animator.SetBool("CrouchRunNorthWest", false);
            animator.SetBool("CrouchRunSouthEast", false);
            animator.SetBool("CrouchRunSouthWest", false);
        }


        string DetermineDirectionFromAngle(float angle)
        {
            // Adjust the ranges based on your snapped angles
            if((angle >= 330 || angle < 15))
                return "isEast"; // Corresponds to angle 0
            else if((angle >= 15 && angle < 60))
                return "isNorthEast"; // Corresponds to angle 35
            else if((angle >= 60 && angle < 120))
                return "isNorth"; // Corresponds to angle 90
            else if((angle >= 120 && angle < 165))
                return "isNorthWest"; // Corresponds to angle 150
            else if((angle >= 165 && angle < 195))
                return "isWest"; // Corresponds to angle 180
            else if((angle >= 195 && angle < 240))
                return "isSouthWest"; // Corresponds to angle 215
            else if((angle >= 240 && angle < 300))
                return "isSouth"; // Corresponds to angle 270
            else if((angle >= 300 && angle < 345))
                return "isSouthEast"; // Corresponds to angle 330

            return "isEast"; // Default direction
        }


        void SetDirectionBools(bool isWest, bool isEast, bool isSouth, bool isSouthWest, bool isNorthEast, bool isSouthEast, bool isNorth, bool isNorthWest)
        {
            animator.SetBool("isWest", isWest);
            animator.SetBool("isEast", isEast);
            animator.SetBool("isSouth", isSouth);
            animator.SetBool("isSouthWest", isSouthWest);
            animator.SetBool("isNorthEast", isNorthEast);
            animator.SetBool("isSouthEast", isSouthEast);
            animator.SetBool("isNorth", isNorth);
            animator.SetBool("isNorthWest", isNorthWest);
        }


        //Default Attacks:

        bool HasResetAttack = true;
        void HandleAttackAttack()
        {
            var ms = Mouse.current;
            var gp = Gamepad.current;

            // Check if the left mouse button is currently being held down
            if(ms.rightButton.isPressed || (gp != null && gp.rightTrigger.isPressed))
            {
                HasResetAttack = false;
                Debug.Log(" <<< ATTACK ATTACK >>> ");
                bool isRunning = isCurrentlyRunning;
                // Determine the current direction and trigger the appropriate attack attack or attack run attack
                if(animator.GetBool("isNorth"))
                    TriggerAttack(isRunning, "North");
                else if(animator.GetBool("isSouth"))
                    TriggerAttack(isRunning, "South");
                else if(animator.GetBool("isEast"))
                    TriggerAttack(isRunning, "East");
                else if(animator.GetBool("isWest"))
                    TriggerAttack(isRunning, "West");
                else if(animator.GetBool("isNorthEast"))
                    TriggerAttack(isRunning, "NorthEast");
                else if(animator.GetBool("isNorthWest"))
                    TriggerAttack(isRunning, "NorthWest");
                else if(animator.GetBool("isSouthEast"))
                    TriggerAttack(isRunning, "SouthEast");
                else if(animator.GetBool("isSouthWest"))
                    TriggerAttack(isRunning, "SouthWest");
            }
            // Check if the left mouse button was released
            else /*if(Input.GetMouseButtonUp(1))*/
            {
                // Reset attack attack parameters and return to idle state
                if(!HasResetAttack)
                {
                    Debug.Log(" >>> RESTORING ATTACK DIRECTION <<<");
                    ResetAttackAttackParameters();
                    HasResetAttack = true;
                }
                // No need to explicitly set the idle state here since it should naturally follow from resetting the attack parameters
                // and the movement handling logic already sets the appropriate idle direction based on the last known direction.
            }
        }

        void TriggerAttack(bool isRunning, string direction)
        {
            // Randomly choose between AttackAttack and Attack2 for the attack type
            int attackType = Random.Range(0, 2);  // Generates 0 or 1
            string attackParam = (attackType == 0 ? "AttackAttack" : "Attack2") + direction;

            animator.SetBool(attackParam, true);

            // Set the specific attacking flags based on whether the character is running or not
            animator.SetBool("isAttackAttacking", !isRunning);
            animator.SetBool("isAttackRunning", isRunning);
        }



        void ResetAttackAttackParameters()
        {
            // Reset both AttackAttack and Attack2 parameters for all dir ections
            string[] directions = new string[] { "North", "South", "East", "West", "NorthEast", "NorthWest", "SouthEast", "SouthWest" };
            foreach(string dir in directions)
            {
                animator.SetBool("AttackAttack" + dir, false);
                animator.SetBool("Attack2" + dir, false);
                animator.SetBool("AttackRun" + dir, false);
            }

            // After resetting attack attack, restore the direction
            RestoreDirectionAfterAttack();
        }



        void RestoreDirectionAfterAttack()
        {
            // Instead of forcing the player to face a certain direction,
            // we can keep the direction they are currently facing.
            
            animator.SetBool("isAttackAttacking", false);
            animator.SetBool("isAttackRunning", false);
            animator.SetBool("isRunning", false);
 
            animator.SetBool(currentDirection, true);
        }

        //Take Damage:
        public void TriggerTakeDamageAnimation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isTakeDamage' to true to initiate the take damage animation
            animator.SetBool("isTakeDamage", true);
            // Determine the current direction and trigger the appropriate take damage animation
            if(animator.GetBool("isNorth")) animator.SetBool("TakeDamageNorth", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("TakeDamageSouth", true);
            else if(animator.GetBool("isEast")) animator.SetBool("TakeDamageEast", true);
            else if(animator.GetBool("isWest")) animator.SetBool("TakeDamageWest", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("TakeDamageNorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("TakeDamageNorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("TakeDamageSouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("TakeDamageSouthWest", true);

            // Optionally, reset the take damage parameters after a delay or at the end of the animation
            StartCoroutine(ResetTakeDamageParameters());
        }

        IEnumerator ResetTakeDamageParameters()
        {
            // Wait for the length of the animation before resetting
            yield return new WaitForSeconds(1); // Adjust the wait time based on your animation length

            // Reset all take damage parameters to false
            animator.SetBool("isTakeDamage", false);
            animator.SetBool("TakeDamageNorth", false);
            animator.SetBool("TakeDamageSouth", false);
            animator.SetBool("TakeDamageEast", false);
            animator.SetBool("TakeDamageWest", false);
            animator.SetBool("TakeDamageNorthEast", false);
            animator.SetBool("TakeDamageNorthWest", false);
            animator.SetBool("TakeDamageSouthEast", false);
            animator.SetBool("TakeDamageSouthWest", false);

            // Restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }

        //Crouch:
        public void TriggerCrouchIdleAnimation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isCrouchIdling' to true to initiate the crouch idle animation
            animator.SetBool("isCrouchIdling", true);

            // Determine the current direction and trigger the appropriate crouch idle animation
            if(animator.GetBool("isNorth")) animator.SetBool("CrouchIdleNorth", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("CrouchIdleSouth", true);
            else if(animator.GetBool("isEast")) animator.SetBool("CrouchIdleEast", true);
            else if(animator.GetBool("isWest")) animator.SetBool("CrouchIdleWest", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("CrouchIdleNorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("CrouchIdleNorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("CrouchIdleSouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("CrouchIdleSouthWest", true);

        }

        public void ResetCrouchIdleParameters()
        {
            // Reset all crouch idle parameters to false
            animator.SetBool("isCrouchIdling", false);
            animator.SetBool("CrouchIdleNorth", false);
            animator.SetBool("CrouchIdleSouth", false);
            animator.SetBool("CrouchIdleEast", false);
            animator.SetBool("CrouchIdleWest", false);
            animator.SetBool("CrouchIdleNorthEast", false);
            animator.SetBool("CrouchIdleNorthWest", false);
            animator.SetBool("CrouchIdleSouthEast", false);
            animator.SetBool("CrouchIdleSouthWest", false);

            // Optionally, restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }



        //Die
        public void TriggerDie()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Ensure that we're indicating a death state is happening
            animator.SetBool("isDie", true);
            // Check the current direction and trigger the appropriate die animation
            if(currentDirection.Equals("isNorth")) TriggerDeathAnimation("DieNorth");
            else if(currentDirection.Equals("isSouth")) TriggerDeathAnimation("DieSouth");
            else if(currentDirection.Equals("isEast")) TriggerDeathAnimation("DieEast");
            else if(currentDirection.Equals("isWest")) TriggerDeathAnimation("DieWest");
            else if(currentDirection.Equals("isNorthEast")) TriggerDeathAnimation("DieNorthEast");
            else if(currentDirection.Equals("isNorthWest")) TriggerDeathAnimation("DieNorthWest");
            else if(currentDirection.Equals("isSouthEast")) TriggerDeathAnimation("DieSouthEast");
            else if(currentDirection.Equals("isSouthWest")) TriggerDeathAnimation("DieSouthWest");
        }

        private void TriggerDeathAnimation(string deathDirectionParam)
        {
            // Trigger the specific death direction
            animator.SetBool(deathDirectionParam, true);
            // Reset all die direction parameters to ensure only the correct direction is triggered
            StartCoroutine(ResetDieParameters());
        }

        IEnumerator ResetDieParameters()
        {
            yield return new WaitForSeconds(2);

            animator.SetBool("isDie", false);
            animator.SetBool("DieNorth", false);
            animator.SetBool("DieSouth", false);
            animator.SetBool("DieEast", false);
            animator.SetBool("DieWest", false);
            animator.SetBool("DieNorthEast", false);
            animator.SetBool("DieNorthWest", false);
            animator.SetBool("DieSouthEast", false);
            animator.SetBool("DieSouthWest", false);
            // Restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }

        // Special Ability 1:
        public void TriggerSpecialAbility1Animation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isSpecialAbility1' to true to initiate the special ability animation
            animator.SetBool("isSpecialAbility1", true);

            // Determine the current direction and trigger the appropriate special ability animation
            if(animator.GetBool("isNorth")) animator.SetBool("SpecialAbility1North", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("SpecialAbility1South", true);
            else if(animator.GetBool("isEast")) animator.SetBool("SpecialAbility1East", true);
            else if(animator.GetBool("isWest")) animator.SetBool("SpecialAbility1West", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("SpecialAbility1NorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("SpecialAbility1NorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("SpecialAbility1SouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("SpecialAbility1SouthWest", true);

            // Reset the special ability parameters after a delay or at the end of the animation
            StartCoroutine(ResetSpecialAbility1Parameters());
        }

        IEnumerator ResetSpecialAbility1Parameters()
        {
            // Wait for the length of the animation before resetting
            yield return new WaitForSeconds(0.5f); // Adjust the wait time based on your animation length

            // Reset all special ability parameters to false
            animator.SetBool("isSpecialAbility1", false);
            animator.SetBool("SpecialAbility1North", false);
            animator.SetBool("SpecialAbility1South", false);
            animator.SetBool("SpecialAbility1East", false);
            animator.SetBool("SpecialAbility1West", false);
            animator.SetBool("SpecialAbility1NorthEast", false);
            animator.SetBool("SpecialAbility1NorthWest", false);
            animator.SetBool("SpecialAbility1SouthEast", false);
            animator.SetBool("SpecialAbility1SouthWest", false);

            // Optionally, restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }

        // Special Ability 2:
        public void TriggerSpecialAbility2Animation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isSpecialAbility1' to true to initiate the special ability animation
            animator.SetBool("isSpecialAbility2", true);

            // Determine the current direction and trigger the appropriate special ability animation
            if(animator.GetBool("isNorth")) animator.SetBool("SpecialAbility2North", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("SpecialAbility2South", true);
            else if(animator.GetBool("isEast")) animator.SetBool("SpecialAbility2East", true);
            else if(animator.GetBool("isWest")) animator.SetBool("SpecialAbility2West", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("SpecialAbility2NorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("SpecialAbility2NorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("SpecialAbility2SouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("SpecialAbility2SouthWest", true);

            // Reset the special ability parameters after a delay or at the end of the animation
            StartCoroutine(ResetSpecialAbility2Parameters());
        }

        IEnumerator ResetSpecialAbility2Parameters()
        {
            // Wait for the length of the animation before resetting
            yield return new WaitForSeconds(0.5f); // Adjust the wait time based on your animation length

            // Reset all special ability parameters to false
            animator.SetBool("isSpecialAbility2", false);
            animator.SetBool("SpecialAbility2North", false);
            animator.SetBool("SpecialAbility2South", false);
            animator.SetBool("SpecialAbility2East", false);
            animator.SetBool("SpecialAbility2West", false);
            animator.SetBool("SpecialAbility2NorthEast", false);
            animator.SetBool("SpecialAbility2NorthWest", false);
            animator.SetBool("SpecialAbility2SouthEast", false);
            animator.SetBool("SpecialAbility2SouthWest", false);

            // Optionally, restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }


        //Cast spell
        public void TriggerCastSpellAnimation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isCastingSpell' to true to initiate the spell casting animation
            animator.SetBool("isCastingSpell", true);

            // Determine the current direction and trigger the appropriate cast spell animation
            if(animator.GetBool("isNorth")) animator.SetBool("CastSpellNorth", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("CastSpellSouth", true);
            else if(animator.GetBool("isEast")) animator.SetBool("CastSpellEast", true);
            else if(animator.GetBool("isWest")) animator.SetBool("CastSpellWest", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("CastSpellNorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("CastSpellNorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("CastSpellSouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("CastSpellSouthWest", true);

            // Reset the cast spell parameters after a delay or at the end of the animation
            StartCoroutine(ResetCastSpellParameters());
        }

        IEnumerator ResetCastSpellParameters()
        {
            // Wait for the length of the animation before resetting
            yield return new WaitForSeconds(0.5f); // Adjust the wait time based on your animation length

            // Reset all cast spell parameters to false
            animator.SetBool("isCastingSpell", false);
            animator.SetBool("CastSpellNorth", false);
            animator.SetBool("CastSpellSouth", false);
            animator.SetBool("CastSpellEast", false);
            animator.SetBool("CastSpellWest", false);
            animator.SetBool("CastSpellNorthEast", false);
            animator.SetBool("CastSpellNorthWest", false);
            animator.SetBool("CastSpellSouthEast", false);
            animator.SetBool("CastSpellSouthWest", false);

            // Optionally, restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }

        //Kick:
        public void TriggerKickAnimation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isKicking' to true to initiate the kick animation
            animator.SetBool("isKicking", true);

            // Determine the current direction and trigger the appropriate kick animation
            if(animator.GetBool("isNorth")) animator.SetBool("KickNorth", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("KickSouth", true);
            else if(animator.GetBool("isEast")) animator.SetBool("KickEast", true);
            else if(animator.GetBool("isWest")) animator.SetBool("KickWest", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("KickNorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("KickNorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("KickSouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("KickSouthWest", true);

            // Reset the kick parameters after a delay or at the end of the animation
            StartCoroutine(ResetKickParameters());
        }

        IEnumerator ResetKickParameters()
        {
            // Wait for the length of the animation before resetting
            yield return new WaitForSeconds(0.5f); // Adjust the wait time based on your animation length

            // Reset all kick parameters to false
            animator.SetBool("isKicking", false);
            animator.SetBool("KickNorth", false);
            animator.SetBool("KickSouth", false);
            animator.SetBool("KickEast", false);
            animator.SetBool("KickWest", false);
            animator.SetBool("KickNorthEast", false);
            animator.SetBool("KickNorthWest", false);
            animator.SetBool("KickSouthEast", false);
            animator.SetBool("KickSouthWest", false);

            // Optionally, restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }

        //Flip animation:
        public void TriggerFlipAnimation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isFlipping' to true to initiate the flip animation
            animator.SetBool("isFlipping", true);

            // Determine the current direction and trigger the appropriate flip animation
            if(animator.GetBool("isNorth")) animator.SetBool("FrontFlipNorth", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("FrontFlipSouth", true);
            else if(animator.GetBool("isEast")) animator.SetBool("FrontFlipEast", true);
            else if(animator.GetBool("isWest")) animator.SetBool("FrontFlipWest", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("FrontFlipNorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("FrontFlipNorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("FrontFlipSouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("FrontFlipSouthWest", true);

            // Reset the flip parameters after a delay or at the end of the animation
            StartCoroutine(ResetFlipParameters());
        }

        IEnumerator ResetFlipParameters()
        {
            // Wait for the length of the animation before resetting
            yield return new WaitForSeconds(0.5f); // Adjust the wait time based on your animation length

            // Reset all flip parameters to false
            animator.SetBool("isFlipping", false);
            animator.SetBool("FrontFlipNorth", false);
            animator.SetBool("FrontFlipSouth", false);
            animator.SetBool("FrontFlipEast", false);
            animator.SetBool("FrontFlipWest", false);
            animator.SetBool("FrontFlipNorthEast", false);
            animator.SetBool("FrontFlipNorthWest", false);
            animator.SetBool("FrontFlipSouthEast", false);
            animator.SetBool("FrontFlipSouthWest", false);

            // Optionally, restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }


        //rolling

        public void TriggerRollAnimation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isRolling' to true to initiate the roll animation
            animator.SetBool("isRolling", true);

            // Determine the current direction and trigger the appropriate roll animation
            if(animator.GetBool("isNorth")) animator.SetBool("RollingNorth", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("RollingSouth", true);
            else if(animator.GetBool("isEast")) animator.SetBool("RollingEast", true);
            else if(animator.GetBool("isWest")) animator.SetBool("RollingWest", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("RollingNorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("RollingNorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("RollingSouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("RollingSouthWest", true);

            // Reset the roll parameters after a delay or at the end of the animation
            StartCoroutine(ResetRollParameters());
        }

        IEnumerator ResetRollParameters()
        {
            // Wait for the length of the animation before resetting
            yield return new WaitForSeconds(rollTime); // Adjust the wait time based on your animation length

            // Reset all roll parameters to false
            animator.SetBool("isRolling", false);
            animator.SetBool("RollingNorth", false);
            animator.SetBool("RollingSouth", false);
            animator.SetBool("RollingEast", false);
            animator.SetBool("RollingWest", false);
            animator.SetBool("RollingNorthEast", false);
            animator.SetBool("RollingNorthWest", false);
            animator.SetBool("RollingSouthEast", false);
            animator.SetBool("RollingSouthWest", false);

            // Optionally, restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }

        //Slide
        public void TriggerSlideAnimation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isSliding' to true to initiate the slide animation
            animator.SetBool("isSliding", true);

            // Determine the current direction and trigger the appropriate slide animation
            if(animator.GetBool("isNorth")) animator.SetBool("SlidingNorth", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("SlidingSouth", true);
            else if(animator.GetBool("isEast")) animator.SetBool("SlidingEast", true);
            else if(animator.GetBool("isWest")) animator.SetBool("SlidingWest", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("SlidingNorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("SlidingNorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("SlidingSouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("SlidingSouthWest", true);

            // Reset the slide parameters after a delay or at the end of the animation
            StartCoroutine(ResetSlideParameters());
        }

        IEnumerator ResetSlideParameters()
        {
            // Wait for the length of the animation before resetting
            yield return new WaitForSeconds(0.7f); // Adjust the wait time based on your animation length

            // Reset all slide parameters to false
            animator.SetBool("isSliding", false);
            animator.SetBool("SlidingNorth", false);
            animator.SetBool("SlidingSouth", false);
            animator.SetBool("SlidingEast", false);
            animator.SetBool("SlidingWest", false);
            animator.SetBool("SlidingNorthEast", false);
            animator.SetBool("SlidingNorthWest", false);
            animator.SetBool("SlidingSouthEast", false);
            animator.SetBool("SlidingSouthWest", false);

            // Optionally, restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }

        //Pummel
        public void TriggerPummelAnimation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isPummeling' to true to initiate the pummel animation
            animator.SetBool("isPummeling", true);

            // Determine the current direction and trigger the appropriate pummel animation
            if(animator.GetBool("isNorth")) animator.SetBool("PummelNorth", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("PummelSouth", true);
            else if(animator.GetBool("isEast")) animator.SetBool("PummelEast", true);
            else if(animator.GetBool("isWest")) animator.SetBool("PummelWest", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("PummelNorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("PummelNorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("PummelSouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("PummelSouthWest", true);

            // Reset the pummel parameters after a delay or at the end of the animation
            StartCoroutine(ResetPummelParameters());
        }

        IEnumerator ResetPummelParameters()
        {
            // Wait for the length of the animation before resetting
            yield return new WaitForSeconds(0.5f); // Adjust the wait time based on your animation length

            // Reset all pummel parameters to false
            animator.SetBool("isPummeling", false);
            animator.SetBool("PummelNorth", false);
            animator.SetBool("PummelSouth", false);
            animator.SetBool("PummelEast", false);
            animator.SetBool("PummelWest", false);
            animator.SetBool("PummelNorthEast", false);
            animator.SetBool("PummelNorthWest", false);
            animator.SetBool("PummelSouthEast", false);
            animator.SetBool("PummelSouthWest", false);

            // Optionally, restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }

        //Attack spin
        public void TriggerAttackSpinAnimation()
        {
            if(!gameObject.activeInHierarchy)
            {
                return;
            }
            // Set 'isAttackSpinning' to true to initiate the attack spin animation
            animator.SetBool("isAttackSpinning", true);

            // Determine the current direction and trigger the appropriate attack spin animation
            if(animator.GetBool("isNorth")) animator.SetBool("AttackSpinNorth", true);
            else if(animator.GetBool("isSouth")) animator.SetBool("AttackSpinSouth", true);
            else if(animator.GetBool("isEast")) animator.SetBool("AttackSpinEast", true);
            else if(animator.GetBool("isWest")) animator.SetBool("AttackSpinWest", true);
            else if(animator.GetBool("isNorthEast")) animator.SetBool("AttackSpinNorthEast", true);
            else if(animator.GetBool("isNorthWest")) animator.SetBool("AttackSpinNorthWest", true);
            else if(animator.GetBool("isSouthEast")) animator.SetBool("AttackSpinSouthEast", true);
            else if(animator.GetBool("isSouthWest")) animator.SetBool("AttackSpinSouthWest", true);

            // Reset the attack spin parameters after a delay or at the end of the animation
            StartCoroutine(ResetAttackSpinParameters());
        }

        IEnumerator ResetAttackSpinParameters()
        {
            // Wait for the length of the animation before resetting
            yield return new WaitForSeconds(0.5f); // Adjust the wait time based on your animation length

            // Reset all attack spin parameters to false
            animator.SetBool("isAttackSpinning", false);
            animator.SetBool("AttackSpinNorth", false);
            animator.SetBool("AttackSpinSouth", false);
            animator.SetBool("AttackSpinEast", false);
            animator.SetBool("AttackSpinWest", false);
            animator.SetBool("AttackSpinNorthEast", false);
            animator.SetBool("AttackSpinNorthWest", false);
            animator.SetBool("AttackSpinSouthEast", false);
            animator.SetBool("AttackSpinSouthWest", false);

            // Optionally, restore the direction to ensure the character returns to the correct idle state
            RestoreDirectionAfterAttack();
        }


    }
}