﻿using MediatR;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Queries;
public class PaymentQuery : IRequest<PaymentQuery.Result>, IRequest
{
    public PaymentQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }

    public sealed class Result
    {
        public Result(Payment result)
        {
            Data = PaymentDto.FromPayment(result);
        }
        public PaymentDto Data { get; set; }
    }
}
