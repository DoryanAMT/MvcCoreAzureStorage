﻿using Microsoft.AspNetCore.Mvc;
using MvcCoreAzureStorage.Models;
using MvcCoreAzureStorage.Services;

namespace MvcCoreAzureStorage.Controllers
{
    public class AzureBlobsController : Controller
    {
        private ServiceStorageBlobs service;
        public AzureBlobsController(ServiceStorageBlobs service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            List<string> containers = await this.service.GetContainersAsync();
            return View(containers);
        }

        public  IActionResult CreateContainer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateContainer
            (string containerName)
        {
            await this.service.CreateBlobContainerAsync(containerName);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteContainer
            (string containerName)
        {
            await this.service.DeleteContainerAsync(containerName);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListBlobs(string containerName)
        {
            List<BlobModel> blobs =
                await this.service.GetBlobAsync(containerName);
            return View(blobs);
        }

        public IActionResult UploadBlob
            (string containername)
        {
            ViewData["CONTAINER"] = containername;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob
            (string containername, IFormFile file)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadBlobAsync
                    (containername, blobName, stream);
            }
            return RedirectToAction("ListBlobs" , new {containername = containername});
        }
        public async Task<IActionResult> DeleteBlob
            (string containername, string blobname)
        {
            await this.service.DeleteBlobAsync(containername, blobname);
            return RedirectToAction("ListBlobs" , new { containername = containername});
        }
    }
}
