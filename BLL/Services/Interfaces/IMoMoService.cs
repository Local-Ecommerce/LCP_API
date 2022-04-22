using System.Threading.Tasks;
using BLL.Dtos.MoMo.CaptureWallet;
using BLL.Dtos.MoMo.IPN;

namespace BLL.Services.Interfaces
{
    public interface IMoMoService
    {
        /// <summary>
        /// Create Capture Wallet
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        MoMoCaptureWalletResponse CreateCaptureWallet(MoMoCaptureWalletRequest requestData);


        /// <summary>
        /// Process IPN
        /// </summary>
        /// <param name="momoIPNRequest"></param>
        /// <returns></returns>
        Task ProcessIPN(MoMoIPNRequest momoIPNRequest);


        /// <summary>
        /// Update Payment Result
        /// </summary>
        /// <param name="momoIPNRequest"></param>
        Task UpdatePaymentResult(MoMoIPNRequest momoIPNRequest);
    }
}
