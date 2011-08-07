using System.Diagnostics;
using System.Linq;
using Machine.Specifications;
using typefoundry.Impl.Scan;

namespace typefoundry.tests {
    
    public class when_loading_scan_index 
    {
        static Stopwatch watch;
        static ScanIndex index;

        private Because of = () => 
        {
            watch = Stopwatch.StartNew();

            index = new ScanIndex();
            index.Start();

            watch.Stop();

            var assemblyCount = index.CompleteAssemblyList.Count;
            var closers = index.Closers.Where( x => x.Value.Count == 1 ).ToList();
            var singleInterfaces = index.SingleImplementations;
        };
        
        private It should_take_1_seconds = () => 
            watch.ElapsedMilliseconds.ShouldBeLessThan( 1001 );
    }
}
