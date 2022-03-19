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

namespace BLL.Services
{
    public class FirebaseService : IFirebaseService
    {
        private IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly string bucket;

        public FirebaseService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
            bucket = _configuration.GetValue<string>("Firebase:Bucket");
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

                    ResidentResponse residentResponse = new ResidentResponse
                    {
                        ResidentId = document.Id + "_" + (documentDictionary.ContainsKey("role") ? (string)documentDictionary["role"] : ResidentType.CUSTOMER),
                        ResidentName = documentDictionary.ContainsKey("fullname") ? (string)documentDictionary["fullname"] : default,
                        Gender = documentDictionary.ContainsKey("gender") ? (string)documentDictionary["gender"] : default,
                        DeliveryAddress = documentDictionary.ContainsKey("deliveryAddress") ? (string)documentDictionary["deliveryAddress"] : default,
                        DateOfBirth = (DateTime?)documentDictionary["dob"],
                        Type = documentDictionary.ContainsKey("role") ? (string)documentDictionary["role"] : ResidentType.CUSTOMER,
                        AccountId = document.Id,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        Status = (int)ResidentStatus.UNVERIFIED_RESIDENT
                    };

                    extendAccountResponse = new ExtendAccountResponse
                    {
                        AccountId = document.Id,
                        Username = documentDictionary.ContainsKey("username") ? (string)documentDictionary["username"] : default,
                        ProfileImage = "",
                        AvatarImage = "",
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        Status = (int)AccountStatus.ACTIVE_ACCOUNT,
                        RoleId = RoleId.APARTMENT,
                        Residents = new Collection<ResidentResponse> { residentResponse }
                    };

                    break;
                }
            }
            return extendAccountResponse;
        }
    }
}
