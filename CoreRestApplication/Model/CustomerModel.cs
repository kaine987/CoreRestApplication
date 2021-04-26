namespace CoreRestApplication.Model
{
    public abstract class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public AddressModel Address { get; set; }
        public string CustomerType { get; set; }
    }
}