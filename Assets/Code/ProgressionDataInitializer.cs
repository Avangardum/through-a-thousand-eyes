namespace ThroughAThousandEyes
{
    public static class ProgressionDataInitializer
    {
        private static bool _isInitialized;
        
        public static void Initialize()
        {
            if (_isInitialized)
                return;

            ProgressionData<int>.Initialize((x, y) => x + y, (x, y) => x * y);
            ProgressionData<long>.Initialize((x, y) => x + y, (x, y) => x * y);
            ProgressionData<float>.Initialize((x, y) => x + y, (x, y) => x * y);
            ProgressionData<double>.Initialize((x, y) => x + y, (x, y) => x * y);
            ProgressionData<decimal>.Initialize((x, y) => x + y, (x, y) => x * y);
        }
    }
}