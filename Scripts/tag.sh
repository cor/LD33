#! /bin/sh

echo 'adding tag....'
git config --global user.email "cor-bot@pruijs.nl"
git config --global user.name "cor-bot"
export GIT_TAG=$TRAVIS_BRANCH-v0.$TRAVIS_BUILD_NUMBER
git tag $GIT_TAG -a -m "Generated tag from TravisCI for build $TRAVIS_BUILD_NUMBER"
git push -q https://${GH_TOKEN}@${GH_REF} --tags
