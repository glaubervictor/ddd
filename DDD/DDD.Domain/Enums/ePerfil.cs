using System.ComponentModel;

namespace DDD.Domain.Enums
{
    public enum ePerfil
    {
        [Description("Administrador do Sistema")]
        AdministradorSistema = 1,

        [Description("Cliente")]
        Cliente = 2
    }
}
