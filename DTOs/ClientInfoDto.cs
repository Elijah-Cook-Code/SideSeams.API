namespace SideSeams.API.DTOs
{
    public class ClientInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<MeasurementsDto> Measurements { get; set; } = new ();
    }
}
