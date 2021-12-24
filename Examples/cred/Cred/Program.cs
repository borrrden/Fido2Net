using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using Fido2Net;

using McMaster.Extensions.CommandLineUtils;

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using BigInteger = Org.BouncyCastle.Math.BigInteger;

namespace Cred
{
    public enum KeyType
    {
        ECDSA,
        RSA
    }

    [HelpOption("-h|--help")]
    class Program
    {
        private static readonly byte[] Cd = {
            0xf9, 0x64, 0x57, 0xe7, 0x2d, 0x97, 0xf6, 0xbb,
            0xdd, 0xd7, 0xfb, 0x06, 0x37, 0x62, 0xea, 0x26,
            0x20, 0x44, 0x8e, 0x69, 0x7c, 0x03, 0xf2, 0x31,
            0x2f, 0x99, 0xdc, 0xaf, 0x3e, 0x8a, 0x91, 0x6b,
        };

        private static readonly byte[] UserId = {
            0x78, 0x1c, 0x78, 0x60, 0xad, 0x88, 0xd2, 0x63,
            0x32, 0x62, 0x2a, 0xf1, 0x74, 0x5d, 0xed, 0xb2,
            0xe7, 0xa4, 0x2b, 0x44, 0x89, 0x29, 0x39, 0xc5,
            0x56, 0x64, 0x01, 0x27, 0x0d, 0xbb, 0xc4, 0x49,
        };

        [Option("-b|--blob-key", Description = "The key of the device stored blob to reference")]
        public string BlobKey { get; set; }

        [Option("-i|--cred_id", Description = "The file to write the created credential ID to")]
        public string CredentialId { get; set; }

        [Required]
        [Argument(0, Description = "The path to the device to use to generate the credential")]
        public string Device { get; set; }

        [FileExists]
        [Option("-e|--exclude", Description = "A file containing a binary ID of a credential not to create if it exists")]
        public string Exclude { get; set; }

        [Option("-u", Description = "Force U2F instead of FIDO2")]
        public bool ForceU2F { get; set; }

        [Option("-p|--pin", Description = "The pin of the device to use")]
        public string Pin { get; set; }

        [Option("-k|--public-key", Description = "The file to write the resulting public key to")]
        public string PublicKey { get; set; }

        [Option("-r", Description = "Use resident keys when generating the credential (i.e. stored on the device)")]
        public bool ResidentKey { get; set; }

        [Option("-T|--timeout", Description = "Device timeout in seconds")]
        public int Timeout { get; set; }

        [Option("-t|--type", Description = "The key type to use when generating the credential")]
        public KeyType Type { get; set; } = KeyType.ECDSA;

        [Option("-h", Description = "Use the HMAC extension")]
        public bool UseHmac { get; set; }

        [Option("-v", Description = "Whether or not user verification is required for credential generation")]
        public bool UserVerificationRequired { get; set; }

        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private void OnExecute()
        {
            Fido2Settings.Flags = FidoFlags.Debug;
            var ext = FidoExtensions.None;
            if(UseHmac) {
                ext |= FidoExtensions.HmacSecret;
            }

            if(BlobKey != null) {
                ext |= FidoExtensions.LargeBlobKey;
            }

            using (var cred = new FidoCredential()) {
                using (var dev = new FidoDevice()) {
                    dev.Open(Device);
                    if (ForceU2F) {
                        dev.ForceU2F();
                    }

                    if (Exclude != null) {
                        var credId = File.ReadAllBytes(Exclude);
                        cred.Exclude(credId);
                    }

                    cred.SetType(FromKeyType(Type));
                    cred.SetClientData(Cd);
                    cred.Rp = new FidoCredentialRp
                    {
                        Id = "localhost",
                        Name = "sweet home localhost"
                    };

                    cred.SetUser(new FidoCredentialUser
                    {
                        Id = UserId,
                        DisplayName = "john smith",
                        Name = "jsmith"
                    });

                    cred.SetExtensions(ext);

                    if(ResidentKey) {
                        cred.SetResidentKeyRequired(true);
                    }

                    if(UserVerificationRequired) {
                        cred.SetUserVerificationRequried(true);
                    }

                    if(Timeout > 0) {
                        dev.SetTimeout(TimeSpan.FromSeconds(Timeout));
                    }

                    dev.MakeCredential(cred, Pin);
                    dev.Close();
                }

                if(Pin != null) {
                    UserVerificationRequired = true;
                }

                VerifyCred(cred.Format, cred.AuthData, cred.X5C, cred.Signature);
            }
        }

        private void VerifyCred(string format, ReadOnlySpan<byte> authData, ReadOnlySpan<byte> x5C, ReadOnlySpan<byte> signature)
        {
            var ext = FidoExtensions.None;
            if(UseHmac) {
                ext |= FidoExtensions.HmacSecret;
            }

            if(BlobKey != null) {
                ext |= FidoExtensions.LargeBlobKey;
            }

            using (var cred = new FidoCredential()) {
                cred.SetType(FromKeyType(Type));
                cred.SetClientData(Cd);
                cred.Rp = new FidoCredentialRp
                {
                    Id = "localhost",
                    Name = "sweet home localhost"
                };

                cred.AuthData = authData;
                cred.SetExtensions(ext);
                if(ResidentKey) {
                    cred.SetResidentKeyRequired(true);
                }

                if(UserVerificationRequired) {
                    cred.SetUserVerificationRequried(true);
                }

                cred.SetX509(x5C);
                cred.Signature = signature;
                cred.Format = format;
                cred.Verify();

                if (PublicKey != null) {
                    if (Type == KeyType.ECDSA) {
                        WriteEcPublicKey(cred.PublicKey);
                    } else {
                        WriteRsaPublicKey(cred.PublicKey.ToArray());
                    }
                }

                if (CredentialId != null) {
                    File.WriteAllBytes(CredentialId, cred.Id.ToArray());
                }
            }
        }

        private void WriteRsaPublicKey(byte[] publicKey)
        {
            var parameters = new RsaKeyParameters(false, new BigInteger(1, publicKey, 0, 256),
                new BigInteger(1, publicKey, 256, 3));
            using (var fout = new StreamWriter(File.Open(PublicKey, FileMode.Create))) {
                var writer = new PemWriter(fout);
                writer.WriteObject(parameters);
            }
        }

        private void WriteEcPublicKey(ReadOnlySpan<byte> publicKey)
        {
            var ecps = CustomNamedCurves.GetByOid(X9ObjectIdentifiers.Prime256v1) ??
                       ECNamedCurveTable.GetByOid(X9ObjectIdentifiers.Prime256v1);

            byte[] encoded = new byte[publicKey.Length + 1];
            encoded[0] = 0x04;
            publicKey.CopyTo(new Span<byte>(encoded, 1, publicKey.Length));
            var ecdp = new ECDomainParameters(ecps.Curve, ecps.G, ecps.N, ecps.H, ecps.GetSeed());
            var basePoint =
                TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(null, ecdp, encoded));
            var subinfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(basePoint);
            var publicKeyObject = PublicKeyFactory.CreateKey(subinfo);
            using (var fout = new StreamWriter(File.Open(PublicKey, FileMode.Create))) {
                var writer = new PemWriter(fout);
                writer.WriteObject(publicKeyObject);
            }
        }

        private static FidoCose FromKeyType(KeyType type)
        {
            switch (type) {
                case KeyType.ECDSA:
                    return FidoCose.ES256;
                case KeyType.RSA:
                    return FidoCose.RS256;
                default:
                    throw new InvalidOperationException("Unrecognized key type");
            }
        }
    }
}
