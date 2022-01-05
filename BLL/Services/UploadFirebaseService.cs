using BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Firebase.Storage;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

namespace BLL.Services
{
    public class UploadFirebaseService : IUploadFirebaseService
    {
        private IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly string bucket;

        public UploadFirebaseService(IConfiguration configuration, ILogger logger)
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
        public async Task<string> UploadFileToFirebase(IFormFile file, string type, string parent, string fileName)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    string typeOfFile = file.FileName.Substring(file.FileName.IndexOf("."));

                    try
                    {
                        FirebaseStorageTask task = new FirebaseStorage(bucket)
                            .Child(type)
                            .Child(parent)
                            .Child(fileName + typeOfFile)
                            .PutAsync(file.OpenReadStream());
                        return await task;
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.Message + "\nCannot upload image to Firebase Storage");
                        return null;
                    }

                }
            }
            return String.Empty;
        }


        /// <summary>
        /// Upload Files To Firebase
        /// </summary>
        /// <param name="files"></param>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> UploadFilesToFirebase(List<IFormFile> files, string type, string parent, string fileName)
        {
            string urlConcat = String.Empty;
            foreach (var file in files)
            {
                try
                {
                    string url = await UploadFileToFirebase(file, type, parent,
                        fileName + (files.IndexOf(file) + 1));

                    if (file == files[files.Count - 1])
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
                    _logger.Error(e.Message + "\nCannot upload image to Firebase Storage");
                    return null;
                }
            }
            return urlConcat;
        }
    }
}
