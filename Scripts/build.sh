#! /bin/sh

target="$1"

# Change this the name of your project. This will be the name of the final executables as well.
project="$PROJECT_NAME"

#echo "skipping build for testing purposes"

echo "Deleting Build/ directory"
rm -rf Build/

if [ "$target" == "" ] || [ "$target" == "osx" ]; then
  echo "Attempting to build $project for OS X"
  /Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -batchmode \
    -nographics \
    -silent-crashes \
    -logFile $(pwd)/unity.log \
    -projectPath $(pwd) \
    -buildOSXUniversalPlayer "$(pwd)/Build/osx/$project.app" \
    -quit

  echo 'Logs from OS X build'
  cat $(pwd)/unity.log
  rm $(pwd)/unity.log
  if [ ! -d "$(pwd)/Build/osx" ]; then
    echo "$(pwd)/Build/osx" does not exist
    exit 2
  else
    echo Zipping osx build
    cd ./Build/osx
    zip -r osx_$TRAVIS_BRANCH-v0.$TRAVIS_BUILD_NUMBER.zip $project.app
    cd ../../
    mv ./Build/osx/osx_$TRAVIS_BRANCH-v0.$TRAVIS_BUILD_NUMBER.zip ./Build/osx_$TRAVIS_BRANCH-v0.$TRAVIS_BUILD_NUMBER.zip
    rm -rf Build/osx/
  fi
fi

if [ "$target" == "" ] || [ "$target" == "linux" ]; then
  echo "Attempting to build $project for Linux"
  /Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -batchmode \
    -nographics \
    -silent-crashes \
    -logFile $(pwd)/unity.log \
    -projectPath $(pwd) \
    -buildLinuxUniversalPlayer "$(pwd)/Build/linux/$project.exe" \
    -quit

  echo 'Logs from Linux build'
  cat $(pwd)/unity.log
  rm $(pwd)/unity.log
  if [ ! -d "$(pwd)/Build/linux" ]; then
    echo "$(pwd)/Build/linux" does not exist
    exit 3
  else
    echo Zipping linux build
    cd ./Build/
    mv linux/ $project/
    zip -r linux_$TRAVIS_BRANCH-v0.$TRAVIS_BUILD_NUMBER.zip $project/
    cd ../
    rm -rf Build/$project/
  fi
fi

if [ "$target" == "" ] || [ "$target" == "windows" ]; then
  echo "Attempting to build $project for Windows"
  /Applications/Unity/Unity.app/Contents/MacOS/Unity \
    -batchmode \
    -nographics \
    -silent-crashes \
    -logFile $(pwd)/unity.log \
    -projectPath $(pwd) \
    -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe" \
    -quit

  echo 'Logs from Windows build'
  cat $(pwd)/unity.log
  rm $(pwd)/unity.log
  if [ ! -d "$(pwd)/Build/windows" ]; then
    echo "$(pwd)/Build/windows" does not exist
    exit 1
  else
    echo Zipping windows build
    cd ./Build/
    mv windows/ $project/
    zip -r windows_$TRAVIS_BRANCH-v0.$TRAVIS_BUILD_NUMBER.zip $project/
    cd ../
    rm -rf Build/$project/
  fi
fi

ls .
