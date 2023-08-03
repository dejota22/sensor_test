using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

/* 
 * Criptografia de senha em MD5 Hash.
 *
 * Visite nossa página http://www.codigoexpresso.com.br
 * 
 * public static string CalculaHash(string Senha)
 *        criptografa uma senha em MD5 Hash
 *
 */

namespace Core
{
    public static class MD5Hash
    {
        /// <summary>
        /// Calcula MD% Hash de uma determinada string passada como parametro
        /// </summary>
        /// <param name="Senha">String contendo a senha que deve ser criptografada para MD5 Hash</param>
        /// <returns>string com 32 caracteres com a senha criptografada</returns>
        public static string CalculaHash(string Senha)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Senha);
                byte[] hash = md5.ComputeHash(inputBytes);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString(); // Retorna senha criptografada 
            }
            catch (Exception)
            {
                return null; // Caso encontre erro retorna nulo
            }
        }

        private static Random random = new Random();
        public static string GerarSenhaAleatoria()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

            var senha = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return senha;
        }
    }

}