using System.Threading;
using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IMovement
    {
        public UniTask Move(CancellationToken cancellationToken);
    }
}