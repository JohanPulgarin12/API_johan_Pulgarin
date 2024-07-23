using Microsoft.IdentityModel.Tokens;
using Models.Dto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParametrizacionesApi.Util
{
    public static class UtilWebApi
    {
        //public static AttentionRecording ValidarToken(string secreto, string token)
        //{

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var validationParameters = new TokenValidationParameters()
        //    {
        //        ValidateLifetime = false, // Because there is no expiration in the generated token
        //        ValidateAudience = false, // Because there is no audiance in the generated token
        //        ValidateIssuer = false,   // Because there is no issuer in the generated token
        //        ValidIssuer = "Sample",
        //        ValidAudience = "Sample",
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secreto)) // The same key as the one that generate the token
        //    };

        //    SecurityToken validatedToken;
        //    AttentionRecording attentionRecording = new AttentionRecording();
        //    try
        //    {
        //        var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        //        attentionRecording.IdUsuario = Int32.Parse(principal.Claims.FirstOrDefault(claim => claim.Type == "IdUsuario").Value);//.Claims..FirstOrDefault(c => c.Type == "IdCita")?.Value;
        //        attentionRecording.IdEmpresa = Int32.Parse(principal.Claims.FirstOrDefault(claim => claim.Type == "IdEmpresa").Value);//.Claims..FirstOrDefault(c => c.Type == "IdCita")?.Value;
                
        //        return attentionRecording;
        //    }
        //    catch (Exception exception)
        //    {                
        //        return null;
        //    }

        //}

        public static string createToken(string secreto, int IdUsuario, int IdEmpresa)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secreto));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var secToken = new JwtSecurityToken(
                signingCredentials: credentials,
                issuer: "Sample",
                audience: "Sample",
                claims: new[]
                {
                    new Claim("IdUsuario", IdUsuario.ToString()),
                    new Claim("IdEmpresa", IdEmpresa.ToString())
                },
                expires: DateTime.UtcNow.AddMinutes(5));

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(secToken);

        }
    }
}
