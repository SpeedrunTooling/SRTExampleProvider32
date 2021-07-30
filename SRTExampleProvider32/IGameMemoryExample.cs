using System;
using SRTExampleProvider32.Structs;
using SRTExampleProvider32.Structs.GameStructs;

namespace SRTExampleProvider32
{
    public interface IGameMemoryExample
    {
        string GameName { get; }
        string VersionInfo { get; }
        GameExampleStruct Example { get; set; }
    }
}
