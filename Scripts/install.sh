#! /bin/sh

# This link changes from time to time. I haven't found a reliable hosted installer package for doing regular
# installs like this. You will probably need to grab a current link from: http://unity3d.com/get-unity/download/archive

#echo 'Skipping Unity install for testing purposes'
echo 'Downloading from http://netstorage.unity3d.com/unity/afd2369b692a/MacEditorInstaller/Unity-5.1.2f1.pkg: '
curl -o Unity.pkg http://netstorage.unity3d.com/unity/afd2369b692a/MacEditorInstaller/Unity-5.1.2f1.pkg

echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /
