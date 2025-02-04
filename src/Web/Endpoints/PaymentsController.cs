using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Commands;
using PaymentGateway.Application.Queries;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Web.Endpoints;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly ISender _sender;
    public PaymentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetPayment(Guid id)
    {
        var result = await _sender.Send<PaymentQuery.Result>(new PaymentQuery(id));

        if (result?.Data == null)
            return Results.NotFound();

        return Results.Ok(result.Data);
    }

    [HttpPost("process")]
    public async Task<IResult> CreatePayment([FromBody]PaymentCommand command)
    {
        var result = await _sender.Send(command);

        if (result == null)
            return Results.BadRequest();

        if (result.State == PaymentStates.Failed)
            return Results.BadRequest(result.RejectionReason);

        return Results.Created($"api/payments/{result.Id}", PaymentDto.FromPayment(result));
    }
}
