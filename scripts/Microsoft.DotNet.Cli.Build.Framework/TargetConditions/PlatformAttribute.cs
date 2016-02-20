using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.Cli.Build.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class BuildPlatformAttribute : TargetConditionAttribute
    {
        public IEnumerable<BuildPlatform> BuildPlatforms { get; }

        public BuildPlatformAttribute(params BuildPlatform[] platforms)
        {
            BuildPlatforms = platforms;
        }

        public override bool EvaluateCondition()
        {
            foreach (var platform in BuildPlatforms)
            {
                if (platform == CurrentPlatform.Current)
                {
                    return true;
                }
            }

            return false;
        }
    }
}