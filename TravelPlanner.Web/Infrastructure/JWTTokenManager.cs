using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.Extensions.Options;
using TravelPlanner.BusinessLogic.Extensions;
using TravelPlanner.BusinessLogic.Security;
using TravelPlanner.DomainModel;
using TravelPlanner.Web.Models;

namespace TravelPlanner.Web.Infrastructure
{
    public class JWTTokenManager : IAuthTokenManager
    {
        private readonly JWTSettings _jwtSettings;

        public JWTTokenManager(IOptions<JWTSettings> optionsAccessor)
        {
            _jwtSettings = optionsAccessor.Value;
        }

        public string GetIdToken(User user)
        {
            var payload = new Dictionary<string, object>
            {
                { "id", user.Id },
                { "username", user.UserName },
                { "email", user.Email },
                { "phone", user.Phone }
            };
            return GetToken(payload);
        }

        public string GetAccessToken(User user)
        {
            var payload = new Dictionary<string, object>
            {
                { "sub", user.Id },
                { "email", user.Email }
            };
            return GetToken(payload);
        }

        private string GetToken(Dictionary<string, object> payload)
        {
            var secret = _jwtSettings.SecretKey;

            payload.Add("iss", _jwtSettings.Issuer);
            payload.Add("aud", _jwtSettings.Audience);
            payload.Add("iat",DateTime.Now.ConvertToUnixTimestamp());
            payload.Add("exp", DateTime.Now.AddYears(1).ConvertToUnixTimestamp());

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }
    }
}
