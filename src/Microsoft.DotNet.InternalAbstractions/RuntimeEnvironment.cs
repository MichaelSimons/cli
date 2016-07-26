// Copyright(c) .NET Foundation and contributors.All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Security;
using Microsoft.DotNet.InternalAbstractions.Native;
using Microsoft.Win32;

namespace Microsoft.DotNet.InternalAbstractions
{
    public static class RuntimeEnvironment
    {
        private static readonly string OverrideEnvironmentVariableName = "DOTNET_RUNTIME_ID";

        public static Platform OperatingSystemPlatform { get; } = PlatformApis.GetOSPlatform();

        public static string OperatingSystemVersion { get; } = PlatformApis.GetOSVersion();

        public static string OperatingSystem { get; } = PlatformApis.GetOSName();

        public static string RuntimeArchitecture { get; } = GetArch();

        public static bool? IsDockerContainer { get; } = GetIsDockerContainer();

        private static string GetArch()
        {
#if NET451
            return Environment.Is64BitProcess ? "x64" : "x86";
#else
            return IntPtr.Size == 8 ? "x64" : "x86";
#endif
        }

        public static string GetRuntimeIdentifier()
        {
            return
                Environment.GetEnvironmentVariable(OverrideEnvironmentVariableName) ??
                (GetRIDOS() + GetRIDVersion() + GetRIDArch());
        }

        private static string GetRIDArch()
        {
            if (!string.IsNullOrEmpty(RuntimeArchitecture))
            {
                return $"-{RuntimeArchitecture.ToLowerInvariant()}";
            }
            return string.Empty;
        }

        private static string GetRIDVersion()
        {
            // Windows RIDs do not separate OS name and version by "." due to legacy
            // Others do, that's why we have the "." prefix on them below
            switch (OperatingSystemPlatform)
            {
                case Platform.Windows:
                    return GetWindowsProductVersion();
                case Platform.Linux:
                    return $".{OperatingSystemVersion}";
                case Platform.Darwin:
                    return $".{OperatingSystemVersion}";
                default:
                    return string.Empty; // Unknown Platform? Unknown Version!
            }
        }

        private static string GetWindowsProductVersion()
        {
            var ver = Version.Parse(OperatingSystemVersion);
            if (ver.Major == 6)
            {
                if (ver.Minor == 1)
                {
                    return "7";
                }
                else if (ver.Minor == 2)
                {
                    return "8";
                }
                else if (ver.Minor == 3)
                {
                    return "81";
                }
            }
            else if (ver.Major == 10 && ver.Minor == 0)
            {
                // Not sure if there will be  10.x (where x > 0) or even 11, so let's be defensive.
                return "10";
            }
            return string.Empty; // Unknown version
        }

        private static string GetRIDOS()
        {
            switch (OperatingSystemPlatform)
            {
                case Platform.Windows:
                    return "win";
                case Platform.Linux:
                    return OperatingSystem.ToLowerInvariant();
                case Platform.Darwin:
                    return "osx";
                default:
                    return "unknown";
            }
        }

        public static bool? GetIsDockerContainer()
        {
            switch (OperatingSystemPlatform)
            {
                case Platform.Windows:
                    try
                    {
                        // Only use this registry setting for telemetry purposes – do not change product behavior inside containers based on this key. 
                        using (var subkey = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control"))
                        {
                            return subkey != null && subkey.GetValue("ContainerType") != null;
                        }
                    }
                    catch (SecurityException)
                    {
                        return null;  // unkown
                    }
                case Platform.Linux:
                    return File.Exists("/.dockerenv");
                case Platform.Darwin:
                default:
                    return false;
            }
        }
    }
}
