using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Security
{
    public interface IEncryptionService
    {
        public string GetSha256(string strSha256);
        public string CifrarCadena(string CadenaOriginal);
        public string DescifrarCadena(string CadenaCifrada);
    }
}
