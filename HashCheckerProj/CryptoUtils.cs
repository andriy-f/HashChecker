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
                    throw new ArgumentException(@"Unrecognized algorithm index", "hashTypeId");
            }

            return algorithm;
        }

        /// <summary>
        /// Deduct hash type from hash length
        /// </summary>
        /// <param name="hashLength">Length of string hash</param>
        /// <returns></returns>
        [Obsolete]
        public static int DetectHashTypeIdx(int hashLength)
        {
            switch (hashLength)
            {
                case 8: // CRC32: 32 bit = 4 bytes = 8 characters
                    return 0;
                case 32: // md5: 128 bit = 16 bytes = 32 characters
                    return 1;
                case 40: // sha1: 160 bit = 20 bytes = 40 characters
                    return 3;
                case 64: // sha256: 256 bit = 32 bytes = 64 characters                      
                    return 4;
                case 96: // sha384: 384 bit = 48 bytes = 96 characters   
                    return 5;
                case 128: // sha512: 512 bit = 64 bytes = 128 characters   
                    return 6;
                default:
                    throw new ArgumentException(@"Can't detect hash type", "hashLength");
            }
        }

        /// <summary>
        /// Deduct hash type from hash length
        /// </summary>
        /// <param name="hashLength">Length of string hash</param>
        /// <returns></returns>
        public static HashType DetectHashType(int hashLength)
        {
            switch (hashLength)
            {
                case 8: // CRC32: 32 bit = 4 bytes = 8 characters
                    return HashType.Crc32;
                case 32: // md5: 128 bit = 16 bytes = 32 characters
                    return HashType.Md5;
                case 40: // sha1: 160 bit = 20 bytes = 40 characters
                    return HashType.Sha1;
                case 64: // sha256: 256 bit = 32 bytes = 64 characters                      
                    return HashType.Sha256;
                case 96: // sha384: 384 bit = 48 bytes = 96 characters   
                    return HashType.Sha384;
                case 128: // sha512: 512 bit = 64 bytes = 128 characters   
                    return HashType.Sha512;
                default:
                    throw new ArgumentException(@"Can't detect hash type", "hashLength");
            }
        }
    }
}
