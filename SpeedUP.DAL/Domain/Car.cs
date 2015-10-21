using System.Collections.Generic;

namespace SpeedUP.DAL.Domain
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public virtual IList<Part> Parts { get; set; }
    }
}
