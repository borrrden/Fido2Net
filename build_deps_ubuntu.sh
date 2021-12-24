#!/bin/bash
  
which git > /dev/null || (echo "git not found; aborting..."; exit 1)
which cmake > /dev/null || (echo "cmake not found; aborting..."; exit 1)

pushd `dirname $0`
if [[ ! -d libfido2 ]]; then
    git clone https://github.com/yubico/libfido2 --branch 1.9.0
fi

mkdir -p libfido2/build
pushd libfido2/build

if [[ ! -d libcbor ]]; then
    git clone https://github.com/pjk/libcbor --branch v0.8.0
fi

mkdir -p libcbor/build
pushd libcbor/build
cmake -DCMAKE_BUILD_TYPE=Release ..
make -j8
echo "Need sudo rights to install libcbor..."
sudo make install
popd

echo "Need sudo rights to install apt-get packages..."
sudo apt-get update
sudo apt-get install -y pkg-config libssl-dev libudev-dev

cmake -DCMAKE_BUILD_TYPE=Release ..
make -j8
echo "Need sudo rights to install libfido2..."
sudo make install

popd
popd
