using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Aleab.Common.Logging;
using Aleab.Common.Logging.Extensions;
using Aleab.Common.Logging.Interfaces;

namespace Aleab.Common.Serialization
{
    public class BinarySerializer<T> where T : class
    {
        private static readonly ILogger logger = LogManager.GetLogger(typeof(BinarySerializer<T>));

        private readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        public void Serialize(T data, string filepath)
        {
            if (data == null || string.IsNullOrWhiteSpace(filepath))
                return;

            using (var fs = new FileStream(filepath, FileMode.Create))
            {
                this.binaryFormatter.Serialize(fs, data);
            }
        }

        public T Deserialize(string filepath)
        {
            if (!File.Exists(filepath))
                return null;

            T data;
            using (var fs = new FileStream(filepath, FileMode.Open))
            {
                try
                {
                    data = (T)this.binaryFormatter.Deserialize(fs);
                }
                catch (SerializationException e)
                {
                    logger.Error($"Error while deserializing Spotify's token data from \"{filepath}\"", e);
                    throw;
                }
            }

            return data;
        }
    }
}