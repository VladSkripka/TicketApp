using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace TestApp.Models
{
    public class Flight
    {
        [Key]
        public int PublicID { get; set; }
        public int FID { get; set; }
        public string departure_point { get; set; }
        public string destination { get; set; }
        public DateTime date { get; set; }
        public DateTime dep_time { get; set; }
        public DateTime arrival_time { get; set; }
        public float ticket_price { get; set; }
        public int sites { get; set; }
        public int sites_left { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }

    public class Ticket
    {
        [Key]
        public int ID { get; set; }
        public int Public_ID { get; set; }
        public string passenger_first { get; set; }
        public string passenger_last { get; set; }
        public Flight Flight { get; set; }
    }

    public class SearchViewModel
    {
        public string DeparturePoint { get; set; }
        public string Destination { get; set; }
    }
}
