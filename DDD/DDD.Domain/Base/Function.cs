using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.IO;

namespace DDD.Domain.Base
{
    public static class Function
    {
        public static string QuebraDeLinha(this string value)
        {
            return value.Replace(Environment.NewLine, "<br/>");
        }

        public static string Capitalize(string value)
        {
            if (value == null)
                return string.Empty;

            if (value.Length == 0)
                return value;

            value = value.ToLower();

            StringBuilder result = new StringBuilder(value);
            result[0] = char.ToUpper(result[0]);
            for (int i = 1; i < result.Length; ++i)
            {
                if (char.IsWhiteSpace(result[i - 1]))
                    result[i] = char.ToUpper(result[i]);
            }

            result = result.Replace(" Da ", " da ");
            result = result.Replace(" Das ", " das ");
            result = result.Replace(" De ", " de ");
            result = result.Replace(" De, ", " de, ");
            result = result.Replace(" Do ", " do ");
            result = result.Replace(" Dos ", " dos ");
            result = result.Replace(" E ", " e ");
            result = result.Replace(" Ao ", " ao ");

            return result.ToString();
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name;
        }

        public static string GetPropertyName<T, KProperty>(Expression<Func<T, KProperty>> propertyLambda)
        {
            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name;
        }

        public static string OnlyNumbers(string strNumbers)
        {
            if (strNumbers == null)
                return null;

            List<char> numbers = new List<char>("0123456789");
            StringBuilder toReturn = new StringBuilder(strNumbers.Length);
            CharEnumerator enumerator = strNumbers.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (numbers.Contains(enumerator.Current))
                    toReturn.Append(enumerator.Current);
            }

            return toReturn.ToString();
        }

        public static bool ValidarCPF(string cpf)
        {
            if (cpf.Length != 11)
                return false;

            bool igual = true;
            for (int i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "12345678909")
                return false;

            int[] numeros = new int[11];
            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                    cpf[i].ToString());

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;
            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;

