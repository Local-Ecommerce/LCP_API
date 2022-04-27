using BLL.Dtos.Account;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IFirebaseService
    {
        /// <summary>
        /// Upload File To Firebase
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<string> UploadFileToFirebase(string file, string type, string parent, string fileName);


        /// <summary>
        /// Upload Files To Firebase
        /// </summary>
        /// <param name="files"></param>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <param name="fileName"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<string> UploadFilesToFirebase(string[] files, string type, string parent, string fileName, int order);


        /// <summary>
        /// Get UID By Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<string> GetUIDByToken(string token);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Task<ExtendAccountResponse> GetUserDataFromFirestoreByUID(string uid);


        /// <summary>
        /// Push Notification
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="receiverId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        Task PushNotification(string senderId, string receiverId, string image);
    }
}
