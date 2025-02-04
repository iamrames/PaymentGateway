using System.Text;
using Newtonsoft.Json;
using PaymentGateway.Application.BankProviders;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Infrastructure.BankProvider;
public class FakeBankClient : IAcquiringBank
{
    private readonly IHttpClientFactory _clientFactory;

    public FakeBankClient(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<BankPaymentResult> Process(Payment payment)
    {
        var client = _clientFactory.CreateClient("FakeBank");

        var dataAsString = JsonConvert.SerializeObject(new
        {
            payment.Amount,
            CardName = payment.Card!.Name,
            CardNumber = payment.Card.Number,
            CardExpireMonth = payment.Card.ExpireMonth,
            CardExpireYear = payment.Card.ExpireYear,
            CardCVV = payment.Card.CVV
        });
        var content = new StringContent(dataAsString, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync("/retrieve", content);
            var responseContent = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                await response.Content.ReadAsStringAsync());

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return new BankPaymentResult()
                {
                    Success = false,
                    Message = responseContent!.ContainsKey("message") ? responseContent["message"].ToString() : null,
                    Status = responseContent!.ContainsKey("status") ? (PaymentStates)responseContent["status"] : null
                };
            }

            return new BankPaymentResult()
            {
                Success = response.IsSuccessStatusCode,
                Id = responseContent!.ContainsKey("id") ? responseContent["id"].ToString() : null,
                Message = responseContent!.ContainsKey("message") ? responseContent["message"].ToString() : null,
                Status = responseContent!.ContainsKey("status") ? (PaymentStates)responseContent["status"] : null
            };
        }
        catch (Exception e)
        {
            return new BankPaymentResult()
            {
                Success = false,
                Message = $"Error connecting to Fake Bank: {e.Message}"
            };
        }
    }
}
