namespace ApiEventos.Domain.Models
{
    public class StorageFile : BaseEntity
    {
        public Guid GuidStorageId { get; private set; }
        public int CodigoEvento { get; set; }
        public string Empresa { get; set; }
        public string Name { get; set; }
        public string Url { get; private set; }
        public string Metadata { get; set; }
        public string Type { get; set; }
        public string Extension { get; set; }
        public Byte[] Bytes { get; set; }
        public StorageFile(int codigoEvento, string empresa, string name, string metadata, string type, string extension, byte[] bytes)
        {
            GuidStorageId = Guid.NewGuid();
            CodigoEvento = codigoEvento;
            Empresa = empresa;
            Name = name;
            Url = $"eventos/empresas/{Empresa}/{CodigoEvento}/documentos/{GuidStorageId}";
            Metadata = metadata;
            Type = type;
            Extension = extension;
            Bytes = bytes;
        }
    }
}
