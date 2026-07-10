using System.ComponentModel.DataAnnotations;

namespace MediBook.Models.ViewModels
{
    public class AddDoctorViewModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization is required.")]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Qualification is required.")]
        public string Qualification { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years.")]
        public int? Experience { get; set; }

        [Range(0, 10000, ErrorMessage = "Consultation fee must be a positive value.")]
        public decimal? ConsultationFee { get; set; }

        public string? Biography { get; set; }
    }
}
