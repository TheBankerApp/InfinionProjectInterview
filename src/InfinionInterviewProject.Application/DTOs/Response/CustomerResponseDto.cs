using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinionInterviewProject.Application.DTOs.Response
{
    using System;

    namespace InfinionInterviewProject.Application.DTOs.Response
    {
        public class CustomerResponseDto
        {
            public Guid Id { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string State { get; set; }
            public string Lga { get; set; }
            public bool IsPhoneVerified { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }

}
