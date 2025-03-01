using System;
using UnityEngine;

namespace UntilTheEnd
{
    public static class InputListener
    {
        public static event Action OnPressed_ESC;
        public static event Action OnPressed_E;

        public static void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnPressed_ESC?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                OnPressed_E?.Invoke();
            }
        }
    }
}
