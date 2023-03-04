using Newtonsoft.Json;
using System;

namespace Toastify.Core
{
    public class SecureProxyConfigJsonConverter : JsonConverter
    {
        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            SpotifyProxyConfig proxyConfig = (SpotifyProxyConfig)value;
            SpotifyProxyConfig newProxyConfig = new SpotifyProxyConfig(proxyConfig.ProxyConfig)
            {
                Password = null
            };

            serializer.Serialize(writer, newProxyConfig);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            SpotifyProxyConfig existing = (SpotifyProxyConfig)existingValue;
            if (existing != null)
            {
                serializer.Populate(reader, existing);
                return existing;
            }
            return (SpotifyProxyConfig)serializer.Deserialize(reader, typeof(SpotifyProxyConfig));
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SpotifyProxyConfig);
        }
    }
}