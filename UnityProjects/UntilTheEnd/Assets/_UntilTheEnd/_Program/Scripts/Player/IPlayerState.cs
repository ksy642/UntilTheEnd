namespace UntilTheEnd
{
    public interface IPlayerState
    {
        public void EnterState(TestPlayer player);  // 상태 진입할 때 실행
        public void UpdateState(TestPlayer player); // 상태 업데이트
        public void ExitState(TestPlayer player);   // 상태 종료할 때 실행
    }
}