using System.Runtime.InteropServices;

namespace MMALSharp.Native.Clock
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MmalClockLatency
    {
        public long Target;
        public long AttackPeriod;
        public long AttackRate;

        public MmalClockLatency(long target, long attackPeriod, long attackRate)
        {
            Target = target;
            AttackPeriod = attackPeriod;
            AttackRate = attackRate;
        }
    }
}
