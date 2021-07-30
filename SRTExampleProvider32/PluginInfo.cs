using SRTPluginBase;
using System;

namespace SRTExampleProvider32
{
    internal class PluginInfo : IPluginInfo
    {
        public string Name => "Game Memory Provider SRT Example Provider 32bit.";

        public string Description => "A game memory provider plugin for SRT Example Provider 32bit.";

        public string Author => "VideoGameRoulette";

        public Uri MoreInfoURL => new Uri("https://github.com/VideoGameRoulette");

        public int VersionMajor => assemblyFileVersion.ProductMajorPart;

        public int VersionMinor => assemblyFileVersion.ProductMinorPart;

        public int VersionBuild => assemblyFileVersion.ProductBuildPart;

        public int VersionRevision => assemblyFileVersion.ProductPrivatePart;

        private System.Diagnostics.FileVersionInfo assemblyFileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
    }
}
