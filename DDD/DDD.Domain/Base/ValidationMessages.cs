namespace DDD.Domain.Base
{
    public static class ValidationMessages
    {
        public static string Required()
        {
            return "Este campo é requerido.";
        }

        public static string InvalidEmail()
        {
            return "Email inválido.";
        }

        public static string AlreadyEmail()
        {
            return "Este email já está cadastrado.";
        }

        public static string MaxAllowChar(int max)
        {
            return $"Este campo ultrapassou o máximo permitido, {max} caracteres.";
        }

        public static string MaxAllowDig(int max)
        {
            return $"Este campo ultrapassou o máximo permitido, {max} dígitos.";
        }

        public static string MinAndMaxAllowChar(int min, int max)
        {
            return $"O campo aceita entre {min} a {max} caracteres.";
        }

        public static string MinAndMaxAllowDig(int min, int max)
        {
            return $"O campo aceita entre {min} a {max} dígitos.";
        }

        public static string PasswordLength(int min, int max)
        {
            return $"O campo senha deverá possuir entre {min} a {max} caracteres.";
        }

        public static string PasswordNotConfirmed()
        {
            return "O campo confirmar senha não confere.";
        }

        public static string InvalidCpf()
        {
            return "CPF inválido.";
        }

        public static string InvalidCnpj()
        {
            return "CNPJ inválido.";
        }

        public static string InvalidTenant()
        {
            return "Tenant inválido.";
        }

        public static string InvalidDate()
        {
            return "Data inválida.";
        }
    }
}
