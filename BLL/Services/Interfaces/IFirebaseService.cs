﻿using System.Threading.Tasks;

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
    }
}