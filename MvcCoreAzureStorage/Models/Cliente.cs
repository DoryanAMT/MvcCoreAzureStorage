﻿using Azure;
using Azure.Data.Tables;

namespace MvcCoreAzureStorage.Models
{
    public class Cliente:ITableEntity
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public int Salario { get; set; }
        //public int IdCliente { get; set; }
        //public string Empresa { get; set; }
        private string _Empresa;
        public string Empresa {
            get { 
                return this._Empresa; 
                }
            set { this._Empresa = value;
                this.PartitionKey = value;
                } 
        }

        private int _IdCliente;
        public int IdCliente
        {
            get
            {
                return this._IdCliente;
            }
            set
            {
                this._IdCliente = value;
                this.PartitionKey = value.ToString();
            }
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
