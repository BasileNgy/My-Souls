using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class InputHandler : MonoBehaviour
    {
        public float vertical;
        public float horizontal;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        
        public bool rollFlag;
        public bool sprintFlag;
        public float rollInputTimer;
        

        private PlayerControls inputActions;

        private Vector2 movementInput;
        private Vector2 cameraInput;

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed +=
                    inputActions => movementInput = inputActions.ReadValue<Vector2>();

                //performed est un event, on lui ajoute des commandes à faire quand qu'il est appelé ( avec le =+)
                //donc il va lancer les méthodes dans le contexte i pour actualiser la valeur
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
        
        private void HandleRollInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            if (b_Input)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }
    }
}

