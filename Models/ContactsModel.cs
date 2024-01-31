namespace money_transfer_server_side.Models
{
    public class ContactsModel : TransactionsModel, IStatusCodeModel
    {
        public List<ContactModel> Contacts { get; set; } = new List<ContactModel>();
    }
}
