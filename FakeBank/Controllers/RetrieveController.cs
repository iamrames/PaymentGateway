using FakeBank.Enums;
using FakeBank.Models;
using Microsoft.AspNetCore.Mvc;

namespace FakeBank.Controllers;

[ApiController]
[Route("[controller]")]
public class RetrieveController : ControllerBase
{
    private readonly int _delayInMs = 3000;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RetrieveModel retrieveModel)
    {
        await Task.Delay(_delayInMs);
        if (DateTime.Today >
            new DateTime(retrieveModel.CardExpireYear, retrieveModel.CardExpireMonth,
                DateTime.Today.Day))
            return BadRequest(new { Status = PaymentStates.Failed, Message = "EXPIRED"});

        if (retrieveModel.CardNumber == "9999999999999999")
            return BadRequest(new { Status = PaymentStates.Failed, Message = "INVALID" });

        return Ok(new RetrieveResponseModel()
        {
            Id = Guid.NewGuid(),
            State = PaymentStates.Success,
        });
    }
}
