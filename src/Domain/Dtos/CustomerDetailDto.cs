﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.Dtos;
public class CustomerDetailDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}
