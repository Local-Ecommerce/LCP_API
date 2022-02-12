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
using BLL.Dtos.Exception;
using BLL.Dtos;
using System.Net;
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
            foreach (var file in files)
            {
                try
                {
                    string url = await UploadFileToFirebase(file, type, parent,
                        fileName + (Array.IndexOf(files, file) + order + 1));

                    if (file == files[^1])
                    {
                        urlConcat += url;
                    }
                    else
                    {
                        urlConcat += url + "|";
                    }
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
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.GetApplicationDefault(),
                });
                decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            }
            catch (Exception e)
            {
                _logger.Error("[FirenbaseService.GetUIDByToken()]: " + e.Message);

                throw new HttpStatusException(HttpStatusCode.OK,
                    new BaseResponse<string>
                    {
                        ResultCode = (int)AccountStatus.INVALID_FIREBASE_TOKEN,
                        ResultMessage = AccountStatus.INVALID_FIREBASE_TOKEN.ToString(),
                        Data = default
                    });
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
                        ResidentId = document.Id,
                        ResidentName = (string)documentDictionary["fullname"],
                        Gender = (string)documentDictionary["gender"],
                        DeliveryAddress = (string)documentDictionary["deliveryAddress"],
                        ApartmentId = (string)documentDictionary["apartmentId"],
                        DateOfBirth = (DateTime?)documentDictionary["dob"],
                        Type = ResidentType.CUSTOMER,
                        AccountId = document.Id,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        Status = (int)ResidentStatus.ACTIVE_RESIDENT
                    };

                    extendAccountResponse = new ExtendAccountResponse
                    {
                        AccountId = document.Id,
                        Username = (string)documentDictionary["username"],
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
