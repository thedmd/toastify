using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Aleab.Common.Helpers
{
    public static class FileHelper
    {
        #region Static members

        public static Task<string[]> ReadAllLinesAsync(string filePath)
        {
            return ReadAllLinesAsync(filePath, Encoding.UTF8);
        }

        public static async Task<string[]> ReadAllLinesAsync(string filePath, Encoding encoding)
        {
            var lines = new List<string>();

            using (FileStream stream = File.OpenRead(filePath))
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }

            return lines.ToArray();
        }

        #endregion
    }
}