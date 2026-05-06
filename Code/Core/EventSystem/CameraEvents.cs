using Code.Cameras;

namespace EventSystem
{
    public static class CameraEvents
    {
        public static readonly FollowCamChangeEvent FollowCamChangeEvent = new FollowCamChangeEvent();
    }

    public class FollowCamChangeEvent : GameEvent
    {
        public CinemachineCamOwner cam;

        public FollowCamChangeEvent Initializer(CinemachineCamOwner cam)
        {
            this.cam = cam;
            return this;
        }
    }
}