// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;

namespace Softeq.NetKit.Auth.Common.Utility.AppleHttpClient.Helpers
{
    internal static class ClientSecretHelper
    {
        internal static string Create(
            string privateKey, 
            string issuer, 
            string audience, 
            string subject,
            double expires, 
            string keyId)
        {
            var ecPrivateKeyParameters= (ECPrivateKeyParameters)new Org.BouncyCastle.OpenSsl.PemReader(
                    new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(privateKey))))
                .ReadObject();

            var ecPoint = ecPrivateKeyParameters.Parameters.G.Multiply(ecPrivateKeyParameters.D).Normalize();

            var d = ecPrivateKeyParameters.D.ToByteArray();
            var x = ecPoint.AffineXCoord.GetEncoded();
            var y = ecPoint.AffineYCoord.GetEncoded();

            var ecParameters = new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                D = d,
                Q = new ECPoint
                {
                    X = x,
                    Y = y
                }
            };

            var ecdsa = ECDsa.Create(ecParameters);
           
            var handler = new JwtSecurityTokenHandler();

            DateTime? dateTime = DateTime.UtcNow;

            var token = handler.CreateJwtSecurityToken(
                issuer: issuer,
                audience: audience,
                subject: new ClaimsIdentity(new List<Claim> {new Claim("sub", subject)}),
                expires: dateTime.Value.AddMinutes(expires),
                issuedAt: dateTime,
                notBefore: dateTime,
                signingCredentials:
                new SigningCredentials(
                    new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256
                )
            );
            token.Header.Add("kid", keyId);

            return handler.WriteToken(token);
        }
    }
}
