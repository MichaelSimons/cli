using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.PlatformAbstractions;

namespace Microsoft.DotNet.Cli.Build.Framework
{
    public static class CurrentPlatform
    {
        private static BuildPlatform _current;

        public static BuildPlatform Current
        {
            get
            {
                if (_current == default(BuildPlatform))
                {
                    DetermineCurrentPlatform();
                }
                
                return _current;
            }
        }

        public static bool IsWindows
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            }
        }

        public static bool IsOSX
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            }
        }

        public static bool IsUbuntu
        {
            get
            {
                var osname = PlatformServices.Default.Runtime.OperatingSystem;
                return string.Equals(osname, "ubuntu", StringComparison.OrdinalIgnoreCase);
            }
        }

        public static bool IsCentOS
        {
            get
            {
                var osname = PlatformServices.Default.Runtime.OperatingSystem;
                return string.Equals(osname, "centos", StringComparison.OrdinalIgnoreCase);
            }
        }

        private static void DetermineCurrentPlatform()
        {
            if (IsWindows)
            {
                _current = BuildPlatform.Windows;
            }
            else if (IsOSX)
            {
                _current = BuildPlatform.OSX;
            }
            else if (IsUbuntu)
            {
                _current = BuildPlatform.Ubuntu;
            }
            else if (IsCentOS)
            {
                _current = BuildPlatform.CentOS;
            }
        }
    }
}