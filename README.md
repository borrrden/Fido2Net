# Fido2Net

This is a set of .NET classes and bindings for wrapping the [libfido2](https://github.com/Yubico/libfido2) implementation.  You must supply a compiled version of the library for use.  Note that at the time of writing, there is not a good way to compile a shared library for Windows but for those interested I will explain how to add it.

## Main API

### `FidoAssertion`

This class is responsible for handling the **assertion** portion of the FIDO2 logic.  That is to say, a credential has already been created and a remote party would like to validate that the user is in control of said credential.  

Sample Usage:
```c#
void GetAssertion(byte[] challenge, string devicePath, byte[][] allowedCredentials) {
    byte[] clientDataHash = null;
    
    // This format is governed by the WebAuthn specification
    var clientData = new Dictionary<string, object>
    {
        ["type"] = "webauthn.get",
        ["challenge"] = Convert.ToBase64String(challenge),
        ["origin"] = "https://localhost:5000" // or whatever
    };

    using (var sha = SHA256.Create()) {
        var bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(clientData));
        clientDataHash = sha.ComputeHash(bytes);
    }

    // The assertion is populated with a hash of the client data,
    // the relying party, some options
    using (var assert = new FidoAssertion()) {
        assert.ClientDataHash = clientDataHash;
        assert.Rp = "localhost"; 
        assert.SetOptions(true, false); // User must be present, but verification skipped
        
        // This is required unless the key material is actually stored on
        // the device
        foreach (var c in allowedCredentials) {
            assert.AllowCredential(c);
        }
        
        Console.WriteLine("Please press the security key button...");
        using(var device = new FidoDevice()) { 
            device.Open(devicePath);
            device.GetAssert(assert, null);
        }
    }
}
```

### `FidoCredential`

```c#
void MakeCredential(byte[] challenge, byte[][] existingCredentials, string deviceName) {
    byte[] clientDataHash = null;
    var clientData = new Dictionary<string, object>
    {
        ["type"] = "webauthn.create",
        ["challenge"] = Convert.ToBase64String(challenge),
        ["origin"] = "https://localhost:44334"
    };

    using (var sha = SHA256.Create()) {
        var bytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(clientData));
        clientDataHash = sha.ComputeHash(bytes);
    }
    var type = FidoCose.ES256;

    using (var cred = new FidoCredential()) {
        cred.SetType((FidoCose)type);
        cred.SetUser(new FidoCredentialUser
        {
            DisplayName = "Display Name",
            Name = "Real Name",
            Id = Encoding.UTF8.GetBytes("Real Name") // Arbitrary, but must be bytes
        });

        cred.Rp = new FidoCredentialRp
        {
            Id = "localhost"
            Name = "https://localhost:5000" // or whatever
        };

        cred.SetOptions(false, false); // Don't store key material on device, and don't require verification
        foreach (var c in existingCredentials) {
            cred.Exclude(c); // Don't create if it already exists 
        }

        cred.ClientDataHash = clientDataHash;
        Console.WriteLine("Please press the security key button...");
        using(var device = new FidoDevice()) { 
            device.Open(deviceName);
            device.MakeCredential(cred, null);
        }
    }
}
```

### `FidoDevice`

This class abstracts a FIDO2 capable device and lets you interact with it.  It is responsible for credential and assertion creation (see its usage in the `FidoAssertion` and `FidoCredential` examples)

### `FidoDeviceInfoList`

This class will enumerate attached FIDO2 devices for you

Sample Usage:
```c#
using (var list = new FidoDeviceInfoList(64)) {
    foreach(var dev in list) {
        Console.WriteLine(dev.Path);
    }
}
```

## FIDO2 shared library on Windows

To compile the native library as a shared library for Windows, follow the instructions in the libfido2 repo up until building the libfido2 library itself.  At that point, modify the CMakeList.txt file in the `src` directory to remove the `if(NOT MSVC)` guard around the shared library target:

```cmake
if(NOT MSVC) # <---
	add_library(fido2_shared SHARED ${FIDO_SOURCES} ${COMPAT_SOURCES})
	# ...
endif() # <---
```

After that you will need to export the symbol names.  An easy way to do this is to add the following near the top of `fido.h`

```c
#ifndef FIDO_PUBLIC_API
#ifdef _MSC_VER
#define FIDO_PUBLIC_API __declspec(dllexport)
#else
#define FIDO_PUBLIC_API
#endif
#endif
```

After that, simply add `FIBO_PUBLIC_API` to the beginning of every function in the header:

```c
FIDO_PUBLIC_API fido_assert_t *fido_assert_new(void);
FIDO_PUBLIC_API fido_cred_t *fido_cred_new(void);
FIDO_PUBLIC_API fido_dev_t *fido_dev_new(void);
// etc, etc
```

And build with CMake as normal!  Then place the resulting fido_shared2.dll (renamed to fido2.dll) and crypto-44.dll in the output folder of your .NET app.