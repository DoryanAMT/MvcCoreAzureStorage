﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MvcCoreAzureStorage.Models;

namespace MvcCoreAzureStorage.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;
        public ServiceStorageBlobs(BlobServiceClient client)
        {
            this.client = client;
        }

        //  METODO PARA RECUPERAR TODOS LOS NOMBRES DE NUESTROS CONTAINERS 
        public async Task<List<string>> GetContainersAsync()
        {
            List<string> containers = new List<string>();
            await foreach (BlobContainerItem item in this.client.GetBlobContainersAsync())
            {
                containers.Add(item.Name);
            }
            return containers;
        }
        public async Task CreateBlobContainerAsync
            (string containerName)
        {
            await this.client.CreateBlobContainerAsync
                (containerName, PublicAccessType.Blob);
        }
        public async Task DeleteContainerAsync
            (string containerName)
        {
            await this.client.DeleteBlobContainerAsync(containerName);
        }
        //  METODO PARA RECUPERAR TODOS LOS BLOBS DE UN CONTAINER
        public async Task<List<BlobModel>> GetBlobAsync
            (string containerName)
        {
            //  NECESITAMOS UN CLIENTE DE CONTAINER
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            List<BlobModel> models = new List<BlobModel>();
            await foreach (BlobItem item in containerClient.GetBlobsAsync())
            {
                BlobClient blobClient = containerClient.GetBlobClient(item.Name);
                BlobModel blob = new BlobModel();
                blob.Nombre = item.Name;
                blob.Url = blobClient.Uri.AbsoluteUri;
                blob.Container = containerName;
                models.Add(blob);
            }
            return models;
        }
        //  METODO PARA ELIMINAR UN BLOB
        public async Task DeleteBlobAsync
            (string containerName, string blobName)
        {
            BlobContainerClient containerClient = 
                this.client.GetBlobContainerClient(containerName);
            await containerClient.DeleteBlobAsync(blobName);
        }

        //  METODO PARA SUBIR UN BLOB A UN CONTAINER
        public async Task UploadBlobAsync
            (string containerName, string blobName, Stream stream)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            await containerClient.UploadBlobAsync
                (blobName, stream);
        }

        public async Task ReadPrivateImg
            ()
        {

        }
    }
}
