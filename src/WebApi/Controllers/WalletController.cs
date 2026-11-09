using Application.Features.Transactions.CreateWallet;
using BusinessMakerFramework.SourceGenerator.Shared.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class WalletController(IMediator mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> CreateWalletAsync([FromBody] CreateWalletCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }
}
