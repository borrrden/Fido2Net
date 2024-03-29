﻿using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography;

using Fido2Net;

using McMaster.Extensions.CommandLineUtils;

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace Assert
{
    public enum KeyType
    {
        ECDSA,
        RSA,
        EDDSA
    }

    [HelpOption("-h|--help")]
    class Program
    {
        private static readonly byte[] Cd = {
            0xec, 0x8d, 0x8f, 0x78, 0x42, 0x4a, 0x2b, 0xb7,
            0x82, 0x34, 0xaa, 0xca, 0x07, 0xa1, 0xf6, 0x56,
            0x42, 0x1c, 0xb6, 0xf6, 0xb3, 0x00, 0x86, 0x52,
            0x35, 0x2d, 0xa2, 0x62, 0x4a, 0xbe, 0x89, 0x76,
        };

        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Option("-b|--blob-key", Description = "The key of the blob to use")]
        public string BlobKey { get; set; }

        [FileExists]
        [Option("-a|--cred_id", Description = "The path to the file containing the credential ID as binary")]
        public string CredentialId { get; set; }

        [Argument(1, Description = "The device path to use to generate the assertion")]
        [Required]
        public string Device { get; set; }

        [Option("-u", Description = "Force U2F instead of FIDO2")]
        public bool ForceU2F { get; set; }

        [FileExists]
        [Option("-s|--hmac-salt", Description = "The file containing the binary HMAC salt to use")]
        public string HMACSalt { get; set; }

        [Option("-h|--hmac-secret", Description = "The file to write the HMAC secret to")]
        public string HMACSecret { get; set; }

        [Option("-P|--pin", Description = "The pin of the FIDO device")]
        public string Pin { get; set; }

        [FileExists]
        [Required]
        [Argument(0, Description = "The file containing the public key bytes to use for the assertion")]
        public string PublicKey { get; set; }

        [Option("-T|--timeout", Description = "The timeout (in seconds) to use when asserting")]
        public int Timeout { get; set; }

        [Option("-t|--type", Description = "The type of public key to use in the assertion")]
        public KeyType Type { get; set; } = KeyType.ECDSA;

        [Option("-p", Description = "Requires user presence when generating the assertion")]
        public bool UserPresenceRequired { get; set; }

        [Option("-v", Description = "Requires user verification when generating the assertion")]
        public bool UserVerificationRequired { get; set; }

        private void OnExecute()
        {
            Fido2Settings.Flags = FidoFlags.Debug;
            var ext = FidoExtensions.None;
            if (HMACSalt != null) {
                ext |= FidoExtensions.HmacSecret;
            }
            if (BlobKey != null) {
                ext |= FidoExtensions.LargeBlobKey;
            }

            using (var assert = new FidoAssertion()) {
                using (var dev = new FidoDevice()) {
                    dev.Open(Device);
                    if (ForceU2F) {
                        dev.ForceU2F();
                    }

                    assert.SetClientData(Cd);
                    assert.Rp = "localhost";
                    assert.SetExtensions(ext);
                    if (UserPresenceRequired) {
                        assert.SetUserPresenceRequired(true);
                    }

                    if (UserVerificationRequired) {
                        assert.SetUserVerificationRequired(true);
                    }

                    if (Timeout != 0) {
                        dev.SetTimeout(TimeSpan.FromSeconds(Timeout));
                    }

                    if (CredentialId != null) {
                        var credId = File.ReadAllBytes(CredentialId);
                        assert.AllowCredential(credId);
                    }

                    dev.GetAssert(assert, Pin);
                    dev.Close();
                }

                if (assert.Count != 1) {
                    throw new Exception($"{assert.Count} signatures required");
                }

                if (Pin != null) {
                    UserVerificationRequired = true;
                }

                VerifyAssert(assert[0].AuthData, assert[0].Signature, ext);

                if (HMACSecret != null) {
                    File.WriteAllBytes(HMACSecret, assert[0].HmacSecret.ToArray());
                }
            }
        }

        private void VerifyAssert(ReadOnlySpan<byte> authData, ReadOnlySpan<byte> signature, FidoExtensions extensions)
        {
            byte[] keyBytes = null;
            using (var fin = new StreamReader(File.OpenRead(PublicKey))) {
                var reader = new PemReader(fin);
                if (Type == KeyType.ECDSA) {
                    ECPublicKeyParameters parameters = (ECPublicKeyParameters)reader.ReadObject();
                    var x = parameters.Q.XCoord.ToBigInteger().ToByteArray();
                    var y = parameters.Q.YCoord.ToBigInteger().ToByteArray();
                    keyBytes = new byte[64];

                    // Why?  There seems to be an extra byte at the beginning sometimes
                    Array.Copy(x, x.Length - 32, keyBytes, 0, 32);
                    Array.Copy(y, y.Length - 32, keyBytes, 32, 32);
                } else if (Type == KeyType.RSA) {
                    RsaKeyParameters parameters = (RsaKeyParameters)reader.ReadObject();
                    var mod = parameters.Modulus.ToByteArray();
                    var e = parameters.Exponent.ToByteArray();
                    keyBytes = new byte[mod.Length + e.Length];
                    mod.CopyTo(keyBytes, 0);
                    e.CopyTo(keyBytes, mod.Length);
                } else if (Type == KeyType.EDDSA) {
                    throw new NotSupportedException("Original example includes this but not sure why, it doesn't seem supported");
                } else {
                    throw new NotSupportedException("Unsupported key type");
                }
            }

            using (var assert = new FidoAssertion()) {
                assert.SetClientData(Cd);
                assert.Rp = "localhost";
                assert.Count = 1;
                assert.SetAuthData(authData, 0);
                assert.SetExtensions(extensions);
                if(UserPresenceRequired) {
                    assert.SetUserPresenceRequired(true);
                }

                if(UserVerificationRequired) {
                    assert.SetUserVerificationRequired(true);
                }

                assert.SetSignature(signature, 0);
                assert.Verify(0, FromKeyType(Type), keyBytes);
            }
        }

        private static FidoCose FromKeyType(KeyType type)
        {
            switch (type) {
                case KeyType.ECDSA:
                    return FidoCose.ES256;
                case KeyType.RSA:
                    return FidoCose.RS256;
                case KeyType.EDDSA:
                    return FidoCose.EDDSA;
                default:
                    throw new InvalidOperationException("Unrecognized key type");
            }
        }
    }
}
