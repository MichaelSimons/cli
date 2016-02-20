using System;

namespace Microsoft.DotNet.Cli.Build.Framework
{
    public static class CurrentArchitecture
    {
        private static BuildArchitecture _current;

        public static BuildArchitecture Current
        {
            get
            {
                if (_current == null)
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
                return IntPtr.Size != 8;
            }
        }

        public static bool Isx64
        {
            get
            {
                return IntPtr.Size == 8;
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