using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Example.Domain.Enums;
using Example.Domain.Helpers.Configuration;
using Example.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Example.Infrastructure.Auth
{
    public class JwtFactory : IJwtFactory
    {
        public JwtFactory()
        {
        }

        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly JwtOptions _jwtOptions;

        public JwtFactory(IJwtTokenHandler jwtTokenHandler, IOptions<JwtOptions> jwtOptions)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _jwtOptions = jwtOptions.Value;

            ThrowIfInvalidOptions(_jwtOptions);
        }

        public string GenerateEncodedToken(int userId)
        {
            var claims = new[]
            {
                // adding only id for simplicity
                new Claim(ClaimsEnum.userId.ToString(), userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
            };

            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials);

            return _jwtTokenHandler.WriteToken(jwt);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() -
                       new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                      .TotalSeconds);

        private void ThrowIfInvalidOptions(JwtOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtOptions.JtiGenerator));
            }
        }
    }
}
