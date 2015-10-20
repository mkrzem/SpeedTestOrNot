namespace SpeedUP.DAL.Domain
{
    public class Part
    {
        public int Id { get; set; }
        public string PartName { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}
