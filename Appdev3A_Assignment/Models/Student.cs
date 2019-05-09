using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Appdev3A_Assignment.Models
{
   public class Student
    {
     
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        [StringLength(25)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "surname")]
        [StringLength(25)]
        public string Surname { get; set; }

        [JsonProperty(PropertyName = "studentNo")]
        [StringLength(8, ErrorMessage = "Maximum Length Is 8")]
        [Display(Name = "Student Number")]
        public string StudentNo { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "telephone")]
        [StringLength(maximumLength: 10, ErrorMessage = "Maximum Length Is 10")]
        public string Telephone { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        [StringLength(maximumLength: 10, ErrorMessage = "Maximum Length Is 10")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "email")]
        [StringLength(50)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }

     
    }
}
