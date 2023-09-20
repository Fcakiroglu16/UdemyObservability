using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Shared.DTOs
{
    public record PaymentCreateResponseDto
    {
        public string Description { get; set; }
    }
}
