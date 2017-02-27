using System.Diagnostics;
using Newtonsoft.Json;

namespace NvRestInterface.Utilities
{
    public static class Utilities
    {
        public static void PrintDebug(string debugInfo)
        {
            Debug.WriteLine("DEBUG: " + debugInfo);
        }

        public static string SerializeObject(object inputObj)
        {
            return JsonConvert.SerializeObject(inputObj);
        }

        public static object DeSerialiseObject(string inputString)
        {
            return JsonConvert.DeserializeObject(inputString);
        }
    }
}