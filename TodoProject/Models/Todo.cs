using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TodoProject.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Please Enter a description.")]
        public string Description { get; set; }=string.Empty;

        [Required(ErrorMessage ="Please mention the priority Order.")]
        public string PriorityId { get; set; }=string.Empty ;

        [ValidateNever]
        public Priority Priority { get; set; } = null!;

        [Required(ErrorMessage = "Please select a Category.")]
        public string CategoryId {  get; set; }=string.Empty ;

        [ValidateNever]
        public Category Category { get; set; } = null! ;

        [Required(ErrorMessage = "Please select a status.")]
        public string StatusId { get; set; }= string.Empty ;

        [ValidateNever]
        public Status Status{ get; set; } = null!;


    }
}
