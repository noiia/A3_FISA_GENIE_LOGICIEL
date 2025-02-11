namespace ServiceRealTimeState
{
    public class Program
    {
        // ServiceRealTimeState.exe <SaveJobName> <counter_id> <fileInfoName> <fileInfoFullName> <fileInfoLenght> <destinationPathDir> <fileName> <msg>
        public static void Main(string[] args)
        {
            Counters counter = RealTimeState.GetCounterById(int.Parse(args[1]));

            string[] fileInfo = {
                args[3],
                args[4],
                args[5]
            };

            RealTimeState.WriteState(args[0], counter, fileInfo, args[6], args[7], args[8]);
        }
    }
}