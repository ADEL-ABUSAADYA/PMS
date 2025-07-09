using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PMS.Common;

namespace PMS.Data.Interceptors;

public class CancelCommandInterceptor : DbCommandInterceptor
{
    private readonly CancellationTokenProvider _cancellationTokenProvider;
    
    public CancelCommandInterceptor(CancellationTokenProvider cancellationTokenProvider)
    {
        _cancellationTokenProvider = cancellationTokenProvider;
    }
    
    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        if (_cancellationTokenProvider is not null)
        {
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenProvider.CancellationToken);

            cancellationToken = linkedTokenSource.Token;
            
            cancellationToken.Register(command.Cancel);
            
            var reader = await command.ExecuteReaderAsync();
            
            return InterceptionResult<DbDataReader>.SuppressWithResult(reader);
        }
        
        return await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }
}