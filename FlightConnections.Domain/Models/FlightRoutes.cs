using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace FlightConnections.Domain.Models
{
    public class FlightRoutes
    {
        [SwaggerSchema(ReadOnly = true)]
        [Key]
        public int Id { get; set; }
        public string Origin { get; set; }
        public string Destiny { get; set; }
        public decimal Value { get; set; }
    }
}