using System.Diagnostics;

namespace UdemyObservability.ConsoleApp
{
    internal static class ActivitySourceProvider
    {
        internal static ActivitySource Source = new ActivitySource(OpenTelemetryConstants.ActivitySourceName);

        internal static ActivitySource SourceFile = new ActivitySource(OpenTelemetryConstants.ActivitySourceFileName);
    }
}
