using BankID.Model;
using System;
using System.Threading.Tasks;

namespace BankID.Interface
{
    public interface IBankIDService : IDisposable
    {
        Task<BankIDAuthResponse> Auth(BankIDAuthRequestNIN authBody);
        Task<BankIDAuthResponse> Auth(BankIDAuthRequestQR authBody);
        Task<BankIDCancelError> Cancel(BankIDCancelRequest cancelBody);
        Task<BankIDCollectResponse> Collect(BankIDCollectRequest collectBody);
        void InitClient();
        bool ValidAuthResponse(string status);
        bool CancelledAuthResponse(string status);
    }
}