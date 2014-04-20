using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashCheckerProj
{
    using System.Threading;

    using HashCheckerProj.Properties;

    public static class CustomSettings
    {
        public static ThreadPriority BackgroundThreadPriority
        {
            get
            {
                switch (Settings.Default.ThreadPriority)
                {
                    case 0:
                        return ThreadPriority.Highest;
                    case 1:
                        return ThreadPriority.AboveNormal;
                    case 2:
                        return ThreadPriority.Normal;
                    case 3:
                        return ThreadPriority.BelowNormal;
                    case 4:
                        return ThreadPriority.Lowest;
                    default:
                        throw new InvalidOperationException("Invalid index in settings");
                }
            }
        }
    }
}
