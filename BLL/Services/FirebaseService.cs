using BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Firebase.Storage;
using System.Threading.Tasks;
using System;
using System.IO;
using FirebaseAdmin.Auth;
using BLL.Dtos.Account;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using DAL.Constants;
using BLL.Dtos.Resident;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Collections.ObjectModel;
using Firebase.Database;
using Newtonsoft.Json.Linq;
using Firebase.Database.Query;

namespace BLL.Services
{
    public class FirebaseService : IFirebaseService
    {
        private IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly string bucket;
        private readonly string auth;
        private readonly IUtilService _utilService;

        public FirebaseService(IConfiguration configuration, ILogger logger, IUtilService utilService)
        {
            _configuration = configuration;
            _logger = logger;
            bucket = _configuration.GetValue<string>("Firebase:Bucket");
            auth = _configuration.GetValue<string>("Firebase:auth");
            _utilService = utilService;
        }


        /// <summary>
        /// Upload File To Firebase
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> UploadFileToFirebase(string file, string type, string parent, string fileName)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    try
                    {
                        var bytes = Convert.FromBase64String(file);

                        FirebaseStorageTask task = new FirebaseStorage(bucket)
                            .Child(type)
                            .Child(parent)
                            .Child(fileName)
                            .PutAsync(new MemoryStream(bytes));
                        return await task;
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.Message + "\nCannot upload image to Firebase Storage");
                        return null;
                    }

                }
            }
            return string.Empty;
        }


        /// <summary>
        /// Upload Files To Firebase
        /// </summary>
        /// <param name="files"></param>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <param name="fileName"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<string> UploadFilesToFirebase(string[] files, string type, string parent, string fileName, int order)
        {
            string urlConcat = string.Empty;
            if (files != null && files.Length != 0)
                foreach (var file in files)
                {
                    try
                    {
                        string url = await UploadFileToFirebase(file, type, parent,
                            fileName + (Array.IndexOf(files, file) + order + 1));

                        urlConcat += url + "|";
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.Message + "\nCannot upload images to Firebase Storage");
                        return null;
                    }
                }
            return urlConcat;
        }


        /// <summary>
        /// Get UID By Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> GetUIDByToken(string token)
        {
            FirebaseToken decodedToken;
            try
            {
                if (FirebaseApp.DefaultInstance == null)
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.GetApplicationDefault(),
                    });
                decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            }
            catch (Exception e)
            {
                _logger.Error("[FirenbaseService.GetUIDByToken()]: " + e.Message);

                throw new UnauthorizedAccessException();
            }

            return decodedToken.Uid;
        }


        /// <summary>
        /// Get User Data From Firestore By UID
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task<ExtendAccountResponse> GetUserDataFromFirestoreByUID(string uid)
        {
            FirestoreDb db = FirestoreDb.Create("lcp-mobile-8c400");
            CollectionReference usersRef = db.Collection("user");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            ExtendAccountResponse extendAccountResponse = null;

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Id.Equals(uid))
                {
                    Dictionary<string, object> documentDictionary = document.ToDictionary();

                    string residentType = documentDictionary.ContainsKey("role") ? (string)documentDictionary["role"] : ResidentType.CUSTOMER;

                    ResidentResponse residentResponse = new ResidentResponse
                    {
                        ResidentId = document.Id + "_" + (documentDictionary.ContainsKey("role") ? (string)documentDictionary["role"] : ResidentType.CUSTOMER),
                        ResidentName = documentDictionary.ContainsKey("fullname") ? (string)documentDictionary["fullname"] : default,
                        Gender = documentDictionary.ContainsKey("gender") ? (string)documentDictionary["gender"] : default,
                        DeliveryAddress = documentDictionary.ContainsKey("deliveryAddress") ? (string)documentDictionary["deliveryAddress"] : default,
                        DateOfBirth = documentDictionary.ContainsKey("dob") ? DateTime.Parse((string)documentDictionary["dob"]) : null,
                        Type = residentType,
                        AccountId = document.Id,
                        CreatedDate = _utilService.CurrentTimeInVietnam(),
                        UpdatedDate = _utilService.CurrentTimeInVietnam(),
                        Status = residentType.Equals("MarketManager") ? (int)ResidentStatus.VERIFIED_RESIDENT : (int)ResidentStatus.UNVERIFIED_RESIDENT,
                        ApartmentId = (string)documentDictionary["apartmentId"],
                        PhoneNumber = documentDictionary.ContainsKey("phoneNumber") ? (string)documentDictionary["phoneNumber"] : null
                    };

                    extendAccountResponse = new ExtendAccountResponse
                    {
                        AccountId = document.Id,
                        Username = documentDictionary.ContainsKey("username") ? (string)documentDictionary["username"] : default,
                        ProfileImage = "",
                        CreatedDate = _utilService.CurrentTimeInVietnam(),
                        UpdatedDate = _utilService.CurrentTimeInVietnam(),
                        Status = (int)AccountStatus.ACTIVE_ACCOUNT,
                        RoleId = RoleId.APARTMENT,
                        Residents = new Collection<ResidentResponse> { residentResponse }
                    };

                    break;
                }
            }
            return extendAccountResponse;
        }


        /// <summary>
        /// Push Notification
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="receiverId"></param>
        /// <param name="image"></param>
        /// <param name="code"></param>
        public async Task PushNotification(string senderId, string receiverId, string image, string code)
        {
            var firebaseClient = new FirebaseClient(
                    "https://lcp-mobile-8c400-default-rtdb.asia-southeast1.firebasedatabase.app/",
            new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(auth) });

            JObject jObject = new()
            {
                {"createdDate" , (long)((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds()},
                {"data", new JObject() { { "image", $"{image}"}, { "name", ""}, { "id", ""} }},
                {"read", 0},
                { "receiverId", receiverId},
                { "senderId", senderId},
                { "type", "301"}
            };

            var post = await firebaseClient.Child("Notification")
                                            .Child(receiverId)
                                            .PostAsync(jObject.ToString());
        }
    }
}
