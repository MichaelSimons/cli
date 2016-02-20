using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.Cli.Build.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class BuildPlatformAttribute : TargetConditionAttribute
    {
        public IEnumerable<BuildPlatform> BuildPlatforms { get; private set; }

        public BuildPlatformAttribute(params BuildPlatform[] platforms)
        {
            BuildPlatforms = platforms;
        }

        public override bool EvaluateCondition()
        {
            var currentPlatform = CurrentPlatform.Current;

            if (currentPlatform == null)
            {
                throw new Exception("Unrecognized Platform.");
            }

            foreach (var platform in BuildPlatforms)
            {
                if (platform == currentPlatform)
                {
                    return true;
                }
            }

            return false;
        }
    }
}