            return true;
        }

        public static bool ValidarCNPJ(string cnpj)
        {
            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;
            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(cnpj.Substring(nrDig, 1));
                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));

                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);

                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                    else
                        CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                }

                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidaEmail(string email)
        {
            try
            {
                if (email != null && email.Contains(" "))
                    return false;

                MailAddress m = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidarCelular(string numero)
        {
            if (string.IsNullOrEmpty(numero))
                return false;

            if (numero.Length < 14 || numero.Length > 15)
                return false;

            var caracter = numero.Substring(5, 1);
            if ((caracter == "9") ||
                (caracter == "8") ||
                (caracter == "7"))
                return true;
            else
                return false;
        }

        public static string Upper(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.ToUpper();
            else
                return null;
        }

        public static string Lower(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.ToLower();
            else
                return null;
        }

        public static DateTime ConvertaParaDateTime(DateTime data, TimeSpan hora)
        {
            return new DateTime(data.Year, data.Month, data.Day, hora.Hours, hora.Minutes, hora.Seconds);
        }

        public static DateTime ConvertaParaDateTime(DateTime data)
        {
            return new DateTime(data.Year, data.Month, data.Day);
        }

        public static string AmericaName(string fullName)
        {
            return FirstName(fullName, false) + ", " + FirstName(fullName, true, true, false);
        }

        public static string FirstName(string fullName, bool firstName = true, bool middleName = false, bool lastName = true)
        {
            if (fullName == null)
                fullName = string.Empty;

            fullName = fullName.Trim();

            string[] separacao = fullName.Split(' ');

            string resultado = "";


            int i = firstName ? 0 : 1;
            int cont = lastName ? 1 : 2;
            bool meio = middleName;

            while (i <= separacao.Count() - cont)
            {
                if (!meio)
                {
                    if ((i == 0) || (i == separacao.Count() - 1))
                        resultado += " " + separacao[i];
                }
                else
                    resultado += " " + separacao[i];

                i++;
            }

            return resultado.Trim();
        }

        public static List<int> GetIds(string idList, char pchar = ',')
        {
            string[] values = idList.Split(pchar);
            List<int> ids = new List<int>(values.Length);

            foreach (string s in values)
            {
                int i;

                if (int.TryParse(s, out i))
                {
                    ids.Add(i);
                }
            }

            return ids;
        }

        public static string Cryptografa(string value)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }

        public static string EncodeUtf8(string value)
        {
            byte[] bytes = Encoding.Default.GetBytes(value);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string RemoveAcentos(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string TrimNull(this string text)
        {
            if (text != null)
            {
                return text.Trim();
            }

            return null;
        }

        public static bool EnviaEmail(string emailDestino, string assunto, string conteudo)
        {
            //Define os dados do e-mail
            string nomeRemetente = "Contato Help Desk";
            string emailRemetente = "suporte@pontoid.com.br";
            string senha = "jbcm1015";

            //Host da porta SMTP
            string SMTP = "mail.pontoid.com.br";

            string emailDestinatario = emailDestino;
            //string emailComCopia        = "email@comcopia.com.br";
            //string emailComCopiaOculta  = "email@comcopiaoculta.com.br";

            string assuntoMensagem = assunto;
            string conteudoMensagem = conteudo;

            //Cria objeto com dados do e-mail.
            MailMessage objEmail = new MailMessage();

            //Define o Campo From e ReplyTo do e-mail.
            objEmail.From = new MailAddress(nomeRemetente + "<" + emailRemetente + ">");

            //Define os destinatários do e-mail.
            objEmail.To.Add(emailDestinatario);

            //Enviar cópia para.
            //objEmail.CC.Add(emailComCopia);

            //Enviar cópia oculta para.
            //objEmail.Bcc.Add(emailComCopiaOculta);

            //Define a prioridade do e-mail.
            objEmail.Priority = MailPriority.Normal;

            //Define o formato do e-mail HTML (caso não queira HTML alocar valor false)
            objEmail.IsBodyHtml = true;

            //Define título do e-mail.
            objEmail.Subject = assuntoMensagem;

            //Define o corpo do e-mail.
            objEmail.Body = conteudoMensagem;

            //Para evitar problemas de caracteres "estranhos", configuramos o charset para "ISO-8859-1"
            objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");


            // Caso queira enviar um arquivo anexo
            //Caminho do arquivo a ser enviado como anexo
            //string arquivo = Server.MapPath("arquivo.jpg");

            // Ou especifique o caminho manualmente
            //string arquivo = @"e:\home\LoginFTP\Web\arquivo.jpg";

            // Cria o anexo para o e-mail
            //Attachment anexo = new Attachment(arquivo, System.Net.Mime.MediaTypeNames.Application.Octet);

            // Anexa o arquivo a mensagem de email
            //objEmail.Attachments.Add(anexo);

            //Cria objeto com os dados do SMTP
            SmtpClient objSmtp = new SmtpClient();

            //Alocamos o endereço do host para enviar os e-mails  
            objSmtp.Credentials = new NetworkCredential(emailRemetente, senha);
            objSmtp.Host = SMTP;
            objSmtp.Port = 587;
            //Caso utilize conta de email do exchange da locaweb deve habilitar o SSL
            //objEmail.EnableSsl = true;

            //Enviamos o e-mail através do método .send()
            try
            {
                if (IsDebugMode())
                {
                    objEmail.To.Clear();
                    objEmail.To.Add("glaubervictor@gmail.com");
                    objEmail.To.Add("flavioh007@gmail.com");
                }

                objSmtp.Send(objEmail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                //excluímos o objeto de e-mail da memória
                objEmail.Dispose();
                //anexo.Dispose();
            }
        }

        public static string ZerosEsquerda(string strString, int intTamanho)
        {
            string strResult = "";
            for (int intCont = 1; intCont <= (intTamanho - strString.Length); intCont++)
            {
                strResult += "0";
            }

            return strResult + strString;
        }

        public static string FormatarCpfCnpj(string strCpfCnpj)
        {
            if (strCpfCnpj.Length <= 11)
            {
                MaskedTextProvider mtpCpf = new MaskedTextProvider(@"000\.000\.000-00");
                mtpCpf.Set(ZerosEsquerda(strCpfCnpj, 11));
                return mtpCpf.ToString();
            }
            else
            {
                MaskedTextProvider mtpCnpj = new MaskedTextProvider(@"00\.000\.000/0000-00");
                mtpCnpj.Set(ZerosEsquerda(strCpfCnpj, 11));
                return mtpCnpj.ToString();
            }
        }

        public static bool TryParseDateTime(string text, out Nullable<DateTime> nDate)
        {
            DateTime date;
            bool isParsed = System.DateTime.TryParse(text, out date);
            if (isParsed)
                nDate = new Nullable<DateTime>(date);
            else
                nDate = new Nullable<DateTime>();
            return isParsed;
        }

        public static IEnumerable<string[]> AdicionePrefixo(IEnumerable<string[]> lista, string prefixo)
        {
            foreach (var item in lista)
            {
                item[1] = prefixo + item[1];
            }

            return lista;
        }

        public static IEnumerable<string[]> ValideLista<T>(this ICollection<T> lista, string prefixo) where T : class, IValidator
        {
            var validationResults = new List<string[]>();
            if (lista == null) return validationResults;

            var t = typeof(T);
            int i = 0;

            foreach (var item in lista)
            {
                validationResults.AddRange(AdicionePrefixo(item.Validate(), string.Format("{0}[{1}].", prefixo, i)));
                i++;
            }

            return validationResults;
        }

        public static T ConvertaParaEnum<T>(int? valor)
        {
            if (valor.HasValue)
            {
                T result = (T)Enum.ToObject(typeof(T), valor.Value);
                return result;
            }

            return default(T);
        }

        public static string GetEnumDescription<TEnum>(this TEnum value)
        {
            if (value == null)
                return null;

            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = null;

            if (fi != null)
            {
                attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetEnumDescription(string value, Type enumType)
        {
            if (value == null)
                return null;

            FieldInfo fi = enumType.GetField(value.ToString());
            DescriptionAttribute[] attributes = null;

            if (fi != null)
            {
                attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static T GetAttributeFrom<T>(this object instance) where T : Attribute
        {
            return GetAttributeFrom<T>(instance.GetType());
        }

        public static T GetAttributeFrom<T>(this Type instance) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(instance, typeof(T));
        }

        public static string GetDiaDaSemana(DateTime? data)
        {
            if (data.HasValue)
            {
                return Capitalize(new CultureInfo("pt-BR").DateTimeFormat.DayNames[(int)data.Value.DayOfWeek]);
            }

            return string.Empty;
        }

        public static string TraduzirDayOfWeek(DayOfWeek? day)
        {
            if (day.HasValue)
            {
                return Capitalize(new CultureInfo("pt-BR").DateTimeFormat.DayNames[(int)day.Value]);
            }

            return string.Empty;
        }

        public static decimal CalculaPorcentagem(decimal total, decimal valor)
        {
            if (total == 0)
                return 0;

            return valor / total * 100;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void SendEmail(
            string subject,
            string message,
            string emitenteNome,
            string contaEmail,
            string contaSenha,
            string contaSmtp,
            int contaPorta,
            bool ssl,
            string emailDestinatario,
            Dictionary<string, Stream> files = null)
        {
            List<string> destinatarios = new List<string>();
            if (!string.IsNullOrWhiteSpace(emailDestinatario))
                destinatarios.Add(emailDestinatario);

            SendEmail(subject,
                message,
                emitenteNome,
                contaEmail,
                contaSenha,
                contaSmtp,
                contaPorta,
                ssl,
                destinatarios,
                files);
        }

        public static void SendEmail(
            string subject,
            string message,
            string emitenteNome,
            string contaEmail,
            string contaSenha,
            string contaSmtp,
            int contaPorta,
            bool ssl,
            List<string> destinatarios,
            Dictionary<string, Stream> files = null)
        {
            var loginInfo = new NetworkCredential(contaEmail, contaSenha);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient(contaSmtp, contaPorta);

            if (string.IsNullOrWhiteSpace(emitenteNome))
            {
                msg.From = new MailAddress(contaEmail, emitenteNome);
            }
            else
            {
                msg.From = new MailAddress(contaEmail);
            }

            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            if (destinatarios == null || destinatarios.Count == 0)
            {
                throw new Exception("Nenhum destinatário informado.");
            }
            else
            {
                foreach (var destinatario in destinatarios)
                {
                    if (ValidaEmail(destinatario))
                        msg.To.Add(new MailAddress(destinatario));
                }
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    Attachment anexo = new Attachment(file.Value, file.Key);
                    msg.Attachments.Add(anexo);
                }
            }

            smtpClient.EnableSsl = ssl;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }

        public static bool IsDebugMode()
        {
            bool isDebug = false;

#if DEBUG
            isDebug = true;
#endif

            return isDebug;
        }

        public static bool LatitudeValida(string latitude)
        {
            if (string.IsNullOrWhiteSpace(latitude))
                return false;

            latitude = latitude.Replace(".", ",");
            var lati = Double.Parse(latitude);

            string number = lati.ToString().Replace(',', '.');
            if (number.Contains("."))
            {
                int length = number.Substring(number.IndexOf(".")).Length - 1;
                if (length > 6)
                {
                    return false;
                }
            }

            try
            {
                //new System.Device.Location.GeoCoordinate(lati, 1);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool LongitudeValida(string longitude)
        {
            if (string.IsNullOrWhiteSpace(longitude))
                return false;

            longitude = longitude.Replace(".", ",");
            var longi = Double.Parse(longitude);

            string number = longi.ToString().Replace(',', '.');
            if (number.Contains("."))
            {
                int length = number.Substring(number.IndexOf(".")).Length - 1;
                if (length > 6)
                {
                    return false;
                }
            }

            try
            {
                //new System.Device.Location.GeoCoordinate(1, longi);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static byte[] GetBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static List<int> ObtenhaUltimosCincoAnos(int anoReferencia)
        {
            List<int> anos = new List<int>();
            for (int ano = anoReferencia; ano >= (anoReferencia - 5); ano--)
            {
                anos.Add(ano);
            }

            return anos;
        }

        public static bool ValideFormula(string expression)
        {
            var code = GetCodeExpression(expression);
            CompilerResults compilerResults = CompileScript(code);

            return !compilerResults.Errors.HasErrors;
        }

        public static double ExecutaFormula(string expression)
        {
            var code = GetCodeExpression(expression);
            CompilerResults compilerResults = CompileScript(code);

            if (compilerResults.Errors.HasErrors)
            {
                throw new InvalidOperationException("Expressão contém erro de sitaxe.");
            }

            Assembly assembly = compilerResults.CompiledAssembly;
            MethodInfo method = assembly.GetType("Func").GetMethod("func");

            return (double)method.Invoke(null, null);
        }

        private static CompilerResults CompileScript(string source)
        {
            CompilerParameters parms = new CompilerParameters();

            parms.GenerateExecutable = false;
            parms.GenerateInMemory = true;
            parms.IncludeDebugInformation = false;

            CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");

            return compiler.CompileAssemblyFromSource(parms, source);
        }

        private static string GetCodeExpression(string expression)
        {
            string code = string.Format  // Note: Use "{{" to denote a single "{"
            (
                "public static class Func{{ public static double func(){{ return {0};}}}}",
                expression
            );

            return code;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static int CalcularIdade(DateTime dataNascimento, DateTime database)
        {
            if (database.Date < dataNascimento.Date)
            {
                throw new Exception("Data de Nascimento deve ser menor ou igual a database.");

            }
            int years = database.Year - dataNascimento.Year;

            if ((dataNascimento.Month > database.Month) || (dataNascimento.Month == database.Month && dataNascimento.Day > database.Day))
                years--;

            return years;
        }

        public static IEnumerable<T> ObtenhaAlgunsItensEmOrdemAleatoria<T>(this IEnumerable<T> lista, int maxItens)
        {
            Random random = new Random(Environment.TickCount);
            Dictionary<double, T> randomSortTable = new Dictionary<double, T>();
            randomSortTable = lista.ToDictionary(x => random.NextDouble(), y => y);

            return randomSortTable.OrderBy(KVP => KVP.Key).Take(maxItens).Select(KVP => KVP.Value);
        }

        public static string GereProtocolo(DateTime data)
        {
            int tamanho = 4;
            DateTime dateTimeNow = data;
            var ticks = dateTimeNow.Ticks.ToString();
            int startIndex = ticks.Length - tamanho;
            var lastTicks = ticks.Substring(startIndex, tamanho);

            //formato 1506054621 ano[2]-mes[2]-dia[2]-lastTicks[tamanho]
            var result = string.Format("{0:yy}{1:00}{2:00}{3}", dateTimeNow, dateTimeNow.Month, dateTimeNow.Day, lastTicks);
            return result;
        }
    }
}