using System;
using JetBrains.Annotations;
using log4net;
using SpotifyAPI.Web;
using ToastifyAPI.Core;
using ToastifyAPI.Model.Interfaces;

namespace Toastify.Model
{
    public class CurrentlyPlayingObject : ICurrentlyPlayingObject
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CurrentlyPlayingObject));

        #region Public Properties

        public int ProgressMs { get; }
        public bool IsPlaying { get; }
        public ISpotifyTrack Track { get; }
        public SpotifyTrackType Type { get; }

        #endregion

        public CurrentlyPlayingObject(int progressMs, bool isPlaying, ISpotifyTrack track, SpotifyTrackType type)
        {
            this.ProgressMs = progressMs;
            this.IsPlaying = isPlaying;
            this.Track = track;
            this.Type = type;
        }

        // ReSharper disable ConstantConditionalAccessQualifier
        // ReSharper disable once ConstantNullCoalescingCondition
        public CurrentlyPlayingObject([NotNull] CurrentlyPlaying playbackContext)
            : this(playbackContext?.ProgressMs ?? 0, playbackContext?.IsPlaying ?? false, null, SpotifyTrackType.Unknown)
        {
            if (playbackContext == null)
                throw new ArgumentNullException(nameof(playbackContext));

            switch (playbackContext.CurrentlyPlayingType)
            {
                case "track":
                    if (playbackContext.Item is FullTrack)
                        this.Track = new Song(playbackContext.Item as FullTrack);
                    break;

                case "episode":
                    if (playbackContext.Item is FullEpisode)
                        this.Track = new SpotifyTrack(SpotifyTrackType.Episode, (playbackContext.Item as FullEpisode).Name, (playbackContext.Item as FullEpisode).DurationMs / 1000);
                    break;

                case "ad":
                    this.Track = new SpotifyTrack(SpotifyTrackType.Ad);
                    break;

                case "unknown":
                    this.Track = new SpotifyTrack(SpotifyTrackType.Unknown);
                    break;

                default:
                    logger.Error($"Unexpected CurrentlyPlayingType of current playback context: {playbackContext.CurrentlyPlayingType}");
                    this.Track = new SpotifyTrack(SpotifyTrackType.Unknown);
                    break;
            }

            this.Type = this.Track?.Type ?? SpotifyTrackType.Unknown;
        }
        // ReSharper restore ConstantConditionalAccessQualifier
    }
}