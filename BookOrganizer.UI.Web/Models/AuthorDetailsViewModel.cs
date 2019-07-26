using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookOrganizer.UI.Web.Models
{
    public class AuthorDetailsViewModel
    {
        private string firstName;
        private string lastName;

        [MinLength(1, ErrorMessage = "Authors first name should be at minimum 1 character long.")]
        [MaxLength(50, ErrorMessage = "Authors first name should be at maximum 50 character long.")]
        [Required]
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
            }
        }

        [MinLength(1, ErrorMessage = "Authors last name should be at minimum 1 character long.")]
        [MaxLength(50, ErrorMessage = "Authors last name should be at maximum 50 character long.")]
        [Required]
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
             }
        }
    }
}
