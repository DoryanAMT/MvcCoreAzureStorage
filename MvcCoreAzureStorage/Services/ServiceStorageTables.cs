using Azure.Data.Tables;
using MvcCoreAzureStorage.Models;

namespace MvcCoreAzureStorage.Services
{
    public class ServiceStorageTables
    {
        private TableClient tableClient;
        public ServiceStorageTables(TableServiceClient tableService)
        {
            this.tableClient = tableService.GetTableClient("clientes");
        }

        public async Task CreateClientAsync
            (int id, string nombre, string empresa, int salario, int edad)
        {
            Cliente cliente = new Cliente
            {
                IdCliente = id,
                Empresa = empresa,
                Nombre = nombre,
                Salario = salario,
                Edad = edad
            };
            await this.tableClient.AddEntityAsync<Cliente>(cliente);
        }

        //  INTERNAMENTE, SE PUEDEN BUSCAR CLIENTES POR CUALQUIER CAMPO
        //  SI VAMOS A REALIZAR UN BUSQUEDA, POR EJEMPLO PARA DETAILS
        //  NO SE PUEDE BUSCAR SOLAMENTE POR SU ROW KEY, SE GENERA
        //  UNA COMBINACION DE ROW KEY Y PARTITIO KYE PARA BUSCAR
        //  POR ENTIDAD UNICA
        public async Task<Cliente> FindClienteAsync
            (string partitioKey, string rowKey)
        {
            Cliente cliente = await
                this.tableClient.GetEntityAsync<Cliente>(partitioKey, rowKey);
            return cliente;
        }

        //  PARA ELIMINAR UN ELEMENTO UNICO TAMBIEN NECESITAMOS
        //  PARTITIONKEY Y ROWKEY
        public async Task DeleteClientAsync
            (string partitionKey, string rowKey)
        {
            await this.tableClient.DeleteEntityAsync
                (partitionKey, rowKey);
        }
        //  METODO PARA RECUPERAR TODOS LOS CLIENTES
        public async Task<List<Cliente>> GetClientesAsync()
        {
            List<Cliente> clientes = new List<Cliente>();
            //  PARA BUSCAR NECESITAMOS UTILIZAR UN OBJETO QUERY
            //  CON UN FILTER
            var query =
                this.tableClient.QueryAsync<Cliente>
                (filter:"");
            //  DEBEMOS EXTRAER TODOS LOS DATOS DEL QUERY
            await foreach (var item in query)
            {
                clientes.Add(item);
            }
            return clientes;
        }
        //  TODOS LOS CLIENTES POR EMPRESA
        public async Task<List<Cliente>> GetClientesEmpresaAsync
            (string empresa)
        {
            //  TENEMOS DOS TIPOS DE FILTER, LOS DOS SE UTILIZAN
            //  CON Query
            //  1)SI REALIZAMOS EL FILTER CON QueryAsync,
            //  DEBEMOS UTILIZAR UNA
            //  SITAXIS Y ATRAER LOS DATOS DEL 
            //string filtro = "Campo eq valor";
            //string filtro = "Campo eq valor con Campo2 gt valor2 ";
            //string filtro = "Campo lt valor con Campo2 gt valor2 ";
            //string filtroSalario = "Salario gt 250000 and Salario lt 30000 ";
            //var query =
            //    this.tableClient.QueryAsync<Cliente>(filter: filtroSalario);


            //  2)SI REALIZAMOS LA CONSULTA CON Query
            //  PODEMOS UTILIZAR LAMBDA Y EXTRAER LOS DATOS
            //  DIRECTAMENTE, PERO NO ES ASINCRONO
            List<Cliente> clientes = new List<Cliente>();

            var query =
                this.tableClient.Query<Cliente>
                (x => x.Empresa == empresa);
            return query.ToList();
        }
    }
}
