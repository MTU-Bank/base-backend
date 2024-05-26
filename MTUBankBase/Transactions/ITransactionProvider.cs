using MTUBankBase.ServiceManager;
using MTUModelContainer.Auth.Models;
using MTUModelContainer.Database.Models;
using MTUModelContainer.SharedModels;
using MTUModelContainer.Transactions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUBankBase.Transactions
{
    [ServiceDefinition(ServiceType.Transaction)]
    public interface ITransactionProvider : IServiceDefinition
    {
        [ServiceRoute("/api/listAccounts")]
        [RequiresAuth]
        public AccountListResponse ListAccounts();

        [ServiceRoute("/api/createAccount")]
        [RequiresAuth]
        public AccountResponse CreateAccount(AccountCreationRequest accountCreation);

        [ServiceRoute("/api/deleteAccount")]
        [RequiresAuth]
        public SuccessResponse DeleteAccount(AccountRequest accountDelete);

        [ServiceRoute("/api/getAccount")]
        [RequiresAuth]
        public AccountResponse GetAccount(AccountRequest account);

        [ServiceRoute("/api/blockAccount")]
        [RequiresAuth]
        public SuccessResponse BlockAccount(AccountRequest account);

        // returns true if there is a default account you can transfer to by phone num
        // otherwise, false
        [ServiceRoute("/api/lookupAccount")]
        [RequiresAuth]
        public SuccessResponse LookupAccount(AccountLookupRequest lookupRequest);

        [ServiceRoute("/api/transferFunds")]
        [RequiresAuth]
        public SuccessResponse TransferFunds(TransactionRequest tx);

        [ServiceRoute("/api/setAsDefault")]
        [RequiresAuth]
        public SuccessResponse SetAsDefaultAccount(AccountRequest accountRequest);
    }
}
