using cfEngine.Logging;
using UnityEditor;

namespace Script
{
    public class Test
    {
        [MenuItem("Test/Test Save Load")]
        public static void TestSaveLoad()
        {
            Game.Meta.Statistic.Record("Testing");
            Game.UserData.SaveAsync().ContinueWith(task =>
            {
                Log.LogInfo("Saved successfully");
            });
        }
    }
}