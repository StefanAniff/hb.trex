using System.Collections.Generic;

namespace Trex.SmartClient.Core.Model
{
    public class ForecastUserSearchPreset
    {
        public string Name { get; private set; }
        public List<int> UserIds { get; private set; }
 
        public static ForecastUserSearchPreset Create(string name, List<int> userIds)
        {
            return new ForecastUserSearchPreset
                {
                    Name = name,
                    UserIds = userIds
                };
        }
    }
}