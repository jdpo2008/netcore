using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LazZiya.ExpressLocalization.DataAnnotations;

namespace Tgs.DataContract.Identity
{
    public class RegisterModelDto
    {
        [ExRequired]
        [ExStringLength(25, MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [ExRequired]
        [ExStringLength(25, MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [ExRequired]
        [ExStringLength(15, MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [ExStringLength(15, MinimumLength = 6)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [ExRequired]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [ExRequired]
        [ExStringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [ExCompare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
