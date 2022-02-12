using System;

namespace BLL.Dtos.Account
{
    [Serializable]
    public class AccountRequest
    {
        public string FirebaseToken { get; set; }
    }
}
