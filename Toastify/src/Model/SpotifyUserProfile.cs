using ToastifyAPI.Core;
using ToastifyAPI.Model.Interfaces;
using SpotifyAPI.Web;

namespace Toastify.Model
{
    public class SpotifyUserProfile : ISpotifyUserProfile
    {
        #region Public Properties

        public SpotifySubscriptionLevel SubscriptionLevel { get; }

        #endregion

        public SpotifyUserProfile(PrivateUser privateProfile)
        {
            switch (privateProfile.Product)
            {
                case "free":
                    this.SubscriptionLevel = SpotifySubscriptionLevel.Free;
                    break;

                case "open":
                    this.SubscriptionLevel = SpotifySubscriptionLevel.Open;
                    break;

                case "premium":
                    this.SubscriptionLevel = SpotifySubscriptionLevel.Premium;
                    break;

                default:
                    this.SubscriptionLevel = SpotifySubscriptionLevel.Unknown;
                    break;
            }
        }

        public SpotifyUserProfile(PublicUser publicProfile)
        {
            this.SubscriptionLevel = SpotifySubscriptionLevel.Unknown;
        }
    }
}