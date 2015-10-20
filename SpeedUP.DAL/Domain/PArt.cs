namespace SpeedUP.DAL.Domain
{
    public class Part
    {
        public int Id { get; set; }
        public string PartName { get; set; }
        public virtual int CarId { get; set; }
        public virtual int ManufacturerId { get; set; }
    }
}
