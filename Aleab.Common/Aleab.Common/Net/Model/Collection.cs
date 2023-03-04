using System.Collections.Generic;

namespace Aleab.Common.Net.Model
{
    public class Collection<T> : BaseModel where T : BaseModel
    {
        public List<T> Items { get; set; }
    }
}