using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagement.BLL.ViewModels.Plans
{
    public class EditPlanViewModel
    {
        public int Id { get; set; }

        // Display only - locked, cannot be edited
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Display(Name = "Price (EGP)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
        [Display(Name = "Duration (Days)")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string Description { get; set; } = default!;
    
}
}
