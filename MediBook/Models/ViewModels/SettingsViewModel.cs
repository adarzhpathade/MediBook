using System;
using System.ComponentModel.DataAnnotations;
using MediBook.Models.DTOs;

namespace MediBook.Models.ViewModels
{
    public class SettingsViewModel
    {
        public string PatientName { get; set; } = string.Empty;

        // Profile Form
        public PatientProfileDto Profile { get; set; } = new PatientProfileDto();
        
        // Password Form
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmNewPassword { get; set; }

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
