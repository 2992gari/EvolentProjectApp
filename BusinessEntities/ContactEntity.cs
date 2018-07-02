using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class ContactEntity
    {
        public int ID { get; set; }

        [StringLength(7), Required]
        [RegularExpression("^[a-zA-Z]{1,20}$", ErrorMessage= "Enter only Alphabet for First Name")]
        public string FirstName { get; set; }

        [StringLength(7), Required]
        [RegularExpression("^[a-zA-Z]{1,20}$", ErrorMessage = "Enter only Alphabet for Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5)]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "E-mail is not valid")]        
        public string Email { get; set; }

        [Required]  
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Number is not valid")]
        public string PhoneNumber { get; set;}

        [ValidEnumValue]
        [Required(ErrorMessage = "Staus must be Active/Inactive")]
        public ContactStatus Status { get; set; }
    }

    public enum ContactStatus
    {
        Active,
        Inactive
    }

    public class ValidEnumValueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Type enumType = value.GetType();
            bool valid = Enum.IsDefined(enumType, value);
            if (!valid)
            {
                return new ValidationResult(String.Format("{0} is not a valid value for Status type {1}", value, enumType.Name));
            }
            return ValidationResult.Success;
        }
    }

}
