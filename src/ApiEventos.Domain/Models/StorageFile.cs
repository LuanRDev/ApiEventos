using System.ComponentModel.DataAnnotations.Schema;

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
            ValidaStorageFile(codigoEvento, empresa, name, metadata,type,extension,bytes);
            GuidStorageId = Guid.NewGuid();
            CodigoEvento = codigoEvento;
            Empresa = empresa;
            Name = name;
            Metadata = metadata;
            Type = type;
            Extension = extension;
            Bytes = bytes;
            Url = $"eventos/empresas/{Empresa}/{CodigoEvento}/documentos/{GuidStorageId}.{Extension}";
        }
        private void ValidaStorageFile(int codigoEvento, string empresa, string name, string metadata, string type, string extensions, byte[] bytes)
        {
            if(codigoEvento <= 0)
            {
                throw new InvalidOperationException("O campo codigo evento não pode ser menor ou igual a zero.");
            }
            if (string.IsNullOrEmpty(empresa))
            {
                throw new InvalidOperationException("O campo empresa não pode ser vazio ou nulo.");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("O campo name não pode ser vazio ou nulo.");
            }
            if (string.IsNullOrEmpty(metadata))
            {
                throw new InvalidOperationException("O campo metadata não pode ser vazio ou nulo.");
            }
            if(string.IsNullOrEmpty(type))
            {
                throw new InvalidOperationException("O campo type não pode ser vazio ou nulo.");
            }
            if (string.IsNullOrEmpty(extensions))
            {
                throw new InvalidOperationException("O campo extensions não pode ser vazio ou nulo.");
            }
            if(bytes == null)
            {
                throw new InvalidOperationException("O campo bytes não pode ser nulo.");
            }
        }
    }
}
