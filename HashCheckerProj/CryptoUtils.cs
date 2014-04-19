namespace HashCheckerProj
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;

    public static class CryptoUtils
    {
        public static readonly string[] HashTypes = { "sfv", "md5", "sha", "sha1", "sha256", "sha384", "sha512" };

        public static readonly HashSet<string> HashTypesSet;

        public static readonly int HashTypesCount;

        static CryptoUtils()
        {
            HashTypesSet = new HashSet<string>(HashTypes);
            HashTypesCount = HashTypes.Length;
        }

        public static bool IsChecksumType(string typeName)
        {
            return HashTypesSet.Contains(typeName.ToLowerInvariant());
        }

        public static HashAlgorithm ToHashAlgorithm(string hashType)
        {
            HashAlgorithm algorithm;
            switch (hashType.ToLowerInvariant())
            {
                case "sfv": // sfv - CRC32                        
                    algorithm = new CRC32();
                    break;
                case "md5": // md5
                    algorithm = MD5.Create();
                    break;
                case "sha":
                case "sha1": // sha or sha1
                    algorithm = new SHA1CryptoServiceProvider();
                    break;
                case "sha256": // sha256                        
                    algorithm = new SHA256Managed();
                    break;
                case "sha384": // sha384
                    algorithm = new SHA384Managed();
                    break;
                case "sha512": // sha512
                    algorithm = new SHA512Managed();
                    break;
                default:
                    throw new ArgumentException(@"Unsupported hash algorithm", "hashType");
            }

            return algorithm;
        }

        public static HashAlgorithm ToHashAlgorithm(int hashTypeId)
        {
            HashAlgorithm algorithm;
            switch (hashTypeId)
            {
                case 0: // sfv - CRC32                        
                    algorithm = new CRC32();
                    break;
                case 1: // md5
                    algorithm = MD5.Create();
                    break;
                case 2:
                case 3: // sha or sha1
                    algorithm = new SHA1CryptoServiceProvider();
                    break;
                case 4: // sha256                        
                    algorithm = new SHA256Managed();
                    break;
                case 5: // sha384
                    algorithm = new SHA384Managed();
                    break;
                case 6: // sha512
                    algorithm = new SHA512Managed();
                    break;
                default:
                    throw new ArgumentException("Unrecognized algorithm index", "hashTypeId");
            }

            return algorithm;
        }
    }
}
