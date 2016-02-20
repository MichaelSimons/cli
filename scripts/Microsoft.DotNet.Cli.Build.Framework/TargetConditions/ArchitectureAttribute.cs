using System;
using System.Collections.Generic;

namespace Microsoft.DotNet.Cli.Build.Framework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class BuildArchitectureAttribute : TargetConditionAttribute
    {
        public IEnumerable<BuildArchitecture> BuildArchitectures { get; set; }

        public BuildArchitectureAttribute(params BuildArchitecture[] architectures)
        {
            BuildArchitectures = architectures;
        }

        public override bool EvaluateCondition()
        {
            foreach (var architecture in BuildArchitectures)
            {
                if (architecture == CurrentArchitecture.Current)
                {
                    return true;
                }
            }

            return false;
        }
    }
}