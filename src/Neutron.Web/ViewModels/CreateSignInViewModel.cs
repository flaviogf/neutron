using System.ComponentModel.DataAnnotations;

namespace Neutron.Web.ViewModels
{
    public class CreateSignInViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
