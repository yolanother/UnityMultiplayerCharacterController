namespace PlayerComponents
{
    public interface IPlayerOwnershipChanged
    {
        void OnStartLocalPlayer();
        void OnStartRemotePlayer();
    }
}