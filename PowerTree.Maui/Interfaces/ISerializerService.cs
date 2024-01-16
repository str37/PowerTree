using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTree.Maui.Interfaces
{
    public interface ISerializerService
    {
        Task<Stream> Serialize<T>(T @object, CancellationToken cancellationToken);
        ValueTask<T?> Deserialize<T>(Stream stream, CancellationToken cancellationToken);
    }
}
