using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

namespace SmallScaleInc.TopDownPixelCharactersPack1
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 2.0f; // the movement speed of the player
        private Rigidbody2D rb;
        private Vector2 movementDirection;
        private SpriteRenderer spriteRenderer;
        private Animator animator; // Reference to the Animator component
        private bool isRunning = false;

        // Rolling
        public bool isRolling = false;
        public float rollDuration = 0.5f;
        public float rollSpeedMultiplier = 2.0f; // Multiply the speed during a roll

        // Archer specifics
        public bool isActive; // If the character is active
        public bool isRanged; // If the character is an archer OR caster character
        public bool isStealth; // If true, Makes the player transparent when crouched
        public bool isCrouching = false; // Track whether the player is crouching

        public bool isShapeShifter; // If true, Makes the player transparent when crouched
        public bool isSummoner; // If true, Makes the player transparent when crouched
        public GameObject projectilePrefab; // prefab to the projectile
        public GameObject AoEPrefab;
        public GameObject Special1Prefab;
        public GameObject HookPrefab; // Certain characters might have a grappling hook
        public GameObject ShapeShiftPrefab; // Certain characters might have a grappling hook

        public float projectileSpeed = 10.0f; // Speed at which the projectile travels
        public float shootDelay = 0.5f; // Delay in seconds before the projectile is fired

        // Melee specifics
        public bool isMelee; // If the character is a melee character
        public GameObject meleePrefab; // prefab for the melee attack

        // Tracks which special move is queued
        private int queuedSpecialMove = 0;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
            animator = GetComponent<Animator>(); // Get the Animator component
        }

        void Update()
        {
            //HandleMovement();    // Left joystick for movement
            //RotateCharacter();    // Right joystick for facing direction

            // Check if movement keys are pressed (based on joystick input)
            if(movementDirection.magnitude > 0)
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }

            // Trigger flip animation (Left Shift key)
            if(Input.GetKeyDown(KeyCode.LeftShift) && !isRolling)
            {
                TriggerFlipAnimation();
            }

            // Handle crouching
            if(Input.GetKeyDown(KeyCode.C))
            {
                if(isShapeShifter && isActive)
                {
                    StartCoroutine(ShapeShiftDelayed());
                }
                HandleCrouching();
            }

            // Handle special moves and queueing
            //HandleSpecialMoveQueue();

            // Handle attack logic
            if(Input.GetMouseButtonDown(0)) // Left mouse button
            {
                PerformQueuedAttack();
            }
        }

        // Function to trigger the flip animation
        void TriggerFlipAnimation()
        {
            animator.SetBool("isFlipping", true); // Start the flip animation

            // Reset the flip after a small delay (matching the length of the animation)
            StartCoroutine(ResetFlipAnimation());
        }

        // Reset the flip animation after the duration
        IEnumerator ResetFlipAnimation()
        {
            yield return new WaitForSeconds(0.5f); // Adjust the wait time based on the flip animation length
            animator.SetBool("isFlipping", false); // Reset the flip animation
        }

        IEnumerator Roll()
        {
            isRolling = true;
            float originalSpeed = speed;
            speed *= rollSpeedMultiplier; // Increase speed during roll
            yield return new WaitForSeconds(rollDuration);
            speed = originalSpeed; // Restore original speed after roll
            isRolling = false;
        }

        void HandleSpecialMoveQueue()
        {
            if(isActive)
            {
                // Queue special moves (1-6) for the next attack
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {
                    queuedSpecialMove = 1;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha2))
                {
                    queuedSpecialMove = 2;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha3))
                {
                    queuedSpecialMove = 3;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha4))
                {
                    queuedSpecialMove = 4;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha5))
                {
                    queuedSpecialMove = 5;
                }
                else if(Input.GetKeyDown(KeyCode.Alpha6))
                {
                    queuedSpecialMove = 6;
                }
            }
        }

        void PerformQueuedAttack()
        {
            // If a special move is queued, execute it instead of a regular attack
            switch(queuedSpecialMove)
            {
                case 1:
                    StartCoroutine(DeploySpecial1Delayed());
                    break;
                case 2:
                    // Add the special move logic for key 2 if needed
                    break;
                case 3:
                    StartCoroutine(DeployAoEDelayed());
                    break;
                case 4:
                    // Add the special move logic for key 4 if needed
                    break;
                case 5:
                    if(isSummoner)
                    {
                        StartCoroutine(DeployHookDelayed());
                    }
                    else
                    {
                        StartCoroutine(Quickshot());
                    }
                    break;
                case 6:
                    StartCoroutine(CircleShot());
                    break;
                default:
                    PerformAttack(); // If no special move is queued, perform a regular attack
                    break;
            }

            // Reset the queued special move after execution
            queuedSpecialMove = 0;
        }

        void PerformAttack()
        {
            // Add your attack logic here (ranged or melee)
            if(isRanged)
            {
                Invoke(nameof(DelayedShoot), shootDelay);
            }
            else if(isMelee)
            {
                // Add melee attack logic if needed
                if(meleePrefab != null)
                {
                    GameObject meleeAttack = Instantiate(meleePrefab, transform.position, Quaternion.identity);
                    Destroy(meleeAttack, 0.5f); // Destroy melee attack after a delay
                }
            }
        }

        void FixedUpdate()
        {
            // Apply movement to the character based on the left joystick input
            if(movementDirection != Vector2.zero)
            {
                rb.MovePosition(rb.position + movementDirection * speed * Time.fixedDeltaTime);
            }
        }

        //void HandleMovement()
        //{
        //    // Get input from the left joystick
        //    float moveX = Input.GetAxis("Horizontal"); // Left joystick horizontal axis
        //    float moveY = Input.GetAxis("Vertical");   // Left joystick vertical axis

        //    // Set movement direction based on joystick input
        //    movementDirection = new Vector2(moveX, moveY);

        //    // Normalize movement direction to ensure consistent speed
        //    if (movementDirection.magnitude > 1)
        //    {
        //        movementDirection.Normalize();
        //    }
        //}

        private void OnMove(InputValue inputValue)
        {
            movementDirection = inputValue.Get<Vector2>();

            // Normalize movement direction to ensure consistent speed
            if(movementDirection.magnitude > 1)
            {
                movementDirection.Normalize();
            }
        }

        private void OnLook(InputValue inputValue)
        {
            var lookDirection = inputValue.Get<Vector2>();

            // If there is input from the right joystick, rotate the character to face that direction
            if(lookDirection.magnitude > 0.1f) // Avoid very small values
            {
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        private void OnButtonSouth(InputValue inputValue)
        {
            if(isActive)
            {
                queuedSpecialMove = 1;
            }
        }

        private void OnButtonEast(InputValue inputValue)
        {
            if(isActive)
            {
                queuedSpecialMove = 2;
            }
        }

        private void OnButtonWest(InputValue inputValue)
        {
            if(isActive)
            {
                queuedSpecialMove = 3;
            }
        }

        private void OnButtonNorth(InputValue inputValue)
        {
            if(isActive)
            {
                queuedSpecialMove = 4;
            }
        }

        void RotateCharacter()
        {
            // Get input from the right joystick
            float aimX = Input.GetAxis("RightStickHorizontal"); // Right joystick horizontal axis
            float aimY = Input.GetAxis("RightStickVertical");   // Right joystick vertical axis

            // If there is input from the right joystick, rotate the character to face that direction
            if(new Vector2(aimX, aimY).magnitude > 0.1f) // Avoid very small values
            {
                float angle = Mathf.Atan2(aimY, aimX) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        void HandleCrouching()
        {
            isCrouching = !isCrouching; // Toggle crouching
            if(isCrouching && isStealth)
            {
                spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }

        // Shooting logic
        void DelayedShoot()
        {
            Vector2 fireDirection = new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));
            ShootProjectile(fireDirection);
        }

        void ShootProjectile(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));
            Rigidbody2D rbProjectile = projectileInstance.GetComponent<Rigidbody2D>();
            if(rbProjectile != null)
            {
                rbProjectile.velocity = direction * projectileSpeed;
            }
            // Destroy the instantiated prefab after 1.5 seconds
            Destroy(projectileInstance, 1.5f);
        }

        IEnumerator Quickshot()
        {
            yield return new WaitForSeconds(0.1f);
            for(int i = 0; i < 5; i++)
            {
                Vector2 fireDirection = new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad));
                ShootProjectile(fireDirection);
                yield return new WaitForSeconds(0.18f);
            }
        }

        IEnumerator CircleShot()
        {
            float initialDelay = 0.1f;
            float timeBetweenShots = 0.9f / 8;
            yield return new WaitForSeconds(initialDelay);

            for(int i = 0; i < 8; i++)
            {
                float angle = i * 45; // 45-degree increments for 8 directions
                float radianAngle = Mathf.Deg2Rad * angle;
                Vector2 fireDirection = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));
                ShootProjectile(fireDirection);
                yield return new WaitForSeconds(timeBetweenShots);
            }
        }

        // AOE, Special, and Hook attack logic using joystick direction

        IEnumerator DeployAoEDelayed()
        {
            if(AoEPrefab != null) 
            {
                GameObject aoeInstance;
                Vector2 deployPosition = (Vector2)transform.position + movementDirection.normalized;
                aoeInstance = Instantiate(AoEPrefab, deployPosition, Quaternion.identity);
                Destroy(aoeInstance, 0.9f);
            }
            yield return null;
        }

        IEnumerator DeployHookDelayed()
        {
            if(HookPrefab != null)
            {
                Vector2 direction = movementDirection.normalized;
                Vector2 hookPosition = (Vector2)transform.position + direction;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                GameObject hookInstance = Instantiate(HookPrefab, hookPosition, Quaternion.Euler(0, 0, angle));
                Destroy(hookInstance, 1.0f);
            }
            yield return null;
        }

        IEnumerator DeploySpecial1Delayed()
        {
            if(Special1Prefab != null)
            {
                Vector2 deployPosition = (Vector2)transform.position + movementDirection.normalized;
                GameObject specialInstance = Instantiate(Special1Prefab, deployPosition, Quaternion.identity);
                Destroy(specialInstance, 1.0f);
            }
            yield return null;
        }

        // ShapeShift logic
        IEnumerator ShapeShiftDelayed()
        {
            if(ShapeShiftPrefab != null)
            {
                yield return new WaitForSeconds(0.001f);
                GameObject shapeShiftInstance = Instantiate(ShapeShiftPrefab, transform.position, Quaternion.identity);
                Destroy(shapeShiftInstance, 0.9f);
            }
        }
    }
}