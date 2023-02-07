using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEventos.Domain.Models
{
    public class StorageFile : BaseEntity
    {
        [NotMapped]
        public Guid? GuidStorageId { get; private set; }
        public int CodigoEvento { get; set; }
        [NotMapped]
        public string? Empresa { get; set; }
        public string? Name { get; set; }
        public string? Url { get; private set; }
        [NotMapped]
        public string? Metadata { get; set; }
        [NotMapped]
        public string? Type { get; set; }
        [NotMapped]
        public string? Extension { get; set; }
        [NotMapped]
        public Byte[]? Bytes { get; set; }
        public StorageFile() { }
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
