using System;
using Microsoft.Extensions.PlatformAbstractions;

namespace Microsoft.DotNet.Cli.Build.Framework
{
    public static class CurrentArchitecture
    {
        private static BuildArchitecture _current;

        public static BuildArchitecture Current
        {
            get
            {
                if (_current == default(BuildArchitecture))
                {
                    DetermineCurrentArchitecture();
                }
                
                return _current;
            }
        }

        public static bool Isx86
        {
            get
            {
                var archName = PlatformServices.Default.Runtime.RuntimeArchitecture;
                return string.Equals(archName, "x86", StringComparison.OrdinalIgnoreCase);
            }
        }

        public static bool Isx64
        {
            get
            {
                var archName = PlatformServices.Default.Runtime.RuntimeArchitecture;
                return string.Equals(archName, "x64", StringComparison.OrdinalIgnoreCase);
            }
        }

        private static void DetermineCurrentArchitecture()
        {
            if (Isx86)
            {
                _current = BuildArchitecture.x86;
            }
            else if (Isx64)
            {
                _current = BuildArchitecture.x64;
            }
        }
    }
}