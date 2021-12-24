# Fido2Net

This is a set of .NET classes and bindings for wrapping the [libfido2](https://github.com/Yubico/libfido2) implementation.  You must first build the native dependencies via the provided script:  build_deps.ps1 / build_deps_macos.sh / build_deps_ubuntu.sh.  Other variants of Linux should examine the "apt-get" section of the script to determine what dependencies to install (libssl / libudev).

## Status

This implementation has been (mildly) tested and confirmed working on Windows with libfido2 1.9.0.  Previously this library was not pinned to a specific release but since libfido2 is picking up speed, it makes sense to start pinning.  Future 1.x versions may work, but they may require some native dependency tweaking (versions as of 1.9.0 are hardcoded into this repo's dependency build script).

NOTE: On Windows you will *not* see any Yubikey devices unless you run as administrator.  This is by design at Microsoft.  All of this traffic is proxied by Windows Hello and so this is the device you will detect (usually at the path windows://hello).  Again, this is not a bug but a design decision at Microsoft.

## Main API

### `FidoAssertion`

This class is responsible for handling the **assertion** portion of the FIDO2 logic.  That is to say, a credential has already been created and a remote party would like to validate that the user is in control of said credential.  

### `FidoCredential`

This class is what gets created during the **attestation** portion of the FIDO2 logic, and what is used during the **assertion** portion.

### `FidoDevice`

This class abstracts a FIDO2 capable device and lets you interact with it.  It is responsible for credential and assertion creation (see its usage in the `FidoAssertion` and `FidoCredential` examples)

### `FidoDeviceInfoList`

This class will enumerate attached FIDO2 devices for you


## Samples

See the Examples directory
