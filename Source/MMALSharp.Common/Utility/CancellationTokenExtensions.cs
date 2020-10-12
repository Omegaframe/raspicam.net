using System.Threading;
using System.Threading.Tasks;

namespace MMALSharp.Common.Utility
{
    public static class CancellationTokenExtensions
    {
        /// <summary>
        /// Returns a <see cref="Task"/> whose state will be set to <see cref="TaskStatus.Canceled"/> when this <see cref="CancellationToken"/> is canceled.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task.</returns>
        public static Task AsTask(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();
            cancellationToken.Register(() => { tcs.TrySetCanceled(); }, useSynchronizationContext: false);
            return tcs.Task;
        }
    }
}
