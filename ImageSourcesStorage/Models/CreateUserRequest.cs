namespace ImageSourcesStorage.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateUserRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
