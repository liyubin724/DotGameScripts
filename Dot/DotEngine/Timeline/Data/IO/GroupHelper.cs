using Newtonsoft.Json;

namespace DotEngine.Timeline.Data
{
    public static class GroupHelper
    {
        public static GroupData ReadFromJson(string jsonText)
        {
            if(string.IsNullOrEmpty(jsonText))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<GroupData>(jsonText);
        }

        public static string WriteToJson(GroupData groupData)
        {
            if(groupData == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(groupData, Formatting.Indented);
        }
    }
}
