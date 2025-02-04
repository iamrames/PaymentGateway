using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Application.Queries;
using PaymentGateway.Domain.Common;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Application.QueryHandlers;
public class PaymentQueryHandler(IRepository<Payment> repository) :
        IRequestHandler<PaymentQuery, PaymentQuery.Result?>
{

    public async Task<PaymentQuery.Result?> Handle(PaymentQuery request, CancellationToken cancellationToken)
    {
        var payment = await repository.Query.FirstAsync(x => x.Id == request.Id);
        return payment != null ? new PaymentQuery.Result(payment) : null;
    }
}
