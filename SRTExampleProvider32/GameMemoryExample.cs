using SRTExampleProvider32.Structs.GameStructs;
using System.Diagnostics;
using System.Reflection;

namespace SRTExampleProvider32
{
    public class GameMemoryExample : IGameMemoryExample
    {
        public string GameName => "Example Game Name";
        public string VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public GameExampleStruct Example { get => _example; set => _example = value; }
        internal GameExampleStruct _example;
    }
}
