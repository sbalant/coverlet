using Coverlet.Core;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Coverlet.Collector
{
    [DataCollectorFriendlyName("CoverletCollector")]
    [DataCollectorTypeUri("my://coverlet/collector")]
    public class CoverletCollector : DataCollector
    {
        private DataCollectionEnvironmentContext context;
        private DataCollectionLogger logger;
        private DataCollectionSink sink;
        private Coverage _coverage;

        public override void Initialize(
                System.Xml.XmlElement configuration,
                DataCollectionEvents events,
                DataCollectionSink sink,
                DataCollectionLogger logger,
                DataCollectionEnvironmentContext context)
        {
            this.logger = logger;
            this.sink = sink;
            this.context = context;

            events.SessionStart += this.SessionStarted_Handler;
            events.SessionEnd += this.SessionEnded_Handler;

            events.TestCaseStart += this.Events_TestCaseStart;
            events.TestCaseEnd += this.Events_TestCaseEnd;
        }

        private void SessionStarted_Handler(object sender, SessionStartEventArgs args)
        {
            // _coverage = new Coverage("/Users/toni/Workspace/readline/test/ReadLine.Tests/bin/Debug/netcoreapp2.0/ReadLine.Tests.dll", args.Context.SessionId.Id.ToString());
            _coverage = new Coverage("/Users/toni/Workspace/pose/test/Pose.Tests/bin/Debug/netcoreapp2.0/Pose.Tests.dll", args.Context.SessionId.Id.ToString());
            _coverage.PrepareModules();
            
            this.logger.LogWarning(context.SessionDataCollectionContext, (context.SessionDataCollectionContext.HasTestCase).ToString());
        }

        private void SessionEnded_Handler(object sender, SessionEndEventArgs args)
        {
            try
            {
                var result = _coverage.GetCoverageResult();
                CoverageSummary summary = new CoverageSummary(result);
                this.logger.LogWarning(context.SessionDataCollectionContext, result.Modules.Count.ToString());
                foreach (var item in summary.CalculateSummary())
                {
                    this.logger.LogWarning(context.SessionDataCollectionContext, item.Key + " " + item.Value.ToString());
                }
            }
            catch (System.Exception ex)
            {
                this.logger.LogWarning(context.SessionDataCollectionContext, ex.ToString());
            }
            
        }

        private void Events_TestCaseStart(object sender, TestCaseStartEventArgs e)
        {
        }

        private void Events_TestCaseEnd(object sender, TestCaseEndEventArgs e)
        {

        }
    }
}