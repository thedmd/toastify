using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToastifyAPI.Core.Auth;
using ToastifyAPI.Events;
using IToken = ToastifyAPI.Core.Auth.IToken;

namespace Toastify.src.Core.Auth
{
    [Serializable]
    public class Token : IToken, IEquatable<Token>
    {
        public string AccessToken { get; private set; } = string.Empty;

        public string TokenType { get; private set; } = string.Empty;

        public double ExpiresIn { get; private set; } = 0;

        public string RefreshToken { get; private set; } = string.Empty;

        public DateTime CreateDate { get; private set; } = DateTime.MinValue;

        public Token()
        {
        }

        public Token(AuthorizationCodeRefreshResponse response, string refreshToken)
        {
            AccessToken = response?.AccessToken;
            TokenType = response?.TokenType;
            ExpiresIn = (double)response?.ExpiresIn;
            CreateDate = response?.CreatedAt ?? DateTime.MinValue;
            RefreshToken = response?.RefreshToken ?? refreshToken;
        }

        public Token(AuthorizationCodeTokenResponse response)
        {
            AccessToken = response?.AccessToken;
            TokenType = response?.TokenType;
            ExpiresIn = (double)response?.ExpiresIn;
            CreateDate = response?.CreatedAt ?? DateTime.MinValue;
            RefreshToken = response?.RefreshToken;
        }

        public bool Equals(IToken other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(this.AccessToken, other.AccessToken) &&
                   string.Equals(this.TokenType, other.TokenType) &&
                   this.ExpiresIn.Equals(other.ExpiresIn) &&
                   string.Equals(this.RefreshToken, other.RefreshToken) &&
                   this.CreateDate.Equals(other.CreateDate);
        }

        public bool Equals(Token other)
        {
            return this.Equals((IToken)other);
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return other.GetType() == this.GetType() && this.Equals((Token)other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.AccessToken != null ? this.AccessToken.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.TokenType != null ? this.TokenType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.ExpiresIn.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.RefreshToken != null ? this.RefreshToken.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.CreateDate.GetHashCode();
                return hashCode;
            }
        }

        public bool IsExpired()
        {
            return CreateDate.AddSeconds(ExpiresIn) <= DateTime.UtcNow;
        }
    }
}
