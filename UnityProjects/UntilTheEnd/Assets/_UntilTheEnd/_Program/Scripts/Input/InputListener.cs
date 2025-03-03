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
            // 로비일 때는 동작 못하게 설정...이 말은 로비일 때 동작하게 하는 뭔가 만들 수 있음
            if (!GameManager.instance.isLobby)
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
}