using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Int.Infraestructure.Security
{
    public interface IAwsSecretManagerService
    {
        string GetSecret(string secret);
    }
}
