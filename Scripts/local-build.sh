#! /bin/sh

export PROJECT_NAME="LD-template"
export TRAVIS_BRANCH="master"
export TRAVIS_BUILD_NUMBER=42

./Scripts/build.sh "$1"

open ./Build
