using UnityEngine;

namespace UntilTheEnd
{
    public class TrainController : MonoBehaviour
    {
        public float speed = 0f;             // 현재 속도
        public float maxSpeed = 2.5f;       // 최대 속도 (천천히 이동)
        public float acceleration = 1.2f;  // 가속도 (서서히 증가)
        public float deceleration = 1.3f; // 감속도 (서서히 멈춤)
        private bool isMoving = false;   // 기차가 움직이는지 여부

        private void Update()
        {
            // 일단 테스트용으로 F 로 둔거
            // 목적은 캐릭터가 기차에 탔을 때 같이 움직여야된다는건데...추가설정필요할듯
            if (Input.GetKeyDown(KeyCode.F))
            {
                isMoving = !isMoving;
            }




            if (isMoving)
            {
                // 천천히 가속
                speed = Mathf.MoveTowards(speed, maxSpeed, acceleration * Time.deltaTime);
            }
            else
            {
                // 천천히 감속
                speed = Mathf.MoveTowards(speed, 0f, deceleration * Time.deltaTime);
            }

            // X축 방향으로 이동, 처음에 forward  썼는데 이러면 z축으로 움직이더라
            this.gameObject.transform.position += Vector3.right * speed * Time.deltaTime;
        }




        // 일단 외부에서 기차 움직일 수 있게 만들어둔 함수
        public void StartTrain()
        {
            isMoving = true;
        }

        public void StopTrain()
        {
            isMoving = false;
        }
    }
}