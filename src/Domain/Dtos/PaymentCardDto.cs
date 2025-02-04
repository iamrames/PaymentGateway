using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.Dtos;
public class PaymentCardDto
{
    public required string Type { get; set; }
    public required string Name { get; set; }
    public required string Number { get; set; }
    public int ExpireMonth { get; set; }
    public int ExpireYear { get; set; }
    public int CVV { get; set; }
}
