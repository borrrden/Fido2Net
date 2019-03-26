param(
    [string]$CMakePath = "C:\Program Files\CMake\bin\cmake.exe",
    [string]$GitPath = "C:\Program Files\Git\bin\git.exe"
)

$Git = $(Get-Command git -ErrorAction Ignore) | Select-Object -ExpandProperty Source
if([string]::IsNullOrEmpty($Git)) {
    $Git = $GitPath
}

if(-Not (Test-Path $GitPath)) {
    throw "Unable to locate git at $Git"
}

$CMake = $(Get-Command cmake -ErrorAction Ignore) | Select-Object -ExpandProperty Source
if([string]::IsNullOrEmpty($CMake)) {
    $CMake = $CMakePath
}

if(-Not (Test-Path $CMakePath)) {
    throw "Unable to locate cmake at $CMake"
}

Push-Location $PSScriptRoot
try {
    if(-Not (Test-Path libfido2)) {
        & $Git clone https://github.com/yubico/libfido2
    }

    libfido2\windows\build.ps1
    Copy-Item .\libfido2\build\src\Release\fido2.dll Fido2Net\Native
    Copy-Item .\libfido2\build\libressl-2.8.3\build\crypto\Release\crypto-44.dll .\Fido2Net\Native
} finally {
    Pop-Location
}