namespace DDD.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public string SenhaConfirma { get; set; }
        public string Guid { get; set; }
        public bool IsAtivo { get; set; }
    }
}
