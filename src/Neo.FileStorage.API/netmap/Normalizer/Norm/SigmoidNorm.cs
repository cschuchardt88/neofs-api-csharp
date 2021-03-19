

namespace Neo.FileStorage.API.Netmap.Normalize
{
    public class SigmoidNorm : INormalizer
    {
        private readonly double scale;

        public SigmoidNorm(double scale)
        {
            this.scale = scale;
        }

        public double Normalize(double w)
        {
            if (scale == 0) return 0;
            var x = w / scale;
            return x / (1 + x);
        }
    }
}
