using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInput
    {
        private readonly Queue<PlayerInputInfo> inputQueue;
        private bool inputTerm;
        private Joystick joystickInfo;

        public PlayerInput(Transform playerTransform)
        {
            inputQueue = new Queue<PlayerInputInfo>();
            inputTerm = true;
            joystickInfo = playerTransform.gameObject.GetComponent<PlayerBehavior>().JoyStick;
        }

        public bool IsPlayerInputEmpty()
        {
            return inputQueue.Count <= 0;
        }

        public PlayerInputInfo GetPlayerInput()
        {
            return inputQueue.Dequeue();
        }

        public void Update()
        {
            if (inputTerm)
            {
                PlayerInputInfo input = default;

                input.horizontal = joystickInfo.Horizontal;
                input.vertical = joystickInfo.Vertical;

                if (Mathf.Abs(input.horizontal) > 0f || Mathf.Abs(input.vertical) > 0f)
                {
                    inputQueue.Enqueue(input);
                }
            }
        }

        private void GiveInputTerm()
        {
            inputTerm = false;
            Utils.Util.CoSeconds(() =>
            {
                inputTerm = true;
            }, 0.1f);
        }
    }
}