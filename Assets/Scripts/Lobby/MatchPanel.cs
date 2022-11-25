namespace Lobby
{
    public class MatchPanel : Panel
    {
        public void OnJoinRoomBtnClicked()
        {
            menuManager.SwitchPanel(1);
        }

        public void OnCreateRoomBtnClicked()
        {
            menuManager.SwitchPanel(2);
        }

        public void Exit()
        {

        }
    }
}
