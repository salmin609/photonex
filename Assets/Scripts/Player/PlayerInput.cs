using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerInput
    {
        private readonly Queue<PlayerInputInfo> inputQueue;
        private bool inputTerm;

        public PlayerInput()
        {
            inputQueue = new Queue<PlayerInputInfo>();
            inputTerm = true;
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
                if (Input.anyKeyDown)
                {
                    PlayerInputInfo input = default;

                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        input.vertical = 1f;
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        input.vertical = -1f;
                    }
                    else if(Input.GetKeyDown(KeyCode.A))
                    {
                        input.horizontal = -1f;
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        input.horizontal = 1f;
                    }
                    
                    inputQueue.Enqueue(input);

                    GiveInputTerm();
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