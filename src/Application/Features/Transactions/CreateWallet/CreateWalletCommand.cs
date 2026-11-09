namespace Application.Features.Transactions.CreateWallet;

public class CreateWalletCommand : ICommandRequest
{
    public string Name { get; set; } = string.Empty;
}

//internal class CreateWalletCommandHandler(IEventStoreContext context) : CommandRequestHandler<CreateWalletCommand>
//{
//    public override async Task<Result> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
//    {
//        var wallet = new Wallet(request.Name);
//        await context.SaveAsync(nameof(Wallet), wallet.BusinessId.Value, [.. wallet.GetEvents()]);
//        return Ok();
//    }
//}